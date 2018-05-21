using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo;
using Txooo.Data.Entity;

namespace Txooo.Mobile.OA
{
    /// <summary>
    /// 办公用户数据
    /// </summary>
    internal class OAUserInfo : Txooo.Data.Entity.DataRowEntityBase
    {
        long m_userId;

        [DataRowEntityFile("user_id", typeof(long))]
        public long UserId
        {
            get { return m_userId; }
            set { m_userId = value; }
        }

        string m_username;
        [DataRowEntityFile("username", typeof(string))]
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        string m_nickname;
        [DataRowEntityFile("nickname", typeof(string))]
        public string Nickname
        {
            get { return m_nickname; }
            set { m_nickname = value; }
        }
        string m_mobile;
        [DataRowEntityFile("mobile", typeof(string))]
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value; }
        }
        string m_weixinSn;
        [DataRowEntityFile("weixin_sn", typeof(string))]
        public string WeixinSn
        {
            get { return m_weixinSn; }
            set { m_weixinSn = value; }
        }
        string m_feixinSn;
        [DataRowEntityFile("feixin_sn", typeof(string))]
        public string FeixinSn
        {
            get { return m_feixinSn; }
            set { m_feixinSn = value; }
        }
        string m_yixinSn;
        [DataRowEntityFile("yixin_sn", typeof(string))]
        public string YixinSn
        {
            get { return m_yixinSn; }
            set { m_yixinSn = value; }
        }

        #region 工厂方法

        public static OAUserInfo GetUserInfoByPlatformUserSn(PlatformType platform, string userSn)
        {
            OAUserInfo _userInfo = new OAUserInfo();

            string _sql = "SELECT * FROM [View_Mobile_UserInfo] WHERE [weixin_sn]='" + userSn + "'";
            List<OAUserInfo> _list = DataEntityHelper.GetDataRowEntityList<OAUserInfo>("TxoooOffice", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        public static OAUserInfo GetUserInfoByUsername(string username)
        {
            OAUserInfo _userInfo = new OAUserInfo();

            string _sql = "SELECT * FROM [View_Mobile_UserInfo] WHERE [username]='" + username + "'";
            List<OAUserInfo> _list = DataEntityHelper.GetDataRowEntityList<OAUserInfo>("TxoooOffice", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        public static OAUserInfo GetUserInfoByUserId(long userid)
        {
            OAUserInfo _userInfo = new OAUserInfo();

            string _sql = "SELECT * FROM [View_Mobile_UserInfo] WHERE [user_id]='" + userid + "'";
            List<OAUserInfo> _list = DataEntityHelper.GetDataRowEntityList<OAUserInfo>("TxoooOffice", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        public static OAUserInfo GetUserInfoByMobile(string Mobile)
        {
            OAUserInfo _userInfo = new OAUserInfo();

            string _sql = "SELECT * FROM [View_Mobile_UserInfo] WHERE [Mobile]='" + Mobile + "'";
            List<OAUserInfo> _list = DataEntityHelper.GetDataRowEntityList<OAUserInfo>("TxoooOffice", _sql, "");
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        public static bool OAUserBinding(PlatformType platform, string platformUserSn, string mobile)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooOffice"))
            {
                string _sql = "UPDATE [TxoooOffice].[dbo].[Tx_Employee] SET [weixin_sn]='" + platformUserSn + "' WHERE [IsDimissory]=0 And [IsClocked]=0 AND [Mobile]='" + mobile + "'";
                if (helper.SqlExecute(_sql) > 0)
                {
                    return true;
                }
            }
            return true;
        }
        #endregion

    }
}
