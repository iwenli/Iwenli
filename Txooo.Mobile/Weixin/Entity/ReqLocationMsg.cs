using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Txooo.Mobile.Weixin.Entity
{
    /// <summary>
    /// 地理位置消息xml格式
    /// </summary>
    [XmlRootAttribute("xml", Namespace = "", IsNullable = false)]
    public class ReqLocationMsg
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
        /// 消息类型，地理位置为location 
        /// </summary>
        public string MsgType { set; get; }
        /// <summary>
        /// 地理位置纬度 
        /// </summary>
        public string Location_X { set; get; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { set; get; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { set; get; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { set; get; }

    }
}
