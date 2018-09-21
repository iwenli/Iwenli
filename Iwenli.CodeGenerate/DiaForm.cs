using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iwenli.CodeGenerate
{
	public partial class DiaForm : Form
	{
		public Action<string> GetEntityName;

		public DiaForm(string entityName)
		{
			InitializeComponent();
			txtEntity.Text = entityName;
			btnOk.Click += (s, e) =>
			{
				GetEntityName(txtEntity.Text.Trim());
				DialogResult = DialogResult.OK;
				Close();
			};
		}
	}
}
