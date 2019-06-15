using Iwenli.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Iwenli.CodeGenerate.Utils
{
	public class FileService
	{
		static Regex _txoooImgReg = new Regex("img.txooo.com");

		/// <summary>
		/// 上传图片,成功返回url,失败返回空字符串
		/// </summary>
		/// <param name="url">网络图片URL</param>
		/// <param name="uploadType">上传类型，0系统请求，1商品主图，2商品详情，3SKU图片，4商品评价图</param>
		/// <returns></returns>
		public static async Task<string> UploadImageAsync(string url, int timeOut = 10000)
		{
			//如果已经是txooo的连接  不转换
			if (url.IndexOf("img.txooo.com") > -1 || url.IndexOf("imgtx.cn") > -1)
			{
				return url.Replace("http:", "https:").Replace("img.txooo.com", "imgtx.cn");
			}
			using (NewWebClient myWebClient = new NewWebClient())
			{
				myWebClient.Timeout = timeOut;  //设置超时时间
				myWebClient.Credentials = CredentialCache.DefaultCredentials;
				var imgUrl = string.Empty;
				try
				{
					imgUrl = await myWebClient.DownloadStringTaskAsync("http://file.txooo.cc/UpLoadForByte.ashx?tx_down_url=" + url);
				}
				catch (Exception ex)
				{
					throw new Exception($"图片{url}上传到http://file.txooo.cc服务器异常，{ex.Message}");
				}
				if (imgUrl == "Error" || imgUrl.IsNullOrEmpty() || !_txoooImgReg.IsMatch(imgUrl))
				{
					throw new Exception($"图片{url}上传到http://file.txooo.cc服务器失败!");
				}
				return imgUrl.Replace("http:", "https:").Replace("img.txooo.com", "imgtx.cn");
			}
		}

		/// <summary>
		/// 上传图片,成功返回url,失败返回空字符串
		/// </summary>
		/// <param name="image">Image对象的图片</param>
		/// <returns></returns>
		public static async Task<string> UploadImageAsync(Image image, string fileExtension = "")
		{
			return await UploadFileForByteAsync(image.ToBytes(), fileExtension);
		}

		/// <summary>
		/// 向文件服务器上传文件
		/// </summary>
		/// <param name="file">文件流</param>
		/// <param name="fileExtension">文件类型</param>
		/// <returns>返回文件服务地址，错误返回：Error</returns>
		private static async Task<string> UploadFileForByteAsync(byte[] file, string fileExtension = "")
		{
			if (fileExtension.IsNullOrEmpty())
			{
				fileExtension = file.GetExtension();
			}
			if (!fileExtension.StartsWith("."))
			{
				fileExtension = "." + fileExtension;
			}
			NewWebClient myWebClient = new NewWebClient();
			myWebClient.Timeout = 10000;  //设置超时时间
			myWebClient.Credentials = CredentialCache.DefaultCredentials;
			myWebClient.Headers.Add("TxoooUploadFileType", fileExtension);
			byte[] getArray = await myWebClient.UploadDataTaskAsync("http://file.txooo.cc/UpLoadForByte.ashx", file.CompressForByte());
			string imgUrl = Encoding.Default.GetString(getArray);
			if (imgUrl == "Error" || imgUrl.IsNullOrEmpty() || !_txoooImgReg.IsMatch(imgUrl))
			{
				throw new Exception(string.Format("图片上传到http://file.txooo.cc服务器失败!"));
			} 
			return imgUrl.Replace("http:", "https:").Replace("img.txooo.com", "imgtx.cn");
		}
	}

	/// <summary>
	/// 为WebClient增加超时时间
	/// <para>从WebClient派生一个新的类，重载GetWebRequest方法</para>
	/// </summary>
	class NewWebClient : WebClient
	{
		private int _timeout;

		/// <summary>
		/// 超时时间(毫秒)
		/// </summary>
		public int Timeout
		{
			get
			{
				return _timeout;
			}
			set
			{
				_timeout = value;
			}
		}

		public NewWebClient()
		{
			this._timeout = 60000;
		}

		public NewWebClient(int timeout)
		{
			this._timeout = timeout;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			var result = base.GetWebRequest(address);
			result.Timeout = this._timeout;
			return result;
		}
	}
}
