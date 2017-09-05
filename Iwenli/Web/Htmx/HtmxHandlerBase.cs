using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Iwenli.Web.Htmx
{
    public abstract class HtmxHandlerBase : System.Web.IHttpHandler, IRequiresSessionState
    {
        #region 获得当前请求页面代码！！

        internal const string WL_THIS_PAGE_CODE_KEY = "WL_THIS_PAGE_CODE_KEY";
        /// <summary>
        /// 获得当前请求时段的页面代码
        /// </summary>
        /// <returns></returns>
        public static string GetThisPageCode()
        {
            if (HttpContext.Current.Items[WL_THIS_PAGE_CODE_KEY] == null)
            {
                return "";
            }
            return HttpContext.Current.Items[WL_THIS_PAGE_CODE_KEY].ToString();
        }

        internal const string WL_THIS_PAGE_TEMP_VARIABLE = "WL_THIS_PAGE_TEMP_VARIABLE";
        /// <summary>
        /// 获得当前请求页面的临时变量
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetThisPageTempVariable()
        {
            if (HttpContext.Current.Items[WL_THIS_PAGE_TEMP_VARIABLE] == null)
            {
                Dictionary<string, string> _info = new Dictionary<string, string>();
                HttpContext.Current.Items[WL_THIS_PAGE_TEMP_VARIABLE] = _info;
                return _info;
            }
            return HttpContext.Current.Items[WL_THIS_PAGE_TEMP_VARIABLE] as Dictionary<string, string>;
        }

        #endregion

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

        #region Iwenli对象

        protected Url _wlUrl;
        protected IWeb _wlWeb;
        protected ISkin _wlSkin;
        protected ISite _wlSite;
        protected Page _wlPage;

        #endregion
        
        #region 初始化页面信息

        /// <summary>
        /// 初始化当前页面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url"></param>
        /// <param name="web"></param>
        /// <param name="site"></param>
        /// <param name="skin"></param>
        /// <param name="page"></param>
        public virtual void InitPage(HttpContext context, Url url, IWeb web, ISite site, ISkin skin, Page page)
        {
            //Http对象
            Context = context;
            Request = context.Request;
            Response = context.Response;

            //Iwenli对象
            _wlUrl = url;
            _wlWeb = web;
            _wlSite = site;
            _wlSkin = skin;
            _wlPage = page;
        }

        #endregion

        #region IHttpHandler接口

        public bool IsReusable
        {
            get { return false; }
        }

        public abstract void ProcessRequest(HttpContext context);        

        #endregion

        #region 内部变量处理

        /// <summary>
        /// 添加临时变量
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public void AddTempVariable(string name, string value)
        {
            GetThisPageTempVariable()[name] = value;
        }

        /// <summary>
        /// 添加临时变量
        /// </summary>
        /// <param name="variable"></param>
        public void AddTempVariable(Dictionary<string, string> variable)
        {
            Dictionary<string, string> _thisTempVariable = GetThisPageTempVariable();
            foreach (string item in variable.Keys)
            {
                _thisTempVariable[item] = variable[item];
            }
        }

        #endregion
    }
}
