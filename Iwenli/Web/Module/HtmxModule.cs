using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Iwenli.Web.Module
{
    /// <summary>
    /// Htmx页面处理模块
    /// </summary>
    public class HtmxModule : IHttpModule
    {

        const string m_requestViewInfo = @"<br />
            1、htm   为静态页面<br />
            2、html  为动态页面<br />
            3、htmx  删除静态页面，更新页面缓存<br />
            4、htmd  删除静态页面<br />
            5、htms  保存为静态页面<br />
            6、htmn  清空页面，保存为一个空白页(必须存在静态文件)<br />
            7、htmc  缓存页面输出<br />
            ";

        #region IHttpModule 成员

        void IHttpModule.Dispose()
        {

        }

        void IHttpModule.Init(HttpApplication context)
        {
            //7、在当前请求的用户已获授权时发生。
            context.PostAuthorizeRequest += new EventHandler(context_PostAuthorizeRequest);

            #region 11、HTTP 执行管线链中的最后一个事件

            //24、在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
            context.EndRequest += new EventHandler(context_EndRequest);

            #endregion
        }

        #endregion

        void context_PostAuthorizeRequest(object sender, EventArgs e)
        {//在当前请求的用户已获授权时发生。
            if (Url.Current.RequestType == RequestType.HTM)
            {
                #region 访问静态文件

                if (Url.Current.RequestFilePathExtension == ".htm")
                {
                    if (!System.IO.File.Exists(Url.Current.SavePath))
                    {
                        //静态文件不存在，重写URL
                        HttpContext.Current.RewritePath(Url.Current.AbsolutePath + "l",true);
                    }
                }

                #endregion

                if (Url.Current.RequestViewType != RequestViewType.Put)
                {
                    //进行安全认证
                    if (!WebUtility.IPAuthentication())
                    {
                        HttpContext.Current.Response.Redirect("/index.html?command_error");
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        switch (Url.Current.RequestViewType)
                        {
                            case RequestViewType.Put:
                                {
                                    //HtmxFilter中实现
                                }
                                break;
                            case RequestViewType.Edit:
                                break;
                            case RequestViewType.Update:
                                UpdaePage();
                                break;
                            case RequestViewType.Delete:
                                DeletePage();
                                break;
                            case RequestViewType.Save:
                                {
                                    //HtmxFilter中实现
                                }
                                break;
                            case RequestViewType.Null:
                                NullPage();
                                break;
                            case RequestViewType.Cache:
                                CachePage();
                                break;
                            default:
                                break;
                        }
                    }
                }
                
            }
        }

        #region 更新页面,Htmx

        void UpdaePage()
        {
            if (WebUtility.IPAuthentication())
            {
                if (System.IO.File.Exists(Url.Current.SavePath))
                {
                    //删除页面,安全Ip
                    System.IO.File.Delete(Url.Current.SavePath);
                }

                HttpResponse.RemoveOutputCacheItem(Url.Current.AbsolutePath + "l");
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentType = "text/html;charset=utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.Write("页面更新成功！" + m_requestViewInfo);
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 505;
                HttpContext.Current.Response.End();
            }
        }

        #endregion

        #region 删除视图,htmd

        void DeletePage()
        {
            if (System.IO.File.Exists(Url.Current.SavePath))
            {
                if (WebUtility.IPAuthentication())
                {
                    //删除页面,安全Ip
                    System.IO.File.Delete(Url.Current.SavePath);
                    HttpContext.Current.Response.Write("删除成功！" + m_requestViewInfo);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    HttpContext.Current.Response.StatusCode = 505;
                    HttpContext.Current.Response.End();
                }
            }
            HttpContext.Current.Response.Write("没有此文件！");
            HttpContext.Current.Response.End();
        }

        #endregion

        #region 清空视图,htmn

        public void NullPage()
        {
            if (System.IO.File.Exists(Url.Current.SavePath))
            {
                if (WebUtility.IPAuthentication())
                {
                    //清空页面,安全Ip
                    System.IO.File.Delete(Url.Current.SavePath);
                    System.IO.File.WriteAllText(Url.Current.SavePath, "");
                    HttpContext.Current.Response.Write("清空页面成功！" + m_requestViewInfo);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    HttpContext.Current.Response.StatusCode = 505;
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                HttpContext.Current.Response.Write("没有原始文件！" + m_requestViewInfo);
                HttpContext.Current.Response.End();
            }
        }

        #endregion

        #region 缓存页面,Htmc

        void CachePage()
        {
            //静态文件不存在，重写URL
            HttpContext.Current.RewritePath(Url.Current.AbsolutePath + "l");
            Page _page = Web.Instance.GetSite(HttpContext.Current).GetRequestConfig(HttpContext.Current);
            //缓存页面,非编辑试图
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(_page.UpdateTime));
            HttpContext.Current.Response.Cache.SetMaxAge(new TimeSpan((long)((long)_page.UpdateTime * 10000000)));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetValidUntilExpires(true);
            HttpContext.Current.Response.Cache.VaryByParams["none"] = true;//不带参数缓存页面
            HttpContext.Current.Response.Cache.SetVaryByCustom(Url.Current.AbsolutePath + "l");
        }

        #endregion
        
        #region 11、HTTP 执行管线链中的最后一个事件

        void context_EndRequest(object sender, EventArgs e)
        {
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //if (context.Response.StatusCode == 200)
            //{
            //    //静态文件不存在，重写URL
            //    //HttpContext.Current.RewritePath(Url.Current.AbsolutePath + "x");
            //    //context.Response.StatusCode = 200;
            //}
        }

        #endregion
    }
}
