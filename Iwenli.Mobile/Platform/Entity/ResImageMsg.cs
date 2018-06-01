using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 回复图片类消息
    /// </summary>
    public class ResImageMsg : ResMsg 
    {
        /// <summary>
        /// 对应的媒体ID
        /// </summary>
        public string MediaId { set; get; }

        /// <summary>
        /// 本地服务器图片地址
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResImageMsg(string toUserName)
        {
            MsgType = ResMsgType.Image;
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

            return data.ToJson(false);
        }
    }
}
