using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Txooo.Mobile.Weixin.Entity
{
    /// <summary>
    /// 文本消息xml格式
    /// </summary>
    [XmlRootAttribute("xml", Namespace = "", IsNullable = false)]
    public class ReqTextMsg
    {
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
    }

}
