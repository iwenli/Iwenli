using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace Txooo.Mobile.Module
{

    /// <summary>
    /// 二维码抽奖活动具体信息
    /// </summary>
    public class LotteryInfo : DataRowEntityBase
    {
        #region 字段

        private int m_activityId;
        private long m_brandId;
        private string m_activityName;
        private string m_tempId;
        private string m_keywords;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private string m_pic;
        private string m_explain;
        private string m_userIds;
        private DateTime m_addTime;
        private bool m_state;
        private int m_usemanageId;
        private int m_totalcount;
        private int m_countforoneday;
        private int m_mostawardcount;
        private bool m_isshowawardcount;     
        private string m_link;
        private int m_limitCount;

        #endregion

        #region 属性

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
        public long BrandId
        {
            get { return m_brandId; }
            set { m_brandId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ActivityName
        {
            get { return m_activityName; }
            set { m_activityName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TempId
        {
            get { return m_tempId; }
            set { m_tempId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Keywords
        {
            get { return m_keywords; }
            set { m_keywords = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            get { return m_startTime; }
            set { m_startTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            get { return m_endTime; }
            set { m_endTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pic
        {
            get { return m_pic; }
            set { m_pic = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Explain
        {
            get { return m_explain; }
            set { m_explain = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserIds
        {
            get { return m_userIds; }
            set { m_userIds = value; }
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
        public bool State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UsemanageId
        {
            get { return m_usemanageId; }
            set { m_usemanageId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Totalcount 
        {
            get { return m_totalcount; }
            set { m_totalcount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Countforoneday
        {
            get { return m_countforoneday; }
            set { m_countforoneday = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Mostawardcount
        {
            get { return m_mostawardcount; }
            set { m_mostawardcount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Isshowawardcount
        {
            get { return m_isshowawardcount; }
            set { m_isshowawardcount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Link
        {
            get { return m_link; }
            set { m_link = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LimitCount
        {
            get { return m_limitCount; }
            set { m_limitCount = value; }
        }

        #endregion

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static LotteryInfo GetLotteryListByActivityID(string where) 
        {
            List<LotteryInfo> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<LotteryInfo>("TxoooMobile", "View_PLATFORM_ZDL_LotteryInfo", where);
            if (_list.Count != 0)
            {
                return _list[0];
            }
            return null;
        }
    }
}
