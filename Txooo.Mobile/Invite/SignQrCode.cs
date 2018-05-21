using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Invite
{
    public class SignQrCode
    {
        //sign_qr_mapid, sign_id, qr_url, qr_number, brand_id, add_time, qr_type
        public int SignQrMapId { get; set; }
        public int SignId { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string QrUrl { get; set; }

        public int QrNumber { get; set; }

        public int BrandId { get; set; }

        public DateTime AddTime { get; set; }

        public int QrType { get; set; }

        /// <summary>
        /// 根据二维码id拿到签到二维码信息
        /// </summary>
        public static SignQrCode GetSignQrCodeInfoByQrNumber(int qrNumber,long accountId)
        {
            SignQrCode info = null;
            using (TxDataHelper helper = TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql =
                    "select sign_id, account_id, account_name, qr_url, brand_id, add_time, qr_type from [Invite_Sign_Qr_Map] where qr_number=@QrNumber and account_id=@AccountId";
                helper.SpFileValue["@QrNumber"] = qrNumber;
                helper.SpFileValue["@AccountId"] = accountId;

                DataRowCollection rows = helper.SqlGetDataTable(sql, helper.SpFileValue).Rows;
                if (rows.Count > 0)
                {
                    DataRow row = rows[0];
                    info = new SignQrCode()
                    {
                        SignId = Convert.ToInt32(row["sign_id"]),
                        AccountId = Convert.ToInt32(row["account_id"]),
                        AccountName = row["account_name"].ToString(),
                        QrUrl = row["qr_url"].ToString(),
                        BrandId = Convert.ToInt32(row["brand_id"]),
                        AddTime = Convert.ToDateTime(row["add_time"]),
                        QrType = Convert.ToInt32(row["qr_type"])
                    };
                }
                return info;
            }
        }
    }
}
