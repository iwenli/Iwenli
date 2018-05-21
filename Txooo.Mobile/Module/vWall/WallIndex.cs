using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Module.vWall
{
    public class WallIndex
    {
        public int WallId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int QrCode { get; set; }

        public int AccountId { get; set; }


        public static List<WallIndex> GetWallIndexByWhere(string where, Hashtable hashtable)
        {
            List<WallIndex> list = new List<WallIndex>();
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrandShop"))
            {
                string sql = "select * from [vwall_info] where 1=1 " + where;

                DataRowCollection rows = helper.SqlGetDataTable(sql, hashtable).Rows;

                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        list.Add(new WallIndex()
                        {
                            StartTime = Convert.ToDateTime(row["start_time"]),
                            EndTime = Convert.ToDateTime(row["end_time"]),
                            QrCode = Convert.ToInt32(row["qr_code"]),
                            AccountId = Convert.ToInt32(row["account_id"])
                        });
                    }
                }
                return list;
            }
        }
    }
}
