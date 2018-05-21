using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    public class ReqVoiceMsg : ReqMsg
    {
        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { set; get; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { set; get; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { set; get; }

        public ReqVoiceMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Voice;
        }      
    }
}
