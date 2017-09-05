using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Net
{
    public class NetHelper
    {
        #region IP判断

        /// <summary>
        /// IP地址算法
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <returns></returns>
        public static long CountIP(string ipAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] streachip = ipAddress.Split('.');
                long intcip = 0;
                for (int i = 0; i < 4; i++)
                {
                    intcip += (long)(int.Parse(streachip[i]) * System.Math.Pow(256, 3 - i));
                }
                return intcip;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 检查是否在一个IP段
        /// </summary>
        /// <param name="maskAddress">掩码</param>
        /// <param name="address1">网段</param>
        /// <param name="address2">要检查的IP地址</param>
        /// <returns></returns>
        public static bool InSameSubNet(string maskAddress, string address1, string address2)
        {
            long ip0 = System.BitConverter.ToInt32(IPAddress.Parse(maskAddress).GetAddressBytes(), 0);
            long ip1 = System.BitConverter.ToInt32(IPAddress.Parse(address1).GetAddressBytes(), 0);
            long ip2 = System.BitConverter.ToInt32(IPAddress.Parse(address2).GetAddressBytes(), 0);
            return ((ip0 & ip1) == (ip0 & ip2));
        }

        /// <summary>
        /// IP字符串->长整型值 转换
        /// </summary>
        /// <param name="IpString"></param>
        /// <returns></returns>
        public static long IpStringToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            if (ip.Split(separator).Length == 3)
            {
                ip = ip + ".0";
            }
            string[] strArray = ip.Split(separator);
            long num2 = ((long.Parse(strArray[0]) * 0x100L) * 0x100L) * 0x100L;
            long num3 = (long.Parse(strArray[1]) * 0x100L) * 0x100L;
            long num4 = long.Parse(strArray[2]) * 0x100L;
            long num5 = long.Parse(strArray[3]);
            return (((num2 + num3) + num4) + num5);
        }

        /// <summary>
        /// // 长整型值->IP字符串 转换
        /// </summary>
        /// <param name="Ipv"></param>
        /// <returns></returns>
        public static string IntToIpString(long ipInt)
        {
            long num = (long)((ipInt & 0xff000000L) >> 0x18);
            if (num < 0L)
            {
                num += 0x100L;
            }
            long num2 = (ipInt & 0xff0000L) >> 0x10;
            if (num2 < 0L)
            {
                num2 += 0x100L;
            }
            long num3 = (ipInt & 0xff00L) >> 8;
            if (num3 < 0L)
            {
                num3 += 0x100L;
            }
            long num4 = ipInt & 0xffL;
            if (num4 < 0L)
            {
                num4 += 0x100L;
            }
            return (num.ToString() + "." + num2.ToString() + "." + num3.ToString() + "." + num4.ToString());
        }

        #endregion

        #region 加载web页面
        /// <summary>
        /// 获得系统默认编码的页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string LoadPage(string url)
        {
            try
            {
                if (url.Trim().Length == 0) return "";
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;

                byte[] pageData = wc.DownloadData(url);
                return System.Text.Encoding.Default.GetString(pageData);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 根据指定编码获得网页代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string LoadWebPage1(string url, string encode)
        {
            try
            {
                if (url.Trim().Length == 0) return "";
                WebClient wc = new WebClient();
                byte[] pageData = wc.DownloadData(url);
                return System.Text.Encoding.GetEncoding(encode).GetString(pageData);
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 以GB2312编码获得网页代码
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns></returns>
        public static string LoadGB2312WebPage(string url)
        {
            try
            {
                /*byte[] buf = new byte[38192]; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); 

                Stream resStream = response.GetResponseStream(); 

                int count = resStream.Read(buf, 0, buf.Length); 
                string code=System.Text.Encoding.Default.GetString(buf, 0, count);				
                resStream.Close(); 
                return  code ;*/

                if (url.Trim().Length == 0) return "";
                WebClient wc = new WebClient();
                byte[] pageData = wc.DownloadData(url);
                return System.Text.Encoding.GetEncoding("GB2312").GetString(pageData);
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
        }

        /// <summary>
        /// 以UTF8编码获得网页代码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string LoadUTF8WebPage(string url)
        {
            try
            {
                if (url.Trim().Length == 0) return "";
                WebClient wc = new WebClient();
                byte[] pageData = wc.DownloadData(url);
                return System.Text.Encoding.UTF8.GetString(pageData);
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
