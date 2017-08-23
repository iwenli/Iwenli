using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    #region 数据库相关链接字符串含义

    /*
     * Data Source（数据源）／Server（服务器）／Address（地址）／Addr（地址）／Network Address（网络地址）：SQL Server实例的名称或网络地址。
     * User ID（用户ID）：用来登陆数据库的帐户名。
     * Password（密码）／Pwd：与帐户名相对应的密码。
     * Database（数据库）／Initial Catalog（初始编目）：数据库的名称。
     * 
     * Connect Timeout（连接超时）／Connection Timeout（连接超时）：一个到服务器的连接在终止之前等待的时间长度（以秒计），缺省值为15。
     * Connection Lifetime（连接生存时间）：当一个连接被返回到连接池时，它的创建时间会与当前时间进行对比。如果这个时间跨度超过了连接的有效期的话，连接就被取消。其缺省值为0。
     * Max Pool Size（连接池的最大容量）：连接池允许的连接数的最大值，其缺省值为100。
     * Min Pool Size（连接池的最小容量）：连接池允许的连接数的最小值，其缺省值为0。
     * Pooling（池）：确定是否使用连接池。如果值为真的话，连接就要从适当的连接池中获得，或者，如果需要的话，连接将被创建，然后被加入合适的连接池中。其缺省值为真。     * 
     * Enlist（登记）：表示连接池程序是否会自动登记创建线程的当前事务语境中的连接，其缺省值为真。
     */

    #endregion

    #region 数据库类型

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MsSQL数据库
        /// </summary>
        Sql = 0,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle = 1,
        /// <summary>
        /// Access数据库
        /// </summary>
        Access = 2,
        /// <summary>
        /// Mysql数据库
        /// </summary>
        MySql = 3,
        /// <summary>
        /// Db数据库
        /// </summary>
        Db = 4
    }

    #endregion

    /// <summary>
    /// 数据库连接信息
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlRoot("Database")]
    public class DatabaseInfo
    {
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        [System.Xml.Serialization.XmlElement("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region 连接字符串

        private string _connString;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(_connString))
                {//先从配置文件加载
                    _connString = string.Format("Data Source={0};User ID={1};Password={2};Database={3};", DataSource, UserID, Password, Database);
                }
                return _connString;
            }
            set
            {
                _connString = value;
            }
        }

        /// <summary>
        /// 加密后的链接字符串
        /// </summary>
        public string EncryptConnString
        {
            get
            {
                return Text.EncryptHelper.AESEncrypt(ConnString);
            }
        }

        #endregion

        #region 数据库链接相关信息

        private DatabaseType _connType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        [System.Xml.Serialization.XmlElement("Type")]
        public DatabaseType ConnType
        {
            get { return _connType; }
            set { _connType = value; }
        }

        private string _dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        [System.Xml.Serialization.XmlElement("DataSource")]
        public string DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        private string _userId;
        /// <summary>
        /// 用来登陆数据库的帐户名
        /// </summary>
        [System.Xml.Serialization.XmlElement("UserID")]
        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _password;
        /// <summary>
        /// 与帐户名相对应的密码
        /// </summary>
        [System.Xml.Serialization.XmlElement("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _database;
        /// <summary>
        /// 数据库的名称
        /// </summary>
        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        #endregion                

        #region 构造函数

        /// <summary>
        /// 初始化 DatabaseInfo 的新实例
        /// </summary>
        public DatabaseInfo()
        {
        }

        /// <summary>
        /// 初始化 DatabaseInfo 的新实例，默认数据库为SQL数据库
        /// </summary>
        /// <param name="name">数据库名称</param>
        public DatabaseInfo(string name)
        {
            _name = name;
            _connType = DatabaseType.Sql;
        }
        /// <summary>
        ///  初始化 DatabaseInfo 的新实例，默认数据库为SQL数据库
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="connStr">连接字符串</param>
        public DatabaseInfo(string name, string connStr)
        {
            _name = name;
            _connType = DatabaseType.Sql;
            _connString = connStr;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="type">数据库类型</param>
        public DatabaseInfo(string name, string connStr, DatabaseType type)
        {
            _name = name;
            _connType = type;
            _connString = connStr;
        }

        #endregion

    }
}
