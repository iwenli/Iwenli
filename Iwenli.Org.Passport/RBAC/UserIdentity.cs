/*----------------------------------------------------------------
 *  Copyright (C) 2017 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：UserIdentity
 *  所属项目：
 *  创建用户：张玉龙(HouWeiya)
 *  创建时间：2018/4/11 13:57:13
 *  
 *  功能描述：
 *          1、
 *          2、 
 * 
 *  修改标识：  
 *  修改描述：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/

using Iwenli.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.Passport.RBAC
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
