/*---------------------------------------------------------------
 *  Copyright (C) 2012 天下商机（txooo.com）版权所有
 * 
 *  文件名：Txooo.Open.OAuth.cs
 *  所属项目：
 *  功能描述：OAuth协议常用方法
 *  
 *  创建标识：TXOOO-MrLee
 *  修改标识：
 *  修改描述：
---------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Threading;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Txooo.Mobile
{
    /// <summary>
    /// OAuth帮助类
    /// </summary>
    public static class OAuthHelper
    {

        #region 时间戳

        /// <summary>
        /// 用于计算时间戳的时间值
        /// </summary>
        private static DateTime UnixTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <returns></returns>
        public static long GenerateTimestamp()
        {
            return GenerateTimestamp(DateTime.Now);
        }
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GenerateTimestamp(DateTime time)
        {
            return (long)(time.ToUniversalTime() - UnixTimestamp).TotalSeconds;
        }
        /// <summary>
        /// 将时间戳转换为时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertFromTimestamp(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(UnixTimestamp.AddSeconds(timestamp));
        }

        #endregion

        #region 随机码

        /// <summary>
        /// 随机种子
        /// </summary>
        public static Random RndSeed = new Random();
        /// <summary>
        /// 生成一个随机码
        /// </summary>
        /// <returns></returns>
        public static string GenerateRndNonce()
        {
            return string.Concat(
            OAuthHelper.RndSeed.Next(1, 99999999).ToString("00000000"),
            OAuthHelper.RndSeed.Next(1, 99999999).ToString("00000000"),
            OAuthHelper.RndSeed.Next(1, 99999999).ToString("00000000"),
            OAuthHelper.RndSeed.Next(1, 99999999).ToString("00000000"));
        }

        #endregion

        /// <summary>
        /// 连接字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="separator">分隔符</param>
        /// <param name="values">值列表</param>
        /// <returns></returns>
        public static string Join<T>(string separator, IEnumerable<T> values)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (T t in values)
            {
                if (buffer.Length != 0) buffer.Append(separator);
                buffer.Append(t == null ? "" : t.ToString());
            }
            return buffer.ToString();
        }

        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            StringBuilder buffer = new StringBuilder(text.Length);
            byte[] data = Encoding.UTF8.GetBytes(text);
            foreach (byte b in data)
            {
                char c = (char)b;
                if (!(('0' <= c && c <= '9') || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z'))
                    && "-_.~".IndexOf(c) == -1)
                {
                    buffer.Append('%' + Convert.ToString(c, 16).ToUpper());
                }
                else
                {
                    buffer.Append(c);
                }
            }
            return buffer.ToString();
        }

        #region MD5

        /// <summary>
        /// 32位MD5加密字符数据
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <returns></returns>
        public static string MD5(string value)
        {
            return MD5(value, Encoding.UTF8);
        }
        /// <summary>
        /// MD5加密字符
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string MD5(string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value)) return "";

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] output = md5.ComputeHash(encoding.GetBytes(value));

            md5.Clear();

            StringBuilder code = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
            {
                code.Append(output[i].ToString("x2"));
            }
            return code.ToString();
        }

        #endregion

        #region HMACSHA1 签名

        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HMACSHA1(string key, string data)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(key);

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

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
            data.AppendFormat("{0}&{1}&", requestMethod.ToUpper(), OAuthHelper.UrlEncode(requestUrl));
            //处理参数
            if (parameters != null)
            {
                parameters.Sort();
                data.Append(OAuthHelper.UrlEncode(parameters.BuildQueryString(true)));
            }
            return HMACSHA1(key, data.ToString());
        }


        #endregion

    }
}
