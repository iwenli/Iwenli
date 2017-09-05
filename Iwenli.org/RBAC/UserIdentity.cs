using Iwenli.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.RBAC
{
    /// <summary>
    /// 用户身份信息
    /// </summary>
    public class UserIdentity : Identity
    {
        string[] _userInfo;

        public UserIdentity(string username) : base(username)
        {
            _userInfo = username.Split('|');
            _name = _userInfo[0];
        }

        /// <summary>
        /// 帐户名
        /// </summary>
        public string UserName
        {
            get
            {
                if (_userInfo == null)
                {
                    _userInfo = _name.Split('|');
                }
                string returnValue = null;
                if (_userInfo.Length >= 1)
                    returnValue = _userInfo[0];

                return returnValue;
            }
        }

        /// <summary>
        /// 帐户ID
        /// </summary>
        public int UserID
        {
            get
            {
                if (_userInfo == null)
                {
                    _userInfo = _name.Split('|');
                }
                int userId = 0;
                if (_userInfo.Length >= 2)
                    int.TryParse(_userInfo[1], out userId);

                return userId;
            }
        }

        /// <summary>
        /// 是否开启自动登陆
        /// </summary>
        public bool IsAutoLoad
        {
            get
            {
                if (_userInfo == null)
                {
                    _userInfo = _name.Split('|');
                }
                bool auto = false;
                if (_userInfo.Length >= 3)
                    auto = Convert.ToBoolean(int.Parse(_userInfo[2]));
                return auto;
            }
        }
    }
}
