using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    /// <summary>
    /// 提供 Sql2005 数据库操作方法
    ///    1、提供SQL语句，SP操作的方法
    ///    2、封装了数据库事务操作方法
    ///    3、存储过程参数自动加载
    /// </summary>
    public class SqlDataHelper : DataHelper
    {
        #region 构造函数
        /// <summary>
        /// 初始化 SqlDataHelper 的新实例
        /// </summary>
        public SqlDataHelper()
        {
            _dataConnection = new SqlConnection();
            _ifUseTransaction = false;
            _spFileValue = new Hashtable();
        }
        /// <summary>
        /// 初始化 SqlDataHelper 的新实例
        /// </summary>
        /// <param name="ifUseTransaction"></param>
        public SqlDataHelper(bool ifUseTransaction)
        {
            _dataConnection = new SqlConnection();
            _ifUseTransaction = ifUseTransaction;
            _spFileValue = new Hashtable();
        }
        /// <summary>
        /// 初始化 SqlDataHelper 的新实例
        /// </summary>
        /// <param name="connStr"></param>
        public SqlDataHelper(string connStr)
        {
            _connString = connStr;
            _dataConnection = new SqlConnection(connStr);
            _ifUseTransaction = false;
            _spFileValue = new Hashtable();
        }
        /// <summary>
        /// 初始化 SqlDataHelper 的新实例
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="ifUseTransaction"></param>
        public SqlDataHelper(string connStr, bool ifUseTransaction)
        {
            _connString = connStr;
            _dataConnection = new SqlConnection(connStr);
            _ifUseTransaction = ifUseTransaction;
            _spFileValue = new Hashtable();
        }

        #endregion

        /// <summary>
        /// 从数据库中获得对象包含的参数名称(数据表、视图的数据列名称、存储过程参数)
        /// </summary>
        /// <param name="objectName">对象名称</param>     
        public List<string> GetDataObjectParameter(string objectName)
        {
            List<string> parameterList = new List<string>();
            string sql = @"SELECT syscolumns.name AS parameter
		                    FROM sysobjects, syscolumns
		                    WHERE sysobjects.id = syscolumns.id
		                    AND sysobjects.name = '" + objectName + @"'
		                    ORDER BY colid";
            //SqlGetDataTable(sql);
            DataTable _dt = SqlGetDataTable(sql);
            foreach (DataRow item in _dt.Rows)
            {
                parameterList.Add(item[0].ToString());
            }
            return parameterList;
        }


        #region SQL执行

        /*
        public override int SqlExecute(string sqlStr)
        {
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

        public override object SqlScalar(string sqlStr)
        {
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

        public override DataTable SqlGetDataTable(string sqlStr)
        {
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

        public override IDataReader SqlGetDataReader(string sqlStr)
        {
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
        */
        #endregion

        #region 执行带参数的SQL语句

        public override int SqlExecute(string sqlStr, Hashtable parameters)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                //添加存储过程参数
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameters[item]));
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public override DataTable SqlGetDataTable(string sqlStr, Hashtable parameters)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                //添加存储过程参数
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameters[item]));
                }

                IDataReader dr = cmd.ExecuteReader();
                DataTable tempDT = new DataTable("tempDT1");
                tempDT.Load(dr, LoadOption.Upsert);
                return tempDT;
            }
        }

        public override object SqlScalar(string sqlStr, Hashtable parameters)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                //添加存储过程参数
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameters[item]));
                }

                return cmd.ExecuteScalar();
            }
        }

        #endregion

        #region 执行存储过程

        public override int SpExecute(string spName, Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public override object SpScalar(string spName,Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                return cmd.ExecuteScalar();
            }
        }

        public override IDataReader SpGetDataReader(string spName,Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 根据存储过程返回第一表
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public override DataTable SpGetDataTable(string spName, Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                IDataReader dr = cmd.ExecuteReader();
                DataTable tempDT = new DataTable("tempDT1");
                tempDT.Load(dr, LoadOption.Upsert);
                return tempDT;
            }
        }


        public override DataSet SpGetDataSet(string spName, Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                //设置超时时间
                //cmd.CommandTimeout = 1000;
                DataSet ds = new DataSet();
                SqlDataAdapter _a = new SqlDataAdapter(cmd);
                _a.Fill(ds);
                return ds;
            }
        }


        public override int SpGetReturnValue(string spName, Hashtable parameterValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                //添加存储过程返回参数
                SqlParameter _rtnval = cmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                _rtnval.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                //添加存储过程返回参数
                SqlParameter rtnval = cmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                rtnval.Direction = ParameterDirection.ReturnValue;

                //添加存储过程返回参数
                foreach (string item in returnKey)
                {
                    SqlParameter p = new SqlParameter(item, SqlDbType.VarChar, 500);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters[item] = p;
                }
                //执行储存过程
                cmd.ExecuteNonQuery();
                //获得返回值
                foreach (string item in returnKey)
                {
                    returnValue[item] = cmd.Parameters[item].Value;
                }
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue, ref DataTable returnTable)
        {
            Open();
            using (SqlCommand cmd = _dataConnection.CreateCommand() as SqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as SqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (string item in GetDataObjectParameter(spName))
                {
                    cmd.Parameters.Add(new SqlParameter(item, parameterValue[item]));
                }
                //添加存储过程返回参数
                SqlParameter rtnval = cmd.Parameters.Add("ReturnValue", SqlDbType.Int);
                rtnval.Direction = ParameterDirection.ReturnValue;

                //添加存储过程返回参数
                foreach (string item in returnKey)
                {
                    SqlParameter _p = new SqlParameter(item, SqlDbType.VarChar, 500);
                    _p.Direction = ParameterDirection.Output;
                    cmd.Parameters[item] = _p;
                }
                //执行储存过程,填充表                
                IDataReader dr = cmd.ExecuteReader();
                returnTable.Load(dr, LoadOption.Upsert);
                //获得返回值
                foreach (string item in returnKey)
                {
                    returnValue[item] = cmd.Parameters[item].Value;
                }
                return (int)cmd.Parameters["ReturnValue"].Value;
            }
        }

        #endregion
    }
}
