/*----------------------------------------------------------------
 *  Copyright (C) 2017 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：User
 *  所属项目：
 *  创建用户：张玉龙(HouWeiya)
 *  创建时间：2018/4/11 13:57:06
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
    public class User : Principal
    {
        public User(string username)
        {
            _identity = new UserIdentity(username);
        }

        public override bool IsBrowseUrl(Iwenli.Web.Url url)
        {
            return true;
        }

        public override bool IsInWlPermission(string permission)
        {
            return true;
        }

        public override bool IsInWlRole(string role)
        {
            return true;
        }

        public override bool IsInWlService(string service)
        {
            return true;
        }

        public override bool IsInWlGroup(string role)
        {
            throw new NotImplementedException();
        }
    }
}
