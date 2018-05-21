using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    /// <summary>
    /// 回复语言类消息
    /// </summary>
    public class ResVoiceMsg : ResMsg 
    {
        /// <summary>
        /// 对应的媒体ID
        /// </summary>
        public string MediaId { set; get; }

        /// <summary>
        /// 本地服务器语音地址
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResVoiceMsg(string toUserName)
        {
            MsgType = ResMsgType.Voice;
            CreateTime = CommonFunction.GetDatetimeNowString();
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
