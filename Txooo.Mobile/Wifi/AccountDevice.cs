using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Wifi
{   
        public class AccountDevice
        {
            #region 基本信息

            int m_deviceId;
            /// <summary>
            /// 驱动ＩＤ
            /// </summary>
            public int DeviceId
            {
                get { return m_deviceId; }
                set { m_deviceId = value; }
            }
            string m_authKey;
            /// <summary>
            /// 授权码
            /// </summary>
            public string AuthKey
            {
                get { return m_authKey; }
                set { m_authKey = value; }
            }
            int m_brandId;
            /// <summary>
            /// 品牌ＩＤ
            /// </summary>
            public int BrandId
            {
                get { return m_brandId; }
                set { m_brandId = value; }
            }
            long m_accountId;
             /// <summary>
             /// 账号ID
             /// </summary>
            public long AccountId
            {
                get { return m_accountId; }
                set { m_accountId = value; }
            }
            #endregion

            #region SSID信息

            string m_wifiSsid;

            public string WifiSsid
            {
                get { return m_wifiSsid; }
                set { m_wifiSsid = value; }
            }
            string m_wifiKey;

            public string WifiKey
            {
                get { return m_wifiKey; }
                set { m_wifiKey = value; }
            }
            string m_freeSsid;

            public string FreeSsid
            {
                get { return m_freeSsid; }
                set { m_freeSsid = value; }
            }

            #endregion

            #region 获取驱动信息          
            /// <summary>
            /// 根据账号id获取设备信息
            /// </summary>
            /// <param name="accountId"></param>
            /// <returns></returns>
            public static AccountDevice GetDeviceInfoByAccountId(long accountId)
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string _sql = "SELECT [wifi_device_id],[brand_id],[account_id],[auth_key],[wifi_ssid],[wifi_key],[free_ssid] FROM [TxoooMobile].[dbo].[Wifi_Device] WHERE [account_id]=" + accountId;
                    DataTable _table = helper.SqlGetDataTable(_sql);
                    if (_table.Rows.Count == 1)
                    {
                        AccountDevice _device = new AccountDevice();
                        _device.DeviceId = int.Parse(_table.Rows[0]["wifi_device_id"].ToString());
                        _device.BrandId = int.Parse(_table.Rows[0]["brand_id"].ToString());
                        _device.AccountId = long.Parse(_table.Rows[0]["account_id"].ToString());
                        _device.AuthKey = _table.Rows[0]["auth_key"].ToString();
                        _device.FreeSsid = _table.Rows[0]["free_ssid"].ToString();
                        _device.WifiSsid = _device.FreeSsid + "_PRI";// _table.Rows[0]["wifi_ssid"].ToString();
                        _device.WifiKey = _table.Rows[0]["wifi_key"].ToString();

                        return _device;
                    }
                }
                return null;
            }

            #endregion

    }
}
