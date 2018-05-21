/*----------------------------------------------------------------
 *  Copyright (C) 2015 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：AliApiHelper
 *  所属项目：
 *  创建用户：张德良
 *  创建时间：2018/3/20 星期二 上午 9:33:30
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
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Api
{
    /// <summary>
    /// 阿里生活号api
    /// </summary>
    public class AliApiHelper
    {
        #region 构造函数

        AccountInfo m_accountInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account"></param>
        public AliApiHelper(AccountInfo account)
        {
            m_accountInfo = account;
        }

        #endregion
        //消息异步单发接口
        public string SendMsg(string biz_content)
        {
            AlipayOpenPublicMessageCustomSendRequest pushRequst = new AlipayOpenPublicMessageCustomSendRequest();
            pushRequst.BizContent = biz_content;

            //Response.Output.WriteLine(biz_content);
            // Response.End();

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", m_accountInfo.AppId, m_accountInfo.AppSecret, "json", "1.0", "RSA2", m_accountInfo.MchSecret, "utf-8", false);
            AlipayOpenPublicMessageCustomSendResponse pushResponse = client.Execute(pushRequst);
            return pushResponse.Body;
        }

        //模板单发接口
        public string SendMsgByTemp(string biz_content)
        {
            /*
             f5a0fbc3b14c46d5b2a6c8b52865bec7
             {{first.value}}
            提交时间：{{keyword1.value}}
            订单类型：{{keyword2.value}}
            客户信息：{{keyword3.value}}
            订单信息：{{keyword4.value}}
            {{remark.value}}
             */
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", m_accountInfo.AppId, m_accountInfo.AppSecret, "json", "1.0", "RSA2", m_accountInfo.MchSecret, "utf-8", false);
            AlipayOpenPublicMessageSingleSendRequest request = new AlipayOpenPublicMessageSingleSendRequest();
            request.BizContent = biz_content;
            //    "{" +
            //"\"to_user_id\":\"2088002074402861\"," +
            //"\"template\":{" +
            //"\"template_id\":\"f5a0fbc3b14c46d5b2a6c8b52865bec7\"," +
            //"\"context\":{" +      
            //"\"keyword1\":{" +
            //"\"color\":\"#85be53\"," +
            //"\"value\":\"HU7142\"" +
            //"        }," +
            //"\"keyword2\":{" +
            //"\"color\":\"#85be53\"," +
            //"\"value\":\"HU7142\"" +
            //"        }," +
            //"\"first\":{" +
            //"\"color\":\"#85be53\"," +
            //"\"value\":\"HU7142\"" +
            //"        }," +
            //"\"remark\":{" +
            //"\"color\":\"#85be53\"," +
            //"\"value\":\"HU7142\"" +
            //"        }" +
            //"      }" +
            //"    }" +
            //"  }";
            AlipayOpenPublicMessageSingleSendResponse response = client.Execute(request);
            return response.Body;
        }

        /// <summary>
        /// 获取芝麻信用分
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string GetZhimaScore(string accessToken)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", this.m_accountInfo.AppId, this.m_accountInfo.AppSecret, "json", "1.0", "RSA2", this.m_accountInfo.MchSecret, "GBK", false);
            ZhimaCreditScoreGetRequest request = new ZhimaCreditScoreGetRequest();
            request.BizContent = "{" +
            "\"transaction_id\":\"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\"," +
            "\"product_code\":\"w1010100100000000001\"" +
            "  }";
            ZhimaCreditScoreGetResponse response = client.Execute<ZhimaCreditScoreGetResponse>(request, accessToken);
            if (response.Code == "10000")
            {
                return response.ZmScore;
            }
            return "0";
        }
    }
}
