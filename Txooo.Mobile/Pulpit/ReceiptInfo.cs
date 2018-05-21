using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace Txooo.Mobile.Pulpit
{
    /// <summary>
    /// 回执信息
    /// </summary>
    public  class ReceiptInfo : Txooo.Data.Entity.DataRowEntityBase
    {
        long m_receiptId;
        [DataRowEntityFile("receipt_id", typeof(string))]
        public long ReceiptId
        {
            get { return m_receiptId; }
            set { m_receiptId = value; }
        }
        string m_openId;
        [DataRowEntityFile("openId", typeof(string))]
        public string OpenId
        {
            get { return m_openId; }
            set { m_openId = value; }
        }
        string m_platform;
        [DataRowEntityFile("platform", typeof(string))]
        public string Platform
        {
            get { return m_platform; }
            set { m_platform = value; }
        }
        long m_employeeId;
        [DataRowEntityFile("employee_id", typeof(string))]
        public long EmployeeId
        {
            get { return m_employeeId; }
            set { m_employeeId = value; }
        }
        string m_employeeName;
        [DataRowEntityFile("employee_name", typeof(string))]
        public string EmployeeName
        {
            get { return m_employeeName; }
            set { m_employeeName = value; }
        }
        long m_brandId;
        [DataRowEntityFile("brand_id", typeof(string))]
        public long BrandId
        {
            get { return m_brandId; }
            set { m_brandId = value; }
        }
        string m_brandName;
        [DataRowEntityFile("brand_name", typeof(string))]
        public string BrandName
        {
            get { return m_brandName; }
            set { m_brandName = value; }
        }
        string m_name;
        [DataRowEntityFile("name", typeof(string))]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        string m_mobile;
        [DataRowEntityFile("mobile", typeof(string))]
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value; }
        }
        string m_post;
        [DataRowEntityFile("post", typeof(string))]
        public string Post
        {
            get { return m_post; }
            set { m_post = value; }
        }

        public static ReceiptInfo GetReceiptInfoById(string id)
        {
            string _sql = "SELECT * FROM [Tx_Receipt] WHERE [receipt_id]='" + id + "'";
            List<ReceiptInfo> _list = DataEntityHelper.GetDataRowEntityList<ReceiptInfo>("TxoooMobile", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        public static ReceiptInfo GetReceiptInfoByOpenId(PlatformType platform, string sn)
        {
            string _sql = "SELECT * FROM [Tx_Receipt] WHERE [openId]='" + sn + "'";
            List<ReceiptInfo> _list = DataEntityHelper.GetDataRowEntityList<ReceiptInfo>("TxoooMobile", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }
    }
}
