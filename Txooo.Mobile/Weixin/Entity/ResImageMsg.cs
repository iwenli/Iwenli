using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Txooo.Mobile.Weixin.Entity
{

    /// <summary>
    /// 回复图文消息格式
    /// </summary>
    [XmlRootAttribute("xml", Namespace = "", IsNullable = false)]
    public class ResImageMsg
    {

        public ResImageMsg BuildMsg(ReqTextMsg reqTextMsg)
        {
            ToUserName = string.Format("<![CDATA[{0}]]>", reqTextMsg.FromUserName);
            FromUserName = string.Format("<![CDATA[{0}]]>", reqTextMsg.ToUserName);
            Content = string.Format("<![CDATA[]]>");
            CreateTime = WeixinHelper.GetDatetimeNowString();
            MsgType = string.Format("<![CDATA[{0}]]>", "news");
            FuncFlag = "1";
            ArticleCount = 0;
            Articles = new List<Article>();
            return this;
        }

        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName { set; get; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { set; get; }
        /// <summary>
        /// 消息创建时间 
        /// </summary>
        public string CreateTime { set; get; }
        /// <summary>
        /// 消息类型，图文消息必须填写news
        /// </summary>
        public string MsgType { set; get; }
        /// <summary>
        /// 消息内容，图文消息可填空，大小限制在2048字节，字段为空为不合法请求 
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 图文消息个数，限制为10条以内
        /// </summary>
        public int ArticleCount { set; get; }

        public string FuncFlag { set; get; }

        public List<Article> Articles { set; get; }
    }

    [XmlRootAttribute("item", Namespace = "", IsNullable = false)]
    public class Article
    {
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
    }

}
