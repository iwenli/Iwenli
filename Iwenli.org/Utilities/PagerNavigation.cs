#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：PagerNavigation
 *  所属项目：Iwenli.Org.Utilities
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 14:53:08
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.Utilities
{
    public class PagerNavigation
    {
        private int buttonCount = 10;
        /// <summary>
        /// 翻页按钮数量,默认10(暂支持0;5;7;10)
        /// </summary>
        public int ButtonCount
        {
            get
            {
                return buttonCount;
            }

            set
            {
                buttonCount = value;
            }
        }

        private bool isShowTotal = true;
        /// <summary>
        /// 是否显示总页数当前页信息,默认显示
        /// </summary>
        public bool IsShowTotal
        {
            get
            {
                return isShowTotal;
            }

            set
            {
                isShowTotal = value;
            }
        }

        private bool isShowTopLastStr = true;
        /// <summary>
        /// 是否显示首页末页链接,默认显示
        /// </summary>
        public bool IsShowTopLastStr
        {
            get
            {
                return isShowTopLastStr;
            }

            set
            {
                isShowTopLastStr = value;
            }
        }

        private bool isUseImg = false;
        /// <summary>
        /// 是否用图片代替上一页和下一页配合脚本,默认不采用
        /// </summary>
        public bool IsUseImg
        {
            get
            {
                return isUseImg;
            }

            set
            {
                isUseImg = value;
            }
        }

        private bool withPageIndex = true;
        /// <summary>
        /// Url中是否带当前页码
        /// </summary>
        public bool WithPageIndex
        {
            get
            {
                return withPageIndex;
            }

            set
            {
                withPageIndex = value;
            }
        }
        /// <summary>
        /// 自定义链接
        /// </summary>
        /// <returns></returns>
        public delegate string CustomLinkHandler();
        /// <summary>
        /// 当已经没有上一页时可自定义上一页链接
        /// </summary>
        public event CustomLinkHandler PreLink;
        /// <summary>
        /// 当已经没有下一页时可自定义下一页链接
        /// </summary>
        public event CustomLinkHandler NextLink;
        //可添加自定义事件


        /// <summary>
        /// 分页导航函数
        /// </summary>
        /// <param name="totalRecord">总记录条数</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="currentIndex">当前页索引</param>
        /// <param name="absolutePath">请求绝对路径</param>
        /// <param name="urlParam">请求参数集合</param>
        public string GetPageBarCode(int totalRecord, int pageSize, int currentIndex, string absolutePath, string urlParam)
        {
            string pageUrl = absolutePath;

            string query = string.IsNullOrEmpty(urlParam) ? "" : "?" + urlParam;//string.Empty;

            //if (urlParam != null && urlParam.Count > 0)
            //{
            //    for (int i = 0; i < urlParam.Count; i++)
            //    {
            //        if (!string.IsNullOrEmpty(urlParam[i]))
            //        {
            //            if (i == 0)
            //                query += "?" + urlParam.Keys[i] + "=" + escape(urlParam[i]);
            //            else
            //                query += "&" + urlParam.Keys[i] + "=" + escape(urlParam[i]);
            //        }
            //    }
            //}
            if (WithPageIndex)
            {
                int t = pageUrl.LastIndexOf('.') - pageUrl.LastIndexOf('_');

                pageUrl = pageUrl.Remove(pageUrl.LastIndexOf('_'), t);
            }

            string pageBarStr = string.Empty;

            int pageCount = 0;

            if (pageSize != 0)
            {
                pageCount = (totalRecord / pageSize);

                pageCount = ((totalRecord % pageSize) != 0 ? pageCount + 1 : pageCount);

                pageCount = (pageCount == 0 ? 1 : pageCount);
            }
            if (currentIndex < 1)
            {
                currentIndex = 1;
            }

            int nextIndex = currentIndex + 1;

            int preIndex = currentIndex - 1;

            if (currentIndex > 1)
            {
                pageBarStr += IsShowTopLastStr ? "<li id='topPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_1") + query + "\">" + (IsUseImg ? "" : "首页") + "</a></li>\r<li id='prePage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + preIndex) + query + "\">" + (IsUseImg ? "" : "上一页") + "</a></li>\r" : "<li id='prePage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + preIndex) + query + "\">" + (IsUseImg ? "" : "上一页") + "</a></li>\r";
            }
            else
            {
                if (PreLink != null)
                {
                    pageBarStr += PreLink();
                }
                else
                {
                    pageBarStr += IsShowTopLastStr ? "<li id='topPage'>" + (IsUseImg ? "" : "首页") + "</li>\r<li id='prePage'>" + (IsUseImg ? "" : "上一页") + "</li>\r" : "<li id='prePage'>" + (IsUseImg ? "" : "上一页") + "</li>\r";
                }
            }

            //当前页码居中
            if (buttonCount != 0)
            {
                int startCount = 0;

                int endCount = 0;

                switch (buttonCount)
                {
                    case 10:

                        startCount = (currentIndex + 5) > pageCount ? pageCount - 9 : currentIndex - 4;

                        endCount = currentIndex < 5 ? 10 : currentIndex + 5;

                        break;

                    case 7:
                        startCount = (currentIndex + 3) > pageCount ? pageCount - 7 : currentIndex - 3;

                        endCount = currentIndex < 3 ? 7 : currentIndex + 3;

                        break;

                    case 5:

                        startCount = (currentIndex + 2) > pageCount ? pageCount - 5 : currentIndex - 2;

                        endCount = currentIndex < 2 ? 5 : currentIndex + 2;

                        break;

                    default:

                        throw new Exception("当前页码居中,页码数暂支持0;5;7;10");
                }

                if (startCount < 1)
                {
                    startCount = 1;
                }

                if (pageCount < endCount)
                {
                    endCount = pageCount;
                }

                for (int i = startCount; i <= endCount; i++)
                {
                    pageBarStr += currentIndex == i ? "<li id='currentPage'>" + i + "</li>\r" : "<li><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + i) + query + "\">" + i + "</a></li>\r";
                }
            }

            if (currentIndex != pageCount)
            {
                pageBarStr += IsShowTopLastStr ? "<li id='nextPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + nextIndex) + query + "\">" + (IsUseImg ? "" : "下一页") + "</a></li>\r<li id='lastPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + pageCount) + query + "\">" + (IsUseImg ? "" : "尾页") + "</a></li>\r" : "<li id='nextPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + nextIndex) + query + "\">" + (IsUseImg ? "" : "下一页") + "</a></li>\r";
            }
            else
            {
                if (NextLink != null)
                {
                    pageBarStr += NextLink();
                }
                else
                {
                    pageBarStr += IsShowTopLastStr ? "<li id='nextPage'>" + (IsUseImg ? "" : "下一页") + "</li>\r<li id='lastPage'>" + (IsUseImg ? "" : "尾页") + "</li>\r" : "<li id='nextPage'>" + (IsUseImg ? "" : "下一页") + "</li>\r";
                }
            }
            if (isShowTotal)
            {
                pageBarStr += "<li id='totalInfo'>" + currentIndex + "/" + pageCount + "页</li>\r";
            }
            return pageBarStr;
        }

        public string GetPagerbarBuildSiteUse(int totalRecord, int pageSize, int currentIndex, string absolutePath, string urlParam)
        {
            string pageUrl = absolutePath;

            var _urlParam = System.Web.HttpUtility.ParseQueryString(urlParam);
            _urlParam.Remove("pagesize");

            string query = _urlParam.Count == 0 ? "" : "?" + _urlParam.ToString();

            if (WithPageIndex)
            {
                int t = pageUrl.LastIndexOf('.') - pageUrl.LastIndexOf('_');

                pageUrl = pageUrl.Remove(pageUrl.LastIndexOf('_'), t);
            }

            string pageBarStr = string.Empty;

            int pageCount = 0;

            if (pageSize != 0)
            {
                // pageCount = (int)(totalRecord / pageSize * -1) * -1;//(totalRecord+pageSize-1)/pageSize

                pageCount = (totalRecord / pageSize);

                pageCount = ((totalRecord % pageSize) != 0 ? pageCount + 1 : pageCount);

                pageCount = (pageCount == 0 ? 1 : pageCount);
            }
            if (currentIndex < 1)
            {
                currentIndex = 1;
            }

            int nextIndex = currentIndex + 1;

            int preIndex = currentIndex - 1;

            pageBarStr += "<li id='bar_pageSize'><select>\r";

            pageBarStr += "<option value=10>10</option>";
            pageBarStr += "<option value=20>20</option>";
            pageBarStr += "<option value=30>30</option>";
            pageBarStr += "<option value=50>50</option>";

            pageBarStr += "</select><script>$('select','#bar_pageSize').val(" + pageSize + ").change(function(){ var pagesize = $(this).children('option:selected').val();window.location='" + pageUrl.Insert(pageUrl.IndexOf("."), "_1") + query + (query == "" ? "?pagesize=" : "&pagesize=") + "'+pagesize;});</script></li>\r";

            if (isShowTotal)
            {
                pageBarStr += "<li id='totalInfo'>页数&nbsp;" + currentIndex + "/" + pageCount + "页</li>\r";
            }

            if (currentIndex > 1)
            {
                pageBarStr += IsShowTopLastStr ? "<li id='topPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_1") + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "首页") + "</a></li>\r<li id='prePage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + preIndex) + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "上一页") + "</a></li>\r" : "<li id='prePage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + preIndex) + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "上一页") + "</a></li>\r";
            }
            else
            {
                pageBarStr += IsShowTopLastStr ? "<li id='topPage'>" + (IsUseImg ? "" : "首页") + "</li>\r<li id='prePage'>" + (IsUseImg ? "" : "上一页") + "</li>\r" : "<li id='prePage'>" + (IsUseImg ? "" : "上一页") + "</li>\r";
            }


            if (currentIndex != pageCount)
            {
                pageBarStr += IsShowTopLastStr ? "<li id='nextPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + nextIndex) + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "下一页") + "</a></li>\r<li id='lastPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + pageCount) + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "尾页") + "</a></li>\r" : "<li id='nextPage'><a target=\"_self\" href=\"" + pageUrl.Insert(pageUrl.IndexOf("."), "_" + nextIndex) + query + (query == "" ? "?pagesize=" + pageSize : "&pagesize=" + pageSize) + "\">" + (IsUseImg ? "" : "下一页") + "</a></li>\r";
            }
            else
            {
                pageBarStr += IsShowTopLastStr ? "<li id='nextPage'>" + (IsUseImg ? "" : "下一页") + "</li>\r<li id='lastPage'>" + (IsUseImg ? "" : "尾页") + "</li>\r" : "<li id='nextPage'>" + (IsUseImg ? "" : "下一页") + "</li>\r";
            }

            return pageBarStr;
        }

        /// <summary>
        /// MFC测试开发分页码生成（暂无BUG）
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="absolutePath"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public string GetPageBarCodeText(int totalCount, int pageSize, int pageIndex, string absolutePath, string queryString)
        {
            string pageUrl = string.Empty;//生成地址源Url
            int barCount = 10;//最多展示页码数
            int pageBarCount = 0;//真实页码数
            string hrefHtml = "<li><a class='href' href='{0}'>{1}</a></li>";
            string disableHtml = "<li class='disable'>{0}</li>";

            totalCount = totalCount < 0 ? 0 : totalCount;
            pageSize = pageSize < 1 ? 1 : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            queryString = string.IsNullOrEmpty(queryString) ? "" : "?" + queryString;
            pageBarCount = (int)Math.Ceiling((double)totalCount / pageSize);

            #region 分页码获取
            //AddTempVariable("PageBarCode", GetPageBarCode(Convert.ToInt32(Request.QueryString["totalCount"]), Convert.ToInt32(Request.QueryString["pageSize"]), Convert.ToInt32(Request.QueryString["pageIndex"]), "/img/c/" + Request.QueryString["pageIndex"] + "/", "a=9&b=c"));
            //根据需求自定义规则
            //以最后一个/数字/做为分页码
            var arraStr = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            arraStr[arraStr.Length - 1] = "{0}/";
            pageUrl = "/index.html?totalCount=" + totalCount + "&pageSize=" + pageSize + "&pageindex={0}";// "/" + string.Join("/", arraStr);

            #endregion

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<ul class='pagebarcode'>");
            //打开第一页
            if (pageIndex <= 1)
            {
                sbHtml.Append(string.Format(disableHtml, "首页"));
                sbHtml.Append(string.Format(disableHtml, "上一页"));
            }
            else
            {
                sbHtml.Append(string.Format(hrefHtml, string.Format(pageUrl, 1), "首页"));
                sbHtml.Append(string.Format(hrefHtml, string.Format(pageUrl, pageIndex - 1), "上一页"));
            }
            int minCount = pageIndex <= barCount / 2 ? 1 : pageBarCount < pageIndex + barCount / 2 ? pageIndex - barCount + 1 : pageIndex - barCount / 2;
            int maxCount = pageBarCount < pageIndex + barCount / 2 ? pageBarCount + 1 : pageIndex <= barCount / 2 ? barCount + 1 : pageIndex + barCount / 2;
            minCount = minCount < 1 ? 1 : minCount;
            maxCount = maxCount > pageBarCount + 1 ? pageBarCount + 1 : maxCount;
            for (int i = minCount; i < maxCount; i++)
            {
                if (pageIndex == i)
                {
                    sbHtml.Append(string.Format(disableHtml, i));
                }
                else
                {
                    sbHtml.Append(string.Format(hrefHtml, string.Format(pageUrl, i), i));
                }
            }
            //打开最后一页
            if (pageIndex >= pageBarCount)
            {
                sbHtml.Append(string.Format(disableHtml, "下一页"));
                sbHtml.Append(string.Format(disableHtml, "尾页"));
            }
            else
            {
                sbHtml.Append(string.Format(hrefHtml, string.Format(pageUrl, pageIndex + 1), "下一页"));
                sbHtml.Append(string.Format(hrefHtml, string.Format(pageUrl, pageBarCount), "尾页"));
            }
            sbHtml.Append("</ul>");
            return sbHtml.ToString();
        }

    }
}
