using Iwenli.CodeGenerate.Extension;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace Iwenli.CodeGenerate
{
	public partial class JsonForm : Form
	{
		public JsonForm()
		{
			InitializeComponent();
			Load += JsonForm_Load;
		}

		private void JsonForm_Load(object sender, EventArgs e)
		{
			InitScintilla();
			InitEvent();
		}

		private void InitEvent()
		{
			btnJsonBeautify.Click += (s, e) =>
			{
				scintillaJson.Text = scintillaJson.Text.JsonBeautify();
			};
			btnJsonCompact.Click += (s, e) =>
			{
				scintillaJson.Text = scintillaJson.Text.JsonCompact();
			};
			btnGennrate.Click += (s, e) =>
			{
				lblDoneClipboard.Text = "";
				var gen = Prepare();
				if (gen == null) return;
				try
				{
					gen.TargetFolder = null;
					gen.SingleFile = true;
					using (var sw = new StringWriter())
					{
						gen.OutputStream = sw;
						gen.GenerateClasses();
						sw.Flush();
						var lastGeneratedString = sw.ToString();
						Clipboard.SetText(lastGeneratedString);
						scintillaClass.Text = lastGeneratedString;
						lblDoneClipboard.Text = "生成成功，已经拷贝到剪贴板";
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "Unable to generate the code: " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			};
		}
		private JsonClassGenerator Prepare()
		{
			if (scintillaJson.Text == string.Empty)
			{
				MessageBox.Show(this, "JSON源不能为空.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				scintillaJson.Focus();
				return null;
			}

			var gen = new JsonClassGenerator
			{
				Example = scintillaJson.Text,
				InternalVisibility = false,
				CodeWriter = new CSharpCodeWriter(),
				DeduplicateClasses = false,
				TargetFolder = "",
				UseProperties = true,
				MainClass = "Class1",
				SortMemberFields = true,
				UsePascalCase = true,
				UseNestedClasses = false,
				ApplyObfuscationAttributes = false,
				SingleFile = false,
				ExamplesInDocumentation = false,
				Namespace = null,
				NoHelperClass = false,
				SecondaryNamespace = null,
			};
			gen.ExplicitDeserialization = false;

			return gen;
		}
		/// <summary>
		/// 初始化 Scintilla
		/// </summary>
		void InitScintilla()
		{
			scintillaJson.WrapMode = scintillaClass.WrapMode = WrapMode.None;
			scintillaJson.IndentationGuides = scintillaClass.IndentationGuides = IndentView.LookBoth;

			scintillaJson.Styler = new JsonStyler();
			scintillaClass.Styler = new CSharpStyler();  // The thing that sets Json syntax highlighting
			scintillaJson.SetSavePoint();                         // Show the buffer has not been changed.
			scintillaClass.SetSavePoint();
		}
	}
}
