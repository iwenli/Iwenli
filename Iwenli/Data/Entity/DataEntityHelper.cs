using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;


namespace Iwenli.Data.Entity
{
    /// <summary>
    /// 数据类型的实体类,常用操作方法
    /// </summary>
    public class DataEntityHelper
    {

        #region 将数据映射到实体类

        /// <summary>
        /// 将数据行映射到实体类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="row"></param>
        public static void DataRowToEntity(object obj, DataRow row)
        {

#if Debug
            System.Diagnostics.Stopwatch noteSW = new System.Diagnostics.Stopwatch();
            noteSW.Start();
#endif
            //得到obj的类型 
            Type type = obj.GetType();
            //循环公共属性数组 
            foreach (PropertyInfo info in type.GetProperties())
            {
                //判断属性是否能赋值
                if (info.CanWrite)
                {
                    //用户标示是否赋值
                    object value = DBNull.Value;

                    #region 1、通过外部配置文件，赋值

                    //等待完善

                    #endregion

                    #region 2、通过自定义属性，赋值

                    if (value == DBNull.Value)
                    {
                        //将自定义属性数组循环 
                        foreach (DataRowEntityFileAttribute attribute in info.GetCustomAttributes(typeof(DataRowEntityFileAttribute), false))
                        {
                            //如果DataRow里也包括此列 
                            if (row.Table.Columns.Contains(attribute.FieldName))
                            {
                                //将DataRow指定列的值赋给value 
                                value = row[attribute.FieldName];
                                break;
                            }
                        }
                    }

                    #endregion

                    #region 3、通过属性名，赋值

                    if (value == DBNull.Value)
                    {
                        //如果DataRow里也包括此列 
                        if (row.Table.Columns.Contains(info.Name))
                        {
                            //将DataRow指定列的值赋给value 
                            value = row[info.Name];
                        }
                    }

                    #endregion

                    #region 赋值操作

                    ///如果value为null则，处理下一个属性
                    if (value == DBNull.Value) continue;
                    ///将值做转换 
                    if (info.PropertyType.Equals(typeof(string)))
                    {
                        value = value.ToString();
                    }
                    else if (info.PropertyType.Equals(typeof(int)))
                    {
                        value = Convert.ToInt32(value);
                    }
                    else if (info.PropertyType.Equals(typeof(decimal)))
                    {
                        value = Convert.ToDecimal(value);
                    }
                    else if (info.PropertyType.Equals(typeof(DateTime)))
                    {
                        value = Convert.ToDateTime(value);
                    }
                    else if (info.PropertyType.Equals(typeof(double)))
                    {
                        value = Convert.ToDouble(value);
                    }
                    else if (info.PropertyType.Equals(typeof(bool)))
                    {
                        value = Convert.ToBoolean(value);
                    }
                    //利用反射自动将value赋值给obj的相应公共属性 
                    info.SetValue(obj, value, null);

                    #endregion
                }
            }
#if Debug
            noteSW.Stop();
#endif

        }

        #endregion

        #region 将HttpRequest请求对象映射到实体类

        /// <summary>
        /// 将HttpRequest请求对象映射到实体类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="request"></param>
        public static void RequestInfoToEntity(object obj, System.Web.HttpRequest request)
        {
            RequestInfoToEntity(obj, request, "");
        }

        /// <summary>
        /// 将HttpRequest请求对象映射到实体类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="request"></param>
        /// <param name="prefix">请求参数前缀</param>
        public static void RequestInfoToEntity(object obj, System.Web.HttpRequest request, string prefix)
        {

#if Debug
            System.Diagnostics.Stopwatch _noteSW = new System.Diagnostics.Stopwatch();
            _noteSW.Start();
#endif
            //得到obj的类型 
            Type _type = obj.GetType();
            //循环公共属性数组 
            foreach (PropertyInfo info in _type.GetProperties())
            {
                //判断属性是否能赋值
                if (info.CanWrite)
                {
                    //用户标示是否赋值
                    object _value = DBNull.Value;

                    #region 1、通过外部配置文件，赋值

                    //等待完善

                    #endregion

                    #region 2、通过自定义属性，赋值

                    if (_value == DBNull.Value)
                    {
                        //将自定义属性数组循环 
                        foreach (DataRowEntityFileAttribute attribute in info.GetCustomAttributes(typeof(DataRowEntityFileAttribute), false))
                        {
                            if (!string.IsNullOrEmpty(request[prefix + attribute.FieldName]))
                            {
                                _value = request[attribute.FieldName];
                                break;
                            }
                        }
                    }

                    #endregion

                    #region 3、通过属性名，赋值

                    if (_value == DBNull.Value)
                    {
                        if (!string.IsNullOrEmpty(request[prefix + info.Name]))
                        {
                            _value = request[info.Name];
                        }
                    }

                    #endregion

                    #region 赋值操作

                    ///如果value为null则，处理下一个属性
                    if (_value == DBNull.Value) continue;
                    ///将值做转换 
                    if (info.PropertyType.Equals(typeof(string)))
                    {
                        _value = _value.ToString();
                    }
                    else if (info.PropertyType.Equals(typeof(int)))
                    {
                        _value = Convert.ToInt32(_value);
                    }
                    else if (info.PropertyType.Equals(typeof(decimal)))
                    {
                        _value = Convert.ToDecimal(_value);
                    }
                    else if (info.PropertyType.Equals(typeof(DateTime)))
                    {
                        _value = Convert.ToDateTime(_value);
                    }
                    else if (info.PropertyType.Equals(typeof(double)))
                    {
                        _value = Convert.ToDouble(_value);
                    }
                    else if (info.PropertyType.Equals(typeof(bool)))
                    {
                        _value = Convert.ToBoolean(_value);
                    }
                    //利用反射自动将value赋值给obj的相应公共属性 
                    info.SetValue(obj, _value, null);

                    #endregion

                }
            }
#if Debug
            _noteSW.Stop();
#endif

        }






        #endregion

        #region 获取实体属性的值，并保持到哈希表中，以“@”为前缀的索引

        /// <summary>
        /// 获取实体属性的值，并保持到哈希表中，以“@”为前缀的索引
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Hashtable GetObjectProperty(object obj)
        {
#if Debug
            System.Diagnostics.Stopwatch _noteSW = new System.Diagnostics.Stopwatch();
            _noteSW.Start();
#endif
            Hashtable _table = new Hashtable();

            //得到obj的类型 
            Type _type = obj.GetType();
            //循环公共属性数组 
            foreach (PropertyInfo info in _type.GetProperties())
            {
                //判断属性是否能赋值
                if (info.CanRead && info.Name != "Item")
                {
                    object _value = info.GetValue(obj, null);

                    if (_value == null)
                    {
                        _value = string.Empty;
                    }

                    #region 1、通过属性名，赋值

                    _table["@" + info.Name] = _value.ToString();

                    #endregion

                    #region 2、通过自定义属性，赋值

                    //将自定义属性数组循环 
                    foreach (DataRowEntityFileAttribute attribute in info.GetCustomAttributes(typeof(DataRowEntityFileAttribute), false))
                    {
                        _table["@" + info.Name] = _value.ToString();
                    }

                    #endregion

                    #region 3、通过外部配置文件，赋值

                    //等待完善

                    #endregion
                }
            }
#if Debug
            _noteSW.Stop();
#endif
            return _table;
        }

        #endregion

        #region 根据属性操作信息获取实体列表

        #region 获取数据行实体类列表

        /// <summary>
        /// 获取数据行实体类列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetDataRowEntityList<T>()
            where T : DataRowEntityBase
        {
            Type _entityType = typeof(T);
            //获取数据库查询配置属性
            object[] _tempAttr = _entityType.GetCustomAttributes(typeof(DataEntitySelectAttribute), false);
            if (_tempAttr.Length == 0)
            {
                throw new Exception("没有配置实体属性，无法获取实体信息");
            }
            DataEntitySelectAttribute _entityAttribute = _tempAttr[0] as DataEntitySelectAttribute;

            //根据查询信息返回对象
            return GetDataRowEntityList<T>(_entityAttribute.SelectDatabase, _entityAttribute.SelectObject, _entityAttribute.SelectWhere);
        }

        /// <summary>
        /// 获取数据行实体类列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="selectDataBaseName">查询数据库名称</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件</param>
        /// <returns></returns>
        public static List<T> GetDataRowEntityList<T>(string selectDataBaseName, string selectObjet, params string[] selectWhere)
            where T : DataRowEntityBase
        {
            DatabaseInfo _database = DatabaseConfig.Instance.GetDatabaseInfoByCache(selectDataBaseName);
            return GetDataRowEntityList<T>(_database, selectObjet, selectWhere);
        }

        /// <summary>
        /// 获取数据行实体类列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="selectDatabase">查询数据库</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件</param>
        /// <returns></returns>
        public static List<T> GetDataRowEntityList<T>(DatabaseInfo selectDatabase, string selectObjet, params string[] selectWhere)
            where T : DataRowEntityBase
        {
            return GetDataRowEntityList<T>(GetDataTable(selectDatabase, selectObjet, selectWhere));
        }

        /// <summary>
        /// 获取数据行实体类列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        public static List<T> GetDataRowEntityList<T>(DataTable table)
            where T : DataRowEntityBase
        {
            List<T> _list = new List<T>(table.Rows.Count);
            if (table != null)
            {
                Type _entityType = typeof(T);

                foreach (DataRow item in table.Rows)
                {
                    T _entity = Activator.CreateInstance(_entityType) as T;
                    _entity.InitEntity(item);
                    _list.Add(_entity);
                }
            }
            return _list;
        }


        #endregion

        #region 根据查询条件，返回实例列表

        /// <summary>
        ///  根据实体属性查询条件，返回实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetEntityList<T>()
        {
            Type _entityType = typeof(T);
            //获取数据库查询配置属性
            object[] _tempAttr = _entityType.GetCustomAttributes(typeof(DataEntitySelectAttribute), false);
            if (_tempAttr.Length == 0)
            {
                throw new Exception("没有配置实体属性，无法获取实体信息");
            }
            DataEntitySelectAttribute _entityAttribute = _tempAttr[0] as DataEntitySelectAttribute;

            //根据查询信息返回对象
            return GetEntityList<T>(_entityAttribute.SelectDatabase, _entityAttribute.SelectObject, _entityAttribute.SelectWhere);
        }

        /// <summary>
        /// 根据提供的查询条件，返回实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="selectDataBaseName">查询数据库名称</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件（需要带上 AND）</param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(string selectDataBaseName, string selectObjet, params string[] selectWhere)
        {
            DatabaseInfo _database = DatabaseConfig.Instance.GetDatabaseInfoByCache(selectDataBaseName);
            return GetEntityList<T>(_database, selectObjet, selectWhere);
        }

        /// <summary>
        /// 根据提供的查询条件，返回实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="selectDatabase">查询数据库</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件</param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(DatabaseInfo selectDatabase, string selectObjet, params string[] selectWhere)
        {
            return GetEntityList<T>(GetDataTable(selectDatabase, selectObjet, selectWhere));
        }

        /// <summary>
        /// 根据提供的数据表，返回实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> GetEntityList<T>(DataTable table)
        {
            List<T> _list = new List<T>(table.Rows.Count);
            Type _entityType = typeof(T);

            foreach (DataRow item in table.Rows)
            {
                T _entity = (T)Activator.CreateInstance(_entityType);
                DataRowToEntity(_entity, item);
                _list.Add(_entity);
            }
            return _list;
        }

        #endregion

        #region 获取实体数据表

        /// <summary>
        /// 根据实体数据查询属性获取数据表
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(DataEntitySelectAttribute attribute)
        {
            return GetDataTable(attribute.SelectDatabase, attribute.SelectObject, attribute.SelectWhere);
        }
        /// <summary>
        /// 获取实体对应的数据表
        /// </summary>
        /// <param name="dataBaseName">数据库名称</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string dataBaseName, string selectObjet, params string[] selectWhere)
        {
            DatabaseInfo _database = DatabaseConfig.Instance.GetDatabaseInfoByCache(dataBaseName);
            return GetDataTable(_database, selectObjet, selectWhere);
        }

        /// <summary>
        /// 获取实体对应的数据表
        /// </summary>
        /// <param name="database">查询数据库</param>
        /// <param name="selectObjet">查询对象</param>
        /// <param name="selectWhere">查询条件</param>
        /// <returns></returns>
        public static DataTable GetDataTable(DatabaseInfo database, string selectObjet, params string[] selectWhere)
        {
            DataTable _table = null;
            using (DataHelper helper = DataHelper.GetDataHelper(database))
            {
                string _sql = selectObjet;
                if (_sql.ToLower().IndexOf("select") < 0)
                {
                    _sql = "SELECT * FROM " + selectObjet + " WHERE 1=1 ";
                }
                if (selectWhere != null)
                {
                    foreach (string item in selectWhere)
                    {
                        _sql += " " + item;
                    }
                }
                _table = helper.SqlGetDataTable(_sql);
            }
            return _table;
        }

        /// <summary>
        /// 重载：获取数据表格
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表或视图名</param>
        /// <param name="where">查询条件</param>
        /// <param name="whereParams">条件参数</param>
        /// <param name="field">查询字段</param>
        /// <param name="order">排序方式</param>
        /// <param name="start">起始记录数</param>
        /// <param name="limit">终止记录数</param>
        /// <param name="count">总记录数</param>
        /// <param name="m_dataTable">数据表格</param>
        public static DataTable GetTable(string database, string table, string where, Hashtable whereParams, string field, string order, int start, int limit, out int count)
        {
            using (DataHelper dataTool = DataHelper.GetDataHelper(database))
            {
                string _sql = @"select count(*)  from " + table + " " + where;

                //参数赋值
                foreach (DictionaryEntry s in whereParams)
                {
                    dataTool.SpFileValue.Add(s.Key, s.Value);
                }

                //处理总页数
                count = int.Parse(dataTool.SqlScalar(_sql, dataTool.SpFileValue).ToString());

                //获得当前页数据
                _sql = @" select *  from ( select row_number() over ( " + order + " ) TID, " + field + "  from " + table + " " + where + @" ) PageTable where TID between @Start and @Limit";

                dataTool.SpFileValue.Add("@Start", start + 1);//过滤条数
                dataTool.SpFileValue.Add("@Limit", limit + start);//页面大小

                //执行
                return  dataTool.SqlGetDataTable(_sql, dataTool.SpFileValue);
            }
        }

        /// <summary>
        /// 重载：获取数据表格(string where = "", string field = "*", int start = 0, int limit = 999)
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表或视图名</param>
        /// <param name="where">查询条件</param>
        /// <param name="whereParams">条件参数</param>
        /// <param name="field">查询字段</param>
        /// <param name="order">排序方式</param>
        /// <param name="start">起始记录数</param>
        /// <param name="limit">终止记录数</param>
        /// <param name="count">总记录数</param>
        /// <param name="m_dataTable">数据表格</param>
        public static DataTable GetTable(string database, string table, Hashtable whereParams, string order, out int count, string where = "", string field = "*", int start = 0, int limit = 999)
        {
            using (DataHelper dataTool = DataHelper.GetDataHelper(database))
            {
                string _sql = @"select count(*)  from " + table + " WHERE 1=1 " + where;

                //参数赋值
                foreach (DictionaryEntry s in whereParams)
                {
                    dataTool.SpFileValue.Add(s.Key, s.Value);
                }

                //处理总页数
                count = int.Parse(dataTool.SqlScalar(_sql, dataTool.SpFileValue).ToString());

                //获得当前页数据 
                _sql = @" select *  from ( select row_number() over ( ORDER BY " + order + " ) TID, " + field + "  from " + table + " WHERE 1=1 " + where + @" ) PageTable where TID between @Start and @Limit";

                dataTool.SpFileValue.Add("@Start", start + 1);//过滤条数
                dataTool.SpFileValue.Add("@Limit", limit + start);//页面大小

                //执行
                return dataTool.SqlGetDataTable(_sql, dataTool.SpFileValue);
            }
        }

        #endregion

        #endregion

    }
}
