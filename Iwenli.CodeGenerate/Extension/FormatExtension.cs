using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate.Extension
{
	public static class FormatExtension
	{
		/// <summary>
		/// 美化json
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string JsonBeautify(this string text)
		{
			if (text.IsNullOrEmpty()) return "";
			var parsedJson = JsonConvert.DeserializeObject(text);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}

		/// <summary>
		/// 压缩json
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string JsonCompact(this string text)
		{
			if (text.IsNullOrEmpty()) return "";
			var parsedJson = JsonConvert.DeserializeObject(text);
			return JsonConvert.SerializeObject(parsedJson, Formatting.None);
		}
	}
}
