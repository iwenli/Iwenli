/*----------------------------------------------------------------
 *  Copyright (C) 2015 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：AliHandler
 *  所属项目：
 *  创建用户：张德良
 *  创建时间：2018/3/20 星期二 上午 9:13:55
 *  
 *  功能描述：
 *          1、
 *          2、 
 * 
 *  修改标识：  
 *  修改描述：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
using Aop.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using Txooo.Mobile.Api;

namespace Txooo.Mobile.Msgx
{
    /// <summary>
    /// 支付宝生活号
    /// </summary>
    public class AliHandler : System.Web.IHttpHandler, IRequiresSessionState
    {
        AccountInfo m_accountInfo;
        public void ProcessRequest(HttpContext context)
        {
            this.TxLogInfo(GetUrlParam(getRequstParam()));
            string _appId = getXmlNode(getRequestString("biz_content"), "AppId");

            m_accountInfo = AccountInfo.GetAccountInfoByWhere(" AND [AppId]='" + _appId+"'");

            if (m_accountInfo == null) return;

            //验证网关
            if ("alipay.service.check".Equals(getRequestString("service")))
            {
                //Response.Output.WriteLine(GetUrlParam());

                verifygw();
            }
            else if ("alipay.mobile.public.message.notify".Equals(getRequestString("service")))
            {
                string eventType = getXmlNode(getRequestString("biz_content"), "EventType");
                string alipayUserId = getXmlNode(getRequestString("biz_content"), "FromAlipayUserId");
                string UserInfo = getXmlNode(getRequestString("biz_content"), "UserInfo");
                string ActionParam = getXmlNode(getRequestString("biz_content"), "ActionParam");
                string AgreementId = getXmlNode(getRequestString("biz_content"), "AgreementId");
                string AccountNo = getXmlNode(getRequestString("biz_content"), "AccountNo");
                string AppId = getXmlNode(getRequestString("biz_content"), "AppId");
                string CreateTime = getXmlNode(getRequestString("biz_content"), "CreateTime");
                string MsgType = getXmlNode(getRequestString("biz_content"), "MsgType");

                AliApiHelper _api = new AliApiHelper(m_accountInfo);

                if ("follow".Equals(eventType))
                {


                    //用户新关注后，可以给用户发送一条欢迎消息，或者引导消息
                    //如：
                    string biz_content = "{\"msgType\":\"text\",\"text\":{\"content\":\"你好，欢迎来到"+m_accountInfo.Name+"\"},\"toUserId\":\"" + alipayUserId + "\"}";
                    context.Response.Output.WriteLine(_api.SendMsg(biz_content));
                }
                else if ("unfollow".Equals(eventType))
                {

                }
                else if ("click".Equals(eventType))
                {

                }
                else if ("enter".Equals(eventType))
                {

                }
                if ("text".Equals(MsgType))
                {
                    string biz_content = "{\"msgType\":\"text\",\"text\":{\"content\":\"你好，这是对话消息\"},\"toUserId\":\"" + alipayUserId + "\"}";
                    context.Response.Output.WriteLine(_api.SendMsg(biz_content));
                }
            }
        }

        /// <summary>
        /// 转换支付宝的请求为字典数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getAlipayRequstParams()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("service", getRequestString("service"));
            dict.Add("sign_type", getRequestString("sign_type"));
            dict.Add("charset", getRequestString("charset"));
            dict.Add("biz_content", getRequestString("biz_content"));
            dict.Add("sign", getRequestString("sign"));
            return dict;
        }

        /// <summary>
        /// 验签支付宝请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool verifySignAlipayRequest(Dictionary<string, string> param)
        {
            bool result = AlipaySignature.RSACheckV2(param, m_accountInfo.MchSecret, "utf-8", "RSA2", false);
            return result;
        }

        /// <summary>
        /// 支付宝验证商户网关
        /// </summary>
        public void verifygw()
        {
            //  Request.Params;
            Dictionary<string, string> dict = getAlipayRequstParams();
            //string biz_content = AlipaySignature.CheckSignAndDecrypt(dict, Config.alipay_public_key, Config.merchant_private_key, true, false);
            string biz_content = dict["biz_content"];
            if (!verifySignAlipayRequest(dict))
            {
                verifygwResponse(false, m_accountInfo.AppPubKey);
            }
            if ("verifygw".Equals(getXmlNode(biz_content, "EventType")))
            {

                verifygwResponse(true, m_accountInfo.AppPubKey);

            }
        }

        /// <summary>
        /// 按key获取get和post请求
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getRequestString(string key)
        {
            string value = null;
            if (HttpContext.Current.Request.Form.Get(key) != null && HttpContext.Current.Request.Form.Get(key).ToString() != "")
            {
                value = HttpContext.Current.Request.Form.Get(key).ToString();
            }
            else if (HttpContext.Current.Request.QueryString[key] != null && HttpContext.Current.Request.QueryString[key].ToString() != "")
            {
                value = HttpContext.Current.Request.QueryString[key].ToString();
            }

            return value;
        }

        /// <summary>/// 遍历Url中的参数列表/// </summary>
        /// <returns>如:(?userName=keleyi&userType=1)</returns>
        public string GetUrlParam(Dictionary<string, string> param)
        {
            string urlParam = "";
            if (param != null)
            {
                //urlParam = "?";

                foreach (string key in param.Keys)
                {
                    urlParam += key + "=" + param[key] + "&";
                }
                urlParam = urlParam.Substring(0, urlParam.LastIndexOf('&'));
            }
            return urlParam;
        }

        /// <summary>
        /// 获取xml中的事件类型
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string getXmlNode(string xml, string node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNodeList EventType = xmlDoc.GetElementsByTagName(node);
            string type = null;
            if (EventType.Count > 0)
            {
                type = xmlDoc.SelectSingleNode("//" + node).InnerText;
            }
            //Response.Output.WriteLine("EventType:" + EventType);
            return type;
        }

        /// <summary>
        /// 验证网关，签名内容并返回给支付宝xml
        /// </summary>
        /// <param name="_success"></param>
        /// <param name="merchantPubKey"></param>
        /// <returns></returns>
        public string verifygwResponse(bool _success, string merchantPubKey)
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Clear();

            XmlDocument xmlDoc = new XmlDocument(); //创建实例
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmldecl);


            XmlElement xmlElem = xmlDoc.CreateElement("alipay"); //新建元素

            xmlDoc.AppendChild(xmlElem); //添加元素


            XmlNode alipay = xmlDoc.SelectSingleNode("alipay");
            XmlElement response = xmlDoc.CreateElement("response");
            XmlElement success = xmlDoc.CreateElement("success");
            if (_success)
            {
                success.InnerText = "true";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
            }
            else
            {
                success.InnerText = "false";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
                XmlElement err = xmlDoc.CreateElement("error_code");
                err.InnerText = "VERIFY_FAILED";
                response.AppendChild(err);
            }

            XmlElement biz_content = xmlDoc.CreateElement("biz_content");
            biz_content.InnerText = merchantPubKey;
            response.AppendChild(biz_content);

            alipay.AppendChild(response);

            string _sign = AlipaySignature.RSASign(response.InnerXml, m_accountInfo.AppSecret, "utf-8", "RSA2", false);

            XmlElement sign = xmlDoc.CreateElement("sign");
            sign.InnerText = _sign;
            alipay.AppendChild(sign);
            XmlElement sign_type = xmlDoc.CreateElement("sign_type");
            sign_type.InnerText = "RSA2";
            alipay.AppendChild(sign_type);

            HttpContext.Current.Response.Output.Write(xmlDoc.InnerXml);
            this.TxLogInfo(xmlDoc.InnerXml);
            HttpContext.Current.Response.End();

            return null;
        }

        /// <summary>
        /// 获取所有请求参数，转换为字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getRequstParam()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (HttpContext.Current.Request.QueryString != null)
            {
                foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
                {
                    dict.Add(key, HttpContext.Current.Request.QueryString[key]);
                }
            }

            if (HttpContext.Current.Request.Form != null)
            {
                for (int i = 0; i < HttpContext.Current.Request.Params.Count; i++)
                {
                    //  log(Request.Params.Keys[i].ToString() + " -:- " + Request.Params[i].ToString());
                    dict.Add(HttpContext.Current.Request.Params.Keys[i].ToString(), HttpContext.Current.Request.Params[i].ToString());
                }
            }



            return dict;
        }

        public void verifygw_success_response()
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            HttpContext.Current.Response.Clear();
            string resp = AlipaySignature.encryptAndSign("<success>true</success><biz_content>" + m_accountInfo.AppPubKey + "</biz_content>", m_accountInfo.MchSecret, m_accountInfo.AppSecret, "utf-8", false, true);
            HttpContext.Current.Response.Output.WriteLine(resp);
            HttpContext.Current.Response.End();
        }
        public bool IsReusable => false;
    }
}
