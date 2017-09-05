using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// 页面相关配置接口
    /// </summary>
    public interface ISkin : IConfig
    {
        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        string GetTemplate(string templateName);

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool TryGetTemplate(string templateName, out string code);

        void PullOnSkin(ref string htmlStr);

        /// <summary>
        /// 是否当前皮肤
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsThis(HttpContext context);

        /// <summary>
        /// 额外属性
        /// </summary>
        Dictionary<string, string> TemplateList { get; set; }
    }
}
