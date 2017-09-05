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
    /// 站点配置信息的默认实现
    /// </summary>
    public class Site : Config, ISite
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Site()
        {
            _hostList = new List<string>();
            _skinList = new List<ISkin>();
            _pageList = new List<Page>();
            _pulicPageConfig = new List<PagePublic>();
        }

        #endregion

        #region 属性字段

        /// <summary>
        /// 默认皮肤配置信息
        /// </summary>
        protected ISkin _defaulutSkin;

        /// <summary>
        /// 默认皮肤配置信息
        /// </summary>
        public ISkin DefaulutSkin
        {
            get { return _defaulutSkin; }
            set { _defaulutSkin = value; }
        }

        /// <summary>
        /// 皮肤配置信息集合
        /// </summary>
        protected List<ISkin> _skinList;

        /// <summary>
        /// 皮肤配置信息集合
        /// </summary>
        public List<ISkin> SkinList
        {
            get { return _skinList; }
            set { _skinList = value; }
        }

        /// <summary>
        /// 默认页面配置信息
        /// </summary>
        protected Page _defaultPage;

        /// <summary>
        /// 默认页面配置信息
        /// </summary>
        public Page DefaultPage
        {
            get { return _defaultPage; }
            set { _defaultPage = value; }
        }

        /// <summary>
        /// 页面配置信息集合
        /// </summary>
        protected List<Page> _pageList;

        /// <summary>
        /// 页面配置信息集合
        /// </summary>
        public List<Page> PageList
        {
            get { return _pageList; }
            set { _pageList = value; }
        }

        /// <summary>
        /// 公共页面配置
        /// </summary>
        protected List<PagePublic> _pulicPageConfig;
        /// <summary>
        /// 公共页面配置
        /// </summary>
        public List<PagePublic> PulicPageConfig
        {
            get { return _pulicPageConfig; }
            set { _pulicPageConfig = value; }
        }

        #endregion

        #region 加载站点信息

        /// <summary>
        /// 加载站点配置结点信息
        /// </summary>
        /// <param name="node"></param>
        public override void Load(System.Xml.XmlElement node)
        {
            base.Load(node);

            #region 加载配置信息

            //加载默认皮肤
            if (node["DefaultSkin"] != null)
            {
                _defaulutSkin = ConfigManager.LoadConfig(node["DefaultSkin"]) as ISkin;
            }

            //加载皮肤信息
            foreach (XmlElement item in node.SelectNodes("Skin"))
            {
                ISkin _skin = ConfigManager.LoadConfig(item) as ISkin;
                if (_skin != null)
                {
                    _skinList.Add(_skin);
                }
                else
                {
                    this.LogError("站点信息配置错误：" + item.Name);
                }
            }

            //加载默认页面
            if (node["DefaultPage"] != null)
            {
                _defaultPage = ConfigManager.LoadConfig(node["DefaultPage"]) as Page;
            }
            //加载页面信息
            foreach (XmlElement item in node.SelectNodes("Page"))
            {
                Page _page = ConfigManager.LoadConfig(item) as Page;
                if (_page != null)
                {
                    //设置页面默认处理配置信息
                    _page.SetDefault(_defaultPage);
                    //处理页面信息
                    _pageList.Add(_page);
                }
                else
                {
                    this.LogError("页面信息配置错误：" + item.Name);
                }
            }

            //加载公共页面配置信息
            foreach (XmlElement item in node.SelectNodes("PagePublic"))
            {
                PagePublic _config = ConfigManager.LoadConfig(item) as PagePublic;
                if (_config != null)
                {
                    if (_config.DefaultPage == null)
                    {
                        _config.SetDefaultPage(_defaultPage);
                    }
                    if (_config.DefaulutSkin == null)
                    {
                        _config.DefaulutSkin = _defaulutSkin;
                    }
                    _pulicPageConfig.Add(_config);
                }
            }

            #endregion
        }

        #endregion

        #region 获取当前请求对应的皮肤

        /// <summary>
        /// 获取当前请求对应的皮肤
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ISkin GetSkinConfig(HttpContext context)
        {
            foreach (ISkin item in _skinList)
            {
                if (item.IsThis(context))
                {
                    return item;
                }
            }
            return _defaulutSkin;
        }

        #endregion

        #region 获取当前请求对应的处理页面信息

        /// <summary>
        /// 提取页面处理程序
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Page GetRequestConfig(HttpContext context)
        {
            //匹配私有配置
            foreach (Page item in _pageList)
            {
                if (item.IsThis(context))
                {
                    return item;
                }
            }
            //匹配公共配置
            foreach (PagePublic item in _pulicPageConfig)
            {
                foreach (Page page in item.PageList)
                {
                    if (page.IsThis(context))
                    {
                        return page;
                    }
                }
            }
            return _defaultPage;
        }

        #endregion

        #region 检测当前请求是否适应此站点配置

        /// <summary>
        /// 主机
        /// </summary>
        List<string> _hostList;

        /// <summary>
        /// 设置当前站点对应的主机头Host
        /// </summary>
        /// <param name="host"></param>
        public void SetHost(string host)
        {
            if (!string.IsNullOrEmpty(host))
            {
                _hostList.Add(host.ToLower());
            }
        }

        /// <summary>
        /// 监测当前请求是否适应此站点配置,Host完全匹配模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool IsThis(HttpContext context)
        {
            foreach (string item in _hostList)
            {
                if (item == context.Request.Url.Host)
                {
                    return true;
                }
                //if (System.Text.RegularExpressions.Regex.IsMatch(item, context.Request.Url.Host, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                //{
                //    return true;
                //}
            }
            return false;
        }

        #endregion
    }
}
