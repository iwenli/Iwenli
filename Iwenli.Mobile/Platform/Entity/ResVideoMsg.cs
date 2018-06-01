using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 回复视频类消息
    /// </summary>
    public class ResVideoMsg : ResMsg 
    {
        /// <summary>
        /// 对应的媒体ID
        /// </summary>
        public string MediaId { set; get; } 
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 本地服务器视频地址
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResVideoMsg(string toUserName)
        {
            MsgType = ResMsgType.Video;
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
            JsonData data = new JsonData();
            data["touser"] = this.ToUserName;
            var msgtype = this.MsgType.ToString().ToLower();
            data["msgtype"] = msgtype;

            data[msgtype] = new JsonData();

            data[msgtype]["media_id"] = this.MediaId;
            data[msgtype]["title"] = this.Title;
            data[msgtype]["description"] = this.Description;

            return data.ToJson(false);
        }
    }
}
