using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    /// <summary>
    /// 公众平台回复消息
    /// </summary>
    public abstract class ResMsg : ICloneable 
    {       
        /// <summary>
        /// 归属平台信息
        /// </summary>
        public PlatformType Platform { set; get; }
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
        public ResMsgType MsgType { set; get; }


        /// <summary>
        /// 回复类型 0:正常 1：自动回复 2：事件回复
        /// </summary>
        public int ResType
        {
            set;
            get;
        }

        /// <summary>
        /// 接收消息没有 发送消息时为0
        /// </summary>
        public string FuncFlag { set; get; }

        public abstract string GetWeixinResXml();

        public abstract string GetWeixinResJson();


        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }


}
