#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：ReqShortVideoMsg
 *  所属项目：Iwenli.Mobile.Platform.Entity
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/6/11 9:22:56
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

namespace Iwenli.Mobile.Platform.Entity
{
    /// <summary>
    /// 小视频消息
    /// </summary>
    public class ReqShortVideoMsg : ReqMsg
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { set; get; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { set; get; }

        public ReqShortVideoMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.ShortVideo;
        }

    }
}
