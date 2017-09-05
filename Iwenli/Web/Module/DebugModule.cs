using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Web.Module
{
    /// <summary>
    /// 页面请求过程流水，调试模块
    /// </summary>
    public class DebugModule : IHttpModule
    {
        void LogInfo(object sender, EventArgs e, string info)
        {
            if (sender != null)
            {
                HttpApplication application = (HttpApplication)sender;
                HttpContext context = application.Context;
                if (context != null)
                {
                    string _url = context.Request.Url.AbsolutePath;
                    if (_url == "/")
                    {
                        _url = "/index.html";
                    }
                    ILog _log = LogHelper.GetLogger("_debugModule" + _url);
                    _log.Info(info);
                }
                else
                {
                    this.LogInfo(info);
                }
            }
            else
            {
                this.LogInfo(info);
            }
        }


        #region IHttpModule 成员

        void IHttpModule.Dispose()
        {

        }

        void IHttpModule.Init(HttpApplication context)
        {
            _i = 0;
            _noteSW = new Stopwatch();

            #region 1、HTTP 执行管线链中的第一个事件

            //3、在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
            context.BeginRequest += new EventHandler(context_BeginRequest);

            #endregion

            #region 2、安全，用户认证模块事件

            //4、当安全模块已建立用户标识时发生。
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
            //5、当安全模块已建立用户标识时发生。
            context.PostAuthenticateRequest += new EventHandler(context_PostAuthenticateRequest);
            //6、当安全模块已验证用户授权时发生。
            context.AuthorizeRequest += new EventHandler(context_AuthorizeRequest);
            //7、在当前请求的用户已获授权时发生。
            context.PostAuthorizeRequest += new EventHandler(context_PostAuthorizeRequest);

            #endregion

            #region 3、缓存处理事件

            //8、在 ASP.NET 完成授权事件以使缓存模块从缓存中为请求提供服务后发生，
            //从而绕过事件处理程序（例如某个页或 XML Web services）的执行。
            context.ResolveRequestCache += new EventHandler(context_ResolveRequestCache);
            //9、在 ASP.NET 跳过当前事件处理程序的执行并允许缓存模块满足来自缓存的请求时发生。
            context.PostResolveRequestCache += new EventHandler(context_PostResolveRequestCache);

            #endregion

            #region 4、页面处理模块选择事件

            //---------IIS7.0 集成模式事件
            //引发 MapRequestHandler 事件。将根据所请求资源的文件扩展名，选择相应的处理程序。
            //处理程序可以是本机代码模块，如 IIS 7.0 StaticFileModule，也可以是托管代码模块，如 PageHandlerFactory 类（它处理 .aspx 文件）。
            //10、在选择了用来响应请求的处理程序时发生。
            context.MapRequestHandler += new EventHandler(context_MapRequestHandler);

            //11、在 ASP.NET 已将当前请求映射到相应的事件处理程序时发生。
            context.PostMapRequestHandler += new EventHandler(context_PostMapRequestHandler);

            #endregion

            #region 5、会话状态关联事件

            //12、当 ASP.NET 获取与当前请求关联的当前状态（如会话状态）时发生。
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
            //13、在已获得与当前请求关联的请求状态（例如会话状态）时发生。
            context.PostAcquireRequestState += new EventHandler(context_PostAcquireRequestState);

            #endregion

            #region 6、页面处理类关联事件

            //14、恰好在 ASP.NET 开始执行事件处理程序（例如，某页或某个 XML Web services）前发生。
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);

            //15、为该请求调用合适的 IHttpHandler 类的 ProcessRequest 方法（或异步版 IHttpAsyncHandler..::.BeginProcessRequest）。
            //例如，如果该请求针对某页，则当前的页实例将处理该请求。

            //16、在 ASP.NET 事件处理程序（例如，某页或某个 XML Web service）执行完毕时发生。
            context.PostRequestHandlerExecute += new EventHandler(context_PostRequestHandlerExecute);

            #endregion

            #region 7、会话状态保存事件

            //17、在 ASP.NET 执行完所有请求事件处理程序后发生。该事件将使状态模块保存当前状态数据。
            context.ReleaseRequestState += new EventHandler(context_ReleaseRequestState);
            //18、在 ASP.NET 已完成所有请求事件处理程序的执行并且请求状态数据已存储时发生。
            context.PostReleaseRequestState += new EventHandler(context_PostReleaseRequestState);

            #endregion

            #region 8、进行响应筛选

            //19、如果定义了 Filter 属性，则执行响应筛选。

            #endregion

            #region 9、缓存更新事件

            //20、当 ASP.NET 执行完事件处理程序以使缓存模块存储将用于从缓存为后续请求提供服务的响应时发生。
            context.UpdateRequestCache += new EventHandler(context_UpdateRequestCache);
            //21、在 ASP.NET 完成缓存模块的更新并存储了用于从缓存中为后续请求提供服务的响应后，发生此事件。
            context.PostUpdateRequestCache += new EventHandler(context_PostUpdateRequestCache);

            #endregion

            #region 10、日志记录事件

            //---------IIS7.0 集成模式事件
            //22、恰好在 ASP.NET 为当前请求执行任何记录之前发生。
            context.LogRequest += new EventHandler(context_LogRequest);
            //23、在 ASP.NET 处理完 LogRequest 事件的所有事件处理程序后发生。
            context.PostLogRequest += new EventHandler(context_PostLogRequest);

            #endregion

            #region 11、HTTP 执行管线链中的最后一个事件

            //24、在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
            context.EndRequest += new EventHandler(context_EndRequest);

            #endregion

            #region 12、向客户端发送数据事件

            //25、恰好在 ASP.NET 向客户端发送内容之前发生。
            context.PreSendRequestContent += new EventHandler(context_PreSendRequestContent);
            //26、恰好在 ASP.NET 向客户端发送 HTTP 标头之前发生。
            context.PreSendRequestHeaders += new EventHandler(context_PreSendRequestHeaders);

            #endregion

            #region 其他事件

            //当引发未处理的异常时发生。
            context.Error += new EventHandler(context_Error);
            //在释放应用程序时发生。
            context.Disposed += new EventHandler(context_Disposed);

            #endregion
        }

        #endregion

        int _i;

        Stopwatch _noteSW;

        #region 1、HTTP 执行管线链中的第一个事件

        void context_BeginRequest(object sender, EventArgs e)
        {//在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
            _i = 0;
            _noteSW.Reset();
            _noteSW.Start();

            //BeginRequest 事件发出信号表示创建任何给定的新请求。此事件始终被引发，并且始终是请求处理期间发生的第一个事件。

            ////--------------------------------
            ////System.Diagnostics.Trace.WriteLine("并且始终是请求处理期间发生的第一个事件。","asdfs");
            ////为 ASP.NET 宿主应用程序管理 ASP.NET 应用程序域。
            //System.Web.Hosting.ApplicationManager manager = System.Web.Hosting.ApplicationManager.GetApplicationManager();
            ////HostingEnvironment 在托管应用程序的应用程序域内向托管应用程序提供应用程序管理功能和应用程序服务。
            //string appID = HostingEnvironment.ApplicationID;
            ////--------------------------------


            //HttpApplication application = (HttpApplication)sender;
            //HttpModuleCollection mC = application.Modules;

            //HttpContext context = application.Context;
            //获得正在处理的事件名称
            //string s = context.CurrentNotification.ToString();

            ////string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：01、[context_BeginRequest]");
        }

        #endregion

        #region 2、安全，用户认证模块

        void context_AuthenticateRequest(object sender, EventArgs e)
        {//当安全模块已建立用户标识时发生。
            //AuthenticateRequest 事件发出信号表示配置的身份验证机制已对当前请求进行了身份验证。
            //订阅 AuthenticateRequest 事件可确保在处理附加模块或事件处理程序之前对请求进行身份验证。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;

            //String username = System.Security.Principal.WindowsIdentity.GetAnonymous().Name;
            //String username1 = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            //context.User = new System.Security.Principal.WindowsPrincipal(new System.Security.Principal.WindowsIdentity("Administrator"));
            ////string s = context.User.GetType().ToString();
            //throw new NotImplementedException();

            LogInfo(sender, e, (_i++).ToString("00") + "：02、[当安全模块已建立用户标识时发生context_AuthenticateRequest]");
        }

        void context_PostAuthenticateRequest(object sender, EventArgs e)
        {//当安全模块已建立用户标识时发生。
            //PostAuthenticateRequest 事件在 AuthenticateRequest 事件发生之后引发。
            //预订 PostAuthenticateRequest 事件的功能可以访问由 PostAuthenticateRequest 处理的任何数据。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            //System.Security.Principal.WindowsPrincipal wp = context.User as System.Security.Principal.WindowsPrincipal;
            //System.Security.Principal.WindowsIdentity wi = wp.Identity as System.Security.Principal.WindowsIdentity;
            //string s = ((System.Security.Principal.WindowsIdentity)wp.Identity).Name;

            LogInfo(sender, e, (_i++).ToString("00") + "：02、[当安全模块已建立用户标识时发生context_PostAuthenticateRequest]");
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {//当安全模块已验证用户授权时发生。
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：02、[当安全模块已验证用户授权时发生context_AuthorizeRequest]");
        }

        void context_PostAuthorizeRequest(object sender, EventArgs e)
        {//在当前请求的用户已获授权时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：02、[在当前请求的用户已获授权时发生context_PostAuthorizeRequest]");
        }

        #endregion

        #region 3、缓存处理事件

        void context_ResolveRequestCache(object sender, EventArgs e)
        {////在 ASP.NET 完成授权事件以使缓存模块从缓存中为请求提供服务后发生，
            //从而绕过事件处理程序（例如某个页或 XML Web services）的执行。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：03、在 ASP.NET 完成授权事件以使缓存模块从缓存中为请求提供服务后发生context_ResolveRequestCache]");
        }

        void context_PostResolveRequestCache(object sender, EventArgs e)
        {//在 ASP.NET 跳过当前事件处理程序的执行并允许缓存模块满足来自缓存的请求时发生。
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：03、[在 ASP.NET 跳过当前事件处理程序的执行并允许缓存模块满足来自缓存的请求时发生context_PostResolveRequestCache]");
        }

        #endregion

        #region 4、页面处理模块选择事件

        void context_MapRequestHandler(object sender, EventArgs e)
        {//在选择了用来响应请求的处理程序时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：04、[context_MapRequestHandler]");
        }

        void context_PostMapRequestHandler(object sender, EventArgs e)
        { //在 ASP.NET 已将当前请求映射到相应的事件处理程序时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：04、[context_PostMapRequestHandler]");
        }

        #endregion

        #region 5、会话状态关联事件

        void context_AcquireRequestState(object sender, EventArgs e)
        {//当 ASP.NET 获取与当前请求关联的当前状态（如会话状态）时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：05、[context_AcquireRequestState]");
        }

        void context_PostAcquireRequestState(object sender, EventArgs e)
        { //在已获得与当前请求关联的请求状态（例如会话状态）时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：05、[context_PostAcquireRequestState]");
        }

        #endregion

        #region 6、页面处理类关联事件

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：06、[context_PreRequestHandlerExecute]");
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {////在 ASP.NET 事件处理程序（例如，某页或某个 XML Web service）执行完毕时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：06、[context_PostRequestHandlerExecute]");
        }

        #endregion

        #region 7、会话状态保存事件

        void context_ReleaseRequestState(object sender, EventArgs e)
        { //在 ASP.NET 执行完所有请求事件处理程序后发生。该事件将使状态模块保存当前状态数据。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：07、[context_ReleaseRequestState]");
        }

        void context_PostReleaseRequestState(object sender, EventArgs e)
        {//在 ASP.NET 已完成所有请求事件处理程序的执行并且请求状态数据已存储时发生。
            //在引发 PostReleaseRequestState 事件之后，现有的所有响应筛选器都将对输出进行筛选。

            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：07、[context_PostReleaseRequestState]");
        }

        #endregion

        #region 8、进行响应筛选

        //19、如果定义了 Filter 属性，则执行响应筛选。

        #endregion

        #region 9、缓存更新事件

        void context_UpdateRequestCache(object sender, EventArgs e)
        { //当 ASP.NET 执行完事件处理程序以使缓存模块存储将用于从缓存为后续请求提供服务的响应时发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：09、[当 ASP.NET 执行完事件处理程序以使缓存模块存储将用于从缓存为后续请求提供服务的响应时发生context_UpdateRequestCache]");
        }

        void context_PostUpdateRequestCache(object sender, EventArgs e)
        {////在 ASP.NET 完成缓存模块的更新并存储了用于从缓存中为后续请求提供服务的响应后，发生此事件。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            //throw new NotImplementedException();
            LogInfo(sender, e, (_i++).ToString("00") + "：09、[在 ASP.NET 完成缓存模块的更新并存储了用于从缓存中为后续请求提供服务的响应后，发生此事件context_PostUpdateRequestCache]");
        }

        #endregion

        #region 10、日志记录事件

        void context_LogRequest(object sender, EventArgs e)
        {//恰好在 ASP.NET 为当前请求执行任何记录之前发生。
            //即使发生错误，也会引发 LogRequest 事件。您可以为 LogRequest 事件提供一个事件处理程序，以便实现请求的自定义记录。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string path = "E:\\Log.txt";
            //string log = Iwenli.Library.Fso.FsoHelper.ReadFile(path);
            //log += "\r\n" + DateTime.Now.ToString() + "：" + context.Request.UserAgent;
            //Iwenli.Library.Fso.FsoHelper.WriteFile(path, log);
            //throw new NotImplementedException();
            LogInfo(sender, e, (_i++).ToString("00") + "：10、[context_LogRequest]");
        }

        void context_PostLogRequest(object sender, EventArgs e)
        { //在 ASP.NET 处理完 LogRequest 事件的所有事件处理程序后发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();

            LogInfo(sender, e, (_i++).ToString("00") + "：10、[context_PostLogRequest]");
        }

        #endregion

        #region 11、HTTP 执行管线链中的最后一个事件

        void context_EndRequest(object sender, EventArgs e)
        {////在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
            //在调用 CompleteRequest 方法时始终引发 EndRequest 事件。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //string s = context.User.GetType().ToString();
            LogInfo(sender, e, (_i++).ToString("00") + "：11、[context_EndRequest]");
            //LogInfo(sender, e, "页面执行时间："+_noteSW.Elapsed.TotalMilliseconds.ToString());
        }

        #endregion

        #region 12、向客户端发送数据事件

        void context_PreSendRequestContent(object sender, EventArgs e)
        {//恰好在 ASP.NET 向客户端发送内容之前发生。
            //PreSendRequestContent 事件可能发生多次。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            LogInfo(sender, e, (_i++).ToString("00") + "：12、[context_PreSendRequestContent]");
        }

        void context_PreSendRequestHeaders(object sender, EventArgs e)
        {//恰好在 ASP.NET 向客户端发送 HTTP 标头之前发生。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            LogInfo(sender, e, (_i++).ToString("00") + "：12、[context_PreSendRequestHeaders]");

            LogInfo(sender, e, "请求执行时间：" + _noteSW.Elapsed.TotalMilliseconds.ToString() + "\r\n ==================================================================================== \r\n");
            _noteSW.Stop();
        }

        #endregion

        #region 其他事件

        void context_Disposed(object sender, EventArgs e)
        {//在释放应用程序时发生。
            //当创建 Disposed 委托时，请标识将处理事件的方法。
            //若要使事件与事件处理程序相关联，请将 Disposed 委托的一个实例添加到事件。
            //除非移除了 Disposed 委托，否则每次发生该事件时都会调用事件处理程序。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            LogInfo(sender, e, (_i++).ToString("00") + "、[在释放应用程序时发生][context_Disposed");
        }

        void context_Error(object sender, EventArgs e)
        {//当引发未处理的异常时发生。
            //引发 Error 事件的异常可以通过对 HttpServerUtility.GetLastError  方法的调用来访问。
            //如果应用程序生成自定义错误输出，请通过调用 HttpServerUtility.ClearError 方法来取消由 ASP.NET 生成的默认错误消息。
            //HttpApplication application = (HttpApplication)sender;
            //HttpContext context = application.Context;
            //throw new NotImplementedException();
            LogInfo(sender, e, (_i++).ToString("00") + "、[当引发未处理的异常时发生][context_Error");
        }

        #endregion
    }
}
