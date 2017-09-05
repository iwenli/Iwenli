using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Iwenli.Web
{
    /// <summary>
    /// 请求页面配置信息
    /// </summary>
    public abstract class Page : Config
    {
        #region 页面后续处理规则

        PageSaveType _saveWay;
        /// <summary>
        ///  页面保存方法
        /// </summary>
        public PageSaveType SaveWay
        {
            get
            {
                if (_saveWay == PageSaveType.Null)
                {
                    return PageSaveType.Cache;
                }
                return _saveWay;
            }
        }

        int _updateTime;
        /// <summary>
        /// 页面更新时间
        /// </summary>
        public int UpdateTime
        {
            get
            {
                if (_updateTime == 0)
                {
                    return 15 * 60;
                }
                return _updateTime;
            }
        }

        string _location;
        /// <summary>
        /// 允许缓存的位置
        /// </summary>
        public string Location
        {
            get { return _location; }
        }
        string _custom;
        /// <summary>
        /// 自定义缓存健
        /// </summary>
        public string Custom
        {
            get { return _custom; }
        }
        string _header;
        /// <summary>
        /// 根据标头缓存
        /// </summary>
        public string Header
        {
            get { return _header; }
        }
        string _param;
        /// <summary>
        /// 根据参数缓存
        /// </summary>
        public string Param
        {
            get { return _param; }
        }

        #endregion

        #region 字段
        string _handlerTypeStr;
        Type _handlerType;
        #endregion

        #region 0、构造函数

        public Page()
        {
            _saveWay = PageSaveType.Null;
            _updateTime = 0;
            _parseList = new List<IParser>();
        }

        #endregion
        
        #region 1、加载配置信息

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="node"></param>
        public override void Load(XmlElement node)
        {
            base.Load(node);

            foreach (XmlNode item in node.ChildNodes)
            {
                if (item is XmlElement)
                {
                    if (item.Name == "Output")
                    {
                        #region 后续处理

                        if (item.Attributes["way"] != null)
                        {
                            Enum.TryParse<PageSaveType>(item.Attributes["way"].Value, out _saveWay);
                        }
                        if (item.Attributes["time"] != null)
                        {
                            int.TryParse(item.Attributes["time"].Value, out _updateTime);
                        }
                        if (item.Attributes["location"] != null)
                        {
                            _location = item.Attributes["location"].Value;
                        }
                        if (item.Attributes["custom"] != null)
                        {
                            _custom = item.Attributes["custom"].Value;
                        }
                        if (item.Attributes["header"] != null)
                        {
                            _header = item.Attributes["header"].Value;
                        }
                        if (item.Attributes["param"] != null)
                        {
                            _param = item.Attributes["param"].Value;
                        }

                        #endregion
                    }
                    else if (item.Name == "MappedRegexStr")
                    {
                        _mappedRegexStr = node["MappedRegexStr"].InnerText;
                        _mappedRegex = new Regex(node["MappedRegexStr"].InnerText, RegexOptions.IgnoreCase);
                    }
                    else if (item.Name == "PageTemplate")
                    {
                        _templateRegexStr = node["PageTemplate"].InnerText;
                    }
                    else if (item.Name == "Handler")
                    {
                        #region 页面处理程序

                        //页面处理程序
                        if (item.Attributes["type"] != null)
                        {
                            _handlerTypeStr = item.Attributes["type"].Value;
                            _handlerType = System.Web.Compilation.BuildManager.GetType(_handlerTypeStr, false, false);
                            if (_handlerType == null)
                            {
                                throw new WlException("页面处理程序类型错误：没有找到对应的类型[" + item.Attributes["type"].Value + "]");
                            }
                        }
                        if (_handlerType == null)
                        {
                            throw new WlException("配置结点没有设置页面处理程序，请检查配置文件");
                        }
                        #endregion
                    }
                    else if (item.Name == "Parsers")
                    {
                        #region 解析者

                        _clearParsers = item["clear"] != null;

                        foreach (XmlElement addnode in item.SelectNodes("add"))
                        {
                            IParser _pw = ConfigManager.LoadConfig((XmlElement)addnode) as IParser;
                            if (_pw != null)
                            {
                                _parseList.Add(_pw);
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region 2、设置默认页面处理配置

        /// <summary>
        /// 默认页面配置信息
        /// </summary>
        protected Page _defaultRequestConfig;

        /// <summary>
        /// 设置默认页面处理配置
        /// </summary>
        /// <param name="page"></param>
        public virtual void SetDefault(Page requestConfig)
        {
            _defaultRequestConfig = requestConfig;
            _saveWay = (_saveWay == PageSaveType.Null) ? requestConfig.SaveWay : _saveWay;
            _updateTime = (_updateTime == 0) ? requestConfig.UpdateTime : _updateTime;

            _location = string.IsNullOrEmpty(_location) ? requestConfig._location : _location;
            _header = string.IsNullOrEmpty(_header) ? requestConfig._header : _header;
            _param = string.IsNullOrEmpty(_param) ? requestConfig._param : _param;
            _custom = string.IsNullOrEmpty(_custom) ? requestConfig._custom : _custom;
        }

        #endregion

        #region 3、是否适应当前请求

        /// <summary>
        /// 是否当前页面
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool IsThis(HttpContext context)
        {
            if (_mappedRegex.Match(Url.Current.AbsolutePath).Success)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 4、获取页面处理方法

        /// <summary>
        /// 获取页面处理方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual IHttpHandler GetHandler(HttpContext context)
        {
            if (_handlerType == null)
            {
                if (_defaultRequestConfig != null)
                {
                    return _defaultRequestConfig.GetHandler(context);
                }
                else
                {
                    throw new WlException("没有配置页面处理程序：请检查配置结点[DefaultPage][Page]是否配置了结点[Handler]");
                }
            }
            else
            {
                IHttpHandler _obj = Activator.CreateInstance(_handlerType) as IHttpHandler;
                return _obj;
            }
        }

        /// <summary>
        /// 设置页面处理方法
        /// </summary>
        /// <param name="handlerTypeStr"></param>
        public virtual void SetHandler(string handlerTypeStr)
        {
            //页面处理程序
            _handlerTypeStr = handlerTypeStr;
            _handlerType = System.Web.Compilation.BuildManager.GetType(_handlerTypeStr, false, false);
        }

        /// <summary>
        /// 设置页面处理方法
        /// </summary>
        /// <param name="handlerType"></param>
        public virtual void SetHandler(Type handlerType)
        {
            _handlerType = handlerType;
            _handlerTypeStr = handlerType.FullName;
        }

        #endregion

        #region 5、模板信息，待完善正则匹配

        /// <summary>
        /// URL正则规则字符串
        /// </summary>
        string _mappedRegexStr;
        /// <summary>
        /// URL正则规则匹配字符串
        /// </summary>
        public string MappedRegexStr
        {
            get { return _mappedRegexStr; }
            set { _mappedRegexStr = value; }
        }

        /// <summary>
        /// URL正则规则
        /// </summary>
        Regex _mappedRegex;

        /// <summary>
        /// 模板正则规则
        /// </summary>
        string _templateRegexStr;
        /// <summary>
        /// 模板正则规则
        /// </summary>
        public string TemplateRegexStr
        {
            get { return _templateRegexStr; }
            set { _templateRegexStr = value; }
        }

        #region 5、获取当前请求对应的模板名称

        /// <summary>
        /// 获取当前请求页面对应的模板名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetTemplateName()
        {
            //待完善正则匹配
            string _template = Url.Current.TemplateFile;

            if (!string.IsNullOrEmpty(_templateRegexStr))
            {
                _template = _templateRegexStr;
                if (_templateRegexStr.Contains("$"))
                {
                    _template = System.Text.RegularExpressions.Regex.Replace(Url.Current.AbsolutePath, _mappedRegexStr, _templateRegexStr);
                }
            }

            return _template;
        }

        #endregion

        #endregion

        #region 6、获取页面变量

        /// <summary>
        /// 获取页面变量
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetPageVariable()
        {
            Dictionary<string, string> _info = new Dictionary<string, string>();
            if (_mappedRegexStr != null)
            {
                foreach (KeyValuePair<string, string> item in _variables)
                {
                    if (item.Value.Contains("$"))
                    {
                        string _value = Regex.Replace(Url.Current.AbsolutePath, _mappedRegexStr, item.Value, RegexOptions.IgnoreCase);
                        _info.Add(item.Key, _value);
                    }
                    else
                    {
                        _info.Add(item.Key, item.Value);
                    }
                }
            }
            return _info;
        }

        #endregion

        #region 7、解析处理者集合

        List<IParser> _parseList;
        /// <summary>
        /// 解析处理类列表
        /// </summary>
        public List<IParser> ParseList
        {
            get { return _parseList; }
            set { _parseList = value; }
        }

        bool _clearParsers = false;

        /// <summary>
        /// 获取解析处理程序集合
        /// </summary>
        /// <returns></returns>
        public virtual List<IParser> GetParseList()
        {
            List<IParser> _list = new List<IParser>();
            if (!_clearParsers
                && _defaultRequestConfig != null)
            {
                foreach (IParser item in _defaultRequestConfig.GetParseList())
                {
                    _list.Add(item);
                }
            }
            foreach (IParser item in _parseList)
            {
                _list.Add(item);
            }
            return _list;
        }

        #endregion        
    }
}
