using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 回复音乐类消息
    /// </summary>
    public class ResMusicMsg : ResMsg 
    {
        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicURL { set; get; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { set; get; }
        /// <summary>
        /// 缩略图的媒体id，通过上传多媒体文件，得到的id
        /// </summary>
        public string ThumbMediaId { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResMusicMsg(string toUserName)
        {
            MsgType = ResMsgType.Music;
            CreateTime = DateTime.Now.ToUnixTicks().ToString();
            FuncFlag = "0";
            ToUserName = toUserName;
        }



        public override string GetWeixinResXml()
        {
            throw new NotImplementedException();
        }

        public override string GetWeixinResJson()
        {
            throw new NotImplementedException();
        }
    }
}
