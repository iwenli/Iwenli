using System;
using System.Windows.Forms;

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
            //this.txtOutput.Text = EncryptHelper.StringToBase16(this.txtInput.Text.Trim());

            //using (DataHelper helper = DataHelper.GetDataHelper("TxoooAgent"))
            //{
            //    string _sql = @"SELECT TOP 10 user_id,user_name,password,nick_name,show_phone FROM dbo.sales_user WHERE USER_ID > 10";
            //    var _dt = helper.SqlGetDataTable(_sql);
            //    txtOutput.Text = JsonConvert.SerializeObject(_dt);
            //}

            //using (DataHelper helper = DataHelper.GetDataHelper("Iwenli"))
            //{
            //    //string _sql = @"SELECT * FROM [Customer] order by [CustomerId] desc";
            //    //var _dt = helper.SqlGetDataTable(_sql);

            //    //var baseList = helper.GetDatabaseList();
            //    //txtOutput.Text = JsonConvert.SerializeObject(helper.GetDatabaseList());
            //}

            var text  = txtInput.Text.ToString().AESEncrypt();
            this.txtOutput.Text = text;
        }
    }
}
