using Iwenli.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// Web相关公共处理函数
    /// </summary>
    public static class WebUtility
    {
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        #region Http 编码 解码

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        #endregion

        #region 获得物理路径

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isRoot">是否相对根路径</param>
        /// <returns></returns>
        public static string GetMapPath(string path, bool isRoot)
        {
            if (isRoot && path.Length > 1)
            {
                path = path.Replace("\\", "/");
                if (path.Remove(1) != "~")
                {
                    if (path.Remove(1) != "/")
                    {
                        path = "~/" + path;
                    }
                    else
                    {
                        path = "~" + path;
                    }
                }
            }
            return HttpContext.Current.Server.MapPath(path);
        }

        #endregion

        #region 获得当前请求IP地址

        /// <summary>
        /// 获得当前请求IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            return GetIP(HttpContext.Current);
        }

        /// <summary>
        /// 获得请求IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIP(HttpContext context)
        {
            string result = String.Empty;

            try
            {
                result = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1)
                    {//没有“.”肯定是非IPv4格式 
                        result = null;
                    }
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (StringHelper.IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //找到不是内网的地址 
                                }
                            }
                        }
                        else if (StringHelper.IsIPAddress(result))
                        {//代理即是IP格式 
                            return result;
                        }
                        else
                        {
                            result = null;    //代理中的内容 非IP，取IP 
                        }
                    }
                }

                if (string.IsNullOrEmpty(result))
                {//获得访问IP地址
                    result = context.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (!StringHelper.IsIPAddress(result))
                {
                    result = "127.0.0.1";
                }
            }
            catch (Exception ex)
            {

            }
            if (string.IsNullOrEmpty(result))
            {//获得访问IP地址
                result = "127.0.0.1";
            }
            return result;
        }

        #endregion

        #region IP验证

        /// <summary>
        /// 验证IP是否是安全IP
        /// </summary>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public static bool IPAuthentication(string ip)
        {
            foreach (string[] item in Security.SecurityConfig.Instance.SecurityIP)
            {
                if (NetHelper.InSameSubNet(item[1], item[0], ip))
                {//找到允许IP
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证当前IP是否是安全IP
        /// </summary>
        /// <returns></returns>
        public static bool IPAuthentication()
        {
            string _ip = GetIP();
            return IPAuthentication(_ip);
        }

        #endregion
    }
}
