using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 接受消息类型
    /// </summary>
    public enum ReqMsgType : int
    {
        /// <summary>
        /// 事件消息
        /// </summary>
        Event = -1,
        /// <summary>
        /// 文本类消息
        /// </summary>
        Text = 0,
        /// <summary>
        /// 图片消息
        /// </summary>
        Image = 1,
        /// <summary>
        /// 语音消息
        /// </summary>
        Voice = 2,
        /// <summary>
        /// 视频消息
        /// </summary>
        Video = 3,
        /// <summary>
        /// 地理位置消息
        /// </summary>
        Location = 4,
        /// <summary>
        /// 链接消息
        /// </summary>
        Link = 5
    }   
}
