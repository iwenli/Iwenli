#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：User
 *  所属项目：Iwenli.Web.Authorization
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 8:57:53
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using Iwenli.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web.Authorization
{
    /// <summary>
    /// 公共认证用户模型
    /// uName|uId|isAutoLoad|预留
    /// </summary>
    public class User : Principal
    {
        public User(string username)
        {
            _identity = new UserIdentity(username);
        }

        /// <summary>
        /// 目录认证
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override bool IsBrowseUrl(Url url)
        {
            return true;
        }

        /// <summary>
        /// 部门认证
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override bool IsInWlGroup(string role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public override bool IsInWlPermission(string permission)
        {
            return true;
        }

        /// <summary>
        /// 角色认证
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public override bool IsInWlRole(string role)
        {
            return true;
        }

        /// <summary>
        /// 服务认证
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public override bool IsInWlService(string service)
        {
            return true;
        }
    }
}
