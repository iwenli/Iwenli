using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iwenli.CodeGenerate
{
	public partial class BaseForm : Form
	{
		string author = "iwenli";
		protected virtual string ModelName { get; set; } = "ORM生成";

		/// <summary>
		/// 通用配置
		/// </summary>
		protected void InitSetting()
		{
			Text = $"代码生成工具-{ModelName}  v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}  by:{author}";
		}

		public BaseForm()
		{
			InitializeComponent();

		}
	}
}
