using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    /// <summary>
    /// 回复文本类消息
    /// </summary>
    public class ResTextMsg : ResMsg 
    {
        /// <summary>
        /// 消息内容，大小限制在2048字节，字段为空为不合法请求 
        /// </summary>
        public string Content { set; get; }

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResTextMsg(string toUserName)
        {
            MsgType = ResMsgType.Text;
            CreateTime = CommonFunction.GetDatetimeNowString();
            FuncFlag = "0";
            ToUserName = toUserName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reqmsg"></param>
        public ResTextMsg(ReqMsg reqmsg)
        {
            MsgType = ResMsgType.Text;
            CreateTime = CommonFunction.GetDatetimeNowString();
            FuncFlag = "0";
            ToUserName = reqmsg.FromUserName;
            FromUserName = reqmsg.ToUserName;
            Platform = reqmsg.Platform;
        }

        #endregion
      

        public override string GetWeixinResXml()
        {
            string msgType = MsgType.ToString().ToLower();
            string _str = "";

            _str += "<xml>";

            _str += "<ToUserName><![CDATA[" + ToUserName + "]]></ToUserName>";
            _str += "<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>";
            _str += "<CreateTime>" + CreateTime + "</CreateTime>";
            _str += "<MsgType><![CDATA[" + msgType + "]]></MsgType>";
            _str += "<Content><![CDATA[" + Content + "]]></Content>";
            _str += "<FuncFlag>0</FuncFlag>";

            _str += "</xml>";

            return _str;
        }

        public override string GetWeixinResJson()
        {
            JsonData data = new JsonData();
           
            data["touser"] = this.ToUserName;
            var msgtype = this.MsgType.ToString().ToLower();
            data["msgtype"] = msgtype;

            data[msgtype] = new JsonData();
            data[msgtype]["content"] = this.Content;

            return data.ToJson(false);
        }
    }
}
