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
        /// <summary>
        /// 生成方式
        /// </summary>
        int GenerateStyle = 2;
        /// <summary>
        /// 数据库名称
        /// </summary>
        string DatabaseName = string.Empty;
        /// <summary>
        /// 私有字段前缀
        /// </summary>
        string FieldsPrefix = "_";
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
            //生成方式
            tscbGenerateStyle.Items.Clear();
            tscbGenerateStyle.Items.Add("方式1");
            tscbGenerateStyle.Items.Add("方式2");
            tscbGenerateStyle.SelectedIndex = 0;
        }

        /// <summary>
        /// 事件绑定
        /// </summary>
        void InitEvent()
        {
            tscbDbList.SelectedIndexChanged += Tscb_SelectedIndexChanged;
            tscbGenerateStyle.SelectedIndexChanged += Tscb_SelectedIndexChanged;
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
                    using (DataHelper helper = DataHelper.GetDataHelper(DatabaseName))
                    {
                        //表或视图名
                        var name = control.SelectedItem.ToString();

                        //@xtype = 'U'  | 'V'   [表|试图]
                        StringBuilder sqlSb = new StringBuilder();
                        sqlSb.Append("SELECT fieldName = a.name,fieldSummary =isnull(g.[value],''),fieldType = b.name ");
                        sqlSb.Append("FROM dbo.syscolumns a ");
                        sqlSb.Append("left join dbo.systypes b on a.xusertype=b.xusertype ");
                        sqlSb.Append("inner join dbo.sysobjects d on a.id=d.id and d.xtype=@xtype and d.name<>'dtproperties' ");
                        sqlSb.Append("left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id ");
                        sqlSb.Append("WHERE d.name = @name");

                        helper.SpFileValue["@name"] = name;
                        helper.SpFileValue["@xtype"] = name.ToUpper().IndexOf("VIEW") > -1 ? "V" : "U";

                        var dt = helper.SqlGetDataTable(sqlSb.ToString(), helper.SpFileValue);
                        dt.TableName = name;

                        if (name.ToUpper().IndexOf("VIEW") == -1)
                        {
                            GenerateEntityCode(dt);
                            GenerateDbCode(dt);
                        }
                        GenerateViewCode(dt);
                    }
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
                    DatabaseName = control.SelectedItem.ToString();
                    DataTable dt;
                    using (DataHelper helper = DataHelper.GetDataHelper(DatabaseName))
                    {
                        string sqlStr = "Select Name FROM SysObjects Where XType='U' or XType='V' ORDER BY Name;";
                        dt = helper.SqlGetDataTable(sqlStr);
                    }
                    lbDbObjectList.Items.Clear();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        lbDbObjectList.Items.Add(dataRow["Name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据连接错误[{0}]，请检查配置文件".FormatWith(ex.Message));
                }
            }
            if (control.Name == "tscbGenerateStyle")
            {
                if (control.SelectedItem.ToString().Equals("方式1"))
                {
                    GenerateStyle = 2;
                }
                else
                {
                    GenerateStyle = 1;
                }
            }
        }

        #region 生成代码的方法
        private void GenerateDbCode(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("---------应用于添加和更新--------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("using (DataHelper helper = DataHelper.GetDataHelper(\"{0}\")){1}", DatabaseName, Environment.NewLine);
            stringBuilder.Append("{" + Environment.NewLine);
            foreach (DataRow dataRow in dt.Rows)
            {
                stringBuilder.Append(string.Format("\thelper.SpFileValue[\"@{0}\"] = {1};\r\n", dataRow["fieldName"].ToString(), dt.TableName + "Info." + this.GetFormatName(dataRow["fieldName"].ToString())));
            }
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(string.Format("\treturn helper.SpExecute(\"{0}\");\r\n", "SP_Insert" + GetFormatName(dt.TableName)));
            stringBuilder.Append("}" + Environment.NewLine);
            stringBuilder.AppendFormat("{0}{0}---------添加存储过程--------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("INSERT INTO [dbo].[{0}]{1}", dt.TableName, Environment.NewLine);
            stringBuilder.Append("(");
            foreach (DataRow dataRow in dt.Rows)
            {
                stringBuilder.Append("[" + dataRow["fieldName"].ToString() + "] ,");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.AppendFormat("){0}", Environment.NewLine);
            stringBuilder.Append(" VALUES(");
            foreach (DataRow dataRow in dt.Rows)
            {
                stringBuilder.Append("@" + dataRow["fieldName"].ToString() + " ,");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.AppendFormat("){0}", Environment.NewLine);
            stringBuilder.AppendFormat("{0}{0}---------更新存储过程--------{0}{0}", Environment.NewLine);
            stringBuilder.AppendFormat("UPDATE [dbo].[{0}]{1}", dt.TableName, Environment.NewLine);
            stringBuilder.Append(" SET");
            foreach (DataRow dataRow in dt.Rows)
            {
                stringBuilder.Append(string.Concat(new string[]
                {
                    " [",
                    dataRow["fieldName"].ToString(),
                    "]=@",
                    dataRow["fieldName"].ToString(),
                    " ,"
                }));
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.AppendFormat("{0}WHERE{0}", Environment.NewLine);
            this.rtbDb.Text = stringBuilder.ToString();
        }
        private void GenerateEntityCode(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Concat(new string[]
            {
                "[TxTable(Name = \"",
                dt.TableName,
                "\", Base = \"",
                DatabaseName,
                "\")]\r\n"
            }));
            stringBuilder.Append("public class " + this.GetFormatName(dt.TableName) + EntitySuffix + "\r\n{\r\n");

            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            if (GenerateStyle == 1)
            {
                stringBuilder1.AppendFormat("\t#region 字段{0}", Environment.NewLine);
            }
            stringBuilder2.AppendFormat("\t#region 属性{0}", Environment.NewLine);
            int num = 0;
            foreach (DataRow dataRow in dt.Rows)
            {
                string text = dataRow["fieldName"].ToString();
                char c = text[0];
                if (char.IsLower(c))
                {
                    num++;
                }
            }
            foreach (DataRow dataRow in dt.Rows)
            {
                string text = dataRow["fieldName"].ToString();
                string str = dataRow["fieldSummary"].ToString();
                string netType = this.GetNetType(dataRow["fieldType"].ToString());
                string text2 = (num > 3) ? this.GetFormatName(text) : text;
                stringBuilder2.Append("\r\n/// <summary>\r\n");
                stringBuilder2.Append("/// " + str + "\r\n");
                stringBuilder2.Append("/// </summary>\r\n");
                if (GenerateStyle == 2)
                {
                    stringBuilder2.AppendFormat("[Column(Name = \"{0}\")]\r\n", text);
                }
                stringBuilder2.Append(string.Concat(new string[]
                {
                        "public ",
                        netType,
                        " ",
                        text2
                }));

                if (GenerateStyle == 1)
                {
                    string extName = this.GetFirstLower(text2);
                    string privateName = this.GetPrivateName(extName);
                    stringBuilder1.Append(string.Format("private {0} {1};\r\n", netType, privateName));
                    stringBuilder2.Append("\r\n{\r\n");
                    stringBuilder2.Append("\tget { return " + privateName + "; }\r\n");
                    stringBuilder2.Append("\tset { " + privateName + " = value; }\r\n");
                    stringBuilder2.Append("}\r\n");
                }
                else
                {
                    stringBuilder2.Append("{get;set;}\r\n");
                }
            }
            
            if (GenerateStyle == 1)
            {
                stringBuilder1.Append("#endregion");
            }
            stringBuilder2.Append("#endregion");
            this.rtbEntity.Text = stringBuilder.ToString() 
                + stringBuilder1.ToString().Replace("\r\n", "\r\n\t")
                + "\r\n"
                + stringBuilder2.ToString().Replace("\r\n", "\r\n\t")
                + "\r\n}";
        }
        private void GenerateViewCode(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            int num = 0;
            foreach (DataRow dataRow in dt.Rows)
            {
                string text = dataRow["fieldName"].ToString();
                char c = text[0];
                if (char.IsLower(c))
                {
                    num++;
                }
            }
            string text2 = "";
            foreach (DataRow dataRow in dt.Rows)
            {
                string text = dataRow["fieldName"].ToString();
                text2 += ((num > 3) ? string.Concat(new string[]
                {
                    "[",
                    text,
                    "] as ",
                    this.GetFormatName(text),
                    ","
                }) : ("[" + text + "] ,"));
            }
            stringBuilder.Append(text2.TrimEnd(new char[]
            {
                ','
            }));
            stringBuilder.Append("\r\nFROM [dbo].[" + dt.TableName + "]");
            this.rtbView.Text = stringBuilder.ToString();
        }
        #endregion

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
