namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ReqTextMsg : ReqMsg
    {
        /// <summary>
        /// 消息内容，大小限制在2048字节，字段为空为不合法请求 
        /// </summary>
        public string Content { set; get; }

        public ReqTextMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Text;
        }
    }
}
