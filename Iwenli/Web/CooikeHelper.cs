#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：CooikeHelper
 *  所属项目：Iwenli.Web
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 11:03:34
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web
{
    /// <summary>
    /// cooike操作
    /// </summary>
    public class CooikeHelper
    {
        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(小时),0为关闭页面失效</param> 
        public static void SetCookie(string CookieName, string CookieValue, double CookieTime)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            myCookie.Domain = new Domain(HttpContext.Current.Request.Url.Host).MainDomain;
            DateTime now = DateTime.Now;
            myCookie.Value = CookieValue;
            if (CookieTime != 0)
                myCookie.Expires = now.AddHours(CookieTime);
            if (HttpContext.Current.Response.Cookies[CookieName] != null)
                HttpContext.Current.Response.Cookies.Remove(CookieName);
            HttpContext.Current.Response.Cookies.Add(myCookie);
            if (string.IsNullOrEmpty(GetCookie(CookieName)))
            {
                HttpContext.Current.Session[CookieName] = CookieValue;
            }
        }

        /// <summary> 
        /// 取得Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <returns>Cookie的值</returns> 
        public static string GetCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            myCookie.Domain = new Domain(HttpContext.Current.Request.Url.Host).MainDomain;
            myCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (myCookie == null)
            {
                var _session = HttpContext.Current.Session[CookieName];
                return _session != null ? _session.ToString() : string.Empty;
            }
            return myCookie.Value;
        }

        /// <summary> 
        /// 清除Cookie
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        public static void ClearCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            myCookie.Domain = new Domain(HttpContext.Current.Request.Url.Host).MainDomain;
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddYears(-2);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
    }
}
