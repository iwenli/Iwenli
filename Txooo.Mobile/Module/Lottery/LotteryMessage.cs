//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Txooo.Data.Entity;

//namespace Txooo.Mobile.Module
//{
//    public class LotteryMessage : DataRowEntityBase
//    {
//        #region 字段

//        private int m_msgId;

//        private int m_activityId;

//        private int m_msgType;

//        private string m_msgContent;

//        private DateTime m_addTime;

//        private int m_activityType;

//        #endregion

//        #region 属性

//        /// <summary>
//        /// 自增标识
//        /// </summary>
//        public int MsgId
//        {
//            get { return m_msgId; }
//            set { m_msgId = value; }
//        }

//        /// <summary>
//        /// 活动id
//        /// </summary>
//        public int ActivityId
//        {
//            get { return m_activityId; } 
//            set { m_activityId = value; }
//        }

//        /// <summary>
//        /// 0未开始1未中奖2已中奖3已过期
//        /// </summary>
//        public int MsgType
//        {
//            get { return m_msgType; }
//            set { m_msgType = value; }
//        }

//        /// <summary>
//        /// 消息内容
//        /// </summary>
//        public string MsgContent
//        {
//            get { return m_msgContent; }
//            set { m_msgContent = value; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public DateTime AddTime
//        {
//            get { return m_addTime; }
//            set { m_addTime = value; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public int ActivityType
//        {
//            get { return m_activityType; }
//            set { m_activityType = value; }
//        }

//        #endregion


//        public static List<LotteryMessage> GetMessageList(string where)
//        {
//            List<LotteryMessage> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<LotteryMessage>("TxoooMobile", "View_PLATFORM_LYY_ActivityMessage", where);
//            if (_list.Count != 0)
//            {
//                return _list;
//            }
//            return null;
//        }
//    }
//}
