using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Txooo.Mobile.Serialization;

namespace Txooo.Mobile.Weixin
{
    public class RequestHelp
    {
        /// <summary>
        /// 接受request请求输出content
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ReceiveMsg(HttpRequest request)
        {

            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(request.InputStream, Encoding.UTF8))
            {
                return responseReader.ReadToEnd();
            }
        }

        public static ReqTextMsg ReceiveTextMsg(HttpRequest request)
        {

            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(request.InputStream, Encoding.UTF8))
            {
                stringResponse = responseReader.ReadToEnd();
            }

            return ReceiveTextMsg(stringResponse);
        }
        public static ReqTextMsg ReceiveTextMsg(string textMsg)
        {
            try
            {
                XmlSerialization xmls = new XmlSerialization();
                return xmls.ParseToObj<ReqTextMsg>(textMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ReqLocationMsg ReceiveLocationMsg(HttpRequest request)
        {

            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(request.InputStream, Encoding.UTF8))
            {
                stringResponse = responseReader.ReadToEnd();
            }

            return ReceiveLocationMsg(stringResponse);
        }

        public static ReqLocationMsg ReceiveLocationMsg(string locationMsg)
        {

            try
            {
                XmlSerialization xmls = new XmlSerialization();
                return xmls.ParseToObj<ReqLocationMsg>(locationMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ReqImageMsg ReceiveImageMsg(HttpRequest request)
        {

            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(request.InputStream, Encoding.UTF8))
            {
                stringResponse = responseReader.ReadToEnd();
            }

            return ReceiveImageMsg(stringResponse);
        }

        public static ReqImageMsg ReceiveImageMsg(string imageMsg)
        {
            try
            {
                XmlSerialization xmls = new XmlSerialization();
                return xmls.ParseToObj<ReqImageMsg>(imageMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
