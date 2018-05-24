#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：Files
 *  所属项目：Iwenli.Net
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 15:43:39
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Net
{
    /// <summary>
    /// 文件集
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 
        /// </summary>
        public Files()
        {
            this.Items = new List<KeyValuePair<string, UploadFile>>();
        }

        /// <summary>
        /// 文件集
        /// </summary>
        public List<KeyValuePair<string, UploadFile>> Items
        {
            get;
            private set;
        }
        /// <summary>
        /// 清空所有文件
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            this.Items.Clear();
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public void Add(string name, UploadFile file)
        {
            this.Items.Add(new KeyValuePair<string, UploadFile>(name, file));
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 根据本地文件地址实例化
        /// </summary>
        /// <param name="fileUri">文件地址，可以为本地文件绝对地址(c:\files\test.jpg)，也可以为网络文件绝对地址(http://www.domain.com/files/test.jpg)</param>
        public UploadFile(string fileUri)
        {
            this.SetFileUri(fileUri);
            this.ContentType = GetContentType(Path.GetExtension(this.FileName));
        }
        /// <summary>
        /// 根据本地文件地址与文件类型实例化
        /// </summary>
        /// <param name="fileUri">文件地址，可以为本地文件绝对地址(c:\files\test.jpg)，也可以为网络文件绝对地址(http://www.domain.com/files/test.jpg)</param>
        /// <param name="contentType">文件类型</param>
        public UploadFile(string fileUri, string contentType)
        {
            this.SetFileUri(fileUri);
            this.ContentType = contentType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileUri">文件地址可为本地也可为网络地址</param>
        /// <param name="fileName">文件名+扩展名</param>
        /// <param name="contentType">文件类型</param>
        public UploadFile(string fileUri, string fileName, string contentType)
        {
            this.FileUri = new Uri(fileUri);
            this.FileName = fileName;
            this.ContentType = contentType;
        }
        /// <summary>
        /// 根据文件名、文件类型与文件流实现化
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="stream">文件数据流</param>
        public UploadFile(string fileName, string contentType, Stream stream)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
            this.FileStream = stream;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// 设置文件URI地址
        /// </summary>
        private Uri FileUri = null;
        /// <summary>
        /// 设置文件URI地址
        /// </summary>
        /// <param name="fileUri"></param>
        private void SetFileUri(string fileUri)
        {
            this.FileUri = new Uri(fileUri);
            this.FileName = Path.GetFileName(this.FileUri.AbsolutePath);
        }
        /// <summary>
        /// 文件数据流
        /// </summary>
        private Stream FileStream;

        /// <summary>
        /// 将当前的文件数据写入到某个数据流中
        /// </summary>
        /// <param name="stream"></param>
        public void WriteTo(Stream stream)
        {
            byte[] buffer = new byte[512];
            int size = 0;
            if (this.FileUri != null)
            {
                if (this.FileUri.IsFile)
                {
                    if (File.Exists(this.FileUri.LocalPath))
                    {
                        //写入本地文件流
                        using (System.IO.FileStream reader = new FileStream(this.FileUri.LocalPath, FileMode.Open, FileAccess.Read))
                        {
                            while ((size = reader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, size);
                            }
                        }
                    }
                }
                else
                {
                    //网络流
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Referer, this.FileUri.OriginalString);
                        try
                        {
                            var data = client.DownloadData(this.FileUri);
                            stream.Write(data, 0, data.Length);
                        }
                        catch { }
                    }
                }
            }
            if (this.FileStream != null)
            {
                //写入文件流
                //this.FileStream.Position = 0;
                while ((size = this.FileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, size);
                }
            }
        }

        /// <summary>
        /// 根据文件扩展名获取文件类型
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        /// <returns></returns>
        private string GetContentType(string fileExt)
        {
            string contentType = "application/octetstream";
            if (!string.IsNullOrEmpty(fileExt))
            {
                fileExt = fileExt.ToLower();

                //尝试从注册表中取值
                bool hasError = false;
                try
                {
                    RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(fileExt);
                    if (regKey != null)
                    {
                        object v = regKey.GetValue("Content Type");
                        if (v != null) contentType = v.ToString();
                    }
                }
                catch (SecurityException)
                {
                    hasError = true;
                }
                catch (UnauthorizedAccessException)
                {
                    hasError = true;
                }

                if (hasError || string.IsNullOrEmpty(contentType))
                {
                    //处理常用图片文件的ContentType
                    contentType = GetCommonFileContentType(fileExt);
                }
            }

            return contentType;
        }
        /// <summary>
        /// 获取通用文件的文件类型
        /// </summary>
        /// <param name="fileExt">文件扩展名.如".jpg",".gif"等</param>
        /// <returns></returns>
        private string GetCommonFileContentType(string fileExt)
        {
            switch (fileExt)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".png":
                    return "image/png";
                default:
                    return "application/octetstream";
            }
        }
    }
}
