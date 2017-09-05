using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Iwenli.Web.Ajax
{
    public class AjaxFactory : IHttpHandlerFactory
    {

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            var _path = Regex.Match(url, @"\/(.*?)\.ajax", RegexOptions.IgnoreCase);
            var _className = _path.Groups[1].Value.Replace('_', '.');

            Type handlerType = System.Web.Compilation.BuildManager.GetType(_className, false, true);
            if (handlerType == null)
            {
                throw new NullReferenceException("找不到此类型名:" + _className);
            }
            var handler = Activator.CreateInstance(handlerType) as IHttpHandler;
            return handler;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
