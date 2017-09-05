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
    /// 页面执行时间监控模块
    /// </summary>
    public class StopwatchModule : IHttpModule
    {
        public void Dispose()
        {

        }

        /// <summary>
        /// 允许记录时间间隔
        /// </summary>
        public static int RunNoteTime
        {
            set { StopwatchModule._runNoteTime = value; }
        }
        static int _runNoteTime = 200;


        public void Init(HttpApplication context)
        {
            _noteSW = new Stopwatch();

            #region 1、HTTP 执行管线链中的第一个事件

            //3、在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
            context.BeginRequest += new EventHandler(context_BeginRequest);

            #endregion

            #region 11、HTTP 执行管线链中的最后一个事件

            //24、在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
            context.EndRequest += new EventHandler(context_EndRequest);

            #endregion
        }

        Stopwatch _noteSW;

        #region 1、HTTP 执行管线链中的第一个事件

        void context_BeginRequest(object sender, EventArgs e)
        {
            _noteSW.Reset();
            _noteSW.Start();
        }

        #endregion

        #region 11、HTTP 执行管线链中的最后一个事件

        void context_EndRequest(object sender, EventArgs e)
        {
            if (_noteSW.Elapsed.TotalMilliseconds > _runNoteTime)
            {
                this.LogInfo("请求执行时间大于" + _runNoteTime + "毫秒：[" + HttpContext.Current.Request.Url.ToString() + "][" + _noteSW.Elapsed.TotalMilliseconds.ToString() + "]");
            }
            _noteSW.Stop();
        }

        #endregion
    }
}
