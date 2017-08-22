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
using Iwenli;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 50; i++)
            {
                this.LogInfo(i.ToString());
                if (i == 49) {
                    try
                    {
                      var j =   i / 0;
                    }
                    catch (Exception ex)
                    {
                        this.LogError(ex.Message,ex); 
                    }
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //this.txtOutput.Text = EncryptHelper.AESDecrypt(this.txtInput.Text.Trim());
            //MessageBox.Show(EncryptHelper.AESDecrypt(this.txtInput.Text.Trim()));
            this.txtOutput.Text = EncryptHelper.StringToBase16(this.txtInput.Text.Trim());
            
        }
    }
}
