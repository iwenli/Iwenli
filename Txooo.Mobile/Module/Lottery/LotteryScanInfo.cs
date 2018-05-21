using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace Txooo.Mobile.Module
{


    /// <summary>
    /// 二维码抽奖参与者信息
    /// </summary>
    public class LotteryJoinInfo : DataRowEntityBase
    {
        #region 字段

        private long m_joinId;

        private string m_joinName;

        private string m_mobile;

        private int m_activityId;

        private int m_activityType;

        private string m_source;

        private string m_ip;

        private int m_joinMode;

        private string m_sn;

        private int m_code;

        private bool m_isWin;

        private DateTime m_addTime;

        private bool m_ischanged;

        private DateTime m_changeTime;

        private string m_prizeName;

        private int m_prizeId;

        #endregion

        #region 属性

        /// <summary>
        /// 自增标识
        /// </summary>
        public long JoinId
        {
            get { return m_joinId; }
            set { m_joinId = value; }
        }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string JoinName
        {
            get { return m_joinName; }
            set { m_joinName = value; }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value; }
        }

        /// <summary>
        /// 参加活动ID
        /// </summary>
        public int ActivityId
        {
            get { return m_activityId; }
            set { m_activityId = value; }
        }

        /// <summary>
        /// 活动类型
        /// </summary>
        public int ActivityType
        {
            get { return m_activityType; } 
            set { m_activityType = value; }
        }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source
        {
            get { return m_source; }
            set { m_source = value; }
        }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        /// <summary>
        /// 参与方式 1网页 2二维码
        /// </summary>
        public int JoinMode
        {
            get { return m_joinMode; }
            set { m_joinMode = value; }
        }

        /// <summary>
        /// 随机标识
        /// </summary>
        public string Sn
        {
            get { return m_sn; }
            set { m_sn = value; }
        }

        /// <summary>
        /// 所抽到的随机数
        /// </summary>
        public int Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        /// <summary>
        /// 是否中奖0未1中
        /// </summary>
        public bool IsWin
        {
            get { return m_isWin; }
            set { m_isWin = value; }
        }

        /// <summary>
        /// 参与时间
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }

        /// <summary>
        /// 是否兑奖0未1已
        /// </summary>
        public bool Ischanged
        {
            get { return m_ischanged; }
            set { m_ischanged = value; }
        }

        /// <summary>
        /// 兑奖时间
        /// </summary>
        public DateTime ChangeTime
        {
            get { return m_changeTime; }
            set { m_changeTime = value; }
        }

        /// <summary>
        /// 中奖物品名称
        /// </summary>
        public string PrizeName
        {
            get { return m_prizeName; }
            set { m_prizeName = value; }
        }

        /// <summary>
        /// 奖品id
        /// </summary>
        public int PrizeId
        {
            get { return m_prizeId; }
            set { m_prizeId = value; }
        }

        #endregion

        public static IList<LotteryJoinInfo> GetScanInfo(string where)
        {
            return Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<LotteryJoinInfo>("TxoooMobile", "View_PLATFORM_ZDL_JoinUser", where);
        }
    }
}
