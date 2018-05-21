using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Module.Catering
{
    /// <summary>
    /// 餐饮桌位二维码
    /// </summary>
    public class CateringTableCodeInfo
    {
        //table_qrcode_id, brand_id, outlet_id, table_id, qr_code, scan_count, qr_num, account_id, add_time
        public int TableQrCodeId { get; set; }

        public int BrandId { get; set; }

        public int OutletId { get; set; }

        public int TableId { get; set; }

        public string QrCode { get; set; }

        public int ScanCount { get; set; }

        public int QrNum { get; set; }

        public int AccountId { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 根据二维码数字获取桌位信息和门店信息
        /// </summary>
        public static CateringTableCodeInfo GetCateringInfoByQrNumber(int qrNumber, int accountId)
        {
            CateringTableCodeInfo info = null;
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql = "select table_qrcode_id, brand_id, outlet_id, table_id, qr_code, scan_count, qr_num, account_id, add_time from [catering_table_qrcode] where qr_num=@QrNum and account_id=@AccountId";

                helper.SpFileValue["@QrNum"] = qrNumber;
                helper.SpFileValue["@AccountId"] = accountId;

                DataTable table = helper.SqlGetDataTable(sql, helper.SpFileValue);
                if (table.Rows.Count > 0)
                {
                    info = new CateringTableCodeInfo();
                    DataRow row = table.Rows[0];
                    info.TableQrCodeId = Convert.ToInt32(row["table_qrcode_id"]);
                    info.BrandId = Convert.ToInt32(row["brand_id"]);
                    info.OutletId = Convert.ToInt32(row["outlet_id"]);
                    info.TableId = Convert.ToInt32(row["table_id"]);
                    info.QrCode = Convert.ToString(row["qr_code"]);
                }
                return info;
            }
        }

        /// <summary>
        /// 更新扫描数
        /// </summary>
        public static bool UpdateScanCount(int codeId)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string _sql = "update [catering_table_qrcode] set scan_count=scan_count+1 where table_qrcode_id=@CodeId";

                helper.SpFileValue["@CodeId"] = codeId;

                return helper.SqlExecute(_sql, helper.SpFileValue) > 0;
            }
        }
    }

    /// <summary>
    /// 桌位二维码图文信息
    /// </summary>
    public class ImgTxtInfo
    {
        //id, brand_id, outlet_id, title, pic, description, add_time
        public int Id { get; set; }

        public int BrandId { get; set; }

        public int OutletId { get; set; }

        public string Title { get; set; }

        public string Pic { get; set; }

        public string Description { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 根据门店id获取图文信息
        /// </summary>
        public static ImgTxtInfo GetImgTxtInfoByOutletId(int outletId)
        {
            ImgTxtInfo _info = null;
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string _sql =
                    "select id, brand_id, outlet_id, title, pic, description, add_time from [catering_imgtxt_setting] where outlet_id=@OutletId";

                helper.SpFileValue["@OutletId"] = outletId;

                DataRowCollection rows = helper.SqlGetDataTable(_sql, helper.SpFileValue).Rows;

                if (rows.Count > 0)
                {
                    DataRow _row = rows[0];

                    _info = new ImgTxtInfo()
                    {
                        Title = _row["title"].ToString(),
                        Pic = _row["pic"].ToString(),
                        Description = _row["description"].ToString()
                    };
                }
                return _info;
            }
        }
    }
}
