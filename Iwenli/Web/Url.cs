using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// 请求URL信息
    /// </summary>
    public class Url
    {
        #region 获得当前请求 URl 信息

        internal const string PAGE_REQUEST_URL_INFO_KEY = "IWENLI_PAGE_REQUEST_URL_INFO_KEY";

        /// <summary>
        /// 获得当前请求的 URL 信息
        /// </summary>
        public static Url Current
        {
            get
            {
                if (HttpContext.Current.Items[PAGE_REQUEST_URL_INFO_KEY] == null)
                {
                    HttpContext.Current.Items[PAGE_REQUEST_URL_INFO_KEY] = new Url(HttpContext.Current);
                }
                return HttpContext.Current.Items[PAGE_REQUEST_URL_INFO_KEY] as Url;
            }
            set
            {
                HttpContext.Current.Items[PAGE_REQUEST_URL_INFO_KEY] = value as Url;
            }
        }

        #endregion

        #region URL相关信息

        /// <summary>
        /// 域名信息
        /// </summary>
        private Domain _domain;
        /// <summary>
        /// 域名信息
        /// </summary>
        public Domain Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        /// <summary>
        /// 请求路径
        /// </summary>
        private string _absolutePath;
        /// <summary>
        /// 请求路径,带“/”，小写副本
        /// </summary>
        public string AbsolutePath
        {
            get { return _absolutePath; }
            set { _absolutePath = value; }
        }
        /// <summary>
        /// 请求字符串
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _queryString; }
            set { _queryString = value; }
        }
        /// <summary>
        /// 请求字符
        /// </summary>
        NameValueCollection _queryString;

        #endregion

        #region 属性

        /// <summary>
        /// 请求文件扩展名
        /// </summary>
        private string _requestFilePathExtension;
        /// <summary>
        /// 请求文件扩展名
        /// </summary>
        public string RequestFilePathExtension
        {
            get { return _requestFilePathExtension; }
        }

        RequestType _requestType;
        /// <summary>
        /// 页面请求类型
        /// </summary>
        public RequestType RequestType
        {
            get { return _requestType; }
            set { _requestType = value; }
        }

        /// <summary>
        /// 页面请求视图
        /// </summary>
        private RequestViewType _requestViewType;
        /// <summary>
        /// 页面请求视图
        /// </summary>
        public RequestViewType RequestViewType
        {
            get { return _requestViewType; }
            set { _requestViewType = value; }
        }

        private string _templateFile;
        /// <summary>
        /// 对应的模板文件
        /// </summary>
        public string TemplateFile
        {
            get { return _templateFile; }
            set { _templateFile = value; }
        }

        private string _savePath;
        /// <summary>
        /// 对于的物理路径
        /// </summary>
        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }

        #endregion


        HttpContext _context;

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Url(HttpContext context)
        {
            _context = context;

            System.Uri _requestURL = context.Request.Url;
            //域名
            _domain = new Domain(_requestURL.Host);
            //查询字符串变量集合
            _queryString = context.Request.QueryString;
            //请求路径
            _absolutePath = _requestURL.AbsolutePath.ToLower();

            #region 请求类型

            //请求文件类型
            _requestFilePathExtension = context.Request.CurrentExecutionFilePathExtension;//string.Empty;
            _requestViewType = RequestViewType.Put;

            //请求类型
            switch (_requestFilePathExtension)
            {
                case ".htm"://静态页面
                    _requestViewType = RequestViewType.Put;
                    _requestType = RequestType.HTM;
                    break;
                case ".html"://动态页面
                    _requestViewType = RequestViewType.Put;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".html", ".htm");
                    break;
                case ".htme"://编辑视图
                    _requestViewType = RequestViewType.Edit;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htme", ".htm");
                    break;
                case ".htms"://保存视图
                    _requestViewType = RequestViewType.Save;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htms", ".htm");
                    break;
                case ".htmx"://更新视图
                    _requestViewType = RequestViewType.Update;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htmx", ".htm");
                    break;
                case ".htmd"://删除视图
                    _requestViewType = RequestViewType.Delete;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htmd", ".htm");
                    break;
                case ".htmn"://清空视图
                    _requestViewType = RequestViewType.Null;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htmn", ".htm");
                    break;
                case ".htmc"://缓存视图
                    _requestViewType = RequestViewType.Cache;
                    _requestType = RequestType.HTM;
                    _absolutePath = _absolutePath.Replace(".htmc", ".htm");
                    break;
                default:
                    if (string.IsNullOrEmpty(context.Request.CurrentExecutionFilePathExtension))
                    {
                        _requestViewType = RequestViewType.Put;
                        _requestType = RequestType.HTM;
                    }
                    else
                    {
                        _requestType = RequestType.Other;
                    }
                    break;
            }

            #endregion

            _templateFile = (_requestType == RequestType.HTM) ? _absolutePath + "l" : _absolutePath;
            //模板路径
            if (string.IsNullOrEmpty(_context.Request.CurrentExecutionFilePathExtension))
            {
                _templateFile = _absolutePath + "index.html";
            }

            //物理路径
            _savePath = context.Server.MapPath(_absolutePath);
        }

        #endregion       

        public override string ToString()
        {
            return _context.Request.Url.ToString();
        }
    }
}
