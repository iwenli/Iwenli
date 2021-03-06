﻿namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ReqVideoMsg : ReqMsg
    {
         /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { set; get; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { set; get; }

        public ReqVideoMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Video;
        }

    }
}
