using Iwenli.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli
{
    /// <summary>
    /// 数据库操作基类，提供数据库操作的基层封装方法
    ///     1、提供数据SQL，SP操作的方法
    ///     2、 封装了数据库事务操作方法
    /// </summary>
    public abstract class DataHelper : IDisposable
    {
        #region 事务处理

        protected bool _ifUseTransaction;
        /// <summary>
        /// 是否使用事务
        /// </summary>
        public bool IfUseTransaction
        {
            get
            {
                return _ifUseTransaction;
            }
            set
            {
                _ifUseTransaction = value;
            }
        }

        protected bool _isBeginTransaction;
        /// <summary>
        /// 是否开始事务
        /// </summary>
        public bool IsBeginTransaction
        {
            get { return _isBeginTransaction; }
            set { _isBeginTransaction = value; }
        }


        private IDbTransaction _dataTransaction;
        /// <summary>
        /// 获得当前事务
        /// </summary>
        /// <returns></returns>
        protected IDbTransaction GetNonceTransaction()
        {
            if (_ifUseTransaction)
            {
                if (_dataTransaction == null)
                {
                    _isBeginTransaction = true;
                    _dataTransaction = _dataConnection.BeginTransaction();
                }
                return _dataTransaction;
            }
            else
            {
                throw new Exception("没有启动事务");
            }
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public virtual void Commit()
        {
            if (_ifUseTransaction)
            {
                _dataTransaction.Commit();
            }
            else
            {
                throw new Exception("没有启动事务，无法执行事务提交");
            }
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public virtual void Rollback()
        {
            if (_ifUseTransaction)
            {
                if (_dataTransaction != null)
                {
                    _dataTransaction.Rollback();
                }
            }
            else
            {
                throw new Exception("没有启动事务，无法执行事务回滚");
            }
        }

        #endregion

        #region 数据库对象

        protected string _connString;
        /// <summary>
        /// 设置数据库连接字符串
        /// </summary>
        public string ConnString
        {
            set
            {
                _connString = value;
            }
        }

        protected IDbConnection _dataConnection;
        /// <summary>
        /// 获得数据库连接对象
        /// </summary>
        public IDbConnection DataConnection
        {
            get
            {
                return _dataConnection;
            }
        }

        /// <summary>
        /// 获得数据库操作对象
        /// </summary>
        public IDbCommand DataCommand
        {
            get
            {
                return _dataConnection.CreateCommand();
            }
        }


        protected Hashtable _spFileValue;
        /// <summary>
        /// 获取或设置存储过程需要使用的参数值，参数名带@
        /// </summary>
        public System.Collections.Hashtable SpFileValue
        {
            get
            {
                return _spFileValue;
            }
            set
            {
                _spFileValue = value;
            }
        }

        #endregion

        /// <summary>
        /// 开启数据库连接
        /// </summary>
        protected virtual void Open()
        {
            if (_dataConnection.State == ConnectionState.Closed)
            {
                this.LogInfo("开启数据连接");
                _dataConnection.ConnectionString = _connString;
                _dataConnection.Open();
            }
        }

        #region 执行SQL语句

        /// <summary>
        /// 执行 SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public virtual int SqlExecute(string sqlStr)
        {
            Open();
            using (IDbCommand cmd = _dataConnection.CreateCommand())
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行 SQL 语句，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
        public virtual object SqlScalar(string sqlStr)
        {
            Open();
            using (IDbCommand cmd = _dataConnection.CreateCommand())
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行SQL语句，获得数据表
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
        public virtual DataTable SqlGetDataTable(string sqlStr)
        {
            Open();
            using (IDbCommand cmd = _dataConnection.CreateCommand())
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                IDataReader dr = cmd.ExecuteReader();
                DataTable tempDT = new DataTable("tempDT1");
                tempDT.Load(dr, LoadOption.Upsert);
                return tempDT;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回只进结果集流的读取方法
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns></returns>
        public virtual System.Data.IDataReader SqlGetDataReader(string sqlStr)
        {
            Open();
            using (IDbCommand cmd = _dataConnection.CreateCommand())
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 执行SQL语句，返回只进结果集流的读取方法
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public virtual System.Data.IDataReader SqlGetDataReader(string sqlStr, CommandBehavior behavior)
        {
            Open();
            using (IDbCommand cmd = _dataConnection.CreateCommand())
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                return cmd.ExecuteReader(behavior);
            }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="parameters">SQL语句中的参数，参数名带@</param>
        /// <returns></returns>
        public virtual int SqlExecute(string sqlStr, Hashtable parameters)
        {
            throw new Exception("该数据库操作类没有实现该方法");
        }

        public virtual System.Data.DataTable SqlGetDataTable(string sqlStr, Hashtable parameters)
        {
            throw new Exception("该数据库操作类没有实现该方法");
        }

        public virtual object SqlScalar(string sqlStr, Hashtable parameters)
        {
            throw new Exception("该数据库操作类没有实现该方法");
        }

        #endregion

        #region 执行存储过程

        /// <summary>
        /// 执行存储过程，返回受影响的行数。
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual int SpExecute(string spName)
        {
            return SpExecute(spName, this._spFileValue);
        }
        public abstract int SpExecute(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual object SpScalar(string spName)
        {
            return SpScalar(spName, this._spFileValue);
        }
        public abstract object SpScalar(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，获得数据表(适用返回一张表的情况)
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual System.Data.DataTable SpGetDataTable(string spName)
        {
            return SpGetDataTable(spName, this._spFileValue);
        }
        public abstract System.Data.DataTable SpGetDataTable(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，获得DataSet（适用返回多张表的情况）
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual System.Data.DataSet SpGetDataSet(string spName)
        {
            return SpGetDataSet(spName, this._spFileValue);
        }
        public abstract System.Data.DataSet SpGetDataSet(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，返回只进结果集流的读取方法。
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual IDataReader SpGetDataReader(string spName)
        {
            return SpGetDataReader(spName, this._spFileValue);
        }
        public abstract IDataReader SpGetDataReader(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，获得返回值
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public virtual int SpGetReturnValue(string spName)
        {
            return SpGetReturnValue(spName, this._spFileValue);
        }
        public abstract int SpGetReturnValue(string spName, System.Collections.Hashtable parameterValue);

        /// <summary>
        /// 执行存储过程，获得返回值
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="returnKey">返回值的键名</param>
        /// <param name="returnValue">返回值的键值表</param>
        /// <returns></returns>
        public virtual int SpGetReturnValue(string spName, string[] returnKey, ref System.Collections.Hashtable returnValue)
        {
            return SpGetReturnValue(spName, this._spFileValue, returnKey, ref returnValue);
        }
        public abstract int SpGetReturnValue(string spName, System.Collections.Hashtable parameterValue, string[] returnKey, ref System.Collections.Hashtable returnValue);

        /// <summary>
        ///  执行存储过程，获得返回值
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="returnValue">返回值的键名</param>
        /// <param name="returnValue">返回值的键值表</param>
        /// <param name="returnTable">返回的数据表</param>
        /// <returns></returns>
        public virtual int SpGetReturnValue(string spName, string[] returnKey, ref Hashtable returnValue, ref DataTable returnTable)
        {
            return SpGetReturnValue(spName, this._spFileValue, returnKey, ref returnValue, ref returnTable);
        }
        public abstract int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue, ref DataTable returnTable);

        #endregion

        #region IDisposable 成员

        bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (!disposing)
                    return;

                if (_dataConnection != null)
                {
                    if (_dataConnection.State != ConnectionState.Closed)
                    {
                        this.LogInfo("关闭数据连接");
                        _dataConnection.Close();
                    }
                    _dataConnection.Dispose();
                    _dataConnection = null;
                    _dataTransaction = null;
                    _connString = null;
                    _spFileValue = null;
                }
                _disposed = true;
            }
        }

        #endregion               

        #region 提取数据操作类的工厂方法

        /// <summary>
        /// 获得数据操作类
        /// </summary>
        /// <param name="name">数据库名称，与[Wl_Data.config]中配置的一致</param>
        /// <returns></returns>
        public static DataHelper GetDataHelper(string name)
        {
            DatabaseInfo connStrInfo = DatabaseConfig.Instance.GetDatabaseInfoByCache(name);
            return GetDataHelper(connStrInfo);
        }

        /// <summary>
        /// 获得数据库操作类
        /// </summary>
        /// <param name="database">数据库信息</param>
        /// <returns></returns>
        public static DataHelper GetDataHelper(DatabaseInfo database)
        {
            return GetDataHelper(database.ConnType, database.ConnString);
        }

        /// <summary>
        /// 获得数据库操作类
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <param name="conn">连接字符</param>
        /// <returns></returns>
        public static DataHelper GetDataHelper(DatabaseType type, string conn)
        {
            switch (type)
            {
                case DatabaseType.Sql:
                    {
                        return new SqlDataHelper(conn);
                    }
                case DatabaseType.Oracle:
                    {
                        return new OracleDataHelper(conn);
                    }
                case DatabaseType.Access:
                    {
                        return new AccessDataHelper(conn);
                    }
                case DatabaseType.MySql:
                    {
                        return new MySqlDataHelper(conn);
                    }
                case DatabaseType.Db:
                    {
                        break;
                    }
                default:
                    {
                        return new SqlDataHelper(conn);
                    }
            }
            return new SqlDataHelper(conn);
        }

        #endregion
    }
}
