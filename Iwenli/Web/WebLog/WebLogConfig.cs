using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli.Web.WebLog
{
    /// <summary>
    /// Web点击流日志数据
    /// </summary>
    public class  WebLogConfig : Config
    {
        /// <summary>
        /// 当前站点Web日志数据配置
        /// </summary>
        public new static WebLogConfig Instance
        {
            get
            {
                return (WebLogConfig)ConfigManager.GetConfigInfo("WebLog");
            }
        }

        public WebLogConfig()
        {
            _IfNote = false;
            _databaseName = "ClickStream";
            _dataTableName = "ClickStreamData";
            _pathIncludeInfo = new List<string>();
            _pathExcludeInfo = new List<string>();
            _userAgentExclude = new List<string>();
            _ipExclude = new List<string>();
        }

        bool _IfNote;
        /// <summary>
        /// 是否记录
        /// </summary>
        public bool IfNote
        {
            get { return _IfNote; }
            set { _IfNote = value; }
        }

        string _databaseName;
        /// <summary>
        /// 记录到库
        /// </summary>
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }
        string _dataTableName;
        /// <summary>
        /// 记录到的表
        /// </summary>
        public string DataTableName
        {
            get { return _dataTableName; }
            set { _dataTableName = value; }
        }

        List<string> _pathExcludeInfo;
        /// <summary>
        /// 页面排除数据
        /// </summary>
        public List<string> PathExcludeInfo
        {
            get { return _pathExcludeInfo; }
            set { _pathExcludeInfo = value; }
        }

        List<string> _pathIncludeInfo;
        /// <summary>
        /// 页面包含数据
        /// </summary>
        public List<string> PathIncludeInfo
        {
            get { return _pathIncludeInfo; }
            set { _pathIncludeInfo = value; }
        }
        List<string> _userAgentExclude;
        /// <summary>
        /// 用户代理排除
        /// </summary>
        public List<string> UserAgentExclude
        {
            get { return _userAgentExclude; }
            set { _userAgentExclude = value; }
        }

        List<string> _ipExclude;
        /// <summary>
        /// IP排除
        /// </summary>
        public List<string> IpExclude
        {
            get { return _ipExclude; }
            set { _ipExclude = value; }
        }


        public override void Load(System.Xml.XmlElement node)
        {
            base.Load(node);
            try
            {
                #region 加载配置数据

                if (node.Attributes["open"] != null
                    && node.Attributes["open"].Value == "true")
                {
                    _IfNote = true;
                }
                if (node["DatabaseName"] != null)
                {
                    _databaseName = node["DatabaseName"].InnerText;

                }
                if (node["DataTableName"] != null)
                {
                    _dataTableName = node["DataTableName"].InnerText;
                }

                //页面包含数据
                _pathIncludeInfo = new List<string>();
                if (node["PathInclude"] != null)
                {
                    foreach (XmlNode item in node["PathInclude"].SelectNodes("add"))
                    {
                        if (item.Attributes["value"] != null)
                        {
                            _pathIncludeInfo.Add(item.Attributes["value"].Value);
                        }
                    }
                }

                //页面排除数据
                _pathExcludeInfo = new List<string>();
                if (node["PathExclude"] != null)
                {
                    foreach (XmlNode item in node["PathExclude"].SelectNodes("add"))
                    {
                        if (item.Attributes["value"] != null)
                        {
                            _pathExcludeInfo.Add(item.Attributes["value"].Value);
                        }
                    }
                }

                //代理排除数据
                _userAgentExclude = new List<string>();
                if (node["UserAgentExclude"] != null)
                {
                    foreach (XmlNode item in node["UserAgentExclude"].SelectNodes("add"))
                    {
                        if (item.Attributes["value"] != null)
                        {
                            _userAgentExclude.Add(item.Attributes["value"].Value);
                        }
                    }
                }

                _ipExclude = new List<string>();
                if (node["IpExclude"] != null)
                {
                    foreach (XmlNode item in node["IpExclude"].SelectNodes("add"))
                    {
                        if (item.Attributes["value"] != null)
                        {
                            _userAgentExclude.Add(item.Attributes["value"].Value);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                this.LogError("加载Web日志配置文件错误：" + ex.Message);
            }
        }
    }
}
