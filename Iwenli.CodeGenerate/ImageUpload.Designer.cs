namespace Iwenli.CodeGenerate
{
	partial class ImageUpload
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageUpload));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.picShow = new System.Windows.Forms.PictureBox();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.btnSelect);
			this.splitContainer1.Panel1.Controls.Add(this.picShow);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtLog);
			this.splitContainer1.Size = new System.Drawing.Size(474, 339);
			this.splitContainer1.SplitterDistance = 158;
			this.splitContainer1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.Color.DarkMagenta;
			this.label1.Location = new System.Drawing.Point(0, 84);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(3);
			this.label1.Size = new System.Drawing.Size(156, 94);
			this.label1.TabIndex = 3;
			this.label1.Text = "\r\n提示：\r\n1.Ctrl+C可以直接上传剪贴板中的图片url或者图片\r\n2.选择图片从本地上传\r\n";
			// 
			// btnSelect
			// 
			this.btnSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnSelect.Location = new System.Drawing.Point(0, 0);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(156, 84);
			this.btnSelect.TabIndex = 2;
			this.btnSelect.Text = "从文件中选择(&S)";
			this.btnSelect.UseVisualStyleBackColor = true;
			// 
			// picShow
			// 
			this.picShow.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.picShow.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.picShow.Image = ((System.Drawing.Image)(resources.GetObject("picShow.Image")));
			this.picShow.Location = new System.Drawing.Point(0, 178);
			this.picShow.Name = "picShow";
			this.picShow.Size = new System.Drawing.Size(156, 159);
			this.picShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picShow.TabIndex = 0;
			this.picShow.TabStop = false;
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.SystemColors.Info;
			this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new System.Drawing.Size(310, 337);
			this.txtLog.TabIndex = 1;
			this.txtLog.Text = "";
			// 
			// ImageUpload
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(474, 339);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImageUpload";
			this.Text = "ImageUpload";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.PictureBox picShow;
		private System.Windows.Forms.RichTextBox txtLog;
	}
}