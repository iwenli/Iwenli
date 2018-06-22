namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 链接消息
    /// </summary>
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
