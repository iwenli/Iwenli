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
		public Action<string, string> GetEntityName;

		public DiaForm(string entityName, string businessName)
		{
			InitializeComponent();
			txtBusiness.Text = txtEntity.Text = entityName;
			btnCancel.Click += (s, e) =>
			{
				DialogResult = DialogResult.Cancel;
				Close();
			};

			btnOk.Click += (s, e) =>
			{
				GetEntityName(txtEntity.Text.Trim(), txtBusiness.Text.Trim());
				DialogResult = DialogResult.OK;
				Close();
			};
		}
	}
}
