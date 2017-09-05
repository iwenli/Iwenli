using System.Web;

namespace Iwenli.Web.Htmx
{
    /// <summary>
    /// Htmx页面处理工厂
    /// </summary>
    public class HtmxFactory : IHttpHandlerFactory
    {       
        #region IHttpHandlerFactory 成员

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (Web.Instance != null)
            {
                ISite _site = Web.Instance.GetSite(context);
                if (_site == null)
                {
                    throw new WlException("WebApp配置错误,请检查wl.Config配置文件[Web]配置结点！");
                }
                ISkin _skin = _site.GetSkinConfig(context);
                Page _page = _site.GetRequestConfig(context);
                if (_page == null)
                {
                    throw new WlException("WebApp配置错误,请检查wl.Config配置文件[Web]配置结点,没有设定默认页面处理类！");
                }
                IHttpHandler _handler = _page.GetHandler(context);

                context.Response.Filter = new HtmxFilter(context.Response.Filter, Url.Current, _page);

                if (_handler is HtmxHandlerBase)
                {
                    //执行过程
                    HtmxHandlerBase _wlHandler = ((HtmxHandlerBase)_handler);
                    _wlHandler.InitPage(context, Url.Current, Web.Instance, _site, _skin, _page);
                }
                return _handler;
            }
            else
            {
                throw new WlException("WebApp配置错误,请检查wl.Config配置文件[Web]配置结点");
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {

        }

        #endregion
    }
}
