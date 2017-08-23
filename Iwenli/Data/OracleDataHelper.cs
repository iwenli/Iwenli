using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    /// <summary>
    /// 提供 Oracle 数据库操作方法
    /// </summary>
    public class OracleDataHelper :DataHelper
    {
        #region 存储过程

        public OracleDataHelper(string connStr)
        {
            _dataConnection = new OracleConnection(connStr);
            _ifUseTransaction = false;
            _spFileValue = new Hashtable();
        }

        #endregion

        #region 存储过程方法实现

        public override int SpExecute(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override object SpScalar(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override System.Data.DataTable SpGetDataTable(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override DataSet SpGetDataSet(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override System.Data.IDataReader SpGetDataReader(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        public override int SpGetReturnValue(string spName, Hashtable parameterValue, string[] returnKey, ref Hashtable returnValue, ref System.Data.DataTable returnTable)
        {
            throw new NotImplementedException("Oracle 数据库没有实现存储过程");
        }

        #endregion
    }
}
