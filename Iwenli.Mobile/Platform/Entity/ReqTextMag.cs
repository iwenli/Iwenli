using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    public class ReqTextMag : ReqMsg
    {
        /// <summary>
        /// 消息内容，大小限制在2048字节，字段为空为不合法请求 
        /// </summary>
        public string Content { set; get; }

        public ReqTextMag(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Text;
        }

    }
}
