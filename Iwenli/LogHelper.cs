using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli
{
    /// <summary>
    /// 日志处理帮助类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 记录日志对象
        /// 对于独立类，
        /// 请使用引用Txooo命名空间后，使用this.TxLogDebug()记录日志信息
        /// </summary>
        public static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static log4net.Appender.IAppender _allAppender;
        static log4net.Appender.IAppender _errorAppender;
        static log4net.Appender.IAppender _infoAppender;
        static log4net.Appender.IAppender _debugAppender;
        static log4net.Appender.IAppender _warnAppender;
        static log4net.Appender.IAppender _fatalAppender;
        static DateTime _initTime;

        static string _defaultConfigFile;

        #region 监控日志类

        /// <summary>
        /// 内存日志类
        /// </summary>
        static LogMemoryAppender _watchingAppender;

        static bool _logWatching;
        /// <summary>
        /// 启动日志监控
        /// </summary>
        internal static LogMemoryAppender StartLogWatching()
        {
            _logWatching = true;
            if (_watchingAppender == null)
            {
                _watchingAppender = new LogMemoryAppender();
            }
            return _watchingAppender;
        }
        /// <summary>
        /// 停止日志监控
        /// </summary>
        internal static void StopLogWatching()
        {
            _logWatching = false;
        }

        #endregion

        static LogHelper()
        {
            _initTime = DateTime.Now;

            _defaultConfigFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Iwenli.config";

            //监控配置文件信息
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(_defaultConfigFile));

            //读取日志配置文件路径
            _allAppender = GetFileAppender("G_All", log4net.Core.Level.Off, log4net.Core.Level.All);
            _errorAppender = GetFileAppender("G_Error", log4net.Core.Level.Error, log4net.Core.Level.Error);
            _infoAppender = GetFileAppender("G_Info", log4net.Core.Level.Info, log4net.Core.Level.Info);
            _debugAppender = GetFileAppender("G_Debug", log4net.Core.Level.Debug, log4net.Core.Level.Debug);
            _warnAppender = GetFileAppender("G_Warn", log4net.Core.Level.Warn, log4net.Core.Level.Warn);
            _fatalAppender = GetFileAppender("G_Fatal", log4net.Core.Level.Fatal, log4net.Core.Level.Fatal);
            _watchingAppender = new LogMemoryAppender();
        }

        #region 日志文件保存路径

        static string GetLogSavePath()
        {
            if (MemoryCache.Default.Get("LOG_SAVE_PATH") != null)
            {
                return MemoryCache.Default.Get("LOG_SAVE_PATH").ToString();
            }
            else if (System.IO.File.Exists(_defaultConfigFile))
            {
                //读取XML信息
                XmlDocument _reader = new XmlDocument();
                _reader.Load(_defaultConfigFile);
                foreach (XmlNode item in _reader.DocumentElement.ChildNodes)
                {
                    if (item is XmlElement)
                    {
                        if (item.Name == "log4net" && item.Attributes["path"] != null)
                        {
                            if (!string.IsNullOrEmpty(item.Attributes["path"].Value))
                            {
                                List<string> _filePaths = new List<string>();
                                _filePaths.Add(_defaultConfigFile);
                                CacheItemPolicy _cachePolicy = new CacheItemPolicy();
                                _cachePolicy.ChangeMonitors.Add(new HostFileChangeMonitor(_filePaths));

                                string _savePath = item.Attributes["path"].Value + "\\" + _initTime.ToString("yyyyMMdd") + "\\";

                                MemoryCache.Default.Set("LOG_SAVE_PATH", _savePath, _cachePolicy);
                                return _savePath;
                            }
                        }
                    }
                }
            }
            return "./Log/";
        }

        #endregion

        #region 获取一个文件记录FileAppender

        static log4net.Appender.RollingFileAppender GetFileAppender(string name, log4net.Core.Level max, log4net.Core.Level min)
        {
            ///设置过滤器
            log4net.Filter.LevelRangeFilter _levfilter = new log4net.Filter.LevelRangeFilter();
            _levfilter.LevelMax = max;
            _levfilter.LevelMin = min;
            _levfilter.ActivateOptions();

            //设计记录格式 
            log4net.Layout.PatternLayout _layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level - %message%newline");

            //Appender1  
            log4net.Appender.RollingFileAppender _appender = new log4net.Appender.RollingFileAppender();
            //设置本Appander的名称
            _appender.Name = _initTime.ToString("HHmmss") + "." + name + ".Appender";
            _appender.File = GetLogSavePath() + name + ".log";
            //否追加到文件
            _appender.AppendToFile = true;
            _appender.MaxSizeRollBackups = 10;
            //日志文件名是否为静态
            _appender.StaticLogFileName = true;
            //_appender.DatePattern = "";
            //表示是否立即输出到文件，布尔型的。
            //_appender.ImmediateFlush
            //SecurityContext : 比较少应用，对日志进行加密只类的，使用SecurityContextProvider转换。(对日志的保密要求比较高的时候应该可以应用上吧，Log4考虑的还挺周全)
            //LockingModel : 文件锁类型，RollingFileAppender本身不是线程安全的，如果在程序中没有进行线程安全的限制，可以在这里进行配置，确保写入时的安全。有两中类型：FileAppender.ExclusiveLock 和 FileAppender.MinimalLock

            _appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
            //_appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();

            _appender.Layout = _layout;
            _appender.AddFilter(_levfilter);
            _appender.ActivateOptions();

            return _appender;
        }

        #endregion

        #region 提取ILog

        static object m_lockObj = new object();

        static log4net.ILog GetLogImpl(string name)
        {
            log4net.Core.LogImpl _log = log4net.LogManager.GetLogger(name) as log4net.Core.LogImpl;
            if (_log != null)
            {
                log4net.Repository.Hierarchy.Logger _logimpl = _log.Logger as log4net.Repository.Hierarchy.Logger;
                if (_logimpl != null)
                {
                    log4net.Repository.Hierarchy.Logger _logger = (log4net.Repository.Hierarchy.Logger)_logimpl;
                    if (_logger.Appenders.Count == 0)
                    {
                        lock (m_lockObj)
                        {
                            if (_logger.Appenders.Count == 0)
                            {
                                _logger.AddAppender(GetFileAppender(name, log4net.Core.Level.Off, log4net.Core.Level.All));
                                _logger.AddAppender(_allAppender);
                                _logger.AddAppender(_errorAppender);
                                _logger.AddAppender(_infoAppender);
                                _logger.AddAppender(_debugAppender);
                                _logger.AddAppender(_warnAppender);
                                _logger.AddAppender(_fatalAppender);
                                if (_logWatching)
                                {
                                    _logger.AddAppender(_watchingAppender);
                                }
                                //log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetAllRepositories()[0];
                                //hier.GetAppen
                                //log4net.Config.XmlConfigurator.Configure(//.BasicConfigurator.Configure(_appender);
                            }
                        }
                    }
                    else if (_logger.Appenders.Count == 7)
                    {
                        if (_logWatching && _logger.Appenders.Count == 7)
                        {
                            lock (m_lockObj)
                            {
                                if (_logWatching && _logger.Appenders.Count == 7)
                                {
                                    if (_watchingAppender != null)
                                    {
                                        _logger.AddAppender(_watchingAppender);
                                    }
                                }
                            }
                        }
                    }
                    else if (_logger.Appenders.Count == 8)
                    {
                        if (!_logWatching && _logger.Appenders.Count == 8)
                        {
                            lock (m_lockObj)
                            {
                                if (!_logWatching && _logger.Appenders.Count == 8)
                                {
                                    if (_watchingAppender != null)
                                    {
                                        _logger.RemoveAppender(_watchingAppender);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //缓存
            //m_logger.Set(name, _log, m_cachePolicy);
            return _log;
        }

        #endregion

        #region 获得日志记录类

        /// <summary>
        /// 获得日记记录助手，请使用类型的完整名称防止错误
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static log4net.ILog GetLogger(string name)
        {
            return GetLogImpl(name);
        }

        /// <summary>
        /// 获得当前类型的日记记录助手
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static log4net.ILog GetLogger(Type type)
        {
            //  >2.5,170[1W]
            //return log4net.LogManager.GetLogger(type.FullName);

            //  >2.5,270[1W]
            return GetLogImpl(type.FullName);

            //  >2.0,230[1W]
            //return m_logger.Get<log4net.ILog>(type.FullName, GetLogImpl, m_cachePolicy);
        }

        #endregion

        #region 记录日志  <!--级别，OFF、Fatal、ERROR、WARN、INFO、DEBUG、ALL-->
        /// <summary>
        /// 记录Fatal类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        public static void LogFatal(this object obj, string messages)
        {
            GetLogger(obj.GetType()).Fatal(messages);
        }

        /// <summary>
        /// 记录Fatal类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void LogFatal(this object obj, string messages, Exception ex)
        {
            GetLogger(obj.GetType()).Fatal(messages, ex);
        }
        /// <summary>
        /// 记录Error类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        public static void LogError(this object obj, string messages)
        {
            GetLogger(obj.GetType()).Error(messages);
        }

        /// <summary>
        /// 记录Error类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void LogError(this object obj, string messages, Exception ex)
        {
            GetLogger(obj.GetType()).Error(messages, ex);
        }

        /// <summary>
        /// 记录Warn类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        public static void LogWarn(this object obj, string messages)
        {
            GetLogger(obj.GetType()).Warn(messages);
        }

        /// <summary>
        /// 记录Warn类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void LogWarn(this object obj, string messages, Exception ex)
        {
            GetLogger(obj.GetType()).Warn(messages, ex);
        }

        /// <summary>
        /// 记录Info类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        public static void LogInfo(this object obj, string messages)
        {
            GetLogger(obj.GetType()).Info(messages);
        }

        /// <summary>
        /// 记录Info类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void LogInfo(this object obj, string messages, Exception ex)
        {
            GetLogger(obj.GetType()).Info(messages, ex);
        }

        /// <summary>
        /// 记录Debug类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        public static void LogDebug(this object obj, string messages)
        {
            GetLogger(obj.GetType()).Debug(messages);
        }

        /// <summary>
        /// 记录Debug类型日志
        /// </summary>
        /// <param name="obj">当前对象</param>
        /// <param name="messages">日志内容</param>
        /// <param name="ex">异常对象</param>
        public static void LogDebug(this object obj, string messages, Exception ex)
        {
            GetLogger(obj.GetType()).Debug(messages, ex);
        }


        #endregion

    }
}
