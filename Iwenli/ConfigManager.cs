using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli
{
    /// <summary>
    /// 配置文件管理类
    /// </summary>
    public class ConfigManager
    {
        static MemoryCache _configCache;
        static string _defaultConfigFile;

        /// <summary>
        /// 初始化ConfigManage的新实例
        /// </summary>
        static ConfigManager()
        {
            _configCache = new MemoryCache(new Guid().ToString());
            _defaultConfigFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "wl.config");

            //读取配置文件
            if (File.Exists(_defaultConfigFile))
            {
                XmlDocument reader = new XmlDocument();
                reader.Load(_defaultConfigFile);
                foreach (var item in reader.DocumentElement.ChildNodes)
                {
                    if (item is XmlElement)
                    {
                        ConfigAndCache((XmlElement)item);
                    }
                }
            }
        }

        #region 加载并缓存配置信息

        /// <summary>
        /// 加载配置信息，并且缓存
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static IConfig ConfigAndCache(XmlElement node)
        {
            List<string> filePaths = new List<string>();
            filePaths.Add(_defaultConfigFile);
            XmlElement xeNode = node;

            if (xeNode.Attributes["file"] != null)
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                    xeNode.Attributes["file"].Value);
                if (File.Exists(fileName))
                {
                    XmlDocument configXml = new XmlDocument();
                    configXml.Load(fileName);
                    xeNode = configXml.DocumentElement;
                    filePaths.Add(fileName);
                }
            }

            #region 处理子节点外部文件

            using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(node.OuterXml)))
            {
                while (reader.Read())
                {
                    if (reader.GetAttribute("file") != null)
                    {
                        string _file = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + reader.GetAttribute("file"));
                        if (File.Exists(_file))
                        {
                            filePaths.Add(_file);
                        }
                    }
                }
            }

            #endregion

            if ((xeNode.Attributes["type"] != null)
               && (!string.IsNullOrEmpty(xeNode.Attributes["type"].Value))
               )
            {
                //通过反射获得站点配置对象
                Type type = GetType(xeNode.Attributes["type"].Value);
                if (type != null)
                {
                    IConfig obj = Activator.CreateInstance(type) as Config;
                    if (obj != null)
                    {
                        obj.Load(xeNode);
                        //缓存配置信息
                        CacheItemPolicy _cachePolicy = new CacheItemPolicy();
                        _cachePolicy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));
                        _configCache.Set(node.Name, obj, _cachePolicy);
                    }
                    return obj;
                }
            }

            return null;
        }
        #endregion

        #region 加载配置信息

        public static IConfig LoadConfig(string xmlStr)
        {
            XmlDocument configXml = new XmlDocument();
            configXml.LoadXml(xmlStr);
            XmlElement node = configXml.DocumentElement;
            return LoadConfig(node);
        }
        /// <summary>
        /// 通过配置节加载配置信息,没有缓存
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IConfig LoadConfig(XmlElement node)
        {
            List<string> filePaths = new List<string>();
            filePaths.Add(_defaultConfigFile);
            XmlElement xEnode = node;
            if (xEnode.Attributes["file"] != null)
            {
                string _fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + xEnode.Attributes["file"].Value);
                if (System.IO.File.Exists(_fileName))
                {
                    XmlDocument _configXml = new XmlDocument();
                    _configXml.Load(_fileName);
                    xEnode = _configXml.DocumentElement;
                    filePaths.Add(_fileName);
                }
            }

            if ((xEnode.Attributes["type"] != null)
                && (!string.IsNullOrEmpty(xEnode.Attributes["type"].Value))
                )
            {
                //通过反射获得站点配置对象
                Type type = GetType(xEnode.Attributes["type"].Value);
                if (type != null)
                {
                    IConfig obj = Activator.CreateInstance(type) as IConfig;
                    if (obj != null)
                    {
                        obj.Load(xEnode);
                    }
                    return obj;
                }
            }

            return null;
        }
        #endregion

        #region 从配置文件（Wl.config）加载配置信息

        /// <summary>
        /// 加载配置信息，默认缓存提取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IConfig GetConfigInfo(string key)
        {
            if (_configCache.Contains(key))
            {
                return (IConfig)_configCache[key];
            }
            else
            {
                #region 从默认配置文件加载并且缓存

                if (File.Exists(_defaultConfigFile))
                {
                    //读取XML信息
                    XmlDocument reader = new XmlDocument();
                    reader.Load(_defaultConfigFile);
                    foreach (XmlNode item in reader.DocumentElement.ChildNodes)
                    {
                        if (item is XmlElement)
                        {
                            if ((key == item.Name))
                            {
                                return ConfigAndCache((XmlElement)item);
                            }
                        }
                    }
                }

                #endregion
                return null;
            }
        }
        #endregion

        #region 提取类型

        /// <summary>
        /// 提取类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetType(string name)
        {
            Type type = null;
            type = Type.GetType(name, false, false);

            if (type == null)
            {
                type = System.Web.Compilation.BuildManager.GetType(name, false, false);
            }

            if (type == null)
            {
                foreach (System.Reflection.Assembly item in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = item.GetType(name, false, false);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return type;
        }

        #endregion
    }
}
