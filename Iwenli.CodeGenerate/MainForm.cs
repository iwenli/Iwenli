using Iwenli.CodeGenerate.Entity;
using Iwenli.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iwenli.CodeGenerate
{
	public partial class MainForm : Form
	{

		int DataStyle = 0;
		int OutputStyle = 0;

		/// <summary>
		/// 数据库链接信息
		/// </summary>
		DatabaseInfo DatabaseInfo;

		/// <summary>
		/// 表 或 视图 名称
		/// </summary>
		DbTableInfo DbTableInfo;

		/// <summary>
		/// 私有字段前缀
		/// </summary>
		string FieldsPrefix = "m_";
		/// <summary>
		/// 实体后缀
		/// </summary>
		string EntitySuffix = "Info";

		public MainForm()
		{
			InitializeComponent();
			Load += MainForm_Load;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			InitEvent();
			Init();
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
					GenerateEntityCode();
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

		#region 生成代码的方法
		private void GenerateEntityCode()
		{
			StringBuilder _codeSb = new StringBuilder();
			_codeSb.AppendLineDescription(DbTableInfo.Description);
			var _className = GetFormatName(DbTableInfo.Name) + EntitySuffix;
			if (DataStyle == 3)
			{
				_codeSb.AppendLine($"[SugarTable(\"{DbTableInfo.Name}\")]");
			}

			_codeSb.AppendLine($"public class {_className}");
			_codeSb.AppendLine("{");
			#region 属性
			_codeSb.AppendLineWithIndentation("#region 属性");
			foreach (var column in ColumnList)
			{
				var _columnName = GetFormatName(column.DbColumnName);
				var _netType = GetNetType(column.DataType);

				_codeSb.AppendLineDescription(column.ColumnDescription, "\t");
				if (DataStyle == 1)
				{
					_codeSb.AppendLineWithIndentation($"[DataRowEntityFile(\"{column.DbColumnName}\", typeof({_netType}))]");
				}
				else if (DataStyle == 2)
				{
					_codeSb.AppendLineWithIndentation($"[Column(Name = \"{column.DbColumnName}\")]");
				}
				else if (DataStyle == 3)
				{
					_codeSb.AppendLineWithIndentation($"[SugarColumn(ColumnName = \"{column.DbColumnName}\")]");
				}
				if (OutputStyle == 1)
				{
					_codeSb.AppendLineWithIndentation($"[ExtColumnModel(\"{column.ColumnDescription}\", \"{GetFirstLower(_columnName)}\", false)]");
				}
				else if (OutputStyle == 2)
				{
					_codeSb.AppendLineWithIndentation($"[JsonProperty(\"{column.DbColumnName}\")]");
				}
				else if (OutputStyle == 3)
				{
					_codeSb.AppendLineWithIndentation($"[ExtColumnModel(\"{column.ColumnDescription}\", \"{GetFirstLower(_columnName)}\", false)]");
					_codeSb.AppendLineWithIndentation($"[JsonProperty(\"{column.DbColumnName}\")]");
				}
				_codeSb.AppendLineWithIndentation($"public {_netType} {_columnName} " + "{ get; set; }");
			}
			_codeSb.AppendLineWithIndentation("#endregion");
			#endregion
			_codeSb.AppendLine();
			#region 工厂方法
			_codeSb.AppendLineWithIndentation("#region 工厂方法");
			if (DataStyle == 1)
			{
				#region Query
				_codeSb.AppendLineWithIndentation($"public static List<{_className}> Get(IEnumerable<long> idList = null)");
				_codeSb.AppendLineWithIndentation("{");
				_codeSb.AppendLineWithIndentation($"using (TxDataHelper helper = TxDataHelper.GetDataHelper(\"{DatabaseInfo.Name}\"))", "\t\t");
				_codeSb.AppendLineWithIndentation("{", "\t\t");

				_codeSb.AppendLineWithIndentation($"StringBuilder _sql = new StringBuilder(\"SELECT * FROM {DbTableInfo.Name} WITH(NOLOCK)\");", "\t\t\t");
				_codeSb.AppendLineWithIndentation($"if (idList != null && idList.Count() > 0)", "\t\t\t");
				_codeSb.AppendLineWithIndentation("{", "\t\t\t");
				_codeSb.AppendLineWithIndentation("_sql.Append($\" WHERE id IN({ string.Join(\",\", idList)})\");", "\t\t\t\t");
				_codeSb.AppendLineWithIndentation("}", "\t\t\t");
				_codeSb.AppendLineWithIndentation("var _dt = helper.SqlGetDataTable(_sql.ToString());", "\t\t\t");
				_codeSb.AppendLineWithIndentation($"return DataEntityHelper.GetDataRowEntityList<{_className}>(_dt);", "\t\t\t");

				_codeSb.AppendLineWithIndentation("}", "\t\t");
				_codeSb.AppendLineWithIndentation("}");
				#endregion

				#region Insert
				_codeSb.AppendLineWithIndentation($"public static bool Insert({_className} model)");
				_codeSb.AppendLineWithIndentation("{");
				_codeSb.AppendLineWithIndentation($"using (TxDataHelper helper = TxDataHelper.GetDataHelper(\"{DatabaseInfo.Name}\"))", "\t\t");
				_codeSb.AppendLineWithIndentation("{", "\t\t");
				var _conlmus = ColumnList.Where(m => !m.IsPrimarykey).ToList();  //非主键列
				_codeSb.AppendLineWithIndentation($"var _sql = \"INSERT INTO {DbTableInfo.Name}({string.Join(",", _conlmus.Select(m => m.DbColumnName))}) VALUES({string.Join(",", _conlmus.Select(m => "@" + m.DbColumnName))})\";", "\t\t\t");
				foreach (var col in _conlmus)
				{
					_codeSb.AppendLineWithIndentation($"helper.SpFileValue[\"@{col.DbColumnName}\"] = model.{GetFormatName(col.DbColumnName)};", "\t\t\t");
				}
				_codeSb.AppendLineWithIndentation("return helper.SqlExecute(_sql.ToString(),helper.SpFileValue) == 1;", "\t\t\t");

				_codeSb.AppendLineWithIndentation("}", "\t\t");
				_codeSb.AppendLineWithIndentation("}");
				#endregion
			}
			_codeSb.AppendLineWithIndentation("#endregion");
			#endregion

			_codeSb.AppendLine("}");

			rtbEntity.Text = _codeSb.ToString();
		}
		#endregion

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			new JsonForm().Show(this);
		}
	}
}
