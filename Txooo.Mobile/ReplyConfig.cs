using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 回复配置
    /// </summary>
    public class ReplyConfig : Txooo.Data.Entity.DataRowEntityBase
    {
        /// <summary>
        /// 构造函数，提取回复配置数据
        /// </summary>
        /// <param name="replyId"></param>
        public ReplyConfig(long replyId)
        {

        }

        /// <summary>
        /// 构造函数，提取回复配置数据
        /// </summary>
        /// <param name="replyId"></param>
        public ReplyConfig()
        {

        }

        /// <summary>
        /// 订阅回复
        /// </summary>
        public ResMsg SubResMsg { get; set; }
        /// <summary>
        /// 默认回复
        /// </summary>
        public ResMsg DefaultResMsg { get; set; }

        /// <summary>
        /// 关键字规则
        /// </summary>
        private KeywordReply[] KeywordResMsg { get; set; }

        /// <summary>
        /// 根据信息匹配关键词回复内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Platform.ResMsg[] GetResMsg(string message)
        {
            IList<ResMsg> listResMsg = new List<ResMsg>();
            foreach (var reply in KeywordResMsg)
            {
                var words = reply.Keywords.Split(',');
                foreach (var w in words)
                {
                    var matchWord = w.Split('|')[0];
                    var matchtype = w.Split('|')[1];
                    if (matchtype == "0")
                    {
                        if (message.Contains(matchWord))
                        {
                            if (reply.ContentType == 0)
                            {
                                ResTextMsg msg = new ResTextMsg("");
                                msg.Content = reply.ReplyContent;
                                listResMsg.Add(msg);
                                break;
                            }
                            if (reply.ContentType == 4)
                            {                               
                                listResMsg.Add(reply.GetArticles());
                                break;
                            }
                        }
                    }
                    if (matchtype == "1")
                    {
                        if (message == matchWord)
                        {
                            if (reply.ContentType == 0)
                            {
                                ResTextMsg msg = new ResTextMsg("");
                                msg.Content = reply.ReplyContent;
                                listResMsg.Add(msg);
                                break;
                            }
                            if (reply.ContentType == 4)
                            {
                                listResMsg.Add(reply.GetArticles());
                                break;
                            }


                        }
                    }
                }
            }
            return listResMsg.ToArray();
        }




        #region 工厂

        /// <summary>
        /// 获取回复模板配置（缓存，再实现）
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public static ReplyConfig GetReplyConfigByIdFromCache(long tempId)
        {
            return GetReplyConfigByIdFromDatabase(tempId);
        }

        private static object m_lock = new object();
        /// <summary>
        /// 获取回复模板配置(数据)
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public static ReplyConfig GetReplyConfigByIdFromDatabase(long tempId)
        {
            ReplyConfig replyConfig = null;
            DataTable dt;
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                helper.SpFileValue["@tempid"] = tempId;
                dt = helper.SpGetDataTable("SP_Service_ZDL_GetReplyInfo");
            }
            if (dt.Rows.Count > 0)
            {
                replyConfig = new ReplyConfig();
                var defaultDr = dt.Select("reply_type=1")[0];
                var subReplyDr = dt.Select("reply_type=0")[0];
                var keywordReplyDr = dt.Select("reply_type=2")[0];

                int defaultContent_type = Convert.ToInt32(defaultDr["content_type"]);
                int subContent_type = Convert.ToInt32(subReplyDr["content_type"]);

                #region 设置默认回复
                switch ((ResMsgType)defaultContent_type)
                {
                    case ResMsgType.Text:
                        var textMsg = new ResTextMsg("");
                        textMsg.Content = defaultDr["reply_content"].ToString();
                        replyConfig.DefaultResMsg = textMsg;
                        break;
                    case ResMsgType.Image:
                        var imgMsg = new ResImageMsg("");
                        imgMsg.LocalUrl = defaultDr["reply_content"].ToString();
                        replyConfig.DefaultResMsg = imgMsg;
                        break;
                    case ResMsgType.Voice:
                        break;
                    case ResMsgType.Video:
                        break;
                    case ResMsgType.News:
                        KeywordReply reply = new KeywordReply();
                        reply.ContentType = 4;
                        reply.ReplyContent = defaultDr["reply_content"].ToString(); ;
                        var _newsMsg = reply.GetArticles();
                        replyConfig.DefaultResMsg = _newsMsg;
                        break;
                    default:
                        break;
                }
                #endregion

                #region 设置关注回复

                switch ((ResMsgType)subContent_type)
                {
                    case ResMsgType.Text:
                        var subtextMsg = new ResTextMsg("");
                        subtextMsg.Content = subReplyDr["reply_content"].ToString();
                        replyConfig.SubResMsg = subtextMsg;
                        break;
                    case ResMsgType.Image:
                        var imgMsg = new ResImageMsg("");
                        imgMsg.LocalUrl = subReplyDr["reply_content"].ToString();
                        replyConfig.SubResMsg = imgMsg;
                        break;
                    case ResMsgType.Voice:
                        break;
                    case ResMsgType.Video:
                        break;
                    case ResMsgType.News:
                        KeywordReply reply = new KeywordReply();
                        reply.ContentType = 4;
                        reply.ReplyContent = subReplyDr["reply_content"].ToString(); ;
                        var _newsMsg = reply.GetArticles();
                        replyConfig.SubResMsg = _newsMsg;
                        break;
                    default:
                        break;
                }

                #endregion

                #region 设置关键字回复
                replyConfig.KeywordResMsg = KeywordReply.GetKeywordReplyInfoByIdFromDatabase(Convert.ToInt64(keywordReplyDr["reply_id"]));
                #endregion

            }
            return replyConfig;

        }

        #endregion
    }

    /// <summary>
    /// 关键字回复实体
    /// </summary>
    public class KeywordReply : Txooo.Data.Entity.DataRowEntityBase
    {
        public string ReplyContent { get; set; }

        public int ContentType { get; set; }

        public string Keywords { get; set; }

        public ResNewsMsg GetArticles()
        {            
            if (this.ContentType == 4)
            {
                DataTable dt = null;
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string sql = " SELECT * FROM View_PLATFORM_LYY_MaterialTextMsg WHERE MaterialTextMessageId in (" + this.ReplyContent + ")";
                    dt = helper.SqlGetDataTable(sql);
                }

                ResNewsMsg msg = new ResNewsMsg("");
                ResArticle article;
                foreach (DataRow dr in dt.Rows)
                {
                    article = new ResArticle();
                    article.ID = Convert.ToInt32(dr["MaterialTextMessageId"]);
                    article.Title = dr["title"].ToString();
                    article.PicUrl = dr["HostAddr"].ToString();
                    article.Url = (Convert.IsDBNull(dr["OriginalLink"]) || dr["OriginalLink"].ToString() == string.Empty)
                        ? "http://w.txooo.com/showmsg.html?material=" + article.ID : dr["OriginalLink"].ToString();
                    article.Discription = dr["M_abstract"].ToString();
                    msg.Articles.Add(article);
                }
                return msg;
            }
            return null;
        }

        public static KeywordReply[] GetKeywordReplyInfoByIdFromDatabase(long id)
        {
            List<KeywordReply> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<KeywordReply>("TxoooMobile", "View_PLATFORM_ZDL_GetKeywordReply", "　AND [reply_id]=" + id);

            return _list.ToArray();
        }
    }
}
