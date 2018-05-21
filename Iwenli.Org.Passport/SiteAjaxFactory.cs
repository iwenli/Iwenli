/*----------------------------------------------------------------
 *  Copyright (C) 2017 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：SiteAjaxFactory
 *  所属项目：
 *  创建用户：张玉龙(HouWeiya)
 *  创建时间：2018/4/11 13:17:23
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Org.Passport
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
                Iwenli.LogHelper.LogInfo(this, string.Format("路径：{0}。错误：{1}。具体错误：{2}", url, ex.Message, ex));
                return null;
            }
        }

        public void ReleaseHandler(IHttpHandler handler) { }
    }
}
