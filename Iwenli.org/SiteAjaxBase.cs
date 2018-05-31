using Iwenli.Extension;
using Iwenli.Web.Authorization;
using System;
using System.Reflection;
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
                var _user = HttpContext.Current.User.Identity as UserIdentity;
                if (_user == null && HttpContext.Current.Request.Url.AbsolutePath.ToString().ToLower().IndexOf("/app/") > -1)
                {
                    string _userId = HttpContext.Current.Request.QueryString["userid"];
                    string _token = HttpContext.Current.Request.QueryString["token"];
                    if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_token))
                    {
                        return null;
                    }
                    _user = new UserIdentity("旋风小飞侠|" + _userId);
                }
                return _user;
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
            if (method == null) { context.Response.Write(JsonHelper.Json(false, "请求的处理函数不存在")); return; }

            var fun = (Func<HttpContext, string>)method.CreateDelegate(typeof(Func<HttpContext, string>), this);
            context.Response.Write(fun(context));
        }

        public bool IsReusable { get { return false; } }
    }
}
