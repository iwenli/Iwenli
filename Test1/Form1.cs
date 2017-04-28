using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Iwenli.Text;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //this.txtOutput.Text = EncryptHelper.AESDecrypt(this.txtInput.Text.Trim());
            //MessageBox.Show(EncryptHelper.AESDecrypt(this.txtInput.Text.Trim()));
            this.txtOutput.Text = EncryptHelper.StringToBase16(this.txtInput.Text.Trim());
            
        }
    }
}
