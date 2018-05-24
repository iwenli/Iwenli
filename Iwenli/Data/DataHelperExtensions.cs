using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Data
{
    /// <summary>
    /// DataHelpe扩展方法
    /// </summary>
    public static class DataHelperExtensions
    {
        /// <summary>
        /// 获得数据库系统表集合
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static List<string> GetDatabaseList(this DataHelper helper)
        {
            List<string> _list = new List<string>();

            foreach (DataRow item in helper.SqlGetDataTable("select [name] from master.dbo.sysdatabases").Rows)
            {
                _list.Add(item[0].ToString());
            }
            return _list;
        }

        /// <summary>
        /// 获得数据库中数据表集合
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="DatabaseName">数据名称</param>
        /// <returns></returns>
        public static List<string> GetDataTableList(this DataHelper helper, string databaseName)
        {
            List<string> _list = new List<string>();

            //使用数据库
            helper.SqlExecute("Use " + databaseName);
            //查询表
            foreach (DataRow item in helper.SqlGetDataTable("select [name] from sysobjects where xtype = 'u' ").Rows)
            {
                _list.Add(item[0].ToString());
            }
            return _list;
        }

        /// <summary>
        /// 获得数据库中数据表数据列信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns>字段序号,字段名,标识,主键,类型,长度,小数位数,允许空,默认值,字段说明</returns>
        public static DataTable GetDataColumnInfo(this DataHelper helper, string databaseName, string tableName)
        {
            //使用数据库
            helper.SqlExecute("Use " + databaseName);

            #region 查询脚本

            string _sql =
                    @"SELECT        
        
                            a.colorder 字段序号,
                            a.name 字段名,
                            (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) 标识,
                            (case when (SELECT count(*)
                           FROM sysobjects
                            WHERE (name in
                                      (SELECT name
                                    FROM sysindexes
                                    WHERE (id = a.id) AND (indid in
                                              (SELECT indid
                                             FROM sysindexkeys
                                             WHERE (id = a.id) AND (colid in
                                                       (SELECT colid
                                                      FROM syscolumns
                                                      WHERE (id = a.id) AND (name = a.name))))))) AND
                                  (xtype = 'PK'))>0 then '√' else '' end) 主键,
                           b.name 类型,
                           a.length 占用字节数,
                           COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,
                           isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,
                           (case when a.isnullable=1 then '√'else '' end) 允许空,
                           isnull(e.text,'') 默认值,
                           g.[value] AS 字段说明

                            FROM syscolumns a left join systypes b
                            on a.xtype=b.xusertype
                            inner join sysobjects d
                            on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
                            left join syscomments e
                            on a.cdefault=e.id
                            left join sys.extended_properties g
                            on a.id=g.major_id AND a.colid = g.minor_id
                            where 
                                d.name='" + tableName + @"'    --如果只查询指定表,加上此条件
                            order by a.id,a.colorder";

            #endregion

            //查询表
            return helper.SqlGetDataTable(_sql);
        }

    }
}
