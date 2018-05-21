using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace Txooo.Mobile.Module
{

    /// <summary>
    /// 二维码抽奖奖品信息
    /// </summary>
    public class LotteryPrizeInfo : DataRowEntityBase
    {
        #region 字段

        private int m_prizeId;
        private int m_activityType;
        private int m_activityId;
        private string m_prizeName;
        private double m_prizeMoney;
        private int m_prizeCount; 
        private int m_remainCount;
        private string m_prizePic;
        private string m_awardName;
        private double m_rate;
        private DateTime m_addTime;
        private int m_setDrawState;
        private int m_winWayState;
        private long m_prizeShow;
        private int m_rateState;
        private string m_prizeTime;
        private string m_prizeOrder;
        private int m_orderState;

        #endregion

        #region 属性

        /// <summary>
        /// 奖品自增id
        /// </summary>
        public int PrizeId
        {
            get { return m_prizeId; }
            set { m_prizeId = value; }
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
        /// 所属活动ID
        /// </summary>
        public int ActivityId
        {
            get { return m_activityId; }
            set { m_activityId = value; }
        }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName
        {
            get { return m_prizeName; }
            set { m_prizeName = value; }
        }

        /// <summary>
        /// 产品单价
        /// </summary>
        public double PrizeMoney
        {
            get { return m_prizeMoney; }
            set { m_prizeMoney = value; }
        }

        /// <summary>
        /// 产品数量
        /// </summary>
        public int PrizeCount
        {
            get { return m_prizeCount; }
            set { m_prizeCount = value; }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RemainCount
        {
            get { return m_remainCount; }
            set { m_remainCount = value; }
        }

        /// <summary>
        /// 奖品图片
        /// </summary>
        public string PrizePic
        {
            get { return m_prizePic; }
            set { m_prizePic = value; }
        }

        /// <summary>
        /// 奖项名称
        /// </summary>
        public string AwardName
        {
            get { return m_awardName; }
            set { m_awardName = value; }
        }

        /// <summary>
        /// 中奖率
        /// </summary>
        public double Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }

        /// <summary>
        /// 奖项设置:1,按全部，2，按单个
        /// </summary>
        public int SetDrawState
        {
            get { return m_setDrawState; }
            set { m_setDrawState = value; }
        }

        /// <summary>
        /// 中奖方式，1，自动设置， 2，按比率，3，按时间,4,顺序
        /// </summary>
        public int WinWayState
        {
            get { return m_winWayState; }
            set { m_winWayState = value; }
        }

        /// <summary>
        /// 奖品数量是否显示（1：不显示；0：显示）
        /// </summary>
        public long PrizeShow
        {
            get { return m_prizeShow; }
            set { m_prizeShow = value; }
        }

        /// <summary>
        /// 1,中奖率，2， 中奖个数
        /// </summary>
        public int RateState
        {
            get { return m_rateState; }
            set { m_rateState = value; }
        }

        /// <summary>
        /// 按时间分奖json
        /// </summary>
        public string PrizeTime
        {
            get { return m_prizeTime; }
            set { m_prizeTime = value; }
        }      

        /// <summary>
        /// 中奖顺序
        /// </summary>
        public string PrizeOrder
        {
            get { return m_prizeOrder; }
            set { m_prizeOrder = value; }
        }

        /// <summary>
        /// 顺序状态
        /// </summary>
        public int OrderState
        {
            get { return m_orderState; }
            set { m_orderState = value; }
        }

        #endregion               

        public static List<LotteryPrizeInfo> GetPrizeListByLotteryID(string where)
        {
            List<LotteryPrizeInfo> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<LotteryPrizeInfo>("TxoooMobile", "View_PLATFORM_ZDL_PrizeInfo", where);
            if (_list.Count != 0)
            {
                return _list;
            }
            return null;
        }
    }
}
