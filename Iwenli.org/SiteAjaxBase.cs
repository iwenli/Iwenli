using Iwenli.Extension;
using Iwenli.Org.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Iwenli.Org
{
    /// <summary>
    /// ajax请求处理基类
    /// </summary>
    public class SiteAjaxBase : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        protected UserIdentity UserIdentity
        {
            get
            {
                var _txUser = HttpContext.Current.User.Identity as UserIdentity;
                if (_txUser == null && HttpContext.Current.Request.Url.AbsolutePath.ToString().ToLower().IndexOf("/app/") > -1)
                {
                    string _userId = HttpContext.Current.Request.QueryString["userid"];
                    string _token = HttpContext.Current.Request.QueryString["token"];
                    if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_token))
                    {
                        return null;
                    }
                    _txUser = new UserIdentity("xx|" + _userId);
                }
                return _txUser;
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string methodName = context.Request.PathInfo.Replace("/", "");

            MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance
                    | BindingFlags.IgnoreCase
                    | BindingFlags.NonPublic
                    | BindingFlags.Public);
            if (method == null) { context.Response.Write(ToolHelper.Json(false, "请求的处理函数不存在")); return; }

            var fun = (Func<HttpContext, string>)method.CreateDelegate(typeof(Func<HttpContext, string>), this);
            context.Response.Write(fun(context));
        }

        public bool IsReusable { get { return false; } }
    }
}
