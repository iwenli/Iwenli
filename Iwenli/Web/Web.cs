using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Iwenli.Web
{
    /// <summary>
    /// Web应用池
    /// </summary>
    public class Web : Config, IWeb
    {
        public static Web Instance
        {
            get
            {
                return (Web)ConfigManager.GetConfigInfo("Web");
            }
        }

        /// <summary>
        /// 初始化一个 Web 对象
        /// </summary>
        public Web()
        {
            _siteConfigList = new List<ISite>();
        }

        /// <summary>
        /// 站点配置信息
        /// </summary>
        protected List<ISite> _siteConfigList;

        #region 加载配置信息

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <param name="node"></param>
        public override void Load(XmlElement node)
        {
            base.Load(node);

            XmlNodeList _siteNodeList = node.SelectNodes("Site");
            foreach (XmlElement item in _siteNodeList)
            {
                ISite _site = ConfigManager.LoadConfig(item) as ISite;
                if (_site != null)
                {
                    #region 设置站点主机头

                    //设置站点主机头
                    XmlNodeList _hostNodeList = item.SelectNodes("host");
                    foreach (XmlElement host in _hostNodeList)
                    {
                        if (host.Attributes["value"] != null
                            && !string.IsNullOrEmpty(host.Attributes["value"].Value))
                        {
                            _site.SetHost(host.Attributes["value"].Value);
                        }
                    }

                    #endregion
                    _siteConfigList.Add(_site);
                }
                else
                {
                    throw new WlException("站点信息配置错误：" + item.Name + ":" + item.OuterXml);
                }
            }
        }

        #endregion

        #region 匹配当前请求站点信息

        /// <summary>
        /// 匹配当前请求站点信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual ISite GetSite(HttpContext context)
        {
            foreach (ISite item in _siteConfigList)
            {
                if (item.IsThis(context))
                {
                    return item;
                }
            }
            if (_siteConfigList.Count == 0)
            {
                return null;
            }
            return _siteConfigList[0];
        }

        #endregion
    }
}
