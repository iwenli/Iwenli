using System.Reflection;
using System.Web;

namespace Iwenli.Web.Htmx
{
    public class HtmxDataHandler : HtmxHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (Context == null)
            {
                ISite _site = Web.Instance.GetSite(context);
                ISkin _skin = _site.GetSkinConfig(context);
                Page _page = _site.GetRequestConfig(context);
                IHttpHandler _handler = _page.GetHandler(context);
                base.InitPage(context, Url.Current, Web.Instance, _site, _skin, _page);
            }

            string _rtn = string.Empty;
            if (!string.IsNullOrEmpty(Request.PathInfo))
            {
                string _methodName = Request.PathInfo.Substring(1);
                MethodInfo _method = this.GetType().GetMethod(_methodName);
                if (_method != null)
                {
                    _rtn = _method.Invoke(this, new object[] { }).ToString();
                }
                Response.Write(_rtn);
            }
            else
            {
                Response.Write(Default());
            }
        }

        public virtual string Default()
        {
            return "";
        }

    }
}
