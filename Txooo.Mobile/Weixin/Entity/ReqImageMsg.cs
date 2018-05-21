using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Txooo.Mobile.Weixin.Entity
{
    /// <summary>
    /// 图片消息结构
    /// </summary>
    [XmlRootAttribute("xml", Namespace = "", IsNullable = false)]
    public class ReqImageMsg
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
        /// 图片消息为image 
        /// </summary>
        public string MsgType { set; get; }
        /// <summary>
        /// 图片链接Url，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl { set; get; }
    }
}
