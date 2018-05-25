using Iwenli.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Iwenli.Web.WebLog
{
    /// <summary>
    /// Web访问日志数据
    /// </summary>
    public class WebLogInfo
    {
        #region 构造函数

        HttpContext _countext;
        DateTime _startTime;
        DateTime _endTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public WebLogInfo(HttpContext context)
        {
            _startTime = DateTime.Now;
            _countext = context;
            _requestUrl = string.Empty;
            _requestPath = string.Empty;
        }

        #endregion

        #region 初始化记录数据

        /// <summary>
        /// 初始化记录数据
        /// </summary>
        public void InitInfo()
        {
            try
            {
                _clientIp = WebUtility.GetIP(_countext);
                _clientUserAgent = "";
                //取得客户端代理信息,获得跳转来源类型
                if (!string.IsNullOrEmpty(_countext.Request.UserAgent))
                {
                    _clientUserAgent = _countext.Request.UserAgent.ToLower();
                }

                //_clientCookie = _countext.Request.Cookies

                //来源数据
                if (_countext.Request.UrlReferrer != null)
                {
                    _refererHost = _countext.Request.UrlReferrer.Host;
                    _refererPath = _countext.Request.UrlReferrer.AbsolutePath;
                    _refererQuery = _countext.Request.UrlReferrer.Query;
                    _refererUrl = _countext.Request.UrlReferrer.ToString();
                }
                else
                {
                    _refererHost = string.Empty;
                    _refererPath = string.Empty;
                    _refererQuery = string.Empty;
                    _refererUrl = string.Empty;
                }

                //请求数据
                _requestMethod = _countext.Request.HttpMethod;
                _requestHost = _countext.Request.Url.Host.ToLower();
                _requestPath = _countext.Request.Url.AbsolutePath;
                _requestQuery = _countext.Request.Url.Query;
                _requestUrl = _countext.Request.Url.ToString();

                //服务器数据
                _serverName = _countext.Server.MachineName;
                _responseStatus = _countext.Response.StatusCode;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 客户端数据

        /// <summary>
        /// 客户IP
        /// </summary>
        string _clientIp;
        /// <summary>
        /// 客户端代理
        /// </summary>
        string _clientUserAgent;
        /// <summary>
        /// 客户端Cookie
        /// </summary>
        string _clientCookie;

        #endregion

        #region 来源数据

        /// <summary>
        /// 来源页面
        /// </summary>
        string _refererHost;
        string _refererPath;
        string _refererQuery;
        string _refererUrl;

        #endregion

        #region 访问数据

        /// <summary>
        /// 请求方法
        /// </summary>
        string _requestMethod;
        /// <summary>
        /// 请求主机
        /// </summary>
        string _requestHost;
        /// <summary>
        /// 请求页面
        /// </summary>
        string _requestPath;
        /// <summary>
        /// 请求查询数据
        /// </summary>
        string _requestQuery;
        /// <summary>
        /// 请求URL
        /// </summary>
        string _requestUrl;

        #endregion

        #region 服务器数据

        /// <summary>
        /// 服务器名称
        /// </summary>
        string _serverName;
        /// <summary>
        /// 返回状态码
        /// </summary>
        int _responseStatus;

        #endregion

        #region 获取XML数据

        string GetXml()
        {
            //return _originalData;
            StringBuilder _xmlCode = new StringBuilder();

            #region 生成Xml代码

            using (XmlWriter writer = XmlWriter.Create(_xmlCode))
            {
                //历史数据
                writer.WriteStartElement("ClickStreamLog");
                writer.WriteAttributeString("sn", Guid.NewGuid().ToString());

                //原始数据
                writer.WriteStartElement("StratTime");
                writer.WriteAttributeString("ticks", _startTime.Ticks.ToString());
                writer.WriteCData(_startTime.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("EndTime");
                writer.WriteAttributeString("ticks", _endTime.Ticks.ToString());
                writer.WriteCData(_endTime.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("IP");
                writer.WriteAttributeString("Int", Iwenli.Net.NetHelper.IpStringToInt(_clientIp).ToString());
                writer.WriteCData(_clientIp.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("UserAgent");
                writer.WriteCData(_clientUserAgent);
                writer.WriteEndElement();

                writer.WriteStartElement("Referer");
                writer.WriteCData(_refererUrl);
                writer.WriteEndElement();

                writer.WriteStartElement("Request");
                writer.WriteCData(_requestUrl);
                writer.WriteEndElement();

                writer.WriteStartElement("Response");
                writer.WriteAttributeString("status", _responseStatus.ToString());
                //writer.WriteAttributeString("servername", _serverName);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            #endregion

            return _xmlCode.ToString();
        }

        #endregion

        #region 记录数据

        /// <summary>
        /// 记录数据
        /// </summary>
        public void Note()
        {
            _endTime = DateTime.Now;
            try
            {
                WebLogConfig _config = WebLogConfig.Instance;
                if (_config == null)
                {
                    this.LogError("记录Web流量日志数据配置错误");
                    return;
                }
                if (IfNote(_config))
                {
                    _responseStatus = _countext.Response.StatusCode;
                    #region 入库

                    //入库
                    using (DataHelper helper = DataHelper.GetDataHelper(_config.DatabaseName))
                    {
                        string _sql = @"
                        INSERT INTO [dbo].[" + _config.DataTableName + @"]
                                   ([start_time]
                                   ,[end_time]
                                   ,[run_time]
                                   ,[vyear]
                                   ,[vmonth]
                                   ,[vday]
                                   ,[vhour]
                                   ,[vweek]
                                   ,[client_ip]
                                   ,[response_status]
                                   ,[server_name]
                                   ,[referer_host]
                                   ,[referer_path]
                                   ,[request_method]
                                   ,[request_host]
                                   ,[request_path]
                                   ,[strea_xml])
                             VALUES
                                   (@start_time
                                   ,@end_time
                                   ,@run_time
                                   ,@vyear
                                   ,@vmonth
                                   ,@vday
                                   ,@vhour
                                   ,@vweek
                                   ,@client_ip
                                   ,@response_status
                                   ,@server_name
                                   ,@referer_host
                                   ,@referer_path
                                   ,@request_method
                                   ,@request_host
                                   ,@request_path
                                   ,@strea_xml)";

                        helper.SpFileValue["@start_time"] = _startTime.Ticks;
                        helper.SpFileValue["@end_time"] = _endTime.Ticks;
                        helper.SpFileValue["@run_time"] = (_endTime - _startTime).Milliseconds;
                        helper.SpFileValue["@vyear"] = _startTime.Year;
                        helper.SpFileValue["@vmonth"] = _startTime.Month;
                        helper.SpFileValue["@vday"] = _startTime.Day;
                        helper.SpFileValue["@vhour"] = _startTime.Hour;
                        helper.SpFileValue["@vweek"] = (int)_startTime.DayOfWeek;
                        helper.SpFileValue["@client_ip"] = NetHelper.IpStringToInt(_clientIp);
                        helper.SpFileValue["@response_status"] = _responseStatus;
                        helper.SpFileValue["@server_name"] = _serverName;
                        helper.SpFileValue["@referer_host"] = _refererHost;
                        helper.SpFileValue["@referer_path"] = _refererPath;
                        helper.SpFileValue["@request_method"] = _requestMethod;
                        helper.SpFileValue["@request_host"] = _requestHost;
                        helper.SpFileValue["@request_path"] = _requestPath;
                        helper.SpFileValue["@strea_xml"] = GetXml();

                        helper.SqlExecute(_sql, helper.SpFileValue);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.LogError("记录Web流量日志数据错误：" + ex.Message, ex);
            }
        }

        #region 数据是否有效

        bool IfNote(WebLogConfig config)
        {
            if (config.IfNote)
            {
                //UserAgent排除
                string _userAgent = _clientUserAgent.ToLower();
                foreach (string item in config.UserAgentExclude)
                {
                    if (_userAgent.Contains(item))
                    {
                        return false;
                    }
                }
                //页面排除
                string _path = _requestPath.ToLower();
                foreach (string item in config.PathExcludeInfo)
                {
                    if (_path.Contains(item))
                    {
                        return false;
                    }
                }
                //IP排除
                foreach (string item in config.IpExclude)
                {
                    if (_clientIp.Contains(item))
                    {
                        return false;
                    }
                }

                //页面包含
                foreach (string item in config.PathIncludeInfo)
                {
                    if (_path.Contains(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #endregion
    }
}
