using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli.Data
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    public class DatabaseConfig : Config
    {
        /// <summary>
        /// 返回一个DatabaseConfig单例对象
        /// </summary>
        public static DatabaseConfig Instance
        {
            get
            {
                return (DatabaseConfig)ConfigManager.GetConfigInfo("Database");
            }
        }

        private Dictionary<string, DatabaseInfo> databaseList;

        /// <summary>
        /// 配置文件中连接库集合
        /// </summary>
        public Dictionary<string, DatabaseInfo> DataBaseList
        {
            get { return databaseList; }
        }

        /// <summary>
        /// 初始化 DatabaseConfig 的新实例
        /// </summary>
        public DatabaseConfig()
        {
            databaseList = new Dictionary<string, DatabaseInfo>();
        }
        /// <summary>
        /// 从缓存中获取一个数据库的连接信息
        /// </summary>
        /// <param name="name">连接名</param>
        /// <returns></returns>
        public DatabaseInfo GetDatabaseInfoByCache(string name)
        {
            DatabaseInfo _info;
            if (!databaseList.TryGetValue(name, out _info))
            {
                throw new Exception("无该数据库[" + name + "]的连接信息！");
            }
            return _info;
        }

        #region 加载配置文件信息

        /// <summary>
        /// 加载配置文件信息
        /// </summary>
        /// <param name="configNode"></param>
        public override void Load(XmlElement configNode)
        {
            base.Load(configNode);
            foreach (XmlNode item in configNode.SelectSingleNode("ConnString"))
            {
                #region 链接字符串表示的数据库信息
                //数据库名称
                string name = item.Name;
                //连接字符串
                string connStr = item.InnerText;
                if (item.Attributes["IfEncrypt"] != null && item.Attributes["IfEncrypt"].Value == "true")
                {
                    connStr = Text.EncryptHelper.AESDecrypt(connStr);
                }
                //else
                //{
                //    connStr = Text.EncryptHelper.AESDecrypt(connStr);
                //}
                //连接类型
                DatabaseType type = DatabaseType.Sql;
                if (item.Attributes["Type"] != null)
                {
                    switch (item.Attributes["Type"].Value)
                    {
                        case "Sql":
                            type = DatabaseType.Sql;
                            break;
                        case "Oracle":
                            type = DatabaseType.Oracle;
                            break;
                        case "MySql":
                            type = DatabaseType.MySql;
                            break;
                        case "Db":
                            type = DatabaseType.Db;
                            break;
                        case "Access":
                            type = DatabaseType.Access;
                            break;
                        default:
                            type = DatabaseType.Sql;
                            break;
                    }
                }
                if (string.IsNullOrEmpty(connStr))
                {
                    continue;
                }
                DatabaseInfo info = new DatabaseInfo(name, connStr, type);
                if (databaseList.ContainsKey(name))
                {
                    throw new Exception("数据库配置[" + Name + "]已经添加了相同的数据库信息[" + name + "]");
                }
                else
                {
                    databaseList.Add(name, info);
                }
                #endregion
            }
        }

        #endregion
    }
}
