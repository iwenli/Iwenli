using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    /// <summary>
    /// 提供 MySql 数据库操作方法
    /// </summary>
    public class MySqlDataHelper : DataHelper
    {
        #region 构造函数

        public MySqlDataHelper()
        {
            _dataConnection = new MySqlConnection();
            _ifUseTransaction = false;
            _spFileValue = new Hashtable();
        }

        public MySqlDataHelper(bool ifUseTransaction)
        {
            _dataConnection = new MySqlConnection();
            _ifUseTransaction = ifUseTransaction;
            _spFileValue = new Hashtable();
        }
        public MySqlDataHelper(string connStr)
        {
            _connString = connStr;
            _dataConnection = new MySqlConnection(connStr);
            _ifUseTransaction = false;
            _spFileValue = new Hashtable();
        }
        public MySqlDataHelper(string connStr, bool ifUseTransaction)
        {
            _connString = connStr;
            _dataConnection = new MySqlConnection(connStr);
            _ifUseTransaction = ifUseTransaction;
            _spFileValue = new Hashtable();
        }

        #endregion

        #region 存储过程实现

        public override int SpExecute(string spName, Hashtable parameterValue)
        {
            Open();
            using (MySqlCommand cmd = _dataConnection.CreateCommand() as MySqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as MySqlTransaction;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                //添加存储过程参数
                foreach (DictionaryEntry item in parameterValue)
                {
                    cmd.Parameters.Add(new MySqlParameter(item.Key.ToString(), item.Value));
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public override object SpScalar(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override System.Data.DataTable SpGetDataTable(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override DataSet SpGetDataSet(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override System.Data.IDataReader SpGetDataReader(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue, ref DataTable returnTable)
        {
            throw new NotImplementedException("MySql 数据库不支持存储过程");
        }


        public override object SqlScalar(string sqlStr, Hashtable parameters)
        {
            Open();
            using (MySqlCommand cmd = _dataConnection.CreateCommand() as MySqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as MySqlTransaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                //添加存储过程参数
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.Add(new MySqlParameter(item, parameters[item]));
                }
                return cmd.ExecuteScalar();
            }
        }


        public override DataTable SqlGetDataTable(string sqlStr, Hashtable parameters)
        {
            Open();
            using (MySqlCommand cmd = _dataConnection.CreateCommand() as MySqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as MySqlTransaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                //添加存储过程参数
                foreach (string item in parameters.Keys)
                {
                    cmd.Parameters.Add(new MySqlParameter(item, parameters[item]));
                }

                IDataReader dr = cmd.ExecuteReader();
                DataTable tempDT = new DataTable("tempDT1");
                tempDT.Load(dr, LoadOption.Upsert);
                return tempDT;
            }
        }

        /// <summary>
        /// 执行SQL语句，获得数据表
        /// </summary>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
        public override System.Data.DataTable SqlGetDataTable(string sqlStr)
        {
            Open();
            using (MySqlCommand cmd = _dataConnection.CreateCommand() as MySqlCommand)
            {
                if (_ifUseTransaction)
                {
                    cmd.Transaction = GetNonceTransaction() as MySqlTransaction;
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                ds.Tables[0].TableName = "tempDT1";
                return ds.Tables[0];
            }
        }

        #endregion
    }
}
