using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace Iwenli.Web.Ajax
{
    /// <summary>
    /// Ajax交互数据处理类
    /// </summary>
    public class AjaxHandler : IHttpHandler, IRequiresSessionState
    {
        #region Http对象

        /// <summary>
        /// 当前请求HttpContext
        /// </summary>
        protected HttpContext Context;
        /// <summary>
        /// 当前请HttpRequest对象
        /// </summary>
        protected HttpRequest Request;
        /// <summary>
        /// 当前请HttpResponse对象
        /// </summary>
        protected HttpResponse Response;

        #endregion

        #region IHttpHandler 成员

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            Request = context.Request;
            Response = context.Response;

            string _rtn = string.Empty;
            string _methodName = context.Request.PathInfo.Substring(1);

            MethodInfo _method = this.GetType().GetMethod(_methodName,
                BindingFlags.IgnoreCase
                | BindingFlags.Instance
                | BindingFlags.Public);
            if (_method != null)
            {
                _rtn = _method.Invoke(this, new object[] { }).ToString();
            }
            context.Response.Write(_rtn);
        }


        public bool IsReusable
        {
            get { return false; }
        }

        #endregion
    }
}
