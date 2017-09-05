using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli
{
    /// <summary>
    /// 配置文件解析类，读取根目录下的wl.config文件
    /// </summary>
    public class Config : IConfig
    {
        #region 属性
        string _name;
        DateTime _loadTime;
        XmlNode _configNode;
        /// <summary>
        /// 当前配置结点键值对信息
        /// </summary>
        protected Dictionary<string, string> _variables;

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>
        /// 加载节点时间
        /// </summary>
        public DateTime LoadTime
        {
            get
            {
                return _loadTime;
            }
            set
            {
                _loadTime = value;
            }
        }
        /// <summary>
        /// 当前解析的节点
        /// </summary>
        public XmlNode ConfigNode
        {
            get
            {
                return _configNode;
            }
            set
            {
                _configNode = value;
            }
        }
        /// <summary>
        /// 当前配置结点键值对信息
        /// </summary>
        public Dictionary<string, string> Variables
        {
            get
            {
                return _variables;  
            }
            set
            {
                _variables = value;
            }
        }
        #endregion

        /// <summary>
        /// 初始化一个Config实例
        /// </summary>
        public Config()
        {
            _variables = new Dictionary<string, string>();
        }
        /// <summary>
        /// 通过一个XmlElement初始化一个Config实例
        /// </summary>
        /// <param name="node"></param>
        public Config(XmlElement node) : this()
        {
            Load(node);
        }


        /// <summary>
        /// 从XML节点中读取配置信息
        /// </summary>
        /// <param name="node">XML节点</param>
        public virtual void Load(XmlElement node)
        {
            _loadTime = DateTime.Now;
            _name = node.Name;
            _configNode = node;

            #region 提取变量信息

            if (node["Variables"] != null)
            {
                foreach (XmlNode item in node["Variables"].SelectNodes("add"))
                {
                    if (item.Attributes["key"] != null
                        && item.Attributes["value"] != null)
                    {
                        _variables[item.Attributes["key"].Value] = item.Attributes["value"].Value;
                    }
                }
            }
            #endregion
        }
    }
}
