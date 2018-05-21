/*----------------------------------------------------------------
 *  Copyright (C) 2017 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：SiteAjaxBase
 *  所属项目：
 *  创建用户：张玉龙(HouWeiya)
 *  创建时间：2018/4/11 13:19:46
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

using Iwenli.Extension;
using Iwenli.Org.Passport.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Iwenli.Org.Passport
{
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
            if (method == null)
            {
                context.Response.Write(ToolHelper.Json(false, "请求的处理函数不存在", 0));
                context.Response.End();
            }

            var fun = (Func<HttpContext, string>)method.CreateDelegate(typeof(Func<HttpContext, string>), this);
            context.Response.Write(fun(context));
        }

        public bool IsReusable { get { return false; } }

    }
}
