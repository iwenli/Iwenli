﻿using System;
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
using Newtonsoft.Json;
using Iwenli.Data;

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

            using (DataHelper helper = DataHelper.GetDataHelper("Iwenli"))
            {
                //string _sql = @"SELECT * FROM [Customer] order by [CustomerId] desc";
                //var _dt = helper.SqlGetDataTable(_sql);
                txtOutput.Text = JsonConvert.SerializeObject(helper.GetDatabaseList());
            }
        }
    }
}
