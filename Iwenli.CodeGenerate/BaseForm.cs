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

		protected virtual string MessageBoxTitle { get; set; } = "研发工具集";
		/// <summary>
		/// 日志记录容器
		/// </summary>
		protected virtual RichTextBox RichTextBox { get; set; }

		/// <summary>
		/// 通用配置
		/// </summary>
		protected void InitSetting(RichTextBox richTextBox = null)
		{
			RichTextBox = richTextBox;
			Text = $"{MessageBoxTitle}-{ModelName}  v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}  by:{author}";
		}

		public BaseForm()
		{
			InitializeComponent();
		}

		#region 辅助方法
		/// <summary>
		/// 追加警告信息
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		protected void AppendLogWarning(string message, params object[] args)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() =>
				{
					AppendLog(Color.Violet, message, args);
				}));
				return;
			}
			AppendLog(Color.Violet, message, args);
		}
		/// <summary>
		/// 追加错误信息
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		protected void AppendLogError(string message, params object[] args)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() =>
				{
					AppendLog(Color.Red, message, args);
				}));
				return;
			}
			AppendLog(Color.Red, message, args);
		}
		/// <summary>
		/// 添加日志 定义颜色
		/// </summary>
		/// <param name="fontColor"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		protected void AppendLog(Color fontColor, string message, params object[] args)
		{
			if (RichTextBox == null) return;
			if (InvokeRequired)
			{
				Invoke(new Action(() =>
				{
					AppendLog(fontColor, message, args);
				}));
				return;
			}
			RichTextBox.SelectionColor = fontColor;
			AppendLog(message, args);
		}
		/// <summary>
		/// 添加日志
		/// </summary>
		/// <param name="message"></param>
		/// <param name="args"></param>
		protected void AppendLog(string message, params object[] args)
		{
			if (RichTextBox == null) return;
			if (InvokeRequired)
			{
				Invoke(new Action(() =>
				{
					AppendLog(message, args);
				}));
				return;
			}
			string timeL = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			RichTextBox.AppendText(timeL + " => ");
			if (args != null && args.Length > 0)
			{
				message = string.Format(message, args);
			}
			RichTextBox.AppendText(message);
			RichTextBox.AppendText(Environment.NewLine);
			RichTextBox.ScrollToCaret();
		}

		/// <summary>
		/// 显示提示
		/// </summary>
		/// <param name="msg">提示内容</param>
		protected void SM(string msg)
		{
			MessageBox.Show(msg, MessageBoxTitle);
		}
		/// <summary>
		/// 显示错误内容
		/// </summary>
		/// <param name="msg">错误内容</param>
		protected void SRM(string msg)
		{
			MessageBox.Show(msg, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		/// <summary>
		/// 显示信息提示
		/// </summary>
		/// <param name="msg">错误内容</param>
		protected void SIM(string msg)
		{
			MessageBox.Show(msg, MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		/// <summary>
		/// 显示提示，带yes和no提示的
		/// </summary>
		/// <param name="msg">错误内容</param>
		protected DialogResult SMYN(string msg)
		{
			return MessageBox.Show(msg, MessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
		}

		#endregion
	}
}
