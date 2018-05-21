using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Invite
{
    public class InviteReceipt
    {
        #region 字段

        private int m_receiptId;

        private string m_receiptName;

        private int m_inviteId;

        private string m_phone;

        private int m_joinCount;

        private int m_isNotify;

        private DateTime m_receiptTime;

        #endregion

        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int ReceiptId
        {
            get { return m_receiptId; }
            set { m_receiptId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ReceiptName
        {
            get { return m_receiptName; }
            set { m_receiptName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InviteId
        {
            get { return m_inviteId; }
            set { m_inviteId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            get { return m_phone; }
            set { m_phone = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int JoinCount
        {
            get { return m_joinCount; }
            set { m_joinCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int IsNotify
        {
            get { return m_isNotify; }
            set { m_isNotify = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReceiptTime
        {
            get { return m_receiptTime; }
            set { m_receiptTime = value; }
        }

        public DateTime SpecialAddTime { get; set; }

        public int IsReceipt { get; set; }

        public int IsSpecial { get; set; }

        public int AccountId { get; set; }

        public int BrandId { get; set; }

        public int QrNumber { get; set; }

        #endregion

        /// <summary>
        /// 根据qr_number获取回执人信息
        /// </summary>
        /// <returns></returns>
        public static InviteInfo GetReceiptInfoByQrNumber(long qrNumber,long accountId, out int receiptId)
        {
            receiptId = 0;
            InviteInfo info = null;
            using (TxDataHelper helper = TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql = "select receipt_id,invite_id from [Invite_Receipt] where qr_number=@QrNumber and account_id=@AccountId";
                helper.SpFileValue["@QrNumber"] = qrNumber;
                helper.SpFileValue["@AccountId"] = accountId;

                DataRowCollection rows = helper.SqlGetDataTable(sql, helper.SpFileValue).Rows;
                if (rows.Count > 0)
                {
                    receiptId = Convert.ToInt32(rows[0]["receipt_id"]);
                    int inviteId = Convert.ToInt32(rows[0]["invite_id"]);

                    info = InviteInfo.GetInviteInfoByInviteId(inviteId);
                }
                return info;
            }
        }
    }
}
