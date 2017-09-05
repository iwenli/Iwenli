using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli.Web
{
    /// <summary>
    /// 公共页面配置
    /// </summary>
    public class PagePublic : IConfig
    {
        /// <summary>
        /// 初始化一个 PagePublic 实例
        /// </summary>
        public PagePublic() { _pageList = new List<Page>(); }


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

        public void Load(XmlElement node)
        {
            //加载默认页面
            if (node["DefaultPage"] != null)
            {
                _defaultPage = ConfigManager.LoadConfig(node["DefaultPage"]) as Page;
            }
            //加载默认皮肤
            if (node["DefaultSkin"] != null)
            {
                _defaulutSkin = ConfigManager.LoadConfig(node["DefaultSkin"]) as ISkin;
             }
            //加载页面信息
            foreach (XmlElement item in node.SelectNodes("Page"))
            {
                Page _page = ConfigManager.LoadConfig(item) as Page;
                if (_page != null)
                {
                    if (_defaultPage != null)
                    {
                        //设置页面默认处理配置信息
                        _page.SetDefault(_defaultPage);
                    }
                    //处理页面信息
                    _pageList.Add(_page);
                }
                else
                {
                    this.LogError("页面信息配置错误：" + item.Name);
                }
            }
        }

        /// <summary>
        /// 设置页面默认处理配置信息
        /// </summary>
        /// <param name="page"></param>
        public void SetDefaultPage(Page page)
        {
            _defaultPage = page;
            foreach (var item in _pageList)
            {
                item.SetDefault(_defaultPage);
            }
        }
    }
}
