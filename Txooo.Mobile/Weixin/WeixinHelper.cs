using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Weixin
{
    public class WeixinHelper
    {
        static Dictionary<int, string> m_errorCode;

        #region 构造函数

        static WeixinHelper()
        {
            m_errorCode = new Dictionary<int, string>();

            #region 错误消息

            m_errorCode.Add(-1, "系统繁忙");
            m_errorCode.Add(0, "请求成功");
            m_errorCode.Add(40001, "获取access_token时AppSecret错误，或者access_token无效");
            m_errorCode.Add(40002, "不合法的凭证类型");
            m_errorCode.Add(40003, "不合法的OpenID");
            m_errorCode.Add(40004, "不合法的媒体文件类型");
            m_errorCode.Add(40005, "不合法的文件类型");
            m_errorCode.Add(40006, "不合法的文件大小");
            m_errorCode.Add(40007, "不合法的媒体文件id");
            m_errorCode.Add(40008, "不合法的消息类型");
            m_errorCode.Add(40009, "不合法的图片文件大小");
            m_errorCode.Add(40010, "不合法的语音文件大小");
            m_errorCode.Add(40011, "不合法的视频文件大小");
            m_errorCode.Add(40012, "不合法的缩略图文件大小");
            m_errorCode.Add(40013, "不合法的APPID");
            m_errorCode.Add(40014, "不合法的access_token");
            m_errorCode.Add(40015, "不合法的菜单类型");
            m_errorCode.Add(40016, "不合法的按钮个数");
            m_errorCode.Add(40017, "不合法的按钮个数");
            m_errorCode.Add(40018, "不合法的按钮名字长度");
            m_errorCode.Add(40019, "不合法的按钮KEY长度");
            m_errorCode.Add(40020, "不合法的按钮URL长度");
            m_errorCode.Add(40021, "不合法的菜单版本号");
            m_errorCode.Add(40022, "不合法的子菜单级数");
            m_errorCode.Add(40023, "不合法的子菜单按钮个数");
            m_errorCode.Add(40024, "不合法的子菜单按钮类型");
            m_errorCode.Add(40025, "不合法的子菜单按钮名字长度");
            m_errorCode.Add(40026, "不合法的子菜单按钮KEY长度");
            m_errorCode.Add(40027, "不合法的子菜单按钮URL长度");
            m_errorCode.Add(40028, "不合法的自定义菜单使用用户");
            m_errorCode.Add(40029, "不合法的oauth_code");
            m_errorCode.Add(40030, "不合法的refresh_token");
            m_errorCode.Add(40031, "不合法的openid列表");
            m_errorCode.Add(40032, "不合法的openid列表长度");
            m_errorCode.Add(40033, "不合法的请求字符，不能包含\\uxxxx格式的字符");
            m_errorCode.Add(40034, "");
            m_errorCode.Add(40035, "不合法的参数");
            m_errorCode.Add(40036, "");
            m_errorCode.Add(40037, "");
            m_errorCode.Add(40038, "不合法的请求格式");
            m_errorCode.Add(40039, "不合法的URL长度");
            m_errorCode.Add(40050, "不合法的分组id");
            m_errorCode.Add(40051, "分组名字不合法");

            m_errorCode.Add(41001, "缺少access_token参数");
            m_errorCode.Add(41002, "缺少appid参数");
            m_errorCode.Add(41003, "缺少refresh_token参数");
            m_errorCode.Add(41004, "缺少secret参数");
            m_errorCode.Add(41005, "缺少多媒体文件数据");
            m_errorCode.Add(41006, "缺少media_id参数");
            m_errorCode.Add(41007, "缺少子菜单数据");
            m_errorCode.Add(41008, "缺少oauth code");
            m_errorCode.Add(41009, "缺少openid");

            m_errorCode.Add(42001, "access_token超时");
            m_errorCode.Add(42002, "refresh_token超时");
            m_errorCode.Add(42003, "oauth_code超时");

            m_errorCode.Add(43001, "需要GET请求");
            m_errorCode.Add(43002, "需要POST请求");
            m_errorCode.Add(43003, "需要HTTPS请求");
            m_errorCode.Add(43004, "需要接收者关注");
            m_errorCode.Add(43005, "需要好友关系");

            m_errorCode.Add(44001, "多媒体文件为空");
            m_errorCode.Add(44002, "POST的数据包为空");
            m_errorCode.Add(44003, "图文消息内容为空");
            m_errorCode.Add(44004, "文本消息内容为空");

            m_errorCode.Add(45001, "多媒体文件大小超过限制");
            m_errorCode.Add(45002, "消息内容超过限制");
            m_errorCode.Add(45003, "标题字段超过限制");
            m_errorCode.Add(45004, "描述字段超过限制");
            m_errorCode.Add(45005, "链接字段超过限制");
            m_errorCode.Add(45006, "图片链接字段超过限制");
            m_errorCode.Add(45007, "语音播放时间超过限制");
            m_errorCode.Add(45008, "图文消息超过限制");
            m_errorCode.Add(45009, "接口调用超过限制");
            m_errorCode.Add(45010, "创建菜单个数超过限制");

            m_errorCode.Add(45015, "回复时间超过限制");
            m_errorCode.Add(45016, "系统分组，不允许修改");
            m_errorCode.Add(45017, "分组名字过长");
            m_errorCode.Add(45018, "分组数量超过上限");

            m_errorCode.Add(46001, "不存在媒体数据");
            m_errorCode.Add(46002, "不存在的菜单版本");
            m_errorCode.Add(46003, "不存在的菜单数据");
            m_errorCode.Add(46004, "不存在的用户");

            m_errorCode.Add(47001, "解析JSON/XML内容错误");

            m_errorCode.Add(48001, "api功能未授权");

            m_errorCode.Add(50001, "用户未授权该api");

            #endregion
        }

        #endregion

        public static string GetDatetimeNowString()
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01");

            return Convert.ToString(((Int64)ts.TotalSeconds));
        }

        public static DateTime GetDatetimeFromString(string datetimeString)
        {
            return DateTime.Parse("1970-01-01").AddSeconds(Convert.ToDouble(datetimeString));
        }

        #region 获取账号票证

        static Hashtable m_accessTokenList = new Hashtable();
        static object m_lockObject = new object();

        /// <summary>
        /// 获取账号Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appSecret)
        {
            string _key = Txooo.Text.EncryptHelper.MD5(appId + "_" + appSecret);
            string _accessToken = "";

            if (m_accessTokenList[_key] != null)
            {
                KeyValuePair<string, DateTime> _token = (KeyValuePair<string, DateTime>)m_accessTokenList[_key];
                if (_token.Value.AddSeconds(7000)>DateTime.Now)
                {
                    //有效
                    _accessToken = _token.Key;
                }
            }
            if (string.IsNullOrEmpty(_accessToken))
            {
                lock (m_lockObject)
                {
                    if (string.IsNullOrEmpty(_accessToken))
                    {
                        string _url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret + "";
                        string _str = new WebClient().DownloadString(_url);
                        _accessToken = Txooo.TxStringHelper.GetMiddleStr(_str, "{\"access_token\":\"", "\",\"expires_in");
                        m_accessTokenList[_key] = new KeyValuePair<string, DateTime>(_accessToken, DateTime.Now);
                    }
                }
            }
            return _accessToken;
        }

        #endregion

        #region 构造函数

        string m_appId;
        string m_appSecret;
        string m_accessToken;

        public WeixinHelper(string appId,string appSecret)
        {
            //AppId         wx17b0bfd50aa4117b
            //AppSecret     89c6eab9b6243fd229b28baa03e6ab6f

            m_appId = appId;
            m_appSecret = appSecret;
            m_accessToken = GetAccessToken(appId, appSecret);
        }

        #endregion

        #region 用户数据

        static Hashtable m_userInfo = new Hashtable();

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool GetUserInfo(string openId, out OpenUserInfo userInfo, out string errorInfo)
        {
            userInfo = m_userInfo[openId] as OpenUserInfo;
            errorInfo = "";
            try
            {
                if (userInfo == null)
                {
                    string _url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + m_accessToken + "&openid=" + openId;

                    string _str = "";
                    using (WebClient client = new WebClient())
                    {
                        client.Encoding = System.Text.UTF8Encoding.UTF8;
                        _str = client.DownloadString(_url);
                    }

                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        //请求错误                        
                        if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                        {
                            errorInfo = "未知错误：" + _str;
                        }
                        return false;
                    }

                    #region 对象赋值

                    userInfo = new OpenUserInfo(PlatformType.Weixin, openId);

                    if (_json.TryGetValue("subscribe", out value))
                    {
                        userInfo.Subscribe = value.ToString();
                    }
                    if (_json.TryGetValue("nickname", out value))
                    {
                        userInfo.Nickname = value.ToString();
                    }
                    if (_json.TryGetValue("sex", out value))
                    {
                        userInfo.Sex = value.ToString();
                    }
                    if (_json.TryGetValue("language", out value))
                    {
                        userInfo.Language = value.ToString();
                    }
                    if (_json.TryGetValue("city", out value))
                    {
                        userInfo.City = value.ToString();
                    }
                    if (_json.TryGetValue("province", out value))
                    {
                        userInfo.Province = value.ToString();
                    }
                    if (_json.TryGetValue("country", out value))
                    {
                        userInfo.Country = value.ToString();
                    }
                    if (_json.TryGetValue("headimgurl", out value))
                    {
                        userInfo.HeadimgUrl = value.ToString();
                    }
                    if (_json.TryGetValue("subscribe_time", out value))
                    {
                        userInfo.SubscribeTime = value.ToString();
                    }

                    m_userInfo[openId] = userInfo;

                    #endregion
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.TxLogError("获取用户信息错误[" + openId + "]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        #endregion

        #region 获取所有用户数据

        /// <summary>
        /// 获取所有用户数据
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool GetAllUserList(out List<string> userList,out string errorInfo)
        {
            userList = new List<string>();
            errorInfo = "";
            try
            {
                string _nextOpenId = "";
                do
                {
                    string _url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + m_accessToken + "&next_openid=" + _nextOpenId;
                    string _str = new WebClient().DownloadString(_url);
                    LitJson.JsonData _jsonData = LitJson.JsonMapper.ToObject(_str);
                    if (_str.Contains("errcode"))
                    {
                        //请求错误                        
                        if (!m_errorCode.TryGetValue(int.Parse(_jsonData["errcode"].ToString()), out errorInfo))
                        {
                            errorInfo = "未知错误：" + _str;
                        }
                        return false;
                    }
                    else
                    {
                        _nextOpenId = _jsonData["next_openid"].ToString();

                        LitJson.JsonData _openIdList = _jsonData["data"]["openid"];
                        foreach (LitJson.JsonData item in _openIdList)
                        {
                            userList.Add(item.ToString());
                        }
                    }
 

                    #region ///

                    //Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    //object value;
                    //if (_json.TryGetValue("errcode", out value))
                    //{
                    //    //请求错误                        
                    //    if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                    //    {
                    //        errorInfo = "未知错误：" + _str;
                    //    }
                    //    return false;
                    //}
                    //if (_json.TryGetValue("data", out value))
                    //{ 
                    //    Dictionary<string, object> _info = value as Dictionary<string, object>;
                    //    if (_info.TryGetValue("openid", out value))
                    //    {
                    //        object[] _listData = value as object[];
                    //        foreach (object item in _listData)
                    //        {
                    //            userList.Add(item.ToString());
                    //        }
                    //    }
                    //}
                    //if (_json.TryGetValue("next_openid", out value))
                    //{
                    //    _nextOpenId = value.ToString();
                    //}

                    #endregion

                } while (string.IsNullOrEmpty(_nextOpenId));

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("获取用户列表错误[]",ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        #endregion

        #region 发送客服消息

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool SendMessage(ResMsg msg,out string errorInfo)
        {
            errorInfo = "";
            try
            {
                //https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=SQVn0chTEr-zvfcQfcoACcXxCgt6_WJLH3d6mgvH6Pryj8Ars73zco7xlzi5otq6tOKvR7sZ-5EEqliiFdP3LCttw1IyDBl4rlmduHW8qI9NXzyDOll2lB0mbD5GATwZAUmxNDt--Egi2TOYzcYOzA
                string _url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + m_accessToken;
                string _str = "";
                string _body = GetSendJson(msg);

                #region 执行请求

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(_url);
                //_request.KeepAlive = true;
                _request.Method = "POST";
                string _postInfo = _body;
                byte[] _postBytes = Encoding.UTF8.GetBytes(_postInfo);
                _request.ContentLength = _postBytes.Length;
                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(_postBytes, 0, _postBytes.Length);
                }
                //获得请求结果
                using (HttpWebResponse _response = (HttpWebResponse)_request.GetResponse())
                {
                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream xportreaderstream = _response.GetResponseStream();
                        using (StreamReader xportreader = new StreamReader(xportreaderstream, Encoding.GetEncoding("utf-8")))
                        {
                            _str = xportreader.ReadToEnd();
                            xportreader.Close();
                            xportreaderstream.Close();
                            xportreaderstream.Dispose();
                        }
                        _response.Close();
                        _request.Abort();
                    }
                }

                #endregion

                #region 检测数据
                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #region 格式化消息数据

        /// <summary>
        /// 格式化消息数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetSendJson(ResMsg msg)
        {
            string _json = "{";

            _json += "\"touser\":\"" + msg.ToUserName + "\",";
            _json += "\"msgtype\":\"" + msg.MsgType.ToString().ToLower() + "\",";
            _json += "\"" + msg.MsgType.ToString().ToLower() + "\":{";

            if (msg.MsgType == ResMsgType.Text)
            {
                _json += "\"content\":\"" + Txooo.JS.JsonHelper.String2Json(((ResTextMsg)msg).Content) + "\"";
            }
            if (msg.MsgType == ResMsgType.Image)
            {
                //_json += "\"media_id\":\"" + Txooo.JS.JsonHelper.String2Json(((ResTextMsg)msg).Content) + "\"";
            }
            if (msg.MsgType == ResMsgType.News)
            {
                ResNewsMsg _news = (ResNewsMsg)msg;

                _json += "\"articles\": [";
                foreach (ResArticle item in _news.Articles)
                {
                    _json += "{";
                    _json += "\"title\":\"" + item.Title + "\",";
                    _json += "\"description\":\"" + item.Discription + "\",";
                    _json += "\"url\":\"" + item.Url + "\",";
                    _json += "\"picurl\":\"" + item.PicUrl + "\",";
                    _json += "},";
                }
                _json.Substring(0, (_json.Length - 1));
                _json += "]";
            }
            _json += "}";
            _json += "}";
            return _json;
        }

        #endregion

        #endregion

        #region 推送菜单数据

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool DeleteMenu(out string errorInfo)
        {
            errorInfo = "";
            try
            {
                string _url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + m_accessToken;
                string _str = new WebClient().DownloadString(_url);
                
                #region 检测数据

                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("删除自定义菜单错误：", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        /// <summary>
        /// 推送菜单数据
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool PostMenu(MenuInfo[] menu, out string errorInfo)
        {

            errorInfo = "";
            try
            {
                string _url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + m_accessToken;
                string _str = "";
                string _body = GetMenuJsonIncludeSub(menu);

                #region 执行请求

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(_url);
                //_request.KeepAlive = true;
                _request.Method = "POST";
                string _postInfo = _body;
                byte[] _postBytes = Encoding.UTF8.GetBytes(_postInfo);
                _request.ContentLength = _postBytes.Length;
                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(_postBytes, 0, _postBytes.Length);
                }
                //获得请求结果
                using (HttpWebResponse _response = (HttpWebResponse)_request.GetResponse())
                {
                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream xportreaderstream = _response.GetResponseStream();
                        using (StreamReader xportreader = new StreamReader(xportreaderstream, Encoding.GetEncoding("utf-8")))
                        {
                            _str = xportreader.ReadToEnd();
                            xportreader.Close();
                            xportreaderstream.Close();
                            xportreaderstream.Dispose();
                        }
                        _response.Close();
                        _request.Abort();
                    }
                }

                #endregion

                #region 检测数据
                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #region 格式化菜单数据


        string GetMenuJsonIncludeSub(MenuInfo[] menu)
        {
            string _json = "{";
            _json += "\"button\": [";

            foreach (MenuInfo item in menu)
            {
                if (item.SubMenu.Count == 0)
                {//无子菜单
                    _json += GetMenuJson(item) + ",";
                }
                else
                {//有子菜单
                    _json += "{";
                    _json += "\"name\":\"" + item.Name + "\",";
                    _json += "\"sub_button\":[";
                    foreach (MenuInfo subItem in item.SubMenu)
                    {
                        _json += GetMenuJson(subItem) + ",";
                    }
                    _json = _json.Substring(0, (_json.Length - 1));
                    _json += "]";
                    _json += "},";
                }
            }
            _json = _json.Substring(0, (_json.Length - 1));
            _json += "]";
            _json += "}";
            return _json;
        }

        string GetMenuJson(MenuInfo menu)
        {
            string _json = "{";

            if (menu.Type == MenuType.Click)
            {
                _json += "\"type\":\"click\",";
                _json += "\"name\":\"" + menu.Name + "\",";
                _json += "\"key\":\"" + menu.Vaule + "\"";
            }
            if (menu.Type == MenuType.View)
            {
                _json += "\"type\":\"view\",";
                _json += "\"name\":\"" + menu.Name + "\",";
                _json += "\"url\":\"" + menu.Vaule + "\"";
            }
            _json += "}";
            return _json;
        }

        #endregion

        #endregion

        #region 获取带参二维码

        #region 获取永久二维码

        /// <summary>
        /// 获取永久二维码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool GetLimitQrcode(int id,out string ticket,out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            try
            {
                string _url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + m_accessToken;
                string _str = "";
                string _body = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + id + "}}}";

                #region 执行请求

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(_url);
                //_request.KeepAlive = true;
                _request.Method = "POST";
                string _postInfo = _body;
                byte[] _postBytes = Encoding.UTF8.GetBytes(_postInfo);
                _request.ContentLength = _postBytes.Length;
                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(_postBytes, 0, _postBytes.Length);
                }
                //获得请求结果
                using (HttpWebResponse _response = (HttpWebResponse)_request.GetResponse())
                {
                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream xportreaderstream = _response.GetResponseStream();
                        using (StreamReader xportreader = new StreamReader(xportreaderstream, Encoding.GetEncoding("utf-8")))
                        {
                            _str = xportreader.ReadToEnd();
                            xportreader.Close();
                            xportreaderstream.Close();
                            xportreaderstream.Dispose();
                        }
                        _response.Close();
                        _request.Abort();
                    }
                }

                #endregion

                #region 检测数据
                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                    else if (_json.TryGetValue("ticket", out value))
                    {
                        ticket = value.ToString();
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #endregion

        #region 临时二维码

        /// <summary>
        /// 临时二维码
        /// </summary>
        /// <param name="sceneId"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool GetTempQrcode(int sceneId,int expireSeconds, out string ticket, out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            try
            {
                string _url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + m_accessToken;
                string _str = "";
                string _body = _body = "{\"expire_seconds\": " + expireSeconds + ", \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";

                #region 执行请求

                HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(_url);
                //_request.KeepAlive = true;
                _request.Method = "POST";
                string _postInfo = _body;
                byte[] _postBytes = Encoding.UTF8.GetBytes(_postInfo);
                _request.ContentLength = _postBytes.Length;
                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(_postBytes, 0, _postBytes.Length);
                }
                //获得请求结果
                using (HttpWebResponse _response = (HttpWebResponse)_request.GetResponse())
                {
                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream xportreaderstream = _response.GetResponseStream();
                        using (StreamReader xportreader = new StreamReader(xportreaderstream, Encoding.GetEncoding("utf-8")))
                        {
                            _str = xportreader.ReadToEnd();
                            xportreader.Close();
                            xportreaderstream.Close();
                            xportreaderstream.Dispose();
                        }
                        _response.Close();
                        _request.Abort();
                    }
                }

                #endregion

                #region 检测数据
                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                    else if (_json.TryGetValue("ticket", out value))
                    {
                        ticket = value.ToString();
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #endregion

        #endregion

        #region 上传媒体文件



        #endregion

        #region 网页授权获取用户基本信息

        ///获取code后，请求以下链接获取access_token： 
        ///https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code

        public bool Oauth2GetOpenId(string code, out string openId, out string errorInfo)
        {
            errorInfo = "";
            openId = "";
            try
            {
                string _url = "https://api.weixin.qq.com/sns/oauth2/access_token?"
                    + "appid=" + m_appId
                    + "secret=" + m_appSecret
                    + "code=" + code
                    + "grant_type= authorization_code"
                    ;
                string _str = new WebClient().DownloadString(_url);

                #region 检测数据
                if (!string.IsNullOrEmpty(_str))
                {
                    Dictionary<string, object> _json = Txooo.JS.JsonHelper.AnalyseJsonStr(_str);
                    object value;
                    if (_json.TryGetValue("errcode", out value))
                    {
                        if (value.ToString() != "0")
                        {
                            //请求错误                        
                            if (!m_errorCode.TryGetValue(int.Parse(value.ToString()), out errorInfo))
                            {
                                errorInfo = "未知错误：" + _str;
                            }
                            return false;
                        }
                    }
                    if (_json.TryGetValue("openid", out value))
                    {
                        openId = value.ToString();
                        return true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        #endregion
    }
}
