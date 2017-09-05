using Iwenli.Extension;
using Iwenli.Org.Passport.BLL;
using Iwenli.Org.Passport.DAL;
using Iwenli.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Org.Passport.Ajax
{
    public class UserAjax : SiteAjaxBase
    {
        UserBLL _userBll = new UserBLL();

        public string Login(HttpContext context)
        {
            var _form = context.Request.Form;
            string _userName = _form["userName"];
            string _passWord = EncryptHelper.MD5(_form["passWord"]);
            int _userId = UserDAL.Login(_userName, _passWord);
            if (_userId > 0)
            {
                return _userBll.Login(_userName, _passWord);
            }
            return ToolHelper.Json(false, "用户名或密码错误");
        }
    }
}
