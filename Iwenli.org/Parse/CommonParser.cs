#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：CommonParser
 *  所属项目：Iwenli.Org.Parse
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 14:13:32
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

using Iwenli.Data.Entity;
using Iwenli.Extension;
using Iwenli.Org.Utilities;
using Iwenli.Web;
using Iwenli.Web.Parse;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Iwenli.Org.Parse
{
    /// <summary>
    /// Org通用数据解析类
    /// </summary>
    public class CommonParser : ParserBase
    {
        public CommonParser()
        {
            _tagRegexStr = @"<!--WL_Common{(.*?)}-->";
            /*
             <!--WL_Common{db=TxoooAgent;tb=View_V4_OA_JSJ_CompanyInfo;top=5;order=ComIndexId desc;where=(feli=43) AND (til>2);ctype=1;tmp=/vMall/temp/test.html}-->
             * db:数据库名
             * tb:表或视图名
             * top:数据条数
             * order:排序            
             * where:条件            
             * tmp:模板路径
             * page:第几页翻页时有效
             * start:列表从第几条开始读取
             * m:通用不满足时指定处理类
             * btn:分页时页码数量（支持0,5,7,10)默认7
             * field:提取字段默认*
             * output:默认html可以为json
             */
        }

        public override void ParseTag(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode, Web.Parse.Tag pTag)
        {
            try
            {
                if (!string.IsNullOrEmpty(pTag["class"]))
                {
                    string _tagNamespace = "Iwenli.Org.Parse.Tags";

                    string _className = _tagNamespace + "." + pTag["class"];
                    var _type = System.Web.Compilation.BuildManager.GetType(_className, false, false);

                    var _parseObj = Activator.CreateInstance(_type) as ParserBase;

                    _parseObj.ParseTag(urlInfo, site, skin, page, ref pageCode, pTag);
                    return;
                }
                TagEntity _tag = new TagEntity()
                {
                    Database = pTag["db"],
                    Table = pTag["tb"],
                    Count = pTag["top"],
                    Order = pTag["order"],
                    Where = pTag["where"],
                    DataType = pTag["tmp"] == null ? TagEnum.Content : pTag["page"] == null ? TagEnum.List : TagEnum.Pager,
                    Template = pTag["tmp"],
                    Method = pTag["m"],
                    Filed = pTag["field"] ?? "*",
                    Output = pTag["output"] ?? "html",
                    PageType = pTag["pagetype"] ?? "pc"
                };

                DataTable _dt;
                switch (_tag.DataType)
                {
                    case TagEnum.Content:
                        _dt = GetContentDataTable(_tag, pTag);
                        ParseContentTag(_dt, skin, _tag, ref pageCode, pTag);
                        break;
                    case TagEnum.List:
                        _dt = GetListDataTable(_tag, pTag);
                        ParseListTag(_dt, skin, _tag, ref pageCode, pTag);
                        break;
                    case TagEnum.Pager:
                        int _totalCount = 0;
                        _dt = GetPagerDataTable(_tag, pTag, out _totalCount);
                        ParsePagerTag(_dt, skin, _tag, ref pageCode, _totalCount, pTag);
                        break;
                    default:
                        break;
                }
                pageCode = pageCode.Replace(pTag.TagStr, "");

            }
            catch (Exception ex)
            {
                string _errMsg = string.Format("解析{0}时出错：{1}", pTag.TagStr, ex.Message);
                pageCode = pageCode.Replace(pTag.TagStr, _errMsg);
            }
        }

        public void ParseContentTag(DataTable dt, ISkin skin, TagEntity tag, ref StringBuilder pageCode, Tag pTag)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(tag.Template))
                {
                    string _tempCode = skin.GetTemplate(tag.Template);

                    string _code = ParseTools.ParseDataTable(dt, _tempCode);

                    pageCode = pageCode.Replace(pTag.TagStr, _code);
                }
                else
                {
                    if (tag.Output == "html")
                    {
                        string _code = ParseTools.ParseDataTable(dt, pageCode.ToString());
                        _code = _code.Replace(pTag.TagStr, string.Empty);
                        pageCode.Clear().Append(_code);
                    }
                    if (tag.Output == "json")
                    {
                        string _code = JsonHelper.SerializeObject(dt);
                        pageCode = pageCode.Replace(pTag.TagStr, _code);
                    }
                }
            }
            else
            {
                //无数据时处理值标签
                var _matches = ParseTools._valueTagRegex.Matches(pageCode.ToString());
                foreach (Match item in _matches)
                {
                    string _valueTagStr = item.Groups[0].Value;
                    string _formatStr = item.Groups[2].Value;

                    if (_formatStr == "")
                    {
                        //pageCode = pageCode.Replace(_valueTagStr, "");
                    }
                    else
                    {
                        pageCode = pageCode.Replace(_valueTagStr, ParseTools.FormatTag(item.Groups[3].Value, "", null));
                    }
                }
            }
        }

        public DataTable GetContentDataTable(TagEntity tag,Tag pTag)
        {
            if (!string.IsNullOrEmpty(pTag["m"]))
            {
                var _dataHelper = new TagDataHelper(tag, pTag);
                return _dataHelper.GetDataTable(pTag["m"]);
            }
            StringBuilder _sql = new StringBuilder(string.Format(" SELECT TOP {0} " + tag.Filed + " FROM {1} ", tag.Count, tag.Table));
            if (!string.IsNullOrEmpty(tag.Where))
            {
                _sql.Append(" WHERE " + tag.Where);
            }
            if (!string.IsNullOrEmpty(tag.Order))
            {
                _sql.Append(" ORDER BY " + tag.Order);
            }

            DataTable _dt;
            using (DataHelper helper = DataHelper.GetDataHelper(tag.Database))
            {
                _dt = helper.SqlGetDataTable(_sql.ToString());
            }
            return _dt;
        }

        public void ParseListTag(DataTable dt, ISkin skin, TagEntity tag, ref StringBuilder pageCode, Tag pTag)
        {
            if (string.IsNullOrEmpty(tag.Order))
            {
                throw new Exception("必须提供order参数");
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string _tempCode = skin.GetTemplate(tag.Template);

                string _code = tag.Output == "html" ? ParseTools.ParseDataTable(dt, _tempCode) : JsonHelper.SerializeObject(dt);

                pageCode = pageCode.Replace(pTag.TagStr, _code);
            }
        }

        public DataTable GetListDataTable(TagEntity tag,Tag pTag)
        {
            if (string.IsNullOrEmpty(tag.Order))
            {
                throw new Exception("必须提供order参数");
            }
            if (!string.IsNullOrEmpty(pTag["m"]))
            {
                var _dataHelper = new TagDataHelper(tag, pTag);
                return _dataHelper.GetDataTable(pTag["m"]);
            }
            if (!string.IsNullOrEmpty(tag.Where))
            {
                tag.Where = " WHERE " + tag.Where;
            }
            tag.Order = " ORDER BY " + tag.Order;

            string _start = pTag["start"] ?? "0";

            StringBuilder _sql = new StringBuilder(string.Format("SELECT TOP {0} * FROM (SELECT ROW_NUMBER() OVER ({1}) TID, " + tag.Filed + " FROM {2} {3}) [Table] WHERE TID>={4}",
                tag.Count,
                tag.Order,
                tag.Table,
                tag.Where,
                _start));

            DataTable _dt;
            using (DataHelper helper = DataHelper.GetDataHelper(tag.Database))
            {
                _dt = helper.SqlGetDataTable(_sql.ToString());
            }
            return _dt;
        }

        public void ParsePagerTag(DataTable dt, ISkin skin, TagEntity tag, ref StringBuilder pageCode, int totalCount, Tag pTag)
        {
            if (string.IsNullOrEmpty(tag.Order))
            {
                throw new Exception("必须提供order参数");
            }

            string _barCode = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                string _tempCode = skin.GetTemplate(tag.Template);
                string _code = tag.Output == "html" ? ParseTools.ParseDataTable(dt, _tempCode) : JsonHelper.SerializeObject(dt);
                pageCode = pageCode.Replace(pTag.TagStr, _code);

                if (tag.PageType == "pc")
                {
                    int _currentPage = Convert.ToInt32(pTag["page"]);
                    string _btnCount = pTag["btn"] ?? "7";
                    int _pageSize = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pagesize"])
                        ? int.Parse(tag.Count) : int.Parse(HttpContext.Current.Request.QueryString["pagesize"]);

                    PagerNavigation _pagerhelper = new PagerNavigation();
                    _pagerhelper.IsShowTopLastStr = false;

                    if (string.IsNullOrEmpty(pTag["barType"]) || pTag["barType"] != "1")
                    {
                        _barCode = _pagerhelper.GetPagerbarBuildSiteUse(totalCount,
                            _pageSize,
                            _currentPage,
                            HttpContext.Current.Request.Url.AbsolutePath,
                            HttpContext.Current.Request.QueryString.ToString());
                    }
                    else
                    {
                        _barCode = _pagerhelper.GetPageBarCode(totalCount,
                           _pageSize,
                           _currentPage,
                           HttpContext.Current.Request.Url.AbsolutePath,
                           HttpContext.Current.Request.QueryString.ToString());
                    }
                }
                else if (tag.PageType == "wap")
                {
                    //手机滚动加载模板（一般输出json，这里不处理）
                }
            }
            pageCode = pageCode.Replace("{$Pagerbar}", _barCode).Replace("{$CommonTotalCount}", totalCount.ToString());
        }

        public DataTable GetPagerDataTable(TagEntity tag, Tag pTag, out int totalCount)
        {
            int _pageSize = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["pagesize"])
                   ? int.Parse(tag.Count) : int.Parse(HttpContext.Current.Request.QueryString["pagesize"]);

            if (!string.IsNullOrEmpty(pTag["m"]))
            {
                var _dataHelper = new TagDataHelper(tag, pTag);
                DataTable dt = _dataHelper.GetDataTable(pTag["m"]);
                totalCount = _dataHelper.TotalCount;
                return dt;
            }

            int _currentPage = Convert.ToInt32(pTag["page"]);
            string _where = string.IsNullOrEmpty(tag.Where) ? "" : " WHERE " + tag.Where;
            string _order = string.IsNullOrEmpty(tag.Order) ? "" : " ORDER BY " + tag.Order;
            var _dt =DataEntityHelper.GetTable(
                tag.Database,
                tag.Table,
                _where,
                new Hashtable(),
                tag.Filed,
                _order,
                (_currentPage - 1) * _pageSize,
                _pageSize,
                out totalCount);
            return _dt;
        }
    }
}
