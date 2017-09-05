using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli
{
    class LogWatching : IDisposable
    {
        public static LogWatching GetAndStartLogWatching()
        {
            LogWatching watch = new LogWatching();
            watch.StartWatching();
            return watch;
        }

        LogMemoryAppender _watchingAppender;

        public LogWatching()
        {
            //_watchingAppender=  LogHelper.StartLogWatching();
        }

        /// <summary>
        /// 获取并清理日志事件
        /// </summary>
        /// <returns></returns>
        public LoggingEvent[] GetAndClearEvents()
        {
            return _watchingAppender.GetAndClearEvents();
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartWatching()
        {
            _watchingAppender = LogHelper.StartLogWatching();
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopWatching()
        {
            LogHelper.StopLogWatching();
        }

        public void Dispose()
        {
            LogHelper.StopLogWatching();
        }
    }

    /// <summary>
    /// 内存缓存日志记录Appender
    /// </summary>
    public class LogMemoryAppender : MemoryAppender
    {
        public LoggingEvent[] GetAndClearEvents()
        {
            lock (m_eventsList.SyncRoot)
            {
                var events = (LoggingEvent[])m_eventsList.ToArray(typeof(LoggingEvent));
                m_eventsList.Clear();
                return events;
            }
        }
    }
}
