using System;
using System.Text;
using System.Web;

namespace Iwenli.Web.Htmx
{
    /// <summary>
    /// Htmx模板页面处理程序
    /// </summary>
    public class HtmxHandler  : HtmxHandlerBase
    {
        /// <summary>
        /// 页面代码
        /// </summary>
        protected StringBuilder ThisPageCode = new StringBuilder();

        /// <summary>
        /// 页面模板
        /// </summary>
        protected string m_mainTemplate;

        public override void InitPage(HttpContext context, Url url, IWeb web, ISite site, ISkin skin, Page page)
        {
            base.InitPage(context, url, web, site, skin, page);
            //解析HTML页面
            if (_wlUrl.RequestType == RequestType.HTM)
            {
                //处理数据
                m_mainTemplate = page.GetTemplateName();

                LoadMainTemplateBegin();
                LoadMainTemplate();
                LoadMainTemplateEnd();

                ParsePageBegin();
                ParsePage();
                ParsePageEnd();
            }
        }

        public override void ProcessRequest(HttpContext context)
        {
            ThisPageCode = ThisPageCode.Replace("</head>", "<meta name=\"handler_time\" content=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffff") + "\" />\r\n</head>");
            Response.Write(ThisPageCode.ToString());
        }  


        #region 解析模板

        /// <summary>
        /// 0、加载主模板开始
        /// </summary>
        public virtual void LoadMainTemplateBegin() { }

        /// <summary>
        /// 1、加载主模板
        /// </summary>
        public virtual void LoadMainTemplate()
        {
            //提取模板
            ThisPageCode.Append(_wlSkin.GetTemplate(m_mainTemplate));
        }

        /// <summary>
        /// 2、加载主模板结束
        /// </summary>
        public virtual void LoadMainTemplateEnd() { }

        /// <summary>
        /// 3、解析页面开始
        /// </summary>
        public virtual void ParsePageBegin() { }

        /// <summary>
        /// 4、解析页面
        /// </summary>
        public virtual void ParsePage()
        {
            foreach (IParser item in _wlPage.GetParseList())
            {
                item.Parse(_wlUrl, _wlSite, _wlSkin, _wlPage, ref ThisPageCode);
            }
        }

        /// <summary>
        /// 5、解析页面结束
        /// </summary>
        public virtual void ParsePageEnd() { }

        #endregion

     
    }
}
