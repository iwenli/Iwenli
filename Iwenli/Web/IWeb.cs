using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// 站点信息接口
    /// </summary>
    public interface IWeb : IConfig
    {
        /// <summary>
        /// 获得站点信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ISite GetSite(HttpContext context);
    }
}
