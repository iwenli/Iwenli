using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// 站点配置接口
    /// </summary>
    public interface ISite : IConfig
    {
        /// <summary>
        /// 获取当前请求对应的皮肤配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ISkin GetSkinConfig(HttpContext context);

        /// <summary>
        /// 获取当前请求对应的处理页面配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Page GetRequestConfig(HttpContext context);

        /// <summary>
        /// 设置当前站点对应的主机头Host
        /// </summary>
        /// <param name="host"></param>
        void SetHost(string host);

        /// <summary>
        /// 检测当前请求是否适应此站点配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsThis(HttpContext context);
    }
}
