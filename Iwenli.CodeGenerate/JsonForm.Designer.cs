namespace Iwenli.CodeGenerate
{
	partial class JsonForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.scintillaJson = new EasyScintilla.SimpleEditor();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.scintillaClass = new EasyScintilla.SimpleEditor();
			this.btnJsonBeautify = new System.Windows.Forms.Button();
			this.btnJsonCompact = new System.Windows.Forms.Button();
			this.btnGennrate = new System.Windows.Forms.Button();
			this.lblDoneClipboard = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(800, 277);
			this.panel1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.scintillaJson);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(800, 277);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Json源";
			// 
			// scintillaJson
			// 
			this.scintillaJson.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaJson.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaJson.Location = new System.Drawing.Point(3, 17);
			this.scintillaJson.Name = "scintillaJson";
			this.scintillaJson.Size = new System.Drawing.Size(794, 257);
			this.scintillaJson.Styler = null;
			this.scintillaJson.TabIndex = 5;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.lblDoneClipboard);
			this.panel2.Controls.Add(this.btnGennrate);
			this.panel2.Controls.Add(this.btnJsonCompact);
			this.panel2.Controls.Add(this.btnJsonBeautify);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 277);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(800, 40);
			this.panel2.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.groupBox2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 317);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(800, 394);
			this.panel3.TabIndex = 2;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.scintillaClass);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(800, 394);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Class目标";
			// 
			// scintillaClass
			// 
			this.scintillaClass.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scintillaClass.Lexer = ScintillaNET.Lexer.Cpp;
			this.scintillaClass.Location = new System.Drawing.Point(3, 17);
			this.scintillaClass.Name = "scintillaClass";
			this.scintillaClass.Size = new System.Drawing.Size(794, 374);
			this.scintillaClass.Styler = null;
			this.scintillaClass.TabIndex = 5;
			// 
			// btnJsonBeautify
			// 
			this.btnJsonBeautify.Location = new System.Drawing.Point(13, 7);
			this.btnJsonBeautify.Name = "btnJsonBeautify";
			this.btnJsonBeautify.Size = new System.Drawing.Size(84, 23);
			this.btnJsonBeautify.TabIndex = 0;
			this.btnJsonBeautify.Text = "JSON美化(&B)";
			this.btnJsonBeautify.UseVisualStyleBackColor = true;
			// 
			// btnJsonCompact
			// 
			this.btnJsonCompact.Location = new System.Drawing.Point(103, 7);
			this.btnJsonCompact.Name = "btnJsonCompact";
			this.btnJsonCompact.Size = new System.Drawing.Size(85, 23);
			this.btnJsonCompact.TabIndex = 1;
			this.btnJsonCompact.Text = "JSON压缩(&C)";
			this.btnJsonCompact.UseVisualStyleBackColor = true;
			// 
			// btnGennrate
			// 
			this.btnGennrate.Location = new System.Drawing.Point(217, 7);
			this.btnGennrate.Name = "btnGennrate";
			this.btnGennrate.Size = new System.Drawing.Size(91, 23);
			this.btnGennrate.TabIndex = 2;
			this.btnGennrate.Text = "生成C#实体(&G)";
			this.btnGennrate.UseVisualStyleBackColor = true;
			// 
			// lblDoneClipboard
			// 
			this.lblDoneClipboard.AutoSize = true;
			this.lblDoneClipboard.ForeColor = System.Drawing.SystemColors.MenuHighlight;
			this.lblDoneClipboard.Location = new System.Drawing.Point(332, 14);
			this.lblDoneClipboard.Name = "lblDoneClipboard";
			this.lblDoneClipboard.Size = new System.Drawing.Size(0, 12);
			this.lblDoneClipboard.TabIndex = 3;
			// 
			// JsonForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 711);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "JsonForm";
			this.ShowIcon = false;
			this.Text = "JSON解析生成实体工具";
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private EasyScintilla.SimpleEditor scintillaJson;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.GroupBox groupBox2;
		private EasyScintilla.SimpleEditor scintillaClass;
		private System.Windows.Forms.Button btnJsonCompact;
		private System.Windows.Forms.Button btnJsonBeautify;
		private System.Windows.Forms.Button btnGennrate;
		private System.Windows.Forms.Label lblDoneClipboard;
	}
}