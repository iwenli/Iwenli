using Iwenli.Web.Htmx;
using System.Collections.Generic;
using System.Text;

namespace Iwenli.Web.Parse
{
    /// <summary>
    /// 页面变量解析者
    /// 变量替换顺序，先替换页面变量、再替换皮肤变量、后替换站点变量
    /// </summary>
    public class VariableParser : IParser
    {
        const string TMP_TAG_REGEX = @"{$(.*)}";

        /// <summary>
        /// 解析入口
        /// </summary>
        /// <param name="urlInfo"></param>
        /// <param name="site"></param>
        /// <param name="skin"></param>
        /// <param name="page"></param>
        /// <param name="pageCode"></param>
        public void Parse(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode)
        {
            //处理页面临时变量
            foreach (KeyValuePair<string, string> item in HtmxHandler.GetThisPageTempVariable())
            {
                pageCode = pageCode.Replace("{$" + item.Key + "}", item.Value);
            }
            //处理页面变量
            foreach (KeyValuePair<string, string> item in page.GetPageVariable())
            {
                pageCode = pageCode.Replace("{$" + item.Key + "}", item.Value);
            }
            //处理皮肤变量
            foreach (KeyValuePair<string, string> item in ((Config)skin).Variables)
            {
                pageCode = pageCode.Replace("{$" + item.Key + "}", item.Value);
            }
            //处理站点变量
            foreach (KeyValuePair<string, string> item in ((Config)site).Variables)
            {
                pageCode = pageCode.Replace("{$" + item.Key + "}", item.Value);
            }
            //处理基本变量
            pageCode = pageCode.Replace("{$_wl_this_domain}", urlInfo.Domain.DomainInfo);
            pageCode = pageCode.Replace("{$_wl_this_root_domain}", urlInfo.Domain.MainDomain);
        }

        public void Load(System.Xml.XmlElement node)
        {
            //throw new NotImplementedException();
        }
    }
}
