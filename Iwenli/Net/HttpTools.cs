#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：HttpTools
 *  所属项目：Iwenli.Net
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 15:40:26
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Net
{

    /// <summary>
    /// 请求方法
    /// </summary>
    public enum RequestMethod : int
    {
        GET = 1
        , POST
        , PATCH
        , DELETE
        , PUT
    }

    /// <summary>
    /// HTTP请求
    /// </summary>
    public class HttpTools
    {
        public HttpTools(string url)
            : this(url, Encoding.UTF8) { }

        public HttpTools(string url, Encoding charset)
        {
            this.Url = url;
            this.Timeout = 30000;
            this.Charset = charset;
            this.Parameters = new Parameters();
        }
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Charset = Encoding.UTF8;
        /// <summary>
        /// Uri
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求参数集合
        /// </summary>
        public Parameters Parameters { get; private set; }

        public Action<HttpWebRequest> SetHeader;

        #region 方法动作
        /// <summary>
        /// 建立HttpRequest实例
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpWebRequest CreateWebRequest(RequestMethod method, string url)
        {
            var request = CreateRequest(method.ToString(), url, this.Timeout);
            if (SetHeader != null)
            {
                SetHeader(request);
            }

            return request;
        }
        /// <summary>
        /// 建立HttpRequest实例（带证书请求）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="cert"></param>
        /// <returns></returns>
        private HttpWebRequest CreateWebRequest(RequestMethod method, string url, bool isUseCert)
        {
            var request = CreateRequest(method.ToString(), url, this.Timeout, isUseCert);
            if (SetHeader != null)
            {
                SetHeader(request);
            }

            return request;
        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <returns></returns>
        public string Get(bool isencode = true)
        {
            string queryString = this.Parameters.BuildQueryString(isencode);
            string url = this.Url;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = string.Concat(url, url.IndexOf('?') == -1 ? '?' : '&', queryString);
            }
            var request = this.CreateWebRequest(RequestMethod.GET, url);
            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <returns></returns>
        public string Post(bool isencode = true)
        {
            var request = this.CreateWebRequest(RequestMethod.POST, this.Url);
            if (this.SetHeader == null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (this.Parameters.Items.Count != 0)
            {
                string queryString = this.Parameters.BuildQueryString(isencode);
                byte[] data = this.Charset.GetBytes(queryString);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// PATCH请求
        /// </summary>
        /// <returns></returns>
        public string Patch(string val, bool isencode = true)
        {
            var request = this.CreateWebRequest(RequestMethod.PATCH, this.Url);

            if (this.SetHeader == null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            byte[] data = this.Charset.GetBytes(val);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// PUT请求
        /// </summary>
        /// <returns></returns>
        public string PUT(string val, bool isencode = true)
        {
            var request = this.CreateWebRequest(RequestMethod.PUT, this.Url);
            if (this.SetHeader == null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            byte[] data = this.Charset.GetBytes(val);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// DELETE请求
        /// </summary>
        /// <returns></returns>
        public string DELETE(bool isencode = true)
        {
            string queryString = this.Parameters.BuildQueryString(isencode);
            string url = this.Url;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = string.Concat(url, url.IndexOf('?') == -1 ? '?' : '&', queryString);
            }
            var request = this.CreateWebRequest(RequestMethod.DELETE, url);
            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// POST请求适合单值，不包含键名
        /// </summary>
        /// <returns></returns>
        public string Post(string val)
        {
            var request = this.CreateWebRequest(RequestMethod.POST, this.Url);
            if (this.SetHeader == null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            byte[] data = this.Charset.GetBytes(val);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// POST请求适合单值，不包含键名（带证书请求）
        /// </summary>
        /// <returns></returns>
        public string Post(string val, X509Certificate cert)
        {
            var request = this.CreateWebRequest(RequestMethod.POST, this.Url, true);
            request.ClientCertificates.Add(cert);

            if (this.SetHeader == null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            byte[] data = this.Charset.GetBytes(val);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// 提交文件
        /// </summary>
        /// <param name="files">要提交上传的文件列表</param>
        /// <returns></returns>
        public string PostFile(Files files)
        {
            var request = this.CreateWebRequest(RequestMethod.POST, this.Url);

            string boundary = string.Concat("--", GetBoundary());
            request.ContentType = string.Concat("multipart/form-data; boundary=", boundary);
            request.KeepAlive = true;

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] boundaryData = this.Charset.GetBytes("\r\n--" + boundary + "\r\n");
                if (this.Parameters.Items.Count != 0)
                {
                    //写入参数
                    string parameterData = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (var p in this.Parameters.Items)
                    {
                        string item = string.Format(parameterData, p.Key, p.Value);
                        byte[] data = this.Charset.GetBytes(item);
                        ms.Write(boundaryData, 0, boundaryData.Length);
                        ms.Write(data, 0, data.Length);
                    }
                }

                if (files != null)
                {
                    //写入文件数据
                    string fileData = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    foreach (var p in files.Items)
                    {
                        if (p.Value != null)
                        {
                            string item = string.Format(fileData, p.Key, p.Value.FileName, p.Value.ContentType);
                            byte[] data = this.Charset.GetBytes(item);
                            ms.Write(boundaryData, 0, boundaryData.Length);
                            ms.Write(data, 0, data.Length);
                            p.Value.WriteTo(ms);
                        }
                    }
                }

                //写入结束线
                boundaryData = this.Charset.GetBytes("\r\n--" + boundary + "--\r\n");
                ms.Write(boundaryData, 0, boundaryData.Length);

                request.ContentLength = ms.Length;
                using (var stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }

            return ReadAllResponseText(request, this.Charset);
        }

        /// <summary>
        /// 随机边界码
        /// </summary>
        /// <returns></returns>
        public static string GetBoundary()
        {
            string str = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int num2 = random.Next(str.Length);
                builder.Append(str[num2]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 建立请求（带证书请求）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="isUseCert">是否需用证书</param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string method, string url, int timeout, bool isUseCert)
        {
            if (isUseCert)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }

            return CreateRequest(method, url, timeout);
        }


        /// <summary>
        /// 建立请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string method, string url, int timeout)
        {
            var uri = new Uri(url);
            if (uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = ValidateAllCertificate;
                }
                catch { }
            }
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            request.ServicePoint.Expect100Continue = false;
            request.Timeout = timeout;

            return request;
        }

        private static bool ValidateAllCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        /// <summary>
        /// 读取所有输出的文本数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string ReadAllResponseText(HttpWebRequest request, Encoding charset)
        {
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), charset))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion

        #region HMACSHA1 签名

        /// <summary>
        /// HMACSHA1 签名
        /// </summary>
        /// <param name="key">签名密钥</param>
        /// <param name="requestMethod">请求模式（Get\Post）</param>
        /// <param name="requestUrl">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public static string HMACSHA1Signature(string key, string requestMethod, string requestUrl, Parameters parameters)
        {

            StringBuilder data = new StringBuilder(100);
            data.AppendFormat("{0}&{1}&", requestMethod.ToUpper(), Uri.EscapeDataString(requestUrl));
            //处理参数
            if (parameters != null)
            {
                data.Append(parameters.BuildQueryString(true));
            }
            return data.ToString().HMACSHA1(key);
        }

        /// <summary>
        /// 克隆流
        /// </summary>
        /// <param name="inStream"></param>
        /// <returns></returns>
        public static MemoryStream CloneStream(Stream inStream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[128];

            while (true)
            {
                int sz = inStream.Read(buffer, 0, 128);
                if (sz == 0) break;
                ms.Write(buffer, 0, sz);
            }
            ms.Position = 0;
            return ms;
        }

        #endregion

    }
}
