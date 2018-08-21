using Iwenli.Data.Entity;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate.Entity
{
	public class DbColumnInfo : DataRowEntityBase
	{
		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// 表id
		/// </summary>
		public int TableId { get; set; }
		/// <summary>
		/// 列名
		/// </summary>
		public string DbColumnName { get; set; }
		/// <summary>
		/// 数据类型
		/// </summary>
		public string DataType { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public int Length { get; set; }
		/// <summary>
		/// 列描述
		/// </summary>
		public string ColumnDescription { get; set; }
		/// <summary>
		/// 默认值
		/// </summary>
		public string DefaultValue { get; set; }
		/// <summary>
		/// 是否可为空
		/// </summary>
		public bool IsNullable { get; set; }
		/// <summary>
		/// 是否自增
		/// </summary>
		public bool IsIdentity { get; set; }
		/// <summary>
		/// 是否主键
		/// </summary>
		public bool IsPrimarykey { get; set; }


		public object Value { get; set; }
		public int DecimalDigits { get; set; }
		public int Scale { get; set; }
		/// <summary>
		/// 属性名
		/// </summary>
		public string PropertyName { get; set; }
		/// <summary>
		/// 属性类型
		/// </summary>
		public Type PropertyType { get; set; }
	}
}
