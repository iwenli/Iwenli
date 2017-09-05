using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Iwenli.Web.Security
{
    /// <summary>
    /// Web安全配置信息
    /// </summary>
    public class SecurityConfig : Config
    {
        public static SecurityConfig Instance
        {
            get
            {
                return ConfigManager.GetConfigInfo("Security") as SecurityConfig;
            }
        }

        #region 页面相关

        private string _loginUrl;
        /// <summary>
        /// 登录页面地址
        /// </summary>
        public string LoginUrl
        {
            get
            {
                return _loginUrl;
            }
        }
        private string _defaultUrl;
        /// <summary>
        /// 定义在身份验证之后用于重定向的默认 URL。
        /// </summary>
        public string DefaultUrl
        {
            get
            {
                return _defaultUrl;
            }
        }

        #endregion

        #region Cookie相关属性

        private string _key;
        /// <summary>
        /// 加密键值
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }

        private string _cookieName;
        /// <summary>
        /// 认证Cookie名称
        /// </summary>
        public string CookieName
        {
            get
            {
                return _cookieName;
            }
        }

        private bool _requireSSL;
        /// <summary>
        /// 指定是否需要 SSL 连接来传输身份验证 Cookie。
        /// </summary>
        public bool RequireSSL
        {
            get
            {
                return _requireSSL;
            }
        }
        private bool _slidingExpiration;
        /// <summary>
        /// 指定是否启用可调过期时间。可调过期将 Cookie 的当前身份验证时间重置为在单个会话期间收到每个请求时过期。
        /// </summary>
        public bool SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
        }
        private int _timeout;
        /// <summary>
        /// 指定 Cookie 过期前逝去的时间（以整数分钟为单位）。
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
        }

        private string _cookiePath;
        /// <summary>
        /// Cookie路径
        /// </summary>
        public string CookiePath
        {
            get
            {
                return _cookiePath;
            }
        }
        private HttpCookieMode _cookieMode;
        /// <summary>
        /// 定义是否使用 Cookie 以及 Cookie 的行为。
        /// </summary>
        public HttpCookieMode CookieMode
        {
            get
            {
                return _cookieMode;
            }
        }
        private string _cookieDomain;
        /// <summary>
        /// Cookie 中设置的可选域
        /// </summary>
        public string CookieDomain
        {
            get
            {
                if (CookieUseCurrentRootDomain || string.IsNullOrEmpty(_cookieDomain))
                {
                    return Url.Current.Domain.MainDomain;
                }
                return _cookieDomain;
            }
        }
        private bool _cookieUseCurrentRootDomain;
        /// <summary>
        /// 是否使用当前根域
        /// </summary>
        public bool CookieUseCurrentRootDomain
        {
            get { return _cookieUseCurrentRootDomain; }
            set { _cookieUseCurrentRootDomain = value; }
        }

        private bool _validateIP;
        /// <summary>
        /// 指定是否验证IP信息
        /// </summary>
        public bool ValidateIP
        {
            get
            {
                return _validateIP;
            }
        }

        #endregion

        #region 验证类相关

        private bool _checkUrl;
        /// <summary>
        /// 是否验证URL
        /// </summary>
        public bool CheckUrl
        {
            get { return _checkUrl; }
            set { _checkUrl = value; }
        }

        private string _principalType;
        /// <summary>
        /// 用户的类型
        /// </summary>
        public string PrincipalType
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrincipalType"]))
                {
                    return ConfigurationManager.AppSettings["PrincipalType"];
                }
                return _principalType;
            }
        }

        private List<string> _authorizationPath;
        /// <summary>
        /// 需要授权的目录
        /// </summary>
        public List<string> AuthorizationPath
        {
            get
            {
                return _authorizationPath;
            }
        }
        private List<string> _openPath;
        /// <summary>
        /// 开放的路径
        /// </summary>
        public List<string> OpenPath
        {
            get
            {
                return _openPath;
            }
        }
        private List<string> _openDomain;
        /// <summary>
        /// 开放域名
        /// </summary>
        public List<string> OpenDomain
        {
            get
            {
                return _openDomain;
            }
        }
        private List<string[]> _securityIP;
        /// <summary>
        /// 安全IP范围
        /// </summary>
        public List<string[]> SecurityIP
        {
            get
            {
                return _securityIP;
            }
        }

        #endregion

        #region 未授权，页面终止方式

        string _stopInfo;
        /// <summary>
        /// 终止信息
        /// </summary>
        public string StopInfo
        {
            get { return _stopInfo; }
            set { _stopInfo = value; }
        }

        StopBrowseType _stopType;
        /// <summary>
        /// 终止流量的类型
        /// </summary>
        public StopBrowseType StopType
        {
            get { return _stopType; }
            set { _stopType = value; }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public SecurityConfig()
        {
            _loginUrl = "login.aspx";//登录路径
            _defaultUrl = "default.aspx";//定义在身份验证之后用于重定向的默认 URL。

            _cookieName = "WlRBACName";//Cookie 名称
            _requireSSL = false;//指定是否需要 SSL 连接来传输身份验证 Cookie。
            _slidingExpiration = true;//指定是否启用可调过期时间。可调过期将 Cookie 的当前身份验证时间重置为在单个会话期间收到每个请求时过期。
            _timeout = 30;//指定 Cookie 过期前逝去的时间（以整数分钟为单位）。
            _cookiePath = "/";//Cookie路径
            _cookieMode = HttpCookieMode.UseDeviceProfile;//定义是否使用 Cookie 以及 Cookie 的行为。
            _validateIP = false;

            _principalType = "Iwenli.Web.Security.Principal";

            _authorizationPath = new List<string>();
            _openPath = new List<string>();
            _openDomain = new List<string>();

            _securityIP = new List<string[]>();
            _securityIP.Add(new string[] { "127.0.0.1", "255.255.255.255" });

            _stopInfo = "未授权请求";
            _stopType = StopBrowseType.Redirect;

            _cookieUseCurrentRootDomain = true;
        }

        #region 加载配置文件信息

        public override void Load(XmlElement node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "Cookie")
                {
                    #region Cookie信息
                    if (item["Key"] != null)
                    {
                        _key = item["Key"].InnerText;
                    }
                    if (item["Name"] != null)
                    {
                        _cookieName = item["Name"].InnerText;
                    }
                    if (item["RequireSSL"] != null)
                    {
                        _requireSSL = bool.Parse(item["RequireSSL"].InnerText);
                    }
                    if (item["SlidingExpiration"] != null)
                    {
                        _slidingExpiration = bool.Parse(item["SlidingExpiration"].InnerText);
                    }
                    if (item["Timeout"] != null)
                    {
                        _timeout = int.Parse(item["Timeout"].InnerText);
                    }
                    if (item["CookiePath"] != null)
                    {
                        _cookiePath = item["CookiePath"].InnerText;
                    }
                    if (item["CookieMode"] != null)
                    {
                        //_obj._key = item["CookieMode"].InnerText;
                    }
                    if (item["Domain"] != null)
                    {
                        _cookieDomain = item["Domain"].InnerText;
                    }
                    if (item["UseCurrentRootDomain"] != null)
                    {
                        bool.TryParse(item["UseCurrentRootDomain"].InnerText, out _cookieUseCurrentRootDomain);
                    }
                    if (item["ValidateIP"] != null)
                    {
                        bool.TryParse(item["ValidateIP"].InnerText, out _validateIP);
                    }
                    #endregion
                }
                else if (item.Name == "UserType")
                {
                    _principalType = item.InnerText;
                }
                else if (item.Name == "StopInfo")
                {
                    _stopInfo = item.InnerText;
                }
                else if (item.Name == "StopType")
                {
                    if (!Enum.TryParse<StopBrowseType>(item.InnerText, out _stopType))
                    {
                        _stopType = StopBrowseType.Redirect;
                    }
                }
                else if (item.Name == "LoginUrl")
                {
                    _loginUrl = item.InnerText;
                }
                else if (item.Name == "DefaultUrl")
                {
                    _defaultUrl = item.InnerText;
                }
                //需要授权的目录
                else if (item.Name == "AuthorizationPath")
                {
                    List<string> _list = new List<string>();
                    foreach (XmlNode item1 in item.SelectNodes("Add"))
                    {
                        _list.Add(item1.Attributes["value"].Value.ToLower());
                    }
                    _authorizationPath = _list;
                }
                //需要开放的目录
                else if (item.Name == "OpenPath")
                {
                    List<string> _list = new List<string>();
                    foreach (XmlNode item1 in item.SelectNodes("Add"))
                    {
                        _list.Add(item1.Attributes["value"].Value.ToLower());
                    }
                    _openPath = _list;
                }
                //需要开放的域名
                else if (item.Name == "OpenDomain")
                {
                    List<string> _list = new List<string>();
                    foreach (XmlNode item1 in item.SelectNodes("Add"))
                    {
                        _list.Add(item1.Attributes["value"].Value.ToLower());
                    }
                    _openDomain = _list;
                }
                //安全IP信息
                else if (item.Name == "SecurityIP")
                {
                    List<string[]> _list = new List<string[]>();
                    foreach (XmlNode item1 in item.SelectNodes("Add"))
                    {
                        string[] _ip = new string[] { item1.Attributes["ip"].Value, item1.Attributes["mask"].Value };
                        _list.Add(_ip);
                    }
                    _securityIP = _list;
                }
            }
        }

        #endregion
    }
}
