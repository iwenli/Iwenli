using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    public class ReqLinkMsg : ReqMsg
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { set; get; }

        public ReqLinkMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Link;
        }
    }
}
