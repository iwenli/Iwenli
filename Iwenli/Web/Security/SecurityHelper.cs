using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web.Security
{
    public class SecurityHelper
    {
        /// <summary>
        /// 根据用户票证获取Cookie
        /// </summary>
        /// <param name="userTicket"></param>
        /// <returns></returns>
        public static HttpCookie GetAuthCookie(UserTicket userTicket)
        {
            //根据用户票证生成cookie
            HttpCookie cookie = new HttpCookie(SecurityConfig.Instance.CookieName, userTicket.ToString());
            cookie.HttpOnly = false;
            cookie.Path = SecurityConfig.Instance.CookiePath;
            cookie.Secure = SecurityConfig.Instance.RequireSSL;
            if (SecurityConfig.Instance.CookieDomain != null)
            {
                cookie.Domain = SecurityConfig.Instance.CookieDomain;
            }
            //提取当前根域
            //cookie.Domain = Url.Current.Domain.MainDomain;
            if (userTicket.CreatePersistentCookie)
            {//创建持久 Cookie
                cookie.Expires = userTicket.Expiration;
            }
            return cookie;
        }

        /// <summary>
        /// 为提供的用户名创建一个身份验证票证，并将其添加到响应的 Cookie 集合或 URL。
        /// </summary>
        /// <param name="username">已验证身份的用户的名称</param>
        /// <param name="timeOut">过期时间</param>
        /// <param name="createPersistentCookie">若要创建持久 Cookie（跨浏览器会话保存的 Cookie），则为 true；否则为 false</param>
        /// <param name="slidingExpiration">是否启用可调过期时间</param>
        public static void SetAuthCookie(string username, int timeOut, bool createPersistentCookie, bool slidingExpiration)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }
            //生成用户票证
            UserTicket _userTicket = new UserTicket(username, timeOut, slidingExpiration, createPersistentCookie);
            //写入Cookie
            HttpContext.Current.Response.Cookies.Add(GetAuthCookie(_userTicket));

            //设置用户                            
            Type type = System.Web.Compilation.BuildManager.GetType(SecurityConfig.Instance.PrincipalType, false, false);
            HttpContext.Current.User = Activator.CreateInstance(type, new object[] { username }) as Principal;
        }

        /// <summary>
        /// 为提供的用户名创建一个身份验证票证，并将其添加到响应的 Cookie 集合或 URL。
        /// </summary>
        /// <param name="username">已验证身份的用户的名称</param>
        /// <param name="createPersistentCookie">若要创建持久 Cookie（跨浏览器会话保存的 Cookie），则为 true；否则为 false</param>
        public static void SetAuthCookie(string username, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }
            //生成用户票证
            UserTicket _userTicket = new UserTicket(username, SecurityConfig.Instance.Timeout, SecurityConfig.Instance.SlidingExpiration, createPersistentCookie);
            //写入Cookie
            HttpContext.Current.Response.Cookies.Add(GetAuthCookie(_userTicket));
            //设置用户                            
            Type type = System.Web.Compilation.BuildManager.GetType(SecurityConfig.Instance.PrincipalType, false, false);
            HttpContext.Current.User = Activator.CreateInstance(type, new object[] { username }) as Principal;
        }

        /// <summary>
        /// 从浏览器删除 Forms 身份验证票证
        /// </summary>
        public static void SignOut()
        {
            HttpCookie cookie = new HttpCookie(SecurityConfig.Instance.CookieName, "");
            cookie.HttpOnly = false;
            cookie.Path = SecurityConfig.Instance.CookiePath;
            cookie.Secure = SecurityConfig.Instance.RequireSSL;
            if (SecurityConfig.Instance.CookieDomain != null)
            {
                cookie.Domain = SecurityConfig.Instance.CookieDomain;
            }
            cookie.Expires = new DateTime(0x7cf, 10, 12);
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 从Cookie获得用户票证
        /// </summary>
        /// <returns></returns>
        public static UserTicket GetTicketFormCookie()
        {
            HttpCookie _cookie = HttpContext.Current.Request.Cookies[SecurityConfig.Instance.CookieName];
            if (_cookie == null)
            {
                return null;
            }
            try
            {
                UserTicket _userTicket = new UserTicket(_cookie.Value);
                return _userTicket;
            }
            catch (Exception ex)
            {
                SignOut();
                return null;
            }
        }

        /// <summary>
        /// 判断当前用户是否拥有相关权限
        /// </summary>
        /// <param name="str">权限值</param>
        /// <returns></returns>
        public static bool IsHavePotence(string str)
        {
            return HttpContext.Current.User.IsInRole(str);
        }
    }
}
