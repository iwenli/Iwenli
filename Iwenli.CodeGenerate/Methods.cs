using Iwenli.CodeGenerate.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate
{
	public partial class MainForm
	{
		/// <summary>
		/// 获取视图和表信息列表sql
		/// </summary>
		private string GetTableInfoListSql
		{
			get
			{
				var _sql = "";
				switch (DatabaseInfo.ConnType)
				{
					case Data.DatabaseType.MySql:
						_sql = @"SELECT TABLE_NAME AS NAME,TABLE_COMMENT AS Description,CASE TABLE_TYPE  WHEN 'BASE TABLE' THEN 'T' ELSE 'V' END AS XType FROM information_schema.tables
								 WHERE TABLE_SCHEMA=(SELECT DATABASE())  AND TABLE_TYPE IN('BASE TABLE','VIEW')";
						break;
					default:
						_sql = @"SELECT s.Name,Convert(varchar(max),tbp.value) as Description,s.xtype as XType FROM sysobjects s
							LEFT JOIN sys.extended_properties as tbp ON s.id = tbp.major_id and tbp.minor_id = 0 AND(tbp.Name = 'MS_Description' OR tbp.Name is null)  WHERE s.xtype IN('U','V')";
						break;
				}
				return _sql;
			}
		}

		/// <summary>
		/// 获取视图和表信息列表sql
		/// </summary>
		private string GetColumnInfosByTableNameSql
		{
			get
			{
				var _sql = "";
				switch (DatabaseInfo.ConnType)
				{
					case Data.DatabaseType.MySql:
						_sql = $@"SELECT
                                    0 as TableId,
                                    TABLE_NAME as TableName, 
                                    column_name AS DbColumnName,
                                    CASE WHEN  left(COLUMN_TYPE,LOCATE('(',COLUMN_TYPE)-1)='' THEN COLUMN_TYPE ELSE  left(COLUMN_TYPE,LOCATE('(',COLUMN_TYPE)-1) END   AS DataType,
                                    CAST(SUBSTRING(COLUMN_TYPE,LOCATE('(',COLUMN_TYPE)+1,LOCATE(')',COLUMN_TYPE)-LOCATE('(',COLUMN_TYPE)-1) AS signed) AS Length,
                                    column_default  AS  `DefaultValue`,
                                    column_comment  AS  `ColumnDescription`,
                                    CASE WHEN COLUMN_KEY = 'PRI'
                                    THEN true ELSE false END AS `IsPrimaryKey`,
                                    CASE WHEN EXTRA='auto_increment' THEN true ELSE false END as IsIdentity,
                                    CASE WHEN is_nullable = 'YES'
                                    THEN true ELSE false END AS `IsNullable`
                                    FROM
                                    Information_schema.columns where TABLE_NAME='{DbTableInfo.Name}' and  TABLE_SCHEMA=(select database()) ORDER BY TABLE_NAME";
						break;
					default:
						_sql = $@"SELECT sysobjects.name AS TableName,
								   syscolumns.Id AS TableId,
								   syscolumns.name AS DbColumnName,
								   systypes.name AS DataType,
								   syscolumns.length AS [Length],
								   sys.extended_properties.[value] AS [ColumnDescription],
								   syscomments.text AS DefaultValue,
								   syscolumns.isnullable AS IsNullable,
								   columnproperty(syscolumns.id,syscolumns.name,'IsIdentity')as IsIdentity,
								   (CASE
										WHEN EXISTS
											   ( 
                                             			select 1
														from sysindexes i
														join sysindexkeys k on i.id = k.id and i.indid = k.indid
														join sysobjects o on i.id = o.id
														join syscolumns c on i.id=c.id and k.colid = c.colid
														where o.xtype = 'U' 
														and exists(select 1 from sysobjects where xtype = 'PK' and name = i.name) 
														and o.name=sysobjects.name and c.name=syscolumns.name
											   ) THEN 1
										ELSE 0
									END) AS IsPrimaryKey
							FROM syscolumns
							INNER JOIN systypes ON syscolumns.xtype = systypes.xtype
							LEFT JOIN sysobjects ON syscolumns.id = sysobjects.id
							LEFT OUTER JOIN sys.extended_properties ON (sys.extended_properties.minor_id = syscolumns.colid
																		AND sys.extended_properties.major_id = syscolumns.id)
							LEFT OUTER JOIN syscomments ON syscolumns.cdefault = syscomments.id
							WHERE syscolumns.id IN
								(SELECT id
								 FROM sysobjects
								 WHERE xtype IN('u','v'))
							  AND (systypes.name <> 'sysname')
							  AND sysobjects.name='{DbTableInfo.Name}'
							  AND systypes.name<>'geometry'
							  AND systypes.name<>'geography'
							ORDER BY syscolumns.colid";
						break;
				}
				return _sql;
			}
		}

		/// <summary>
		/// 视图和表集合
		/// </summary>
		private List<DbTableInfo> TableViewList
		{
			get
			{
				using (DataHelper helper = DataHelper.GetDataHelper(DatabaseInfo))
				{
					var _dt = helper.SqlGetDataTable(GetTableInfoListSql);
					return Iwenli.Data.Entity.DataEntityHelper.GetDataRowEntityList<DbTableInfo>(_dt);
				}
			}
		}

		/// <summary>
		/// 列集合信息
		/// </summary>
		private List<DbColumnInfo> ColumnList
		{
			get
			{
				using (DataHelper helper = DataHelper.GetDataHelper(DatabaseInfo))
				{
					var _dt = helper.SqlGetDataTable(GetColumnInfosByTableNameSql);
					return Iwenli.Data.Entity.DataEntityHelper.GetDataRowEntityList<DbColumnInfo>(_dt);
				}
			}
		}

		#region 基础扩展方法
		/// <summary>
		/// 下划线分隔字符串转化为表中驼峰表示字符串 table_name_age  -> TableNameAge
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		private string GetFormatName(string fieldName)
		{
			var arr = fieldName.Split('_');
			var result = string.Empty;
			foreach (var str in arr)
			{
				result += GetFirstUpper(str);
			}
			return result;
		}

		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private string GetFirstLower(string str)
		{
			return str[0].ToString().ToLower() + str.Substring(1);
		}
		/// <summary>
		/// 首字母大写
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private string GetFirstUpper(string str)
		{
			return str[0].ToString().ToUpper() + str.Substring(1);
		}

		/// <summary>
		/// 格式化私有字段[加前缀]
		/// </summary>
		/// <param name="extName"></param>
		/// <returns></returns>
		private string GetPrivateName(string extName)
		{
			return FieldsPrefix + extName;
		}

		/// <summary>
		/// DB to DotNet字段转换 
		/// </summary>
		/// <param name="dbTypeName"></param>
		/// <returns></returns>
		private string GetNetType(string dbTypeName)
		{
			string result;
			switch (dbTypeName)
			{
				case "nvarchar":
					result = "string";
					return result;
				case "varchar":
					result = "string";
					return result;
				case "bigint":
					result = "long";
					return result;
				case "int":
					result = "int";
					return result;
				case "tinyint":
					result = "int";
					return result;
				case "smalldatetime":
					result = "DateTime";
					return result;
				case "money":
					result = "double";
					return result;
				case "decimal":
					result = "double";
					return result;
				case "bit":
					result = "bool";
					return result;
				case "datetime":
					result = "DateTime";
					return result;
				case "float":
					result = "double";
					return result;
				case "numeric":
					result = "int";
					return result;
			}
			result = "string";
			return result;
		}
		#endregion
	}
}
