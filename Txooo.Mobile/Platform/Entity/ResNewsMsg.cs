using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Platform
{
    /// <summary>
    /// 回复资讯文章类消息
    /// </summary>
    public class ResNewsMsg : ResMsg 
    {        
        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount { set; get; }

        /// <summary>
        /// 文章数据
        /// </summary>
        public List<ResArticle> Articles { set; get; }

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        public ResNewsMsg(string toUserName)
        {
            MsgType = ResMsgType.News;
            ToUserName = toUserName;
            CreateTime = CommonFunction.GetDatetimeNowString();
            FuncFlag = "0";
            Articles = new List<ResArticle>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reqmsg"></param>
        public ResNewsMsg(ReqMsg reqmsg)
        {
            Platform = reqmsg.Platform;
            ToUserName = reqmsg.FromUserName;
            FromUserName = reqmsg.ToUserName;
            CreateTime = CommonFunction.GetDatetimeNowString();
            MsgType = ResMsgType.News;
            FuncFlag = "0";

            Articles = new List<ResArticle>();
        }

        #endregion

        public override string GetWeixinResXml()
        {

            string _str = "";

            _str += "<xml>";

            _str += "<ToUserName><![CDATA[" + ToUserName + "]]></ToUserName>";
            _str += "<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>";
            _str += "<CreateTime>" + CreateTime + "</CreateTime>";
            _str += "<MsgType><![CDATA[" + MsgType.ToString().ToLower() + "]]></MsgType>";
            _str += "<ArticleCount>" + Articles.Count + "</ArticleCount>";

            _str += "<Articles>";
            foreach (var item in Articles)
            {
                _str += "<item>";

                _str += "<Title><![CDATA[" + item.Title + "]]></Title>";
                _str += "<Description><![CDATA[" + item.Discription + "]]></Description>";
                _str += "<PicUrl><![CDATA[" + item.PicUrl + "]]></PicUrl>";
                _str += "<Url><![CDATA[" + item.Url + "]]></Url>";
                _str += "</item>";
            }
            _str += "</Articles>";
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

            data[msgtype]["articles"] = new JsonData();
            
            JsonData articles;
            foreach (ResArticle item in this.Articles)
            {
                articles = new JsonData();
                articles["title"] = item.Title;
                articles["description"] = item.Discription;
                articles["url"] = string.IsNullOrEmpty(item.Url) ? "http://w.txooo.com/showmsg.html?material=" + item.ID : item.Url;
                articles["picurl"] = item.PicUrl;
                data[msgtype]["articles"].Add(articles);
            }
            return data.ToJson(false);
        }
    }

    /// <summary>
    /// 回复资讯文章类消息实体
    /// </summary>
    public struct  ResArticle
    {
        public string MediaId { get; set; }
        /// <summary>
        /// 图文ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 图文消息标题 
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 图文消息描述 
        /// </summary>
        public string Discription { set; get; }
        /// <summary>
        /// 图片链接(绝对地址)，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80，限制图片链接的域名需要与开发者填写的基本资料中的Url一致 
        /// </summary>
        public string PicUrl { set; get; }
        /// <summary>
        /// 点击图文消息跳转链接 (绝对地址)
        /// </summary>
        public string Url { set; get; }

        public string GetUploadJson()
        {            
            JsonData data = new JsonData();
            data["thumb_media_id"] = this.MediaId;
            data["title"] = this.Title;
            data["content_source_url"] = this.Url;
            data["content"] = this.Discription;
            data["digest"] = this.Discription;
            data["show_cover_pic"] = "1";

            return data.ToJson(false);
        }
    }
}
