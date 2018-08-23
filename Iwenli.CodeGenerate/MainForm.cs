using Iwenli.CodeGenerate.Entity;
using Iwenli.Data;
using ScintillaNET;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Iwenli.CodeGenerate
{
	public partial class MainForm : Form
	{ 
		public MainForm()
		{
			InitializeComponent();
			Load += MainForm_Load;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			InitEvent();
			Init();
			InitScintilla();
		}

		/// <summary>
		/// Control初始化
		/// </summary>
		void Init()
		{
			//db配置项
			tscbDbList.Items.Clear();
			var dbList = DatabaseConfig.Instance?.DataBaseList;
			if (dbList != null)
			{
				foreach (var item in dbList)
				{
					this.tscbDbList.Items.Add(item.Key);
				}
				tscbDbList.SelectedIndex = 0;
			}
			//数据绑定方式
			tscbDataStyle.Items.Clear();
			tscbDataStyle.Items.Add("无");
			tscbDataStyle.Items.Add("TxoooMapping");
			tscbDataStyle.Items.Add("LinqMapping");
			tscbDataStyle.Items.Add("SqlSugarMapping");
			tscbDataStyle.SelectedIndex = 1;

			//输出方式
			tscbOutputStyle.Items.Clear();
			tscbOutputStyle.Items.Add("无");
			tscbOutputStyle.Items.Add("Ext");
			tscbOutputStyle.Items.Add("Json");
			tscbOutputStyle.Items.Add("Ext&Json");
			tscbOutputStyle.SelectedIndex = 0;
		}

		/// <summary>
		/// 事件绑定
		/// </summary>
		void InitEvent()
		{
			tscbDbList.SelectedIndexChanged += Tscb_SelectedIndexChanged;
			tscbOutputStyle.SelectedIndexChanged += Tscb_SelectedIndexChanged;
			tscbDataStyle.SelectedIndexChanged += Tscb_SelectedIndexChanged;
			lbDbObjectList.SelectedIndexChanged += LbDbObjectList_SelectedIndexChanged;
		}
		/// <summary>
		/// 初始化 Scintilla
		/// </summary>
		void InitScintilla()
		{
			TextArea.WrapMode = WrapMode.None;
			TextArea.IndentationGuides = IndentView.LookBoth;

			InitColors();
			InitSyntaxColoring();
			InitNumberMargin();
			InitBookmarkMargin();
			InitCodeFolding();
		}


		/// <summary>
		/// 数据项发生变更时执行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LbDbObjectList_SelectedIndexChanged(object sender, EventArgs e)
		{
			var control = sender as ListBox;
			if (control.SelectedItem != null)
			{
				try
				{
					//表或视图名信息
					DbTableInfo = control.SelectedItem as DbTableInfo;
					TextArea.Text = GenerateEntityCode();
				}
				catch
				{
					MessageBox.Show("获取数据错误");
				}
			}
		}

		/// <summary>
		/// Commbox SelectIndex更改后
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Tscb_SelectedIndexChanged(object sender, EventArgs e)
		{
			var control = sender as ToolStripComboBox;
			if (control.Name == "tscbDbList")
			{
				try
				{
					DatabaseInfo = DatabaseConfig.Instance.GetDatabaseInfoByCache(control.SelectedItem.ToString());
					lbDbObjectList.DataSource = null;
					lbDbObjectList.DataSource = TableViewList.OrderBy(m => m.DbObjectType).ThenBy(m => m.Name).ToList();
					lbDbObjectList.DisplayMember = "Name";
				}
				catch (Exception ex)
				{
					MessageBox.Show("数据连接错误[{0}]，请检查配置文件".FormatWith(ex.Message));
				}
			}
			if (control.Name.ToLower() == "tscbdatastyle")
			{
				DataStyle = tscbDataStyle.SelectedIndex;
			}
			if (control.Name.ToLower() == "tscboutputstyle")
			{
				OutputStyle = tscbOutputStyle.SelectedIndex;
			}
		}
		 

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			new JsonForm().Show(this);
		}
	}
}
