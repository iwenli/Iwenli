using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Txooo.Mobile.Weixin.Entity
{
    /// <summary>
    /// 回复纯文本消息格式
    /// </summary>
    [XmlRootAttribute("xml", Namespace = "", IsNullable = false)]
    public class ResTextMsg
    {
        public ResTextMsg BuildMsg(ReqTextMsg reqTextMsg, string content)
        {
            ToUserName = string.Format("<![CDATA[{0}]]>", reqTextMsg.FromUserName);
            FromUserName = string.Format("<![CDATA[{0}]]>", reqTextMsg.ToUserName);
            Content = string.Format("<![CDATA[{0}]]>", content);
            CreateTime = WeixinHelper.GetDatetimeNowString();
            MsgType = string.Format("<![CDATA[{0}]]>", "text");
            FuncFlag = "0";
            return this;
        }

        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName { set; get; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { set; get; }
        /// <summary>
        /// 消息创建时间 
        /// </summary>
        public string CreateTime { set; get; }
        /// <summary>
        /// 文本消息为text 
        /// </summary>
        public string MsgType { set; get; }
        /// <summary>
        /// 消息内容，大小限制在2048字节，字段为空为不合法请求 
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 接收消息没有 发送消息时为0
        /// </summary>
        public string FuncFlag { set; get; }
    }
}
