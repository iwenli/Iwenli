using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web.Security
{
    public class User : Principal
    {
        public User(string username)
        {
            _identity = new Identity(username);
        }

        public override bool IsBrowseUrl(Url url)
        {
            return false;
        }

        public override bool IsInWlGroup(string role)
        {
            return false;
        }

        public override bool IsInWlPermission(string permission)
        {
            return false;
        }

        public override bool IsInWlRole(string role)
        {
            return false;
        }

        public override bool IsInWlService(string service)
        {
            return false;
        }
    }
}
