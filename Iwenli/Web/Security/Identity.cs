using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web.Security
{
    /// <summary>
    /// 用户标识
    /// </summary>
    public class Identity : MarshalByRefObject, IIdentity
    {
        protected string _name;
        protected bool _isAuthenticated;

        public Identity(string username)
        {
            _name = username;
            _isAuthenticated = true;
        }

        #region IIdentity 成员

        /// <summary>
        /// 获取所使用的身份验证的类型，接口成员
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return "RBAC认证类型！";
            }
        }

        /// <summary>
        /// 否验证了用户，接口成员
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }

        /// <summary>
        /// 用户名,接口成员
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        #endregion
    }
}
