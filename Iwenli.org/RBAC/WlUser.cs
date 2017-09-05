using Iwenli.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.RBAC
{
    public class WlUser : Principal
    {
        public WlUser(string username)
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
