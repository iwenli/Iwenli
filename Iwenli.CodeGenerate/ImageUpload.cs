using Iwenli.CodeGenerate.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iwenli.CodeGenerate
{
	public partial class ImageUpload : BaseForm
	{
		private Regex UrlReg = new Regex(@"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\,\'\/\\\+&amp;%\$?=#_]*)?");

		protected override string ModelName => "图片上传";
		public ImageUpload() : base()
		{
			InitializeComponent();
			Load += ImageUpload_Load;
		}

		private void ImageUpload_Load(object sender, EventArgs e)
		{
			InitSetting(txtLog);
			btnSelect.Click += BtnSelect_Click;
		}

		private async void BtnSelect_Click(object sender, EventArgs e)
		{
			var file = GetImageFiles()?[0];
			if (file != null)
			{
				var image = Image.FromFile(file);
				if (image != null)
				{
					var suffix = file.Substring(file.LastIndexOf("."));  //取得图片后缀
					await showAndUploadImage(image, suffix);
				}
			}
		}
		#region 全局键盘
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.V))
			{
				var _image = Clipboard.GetImage();
				if (_image != null)
				{
					showAndUploadImage(_image);
				}
				else
				{
					var _text = Clipboard.GetText().Trim();
					var _imageUlr = UrlReg.Match(_text).Groups?[0]?.Value ?? "";
					if (!_imageUlr.IsNullOrEmpty())
					{
						AppendLogWarning($"提取到链接[{_imageUlr}]开始尝试上传.");
						showAndUploadImage(_imageUlr);
					}
				}
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		#endregion

		private async Task showAndUploadImage(Image image, string suffix = "")
		{
			picShow.Image = image;
			try
			{
				var _url = await FileService.UploadImageAsync(image, suffix);
				Clipboard.SetDataObject(_url);
				AppendLog($"上传成功（已经自动复制到剪切板）：{_url}");
			}
			catch (Exception ex)
			{
				AppendLogError($"上传失败：{ex.Message}");
			}
		}
		private async Task showAndUploadImage(string url)
		{
			picShow.ImageLocation = url;
			try
			{
				var _txoooFileUrl = await FileService.UploadImageAsync(url);
				Clipboard.SetDataObject(_txoooFileUrl);
				AppendLog($"上传成功（已经自动复制到剪切板）：{_txoooFileUrl}");
			}
			catch (Exception ex)
			{
				AppendLogError($"上传失败：{ex.Message}");
			}
		}
		private string[] GetImageFiles(int maxImageCounts = 1)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "选择文件";
			openFileDialog.Filter = "JPG图片|*.jpg|BMP图片|*.bmp|Gif图片|*.gif|所有文件|*.*";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.Multiselect = maxImageCounts > 1;
			openFileDialog.FilterIndex = 1;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					return openFileDialog.FileNames;
				}
				catch (Exception ex)
				{
					AppendLogError("选择文件异常：" + ex.Message);
				}
			}
			return null;
		}
	}
}
