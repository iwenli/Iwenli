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
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.tscbDbList = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tscbDataStyle = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
			this.tscbOutputStyle = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripLabel5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lbDbObjectList = new System.Windows.Forms.ListBox();
			this.pb = new System.Windows.Forms.ProgressBar();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpRepository = new System.Windows.Forms.TabPage();
			this.tpService = new System.Windows.Forms.TabPage();
			this.scintillaRepository = new ScintillaNET.Scintilla();
			this.scintillaService = new ScintillaNET.Scintilla();
			this.tpCache = new System.Windows.Forms.TabPage();
			this.scintillaCache = new ScintillaNET.Scintilla();
			this.tpEntity = new System.Windows.Forms.TabPage();
			this.scintillaEntity = new ScintillaNET.Scintilla();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tpRepository.SuspendLayout();
			this.tpService.SuspendLayout();
			this.tpCache.SuspendLayout();
			this.tpEntity.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tscbDbList,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tscbDataStyle,
            this.toolStripSeparator1,
            this.toolStripLabel6,
            this.tscbOutputStyle,
            this.toolStripLabel3,
            this.toolStripLabel5,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(800, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(80, 22);
			this.toolStripLabel2.Text = "选择数据库：";
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
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
			this.toolStripLabel1.Text = "数据：";
			// 
			// tscbDataStyle
			// 
			this.tscbDataStyle.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
			this.tscbDataStyle.Name = "tscbDataStyle";
			this.tscbDataStyle.Size = new System.Drawing.Size(120, 25);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel6
			// 
			this.toolStripLabel6.Name = "toolStripLabel6";
			this.toolStripLabel6.Size = new System.Drawing.Size(44, 22);
			this.toolStripLabel6.Text = "输出：";
			// 
			// tscbOutputStyle
			// 
			this.tscbOutputStyle.Name = "tscbOutputStyle";
			this.tscbOutputStyle.Size = new System.Drawing.Size(80, 25);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(12, 22);
			this.toolStripLabel3.Text = " ";
			// 
			// toolStripLabel5
			// 
			this.toolStripLabel5.Name = "toolStripLabel5";
			this.toolStripLabel5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(86, 22);
			this.toolStripButton1.Text = "Json实体生成";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
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
			this.splitContainer1.Panel1.Controls.Add(this.pb);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Size = new System.Drawing.Size(800, 425);
			this.splitContainer1.SplitterDistance = 171;
			this.splitContainer1.TabIndex = 1;
			// 
			// lbDbObjectList
			// 
			this.lbDbObjectList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbDbObjectList.FormattingEnabled = true;
			this.lbDbObjectList.ItemHeight = 12;
			this.lbDbObjectList.Location = new System.Drawing.Point(0, 0);
			this.lbDbObjectList.Name = "lbDbObjectList";
			this.lbDbObjectList.Size = new System.Drawing.Size(171, 409);
			this.lbDbObjectList.TabIndex = 0;
			// 
			// pb
			// 
			this.pb.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.pb.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pb.Enabled = false;
			this.pb.Location = new System.Drawing.Point(0, 409);
			this.pb.Name = "pb";
			this.pb.Size = new System.Drawing.Size(171, 16);
			this.pb.TabIndex = 3;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpEntity);
			this.tabControl1.Controls.Add(this.tpRepository);
			this.tabControl1.Controls.Add(this.tpService);
			this.tabControl1.Controls.Add(this.tpCache);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(625, 425);
			this.tabControl1.TabIndex = 0;
			// 
			// tpRepository
			// 
			this.tpRepository.Controls.Add(this.scintillaRepository);
			this.tpRepository.Location = new System.Drawing.Point(4, 22);
			this.tpRepository.Name = "tpRepository";
			this.tpRepository.Size = new System.Drawing.Size(617, 399);
			this.tpRepository.TabIndex = 1;
			this.tpRepository.Text = "仓储";
			this.tpRepository.UseVisualStyleBackColor = true;
			// 
			// tpService
			// 
			this.tpService.Controls.Add(this.scintillaService);
			this.tpService.Location = new System.Drawing.Point(4, 22);
			this.tpService.Name = "tpService";
			this.tpService.Size = new System.Drawing.Size(617, 399);
			this.tpService.TabIndex = 2;
			this.tpService.Text = "服务";
			this.tpService.UseVisualStyleBackColor = true;
			// 
			// scintillaRepository
			// 
			this.scintillaRepository.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaRepository.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaRepository.Location = new System.Drawing.Point(0, 0);
			this.scintillaRepository.Name = "scintillaRepository";
			this.scintillaRepository.Size = new System.Drawing.Size(617, 399);
			this.scintillaRepository.TabIndex = 2;
			// 
			// scintillaService
			// 
			this.scintillaService.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaService.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaService.Location = new System.Drawing.Point(0, 0);
			this.scintillaService.Name = "scintillaService";
			this.scintillaService.Size = new System.Drawing.Size(617, 399);
			this.scintillaService.TabIndex = 3;
			// 
			// tpCache
			// 
			this.tpCache.Controls.Add(this.scintillaCache);
			this.tpCache.Location = new System.Drawing.Point(4, 22);
			this.tpCache.Name = "tpCache";
			this.tpCache.Size = new System.Drawing.Size(617, 399);
			this.tpCache.TabIndex = 3;
			this.tpCache.Text = "缓存";
			this.tpCache.UseVisualStyleBackColor = true;
			// 
			// scintillaCache
			// 
			this.scintillaCache.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaCache.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaCache.Location = new System.Drawing.Point(0, 0);
			this.scintillaCache.Name = "scintillaCache";
			this.scintillaCache.Size = new System.Drawing.Size(617, 399);
			this.scintillaCache.TabIndex = 3;
			// 
			// tpEntity
			// 
			this.tpEntity.Controls.Add(this.scintillaEntity);
			this.tpEntity.Location = new System.Drawing.Point(4, 22);
			this.tpEntity.Name = "tpEntity";
			this.tpEntity.Padding = new System.Windows.Forms.Padding(3);
			this.tpEntity.Size = new System.Drawing.Size(617, 399);
			this.tpEntity.TabIndex = 0;
			this.tpEntity.Text = "实体";
			this.tpEntity.UseVisualStyleBackColor = true;
			// 
			// scintillaEntity
			// 
			this.scintillaEntity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaEntity.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaEntity.Location = new System.Drawing.Point(3, 3);
			this.scintillaEntity.Name = "scintillaEntity";
			this.scintillaEntity.Size = new System.Drawing.Size(611, 393);
			this.scintillaEntity.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
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
			this.tpRepository.ResumeLayout(false);
			this.tpService.ResumeLayout(false);
			this.tpCache.ResumeLayout(false);
			this.tpEntity.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lbDbObjectList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox tscbDataStyle;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox tscbDbList;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripLabel5;
		private System.Windows.Forms.ToolStripLabel toolStripLabel6;
		private System.Windows.Forms.ToolStripComboBox tscbOutputStyle;
		private System.Windows.Forms.ProgressBar pb;
		private System.Windows.Forms.TabPage tpRepository;
		private System.Windows.Forms.TabPage tpService;
		private ScintillaNET.Scintilla scintillaRepository;
		private ScintillaNET.Scintilla scintillaService;
		private System.Windows.Forms.TabPage tpEntity;
		private ScintillaNET.Scintilla scintillaEntity;
		private System.Windows.Forms.TabPage tpCache;
		private ScintillaNET.Scintilla scintillaCache;
	}
}

