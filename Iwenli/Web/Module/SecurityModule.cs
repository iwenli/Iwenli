using Iwenli.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web.Module
{
    public class SecurityModule : IHttpModule
    {
        #region IHttpModule接口

        public void Dispose()
        {

        }

        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest += application_AuthenticateRequest;
        }

        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            Url _url = Iwenli.Web.Url.Current;
            if (_url.AbsolutePath == "/")
            {
                return;
            }

            //获得认证用户信息
            OnAuthenticate(context);

            if ((context.User == null)
                && (SecurityConfig.Instance.LoginUrl != _url.AbsolutePath))
            {
                //开发域名跳过
                foreach (var item in SecurityConfig.Instance.OpenDomain)
                {
                    if (_url.Domain.DomainInfo == item)
                    {
                        goto URLCHECK;
                    }
                }
                //开放跳过
                foreach (var item in SecurityConfig.Instance.OpenPath)
                {
                    if (_url.AbsolutePath.IndexOf(item) == 0)
                    {
                        goto URLCHECK;
                    }
                }
                //授权目录,跳转登陆页面
                foreach (var item in SecurityConfig.Instance.AuthorizationPath)
                {
                    if (_url.AbsolutePath.IndexOf(item) == 0)
                    {
                        #region 授权页面，未经授权的处理方式

                        switch (SecurityConfig.Instance.StopType)
                        {
                            case StopBrowseType.Redirect:
                                {
                                    string _loginPage = SecurityConfig.Instance.LoginUrl + (SecurityConfig.Instance.LoginUrl.IndexOf("?") > -1 ? "&" : "?") + "ReturnUrl=" + WebUtility.UrlEncode(_url.ToString());
                                    context.Response.Redirect(_loginPage, true);
                                }
                                break;
                            case StopBrowseType.Stop:
                                {
                                    context.Response.Clear();
                                    context.Response.End();
                                }
                                break;
                            case StopBrowseType.Exception:
                                {
                                    throw new WlException("未经授权，禁止访问：" + SecurityConfig.Instance.StopInfo);
                                }
                                break;
                            case StopBrowseType.Info:
                                {
                                    context.Response.Clear();
                                    context.Response.Write(SecurityConfig.Instance.StopInfo);
                                    context.Response.End();
                                }
                                break;
                            default:
                                {
                                    string _loginPage = SecurityConfig.Instance.LoginUrl + (SecurityConfig.Instance.LoginUrl.IndexOf("?") > -1 ? "&" : "?") + "ReturnUrl=" + WebUtility.UrlEncode(_url.ToString());
                                    context.Response.Redirect(_loginPage, true);
                                    break;
                                }
                        }

                        #endregion
                    }
                }
            }

            URLCHECK:

            //进行Url检测
            Principal _principal = context.User as Principal;
            if (_principal != null)
            {
                if (!_principal.IsBrowseUrl(_url))
                {//无权限
                    throw new WlException("你没有浏览该页面的权限，请与管理员联系！");
                }
            }
        }

        #endregion

        #region 认证用户

        private void OnAuthenticate(HttpContext context)
        {
            if (context.User == null)
            {//用户是否认证
                UserTicket _ticket = SecurityHelper.GetTicketFormCookie();
                if (_ticket != null)
                {
                    if (!_ticket.Expired)
                    {
                        #region 票证有效进行延期

                        if (!string.IsNullOrEmpty(_ticket.Username))
                        {
                            bool _isDefer = false;
                            if (SecurityConfig.Instance.ValidateIP)
                            {//需要验证IP
                                if (_ticket.IP != WebUtility.GetIP())
                                {//ip验证不通过，退出登录
                                    this.LogFatal("IP验证不通过，退出登录：[" + _ticket.IP + "]" + WebUtility.GetIP());
                                    SecurityHelper.SignOut();
                                    return;
                                }
                                else
                                {
                                    _isDefer = true;
                                }
                            }
                            //设置用户                            
                            Type type = System.Web.Compilation.BuildManager.GetType(SecurityConfig.Instance.PrincipalType, false, false);
                            context.User = Activator.CreateInstance(type, new object[] { _ticket.Username }) as Principal;
                            //更新过期时间
                            if (_isDefer && _ticket.SlidingExpiration)
                            {
                                //更新Cookie
                                HttpContext.Current.Response.Cookies.Add(SecurityHelper.GetAuthCookie(_ticket));
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        this.LogFatal("登录超时：[" + _ticket.IP + "]" + WebUtility.GetIP());
                    }
                }
            }
            else
            {
                this.LogFatal("已有用户信息：" + context.User.Identity.Name + "@" + WebUtility.GetIP());
            }
        }

        #endregion
    }
}
