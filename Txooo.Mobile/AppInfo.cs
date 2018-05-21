using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;
using Txooo.Mobile.Module;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 应用信息
    /// </summary>
    public class AppInfo : DataRowEntityBase
    {
        #region 字段

        private int m_appId;
        private int m_qrId;
        private int m_activityId;
        private int m_scanCount;
        private ActivityType m_activityType;
        private string m_materialContent;
        private DateTime m_addTime;
        private string m_appName;
        private string m_useIn;
        private long m_accountId;
        private string m_qrUrl;
        private int m_platform;
        private int m_qrNumber;
        private ResMsgType m_contentType;

        #endregion

        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int AppId
        {
            get { return m_appId; }
            set { m_appId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int QrId
        {
            get { return m_qrId; }
            set { m_qrId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ActivityId
        {
            get { return m_activityId; }
            set { m_activityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ScanCount
        {
            get { return m_scanCount; }
            set { m_scanCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActivityType ActivityType
        {
            get { return m_activityType; }
            set { m_activityType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialContent
        {
            get { return m_materialContent; }
            set { m_materialContent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AppName
        {
            get { return m_appName; }
            set { m_appName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UseIn
        {
            get { return m_useIn; }
            set { m_useIn = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get { return m_accountId; }
            set { m_accountId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string QrUrl
        {
            get { return m_qrUrl; }
            set { m_qrUrl = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Platform
        {
            get { return m_platform; }
            set { m_platform = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int QrNumber
        {
            get { return m_qrNumber; }
            set { m_qrNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ResMsgType ContentType
        {
            get { return m_contentType; }
            set { m_contentType = value; }
        }

        #endregion

        public ResMsg[] GetResMsg(ReqMsg msg)
        { 
            ResMsg _msg = null;

            switch (this.ActivityType)
            {
                case ActivityType.QRLOTTERY:
                    var lotteryInfo = LotteryInfo.GetLotteryListByActivityID(" AND ActivityID=" + this.ActivityId);
                    return new QrLottery(msg, lotteryInfo).GetResMsg();

                case ActivityType.BIGDISC: 
                case ActivityType.FIGHT:
                case ActivityType.GUAGUA:
                case ActivityType.LUCKY:
                    _msg = GetNewsByActivityID(msg, this.ActivityType);
                    break;
                     
                case ActivityType.MATERIAL:
                    _msg = GetNewsByMaterial(msg);
                    break;

                default:
                    break;
            }

            //ResNewsMsg _msg = new ResNewsMsg(msg.FromUserName);
            //ResArticle article = new ResArticle();
            //article.Title = this.ActivityName;
            //article.PicUrl = this.Pic;
            //article.Url = this.Link;
            //article.Discription = Explain;
            //_msg.Articles.Add(article);

            return new ResMsg[] { _msg };
        }

        /// <summary>
        /// 根据素材图文消息
        /// </summary>
        /// <returns></returns>
        private ResMsg GetNewsByMaterial(ReqMsg msg)
        {
            switch (this.ContentType)
            {
                case ResMsgType.News:
                    KeywordReply reply = new KeywordReply();
                    reply.ContentType = 4;
                    reply.ReplyContent = this.MaterialContent;
                    var _newsMsg = reply.GetArticles();
                    _newsMsg.ToUserName = msg.FromUserName;
                    return _newsMsg;
                case ResMsgType.Text:
                    ResTextMsg _msg = new ResTextMsg(msg.FromUserName);
                    _msg.Content = this.MaterialContent;
                    return _msg;
                case ResMsgType.Image:
                    ResImageMsg _img = new ResImageMsg(msg.FromUserName);
                    _img.LocalUrl = this.MaterialContent;
                    return _img;
                default:
                    return null;
            }
           
        }

        /// <summary>
        /// 根据活动ID获取图文消息
        /// </summary>
        /// <returns></returns>
        private ResMsg GetNewsByActivityID(ReqMsg msg,ActivityType activityType)
        {
            string _table = string.Format("Activity_{0}", this.ActivityType.ToString());
            DataTable _dt = null;
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                string _sql = "SELECT * FROM " + _table + " WHERE activity_id=" + this.ActivityId;
                _dt = helper.SqlGetDataTable(_sql);
            }
            if (_dt != null && _dt.Rows.Count > 0)
            {                
                ResNewsMsg _msg = new ResNewsMsg(msg.FromUserName);
                ResArticle article = new ResArticle();
                article.Title = _dt.Rows[0]["activity_name"].ToString();
                article.PicUrl = string.Format("http://b.txooo.com/skin/Brand/Activity/default/msg/{0}.jpg", activityType.ToString().ToLower());
                article.Url = _dt.Rows[0]["link"].ToString();
                article.Discription = _dt.Rows[0]["explain"].ToString();
                _msg.Articles.Add(article);
                return _msg;
            }
            return null;
        }

        public void UpdateScanInfo(ReqMsg msg)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                //获取用户ID
                string _sql = "UPDATE [dbo].[Qr_App_Map] SET [scan_count] = [scan_count]+1 WHERE app_id=" + this.AppId;
                helper.SqlExecute(_sql);
            }
        }

        public static List<AppInfo> GetActivityListByAccount(string where)
        {
            List<AppInfo> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<AppInfo>("TxoooMobile", "View_PLATFORM_ZDL_AppView", where);
            if (_list.Count != 0)
            {
                return _list;
            }
            return null;
        }
        
    }


   
        /// <summary>
    /// 活动类型（必须大写)
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// 刮刮卡
        /// </summary>
        GUAGUA = 1,
        /// <summary>
        /// 幸运机
        /// </summary>
        LUCKY = 2,
        /// <summary>
        /// 大转盘
        /// </summary>
        BIGDISC = 3,
        /// <summary>
        /// 一战到底
        /// </summary>
        FIGHT = 4,
        /// <summary>
        /// 二维码抽奖
        /// </summary>
        QRLOTTERY = 5,
        /// <summary>
        /// 素材
        /// </summary>
        MATERIAL=6,
        /// <summary>
        /// 优惠券
        /// </summary>
        COUPON = 10,
        /// <summary>
        /// 会员卡
        /// </summary>
        MEMBERCARD = 11,
    }
}
