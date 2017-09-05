using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Org
{
    public class SiteAjaxFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            try
            {
                var path = Regex.Match(url, @"\/(.*?)\.ajax", RegexOptions.IgnoreCase);
                var classPath = path.Groups[1].Value.Replace('/', '.');

                Type handlerType = System.Web.Compilation.BuildManager.GetType(classPath, false, true);
                var handler = Activator.CreateInstance(handlerType) as SiteAjaxBase;
                return handler;

            }
            catch (Exception ex)
            {
                LogHelper.LogInfo(this, string.Format("路径：{0}。错误：{1}。具体错误：{2}", url, ex.Message, ex));
                return null;
            }
        }

        public void ReleaseHandler(IHttpHandler handler) { }
    }
}
