namespace Iwenli.CodeGenerate
{
	partial class DiaForm
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
			this.btnOk = new System.Windows.Forms.Button();
			this.txtEntity = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(219, 150);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "确认";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// txtEntity
			// 
			this.txtEntity.Location = new System.Drawing.Point(12, 35);
			this.txtEntity.Name = "txtEntity";
			this.txtEntity.Size = new System.Drawing.Size(282, 96);
			this.txtEntity.TabIndex = 5;
			this.txtEntity.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(173, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "自定义实体类名(无需带后缀)：";
			// 
			// DiaForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 194);
			this.ControlBox = false;
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtEntity);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "DiaForm";
			this.Opacity = 0.95D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.RichTextBox txtEntity;
		private System.Windows.Forms.Label label1;
	}
}