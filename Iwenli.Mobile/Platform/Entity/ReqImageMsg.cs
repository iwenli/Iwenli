namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ReqImageMsg : ReqMsg
    {
        /// <summary>
        /// 图片链接Url，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl { set; get; }

        public ReqImageMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Image;
        }

    }
}
