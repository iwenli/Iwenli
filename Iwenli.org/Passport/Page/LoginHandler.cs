using Iwenli.Org.RBAC;
using Iwenli.Web.Htmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Iwenli.Org.Passport.Page
{
    class LoginHandler : HtmxHandler1
    {

        public override void LoadMainTemplateBegin()
        {
            #region 返回地址

            string _returnUrl = Request.QueryString["ReturnUrl"]?? "http://iwenli.org";
            AddTempVariable("ReturnUrl", _returnUrl);

            #endregion

            #region 已登陆状态则返回
            var _user = HttpContext.Current.User.Identity as UserIdentity;
            if (_user != null)
            {
                Response.Redirect(_returnUrl);
                Response.End();
            }
            #endregion

            #region 微信登陆
            if (Context.Request.UserAgent.ToLower().Contains("micromessenger"))
            {
                Response.Redirect("/wxlogin.html?ReturnUrl=" + Uri.EscapeDataString(_returnUrl));
                Response.End();
            }
            #endregion

            base.LoadMainTemplateBegin();
        }
    }
}
