using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Txooo.Mobile.Weixin.Entity;
using Txooo.Mobile.Serialization;

namespace Txooo.Mobile.Weixin
{
    public class ResponseHelp
    {
        /// <summary>
        /// 构建微信文本消息
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="resposneContent"></param>
        /// <returns></returns>
        public static string BuildTextMsgString(ReqTextMsg reqMsg, string resposneContent)
        {
            try
            {
                ResTextMsg msg = new ResTextMsg();
                msg.BuildMsg(reqMsg, resposneContent);

                XmlSerialization xmls = new XmlSerialization();
                string TextMsg = xmls.ParseToStr<ResTextMsg>(msg);
                return TextMsg.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("&lt;", "<").Replace("&gt;", ">");

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 构建微信图文消息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string BuildNewsMsgString(ResImageMsg response)
        {

            if (response.Articles.Count > 10 || response.Articles.Count == 0)
            {
                throw new Exception("图文消息个数大于0小于11.");
            }
            response.ArticleCount = response.Articles.Count;

            try
            {
                XmlSerialization xmls = new XmlSerialization();
                string TextMsg = xmls.ParseToStr<ResImageMsg>(response);
                return TextMsg.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("&lt;", "<").Replace("&gt;", ">").Replace("><Article><", "><item><").Replace("></Article><", "></item><");
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
