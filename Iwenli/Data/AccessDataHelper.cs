using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    /// <summary>
    /// 提供 Access 数据库操作方法
    /// </summary>
    public class AccessDataHelper : DataHelper
    {
        #region 构造函数

        public AccessDataHelper()
        {
            _dataConnection = new OleDbConnection();
            _ifUseTransaction = false;
            //m_spFileValue = new Hashtable();
        }

        public AccessDataHelper(bool ifUseTransaction)
        {
            _dataConnection = new OleDbConnection();
            _ifUseTransaction = ifUseTransaction;
            //m_spFileValue = new Hashtable();
        }
        public AccessDataHelper(string connStr)
        {
            _connString = connStr;
            _dataConnection = new OleDbConnection(connStr);
            _ifUseTransaction = false;
            //m_spFileValue = new Hashtable();
        }
        public AccessDataHelper(string connStr, bool ifUseTransaction)
        {
            _connString = connStr;
            _dataConnection = new OleDbConnection(connStr);
            _ifUseTransaction = ifUseTransaction;
            //m_spFileValue = new Hashtable();
        }

        #endregion

        #region 存储过程实现

        public override int SpExecute(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override object SpScalar(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override System.Data.DataTable SpGetDataTable(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override DataSet SpGetDataSet(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override System.Data.IDataReader SpGetDataReader(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref System.Collections.Hashtable returnValue)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref System.Collections.Hashtable returnValue, ref System.Data.DataTable returnTable)
        {
            throw new NotImplementedException("Access 数据库不支持存储过程");
        }

        #endregion
    }
}
