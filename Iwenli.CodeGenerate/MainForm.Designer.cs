namespace Iwenli.CodeGenerate
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbDbObjectList = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tscbGenerateStyle = new System.Windows.Forms.ToolStripComboBox();
            this.tscbDbList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtbEntity = new System.Windows.Forms.RichTextBox();
            this.rtbView = new System.Windows.Forms.RichTextBox();
            this.rtbDb = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscbGenerateStyle,
            this.toolStripLabel4,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.toolStripLabel2,
            this.tscbDbList});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(572, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbDbObjectList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(572, 425);
            this.splitContainer1.SplitterDistance = 123;
            this.splitContainer1.TabIndex = 1;
            // 
            // lbDbObjectList
            // 
            this.lbDbObjectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDbObjectList.FormattingEnabled = true;
            this.lbDbObjectList.ItemHeight = 12;
            this.lbDbObjectList.Location = new System.Drawing.Point(0, 0);
            this.lbDbObjectList.Name = "lbDbObjectList";
            this.lbDbObjectList.Size = new System.Drawing.Size(123, 425);
            this.lbDbObjectList.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(445, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbEntity);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(437, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "实体";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtbView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(437, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "视图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1.Text = "生成方式：";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(80, 22);
            this.toolStripLabel2.Text = "选择数据库：";
            // 
            // tscbGenerateStyle
            // 
            this.tscbGenerateStyle.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.tscbGenerateStyle.Name = "tscbGenerateStyle";
            this.tscbGenerateStyle.Size = new System.Drawing.Size(81, 25);
            // 
            // tscbDbList
            // 
            this.tscbDbList.DropDownWidth = 120;
            this.tscbDbList.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.tscbDbList.Name = "tscbDbList";
            this.tscbDbList.Size = new System.Drawing.Size(120, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(12, 22);
            this.toolStripLabel3.Text = " ";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel4.Text = "  ";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtbDb);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(437, 399);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "数据库";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtbEntity
            // 
            this.rtbEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbEntity.Location = new System.Drawing.Point(3, 3);
            this.rtbEntity.Name = "rtbEntity";
            this.rtbEntity.Size = new System.Drawing.Size(431, 393);
            this.rtbEntity.TabIndex = 0;
            this.rtbEntity.Text = "";
            // 
            // rtbView
            // 
            this.rtbView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbView.Location = new System.Drawing.Point(3, 3);
            this.rtbView.Name = "rtbView";
            this.rtbView.Size = new System.Drawing.Size(431, 393);
            this.rtbView.TabIndex = 1;
            this.rtbView.Text = "";
            // 
            // rtbDb
            // 
            this.rtbDb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDb.Location = new System.Drawing.Point(3, 3);
            this.rtbDb.Name = "rtbDb";
            this.rtbDb.Size = new System.Drawing.Size(431, 393);
            this.rtbDb.TabIndex = 2;
            this.rtbDb.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "代码生成";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbDbObjectList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox tscbGenerateStyle;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox tscbDbList;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox rtbEntity;
        private System.Windows.Forms.RichTextBox rtbView;
        private System.Windows.Forms.RichTextBox rtbDb;
    }
}

