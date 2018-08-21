using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate.Entity
{
	public class DbTableInfo : Iwenli.Data.Entity.DataRowEntityBase
	{
		/// <summary>
		/// 表或视图名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 类型标识 v视图 t表
		/// </summary>
		public string XType { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		public DbObjectType DbObjectType
		{
			get
			{
				var _type = DbObjectType.All;
				if (XType == "V")
				{
					_type = DbObjectType.View;
				}
				if (XType == "T")
				{
					_type = DbObjectType.Table;
				}
				return _type;
			}
		}
	}

	public enum DbObjectType
	{
		/// <summary>
		/// 表
		/// </summary>
		Table = 0,
		/// <summary>
		/// 视图
		/// </summary>
		View = 1,
		/// <summary>
		/// 全部
		/// </summary>
		All = 2
	}
}
