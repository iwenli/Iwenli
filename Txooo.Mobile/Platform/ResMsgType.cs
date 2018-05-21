using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    /// <summary>
    /// 回复消息类型
    /// </summary>
    public enum ResMsgType : int
    {
        /// <summary>
        /// 回复文本类消息
        /// </summary>
        Text = 0,
        /// <summary>
        /// 回复图片消息
        /// </summary>
        Image = 1,
        /// <summary>
        /// 回复语音消息
        /// </summary>
        Voice = 2,
        /// <summary>
        /// 回复视频消息
        /// </summary>
        Video = 3,
        /// <summary>
        /// 图文类消息
        /// </summary>       
        News = 4,
        /// <summary>
        /// 音乐类消息
        /// </summary>
        Music = 5,
        /// <summary>
        /// 模板消息
        /// </summary>
        Template = 6
    }
}
