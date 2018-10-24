using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate
{
	public static class StringBuilderExtension
	{
		/// <summary>
		/// 添加对象说明
		/// </summary>
		/// <param name="stringBuilder"></param>
		/// <param name="description"></param>
		public static void AppendLineDescription(this StringBuilder stringBuilder, string description, string indentationValue = "")
		{
			stringBuilder.AppendLine($"{indentationValue}/// <summary>");
			stringBuilder.AppendLine($"{indentationValue}/// {description}");
			stringBuilder.AppendLine($"{indentationValue}/// </summary>");
		}

		/// <summary>
		/// 缩进添加整行数据
		/// </summary>
		/// <param name="stringBuilder"></param>
		/// <param name="value"></param>
		/// <param name="indentationValue"></param>

		public static void AppendLineWithIndentation(this StringBuilder stringBuilder, string value, string indentationValue = "\t")
		{
			stringBuilder.AppendLine($"{indentationValue}{value}");
		}
	}
}
