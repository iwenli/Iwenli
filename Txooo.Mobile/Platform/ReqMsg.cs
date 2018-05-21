using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    /// <summary>
    /// 接受到的消息
    /// </summary>
    public abstract class ReqMsg
    {
        /// <summary>
        /// 归属平台信息
        /// </summary>
        public PlatformType Platform { set; get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public ReqMsgType MsgType { set; get; }
        
        #region 基本信息

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { set; get; }
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

        #endregion        
                     
    } 

 
}
