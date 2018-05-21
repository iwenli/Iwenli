using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Txooo.Mobile.Platform;
using LitJson;
using Txooo.Mobile.Platform.Entity;
using Txooo.Mobile.Wifi;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Txooo.Mobile
{
    /// <summary>
    /// 微信API操作
    /// </summary>
    public class WeixinApiHelper : ApiHelper
    {
        //讲堂服务号
        const string jtAppId = "wxafced53284792a47";
        const string jtAppSecret = "3f14d127d26546f0aea680d709317987";

        //商机服务号  
        const string AppId = "wx17b0bfd50aa4117b";
        const string AppSecret = "89c6eab9b6243fd229b28baa03e6ab6f";



        #region 静态构造函数

        /// <summary>
        /// 接口错误消息
        /// </summary>
        static Dictionary<int, string> m_errorCode;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static WeixinApiHelper()
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

        #region 构造函数

        AccountInfo m_accountInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account"></param>
        public WeixinApiHelper(AccountInfo account)
        {
            m_accountInfo = account;
        }

        #endregion

        #region 获取访问Token

        /// <summary>
        /// 是否需要重新获取Token
        /// </summary>
        bool m_isGetAccessToken = false;

        /// <summary>
        /// 获得访问Token
        /// </summary>
        public void GetAccessToken()
        {
            if (m_accountInfo.AccessTokenTime.AddSeconds(7000) < DateTime.Now
                || string.IsNullOrEmpty(m_accountInfo.AccessToken)
                )
            {
                lock (this)
                {
                    m_accountInfo.GetAccessTokenFromDatabase();
                    if (m_accountInfo.AccessTokenTime.AddSeconds(7000) < DateTime.Now)
                    {
                        string token;
                        if (GetAccessToken(out token))
                        {
                            JsonData json = JsonMapper.ToObject(token);
                            m_accountInfo.AccessToken = json["access_token"].ToString();
                            m_accountInfo.AccessTokenTime = DateTime.Now;
                            m_accountInfo.UpdateAccessTokenToDatabase();
                        }
                    }
                }
            }
            if (m_isGetAccessToken)
            {
                lock (this)
                {
                    if (m_isGetAccessToken)
                    {
                        string token;
                        if (GetAccessToken(out token))
                        {
                            //string _accessToken = Txooo.TxStringHelper.GetMiddleStr(_str, "{\"access_token\":\"", "\",\"expires_in");
                            JsonData json = JsonMapper.ToObject(token);
                            m_accountInfo.AccessToken = json["access_token"].ToString(); ;
                            m_accountInfo.AccessTokenTime = DateTime.Now;
                            m_accountInfo.UpdateAccessTokenToDatabase();

                            m_isGetAccessToken = false;
                        }
                    }
                }
            }
        }

        private bool GetAccessToken(out string returnInfo)
        {
            returnInfo = "";
            try
            {
                string _url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + m_accountInfo.AppId + "&secret=" + m_accountInfo.AppSecret + "";

                returnInfo = new WebClient().DownloadString(_url);

                return returnInfo.IndexOf("errcode") == -1;
            }
            catch (Exception ex)
            {
                this.TxLogError("远程获取访问Token错误：" + ex.Message, ex);
            }
            return false;
        }

        /// <summary>
        /// 远程获取访问Token
        /// </summary>
        /// <returns></returns>
        public override bool GetAccessTokenByRemote(out string returnInfo)
        {
            returnInfo = string.Empty;

            if (string.IsNullOrEmpty(m_accountInfo.AccessToken)
                || m_accountInfo.AccessTokenTime.AddSeconds(7000) < DateTime.Now)
            {
                lock (this)
                {
                    try
                    {
                        string _url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + m_accountInfo.AppId + "&secret=" + m_accountInfo.AppSecret + "";

                        returnInfo = new WebClient().DownloadString(_url);

                        bool _isOk = returnInfo.IndexOf("errcode") == -1;

                        if (_isOk)
                        {
                            JsonData json = JsonMapper.ToObject(returnInfo);
                            m_accountInfo.AccessToken = json["access_token"].ToString();
                            m_accountInfo.AccessTokenTime = DateTime.Now;
                            m_accountInfo.UpdateAccessTokenToDatabase();
                        }
                        return _isOk;
                    }
                    catch (Exception ex)
                    {
                        this.TxLogError("远程获取访问Token错误：" + ex.Message, ex);
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region 发送客服消息

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool SendMessage(Platform.ResMsg msg, out string returnInfo)
        {
            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + m_accountInfo.AccessToken;

                HttpTools req = new HttpTools(_url);
                string body = GetSendJson(msg);

                returnInfo = req.Post(body);
                JsonData obj = JsonMapper.ToObject(returnInfo);

                return obj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                returnInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        /// <summary>
        /// 根据openid群发
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool SendAllByOpenId(string openIds, Platform.ResMsg msg, out string returnInfo)
        {
            //{"touser": ["oR5Gjjl_eiZoUpGozMo7dbBJ362A", "oR5Gjjo5rXlMUocSEXKT7Q5RQ63Q" ], "msgtype": "text", "text": { "content": "hello from boxer."}}
            //{"touser":["OPENID1","OPENID2"],"mpnews":{"media_id":"123dsdajkasd231jhksad"},"msgtype":"mpnews"}

            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + m_accountInfo.AccessToken;

                JsonData data = null;
                switch (msg.MsgType)
                {
                    case ResMsgType.Text:
                        {
                            string jsonString = msg.GetWeixinResJson();
                            data = JsonMapper.ToObject(jsonString);
                            string[] openAry = openIds.Split(',');
                            data["touser"] = new JsonData();
                            foreach (var item in openAry)
                            {
                                data["touser"].Add(item);
                            }
                        }
                        break;
                    case ResMsgType.News:
                        {
                            var _newsMsg = (ResNewsMsg)msg;
                            string _upnewsUrl = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + m_accountInfo.AccessToken;

                            var articledata = new JsonData();
                            articledata["articles"] = new JsonData();
                            foreach (var item in _newsMsg.Articles)
                            {
                                var _article = JsonMapper.ToObject(item.GetUploadJson());

                                ResImageMsg _imageMsg = new ResImageMsg("");
                                _imageMsg.LocalUrl = item.PicUrl;
                                string _imgReturnInfo;
                                if (UploadMedia(_imageMsg, out _imgReturnInfo))
                                {
                                    _article["thumb_media_id"] = JsonMapper.ToObject(_imgReturnInfo)["media_id"].ToString();
                                }
                                articledata["articles"].Add(_article);
                            }

                            HttpTools _req = new HttpTools(_upnewsUrl);
                            string _body = articledata.ToJson(false);
                            string _returnInfo = _req.Post(_body);
                            JsonData obj = JsonMapper.ToObject(_returnInfo);


                            data = new JsonData();
                            data["msgtype"] = "mpnews";
                            data["mpnews"] = new JsonData();
                            data["mpnews"]["media_id"] = obj["media_id"].ToString();
                            string[] openAry = openIds.Split(',');
                            data["touser"] = new JsonData();
                            foreach (var item in openAry)
                            {
                                data["touser"].Add(item);
                            }
                        }
                        break;
                }


                HttpTools req = new HttpTools(_url);
                string body = data.ToJson(false);

                returnInfo = req.Post(body);
                JsonData robj = JsonMapper.ToObject(returnInfo);

                return robj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                returnInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;

        }

        /// <summary>
        /// 根据分组群发
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool SendAllByGroup(long groupId, Platform.ResMsg msg, out string returnInfo)
        {
            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=" + m_accountInfo.AccessToken;

                JsonData data = null;
                switch (msg.MsgType)
                {
                    case ResMsgType.Text:
                        {
                            string jsonString = "{\"filter\":{\"is_to_all\":false,\"group_id\":\"2\"},\"text\":{\"content\":\"CONTENT\"},\"msgtype\":\"text\"}";
                            data = JsonMapper.ToObject(jsonString);
                            data["filter"]["group_id"] = groupId;
                            data["text"]["content"] = ((ResTextMsg)msg).Content;
                        }
                        break;
                    case ResMsgType.News:
                        {
                            var _newsMsg = (ResNewsMsg)msg;
                            string _upnewsUrl = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + m_accountInfo.AccessToken;

                            var articledata = new JsonData();
                            articledata["articles"] = new JsonData();
                            foreach (var item in _newsMsg.Articles)
                            {
                                var _article = JsonMapper.ToObject(item.GetUploadJson());

                                ResImageMsg _imageMsg = new ResImageMsg("");
                                _imageMsg.LocalUrl = item.PicUrl;
                                string _imgReturnInfo;
                                if (UploadMedia(_imageMsg, out _imgReturnInfo))
                                {
                                    _article["thumb_media_id"] = JsonMapper.ToObject(_imgReturnInfo)["media_id"].ToString();
                                }
                                articledata["articles"].Add(_article);
                            }

                            HttpTools _req = new HttpTools(_upnewsUrl);
                            string _body = articledata.ToJson(false);
                            string _returnInfo = _req.Post(_body);
                            JsonData obj = JsonMapper.ToObject(_returnInfo);

                            string jsonString = "{\"filter\":{\"is_to_all\":false，\"group_id\":\"2\"},\"mpnews\":{\"media_id\":\"123dsdajkasd231jhksad\"},\"msgtype\":\"mpnews\"}";
                            data = JsonMapper.ToObject(jsonString);
                            data["filter"]["group_id"] = groupId;
                            data["mpnews"]["media_id"] = obj["media_id"].ToString();
                        }
                        break;
                }


                HttpTools req = new HttpTools(_url);
                string body = data.ToJson(false);

                returnInfo = req.Post(body);
                JsonData robj = JsonMapper.ToObject(returnInfo);

                return robj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送分组客服消息[]", ex);
                returnInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #endregion

        #region 获取所有用户列表

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool GetAllUserList(out List<string> userList, out string errorInfo)
        {
            userList = new List<string>();
            errorInfo = "";
            try
            {
                GetAccessToken();
                string _nextOpenId = "";
                do
                {
                    string _url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + m_accountInfo.AccessToken + "&next_openid=" + _nextOpenId;
                    string _str = new WebClient().DownloadString(_url);
                    LitJson.JsonData _jsonData = LitJson.JsonMapper.ToObject(_str);
                    if (_str.Contains("errcode"))
                    {
                        //请求错误                        
                        if (!m_errorCode.TryGetValue(int.Parse(_jsonData["errcode"].ToString()), out errorInfo))
                        {
                            errorInfo = "未知错误：" + _str;
                        }
                        if (errorInfo.Contains("access_token"))
                        {
                            m_isGetAccessToken = true;
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

                } while (string.IsNullOrEmpty(_nextOpenId));

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("获取用户列表错误[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        #endregion

        #region 获取用户数据

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool GetUserInfo(string openId, ref UserInfo userInfo, out string errorInfo)
        {
            errorInfo = "";
            try
            {
                GetAccessToken();

                string _url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + m_accountInfo.AccessToken + "&openid=" + openId;

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
                    if (errorInfo.Contains("access_token"))
                    {
                        m_isGetAccessToken = true;
                    }

                    return false;
                }

                #region 对象赋值

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

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.TxLogError("获取用户信息错误[" + openId + "]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        #endregion

        #region 删除自定义菜单

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool DeleteMenu(out string errorInfo)
        {
            errorInfo = "";
            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + m_accountInfo.AccessToken;
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
                            if (errorInfo.Contains("access_token"))
                            {
                                m_isGetAccessToken = true;
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

        #endregion

        #region 推送菜单数据

        /// <summary>
        /// 推送菜单数据
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool PostMenu(Platform.MenuInfo[] menu, out string returnInfo)
        {
            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + m_accountInfo.AccessToken;

                string _body = GetMenuJsonIncludeSub(menu);
                HttpTools req = new HttpTools(_url);
                returnInfo = req.Post(_body);

                var jsonObj = LitJson.JsonMapper.ToObject(returnInfo);
                return jsonObj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                returnInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

        #endregion

        #region 获取永久二维码

        /// <summary>
        /// 获取永久二维码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool GetLimitQrcode(int id, out string ticket, out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            try
            {
                m_isGetAccessToken = true;

                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + m_accountInfo.AccessToken;

                var json = new JsonData();
                json["action_name"] = "QR_LIMIT_SCENE";
                json["action_info"] = new JsonData();
                json["action_info"]["scene"] = new JsonData();
                json["action_info"]["scene"]["scene_id"] = id;

                string _body = json.ToJson(false);//"{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + id + "}}}";

                HttpTools req = new HttpTools(_url);
                string returnInfo = req.Post(_body);

                bool issucess = returnInfo.IndexOf("errcode") == -1;
                if (!issucess) errorInfo = returnInfo;
                else
                {
                    JsonData retObj = JsonMapper.ToObject(returnInfo);
                    ticket = retObj["ticket"].ToString();
                }
                return issucess;

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

        #region 获取临时二维码

        /// <summary>
        /// 获取临时二维码
        /// </summary>
        /// <param name="sceneId"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public override bool GetTempQrcode(int sceneId, int expireSeconds, out string ticket, out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            //if (sceneId < 100000)
            //{
            //    errorInfo = "临时二维码id必须大于100000";
            //    return false;
            //}
            try
            {
                GetAccessToken();
                string _url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + m_accountInfo.AccessToken;

                var json = new JsonData();
                json["expire_seconds"] = expireSeconds;
                json["action_name"] = "QR_SCENE";
                json["action_info"] = new JsonData();
                json["action_info"]["scene"] = new JsonData();
                json["action_info"]["scene"]["scene_id"] = sceneId;

                string _body = json.ToJson(false);//"{\"expire_seconds\": " + expireSeconds + ", \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";

                HttpTools req = new HttpTools(_url);
                string returnInfo = req.Post(_body);

                bool issucess = returnInfo.IndexOf("errcode") == -1;
                if (!issucess) errorInfo = returnInfo;

                JsonData retObj = JsonMapper.ToObject(returnInfo);
                ticket = retObj["ticket"].ToString();

                return issucess;

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

        #region 格式化消息数据

        /// <summary>
        /// 格式化消息数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetSendJson(ResMsg msg)
        {
            string _data = "";
            switch (msg.MsgType)
            {
                case ResMsgType.Image:
                    {
                        var _imageMsg = (ResImageMsg)msg;
                        string _uploadInfo;
                        if (UploadMedia(_imageMsg, out _uploadInfo))
                        {
                            var obj = JsonMapper.ToObject(_uploadInfo);
                            _imageMsg.MediaId = obj["media_id"].ToString();
                            _data = _imageMsg.GetWeixinResJson();
                        }
                    }
                    break;

                case ResMsgType.Voice:
                    {
                        var _voiceMsg = (ResVoiceMsg)msg;
                        string _uploadInfo;
                        if (UploadMedia(_voiceMsg, out _uploadInfo))
                        {
                            var obj = JsonMapper.ToObject(_uploadInfo);
                            _voiceMsg.MediaId = obj["media_id"].ToString();
                            _data = _voiceMsg.GetWeixinResJson();
                        }
                    }
                    break;

                case ResMsgType.Video:
                    {
                        var _videoMsg = (ResVideoMsg)msg;
                        string _uploadInfo;
                        if (UploadMedia(_videoMsg, out _uploadInfo))
                        {
                            var obj = JsonMapper.ToObject(_uploadInfo);
                            _videoMsg.MediaId = obj["media_id"].ToString();
                            _data = _videoMsg.GetWeixinResJson();
                        }
                    }
                    break;

                //case ResMsgType.Music:
                //    var music = (ResMusicMsg)msg;                   
                //    json[msgtype]["title"] = music.Title;
                //    json[msgtype]["description"] = music.Description;
                //    json[msgtype]["musicurl"] = music.MusicURL;
                //    json[msgtype]["hqmusicurl"] = music.HQMusicUrl;
                //    json[msgtype]["thumb_media_id"] = music.ThumbMediaId;
                //    break;

                default:
                    _data = msg.GetWeixinResJson();
                    break;
            }
            return _data;
        }

        #endregion

        #region 格式化菜单数据

        /// <summary>
        /// 格式化菜单数据
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        string GetMenuJsonIncludeSub(MenuInfo[] menu)
        {
            JsonData Json = new JsonData();
            Json["button"] = new JsonData();

            JsonData btn;
            foreach (MenuInfo item in menu)
            {
                btn = new JsonData();
                if (item.SubMenu.Count > 0)
                {
                    btn["name"] = item.Name;
                    btn["sub_button"] = new JsonData();
                    JsonData subbtn;
                    foreach (MenuInfo subItem in item.SubMenu)
                    {
                        subbtn = new JsonData();
                        if (subItem.Type == MenuType.Click)
                        {
                            subbtn["type"] = "click";
                            subbtn["name"] = subItem.Name;
                            subbtn["key"] = subItem.Vaule;
                        }
                        if (subItem.Type == MenuType.View)
                        {
                            subbtn["type"] = "view";
                            subbtn["name"] = subItem.Name;
                            subbtn["url"] = subItem.Vaule;
                        }
                        btn["sub_button"].Add(subbtn);
                    }
                }
                else
                {
                    if (item.Type == MenuType.Click)
                    {
                        btn["type"] = "click";
                        btn["name"] = item.Name;
                        btn["key"] = item.Vaule;
                    }
                    if (item.Type == MenuType.View)
                    {
                        btn["type"] = "view";
                        btn["name"] = item.Name;
                        btn["url"] = item.Vaule;
                    }
                }

                Json["button"].Add(btn);
            }
            return Json.ToJson(false);
        }

        #endregion

        #region 用户分组操作

        public override bool GetUserGroup(out List<UserGroupInfo> userGroup, out string errorInfo)
        {
            errorInfo = "";
            userGroup = new List<UserGroupInfo>();
            try
            {
                GetAccessToken();
                HttpTools req = new HttpTools("https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + m_accountInfo.AccessToken);

                string returnInfo = req.Get();

                var jsonData = LitJson.JsonMapper.ToObject(returnInfo);
                var jsonGroup = jsonData["groups"];

                UserGroupInfo groupInfo;
                foreach (LitJson.JsonData item in jsonGroup)
                {
                    groupInfo = new UserGroupInfo { GroupID = (int)item["id"], Groupname = item["name"].ToString(), UserCount = (int)item["count"] };
                    userGroup.Add(groupInfo);
                }

                return returnInfo.IndexOf("errcode") == -1;
            }
            catch (Exception ex)
            {
                this.TxLogError("获取用户分组[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        public override bool CreateGroup(UserGroupInfo groupInfo, out string errorInfo)
        {
            errorInfo = "";

            try
            {
                GetAccessToken();

                var jsonData = new LitJson.JsonData();
                jsonData["group"] = new LitJson.JsonData();
                jsonData["group"]["name"] = groupInfo.Groupname;

                string _url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + m_accountInfo.AccessToken;
                HttpTools req = new HttpTools(_url);

                string body = jsonData.ToJson(false);

                string returnInfo = req.Post(body);

                return returnInfo.IndexOf("errcode") == -1;
            }
            catch (Exception ex)
            {
                this.TxLogError("创建分组[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        public override bool ModifyGroupname(UserGroupInfo groupInfo, out string errorInfo)
        {
            errorInfo = "";

            try
            {
                GetAccessToken();

                var jsonData = new LitJson.JsonData();
                jsonData["group"] = new LitJson.JsonData();
                jsonData["group"]["id"] = groupInfo.GroupID;
                jsonData["group"]["name"] = groupInfo.Groupname;

                string _url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + m_accountInfo.AccessToken;

                HttpTools req = new HttpTools(_url);
                string body = jsonData.ToJson(false);

                string returnInfo = req.Post(body);

                var jsonObj = LitJson.JsonMapper.ToObject(returnInfo);
                return jsonObj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("修改分组名称[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        public override bool MoveUserToGroup(string openId, int toGroupID, out string errorInfo)
        {
            errorInfo = "";
            try
            {
                GetAccessToken();

                var jsonData = new LitJson.JsonData();
                jsonData["openid"] = openId;
                jsonData["to_groupid"] = toGroupID;

                string _url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=" + m_accountInfo.AccessToken;
                string body = jsonData.ToJson(false);
                HttpTools req = new HttpTools(_url);
                string returnInfo = req.Post(body);
                var jsonObj = LitJson.JsonMapper.ToObject(returnInfo);
                return jsonObj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("移动用户到分组[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        #endregion

        #region 上传文件
        /// <summary>
        /// 上传媒体
        /// </summary>
        /// <param name="resMsg"></param>
        /// <param name="returnInfo"></param>
        /// <returns></returns>
        public override bool UploadMedia(ResMsg resMsg, out string returnInfo)
        {
            var _msgType = resMsg.MsgType.ToString().ToLower();
            string _fileUrl = "";
            switch (resMsg.MsgType)
            {
                case ResMsgType.Image:
                    _fileUrl = ((ResImageMsg)resMsg).LocalUrl;
                    break;
                //case ResMsgType.Music:
                //    _fileUrl = ((ResMusicMsg)resMsg).MusicURL;
                //break;
                case ResMsgType.Video:
                    _fileUrl = ((ResVideoMsg)resMsg).LocalUrl;
                    break;
                case ResMsgType.Voice:
                    _fileUrl = ((ResVoiceMsg)resMsg).LocalUrl;
                    break;
            }

            HttpTools req = new HttpTools("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + m_accountInfo.AccessToken + "&type=" + _msgType);
            Files files = new Files();
            files.Add("media", new UploadFile(_fileUrl));
            returnInfo = req.PostFile(files);
            return returnInfo.IndexOf("errcode") == -1;
        }

        #endregion

        #region 设置备注名
        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId">openid</param>
        /// <param name="remark">备注名</param>
        /// <param name="errorInfo">输出参数，返回的错误消息</param>
        /// <returns></returns>
        public override bool UpdateRemark(string openId, string remark, out string errorInfo)
        {
            errorInfo = string.Empty;
            bool returnMsg = true;
            try
            {
                GetAccessToken();

                string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}",
                                           m_accountInfo.AccessToken);

                JsonData requestJson = new JsonData();
                requestJson["openid"] = openId;
                requestJson["remark"] = remark;
                string body = requestJson.ToJson(false);

                HttpTools req = new HttpTools(url);
                string returnInfo = req.Post(body);

                JsonData responseJson = JsonMapper.ToObject(returnInfo);
                if (responseJson["errcode"].ToString() != "0")
                {
                    errorInfo = returnInfo;
                    returnMsg = false;
                }
                return returnMsg;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                return false;
            }
        }
        #endregion

        #region 发送模板消息
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="returnInfo"></param>
        /// <returns></returns>
        public override bool SendTempMsg(ResTemplateMsg templateMsg, out string returnInfo)
        {
            try
            {
                GetAccessToken();


                HttpTools tool = new HttpTools(string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", m_accountInfo.AccessToken));

                JsonData jsonData = new JsonData();
                jsonData["touser"] = templateMsg.ToUserName;
                jsonData["template_id"] = templateMsg.TemplateId;
                jsonData["url"] = templateMsg.Url;
                jsonData["topcolor"] = templateMsg.TopColor;
                jsonData["data"] = new JsonData();

                foreach (TemplateData templateData in templateMsg.TemplateDatas)
                {
                    jsonData["data"][templateData.Attribute] = new JsonData();
                    jsonData["data"][templateData.Attribute]["value"] = templateData.Value;
                    jsonData["data"][templateData.Attribute]["color"] = templateData.Color;
                }

                string jsonStr = JsonMapper.ToJson(jsonData);

                returnInfo = tool.Post(jsonStr);//微信返回的json字符串

                JsonData returnJsonData = JsonMapper.ToObject(returnInfo);

                if (returnJsonData["errcode"].ToString() == "0")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                returnInfo = "程序错误：" + ex.Message;
                return false;
            }
        }
        #endregion

        #region 获取微信预支付订单prepay_id
        public string GetWxPayPreId(Parameters preParam, out string returnInfo)
        {
            string _url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            //后台默认参数调用时不传
            preParam.Add("mch_id", this.m_accountInfo.MchId);
            preParam.Add("appid", this.m_accountInfo.AppId);
            preParam.Add("nonce_str", HttpTools.GetBoundary());
            preParam.Add("fee_type", "CNY");
            preParam.Add("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            preParam.Add("goods_tag", "WXG");
            if (preParam.Items.FindAll((kv) => { return kv.Key == "trade_type"; }).Count == 0)
                preParam.Add("trade_type", "JSAPI");
            preParam.Sort();

            string _keyStr = preParam.BuildQueryString(false) + "&key=" + this.m_accountInfo.MchSecret;
            string _sign = Txooo.Text.EncryptHelper.MD5(_keyStr).ToUpper();

            preParam.Add("sign", _sign);

            XElement _root = new XElement("xml");
            XElement _param;
            XCData _cData;
            foreach (var item in preParam.Items)
            {
                _cData = new XCData(item.Value);
                _param = new XElement(item.Key, _cData);
                _root.Add(_param);
            }
            var _sendInfo = new XDocument(_root);

            HttpTools http = new HttpTools(_url);
            returnInfo = http.Post(_sendInfo.ToString());

            XDocument _returnXml = XDocument.Parse(returnInfo);
            XElement _preInfo = _returnXml.Root.Element("prepay_id");

            return _preInfo?.Value;
        }

        #endregion

        #region 获取WxJSAPI验证票据

        static Hashtable m_ticketList = new Hashtable();

        /// <summary>
        /// 获取JSAPI票据
        /// </summary>
        /// <param name="isCreateSign">是否直接返回sign</param>
        /// <param name="signParam">isCreateSign为true时需提供参数noncestr,timestamp,url</param>
        /// <returns></returns>
        public string GetJSAPITicket(bool isCreateSign, Parameters signParam)
        {
            var _ticket = m_ticketList[this.m_accountInfo.AccountId];

            if (_ticket == null || (DateTime)(((KeyValuePair<string, DateTime>)_ticket).Value).AddSeconds(7000) < DateTime.Now)
            {
                string returnInfo;
                bool _isok = this.GetAccessTokenByRemote(out returnInfo);

                if (_isok)
                {
                    HttpTools req = new HttpTools("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + this.m_accountInfo.AccessToken + "&type=jsapi");
                    string wxJs = req.Get();

                    var reJson = LitJson.JsonMapper.ToObject(wxJs);
                    m_ticketList[this.m_accountInfo.AccountId] = _ticket = new KeyValuePair<string, DateTime>(reJson["ticket"].ToString(), DateTime.Now);
                }
            }
            if (!isCreateSign)
            {
                return ((KeyValuePair<string, DateTime>)_ticket).Key;
            }

            signParam.Add("jsapi_ticket", ((KeyValuePair<string, DateTime>)_ticket).Key);
            signParam.Sort();

            var _sign = Txooo.Text.EncryptHelper.SHA1(signParam.BuildQueryString(false));

            return _sign;
        }

        #endregion

        #region 长链接转短链接

        /// <summary>
        /// 长链接转短链接
        /// </summary>
        /// <param name="longUrl"></param>
        /// <param name="returnInfo"></param>
        /// <returns></returns>
        public string Url2Short(string longUrl, out string returnInfo)
        {
            returnInfo = "0";

            try
            {
                GetAccessToken();

                string url = string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}",
                                           m_accountInfo.AccessToken);

                JsonData requestJson = new JsonData();
                requestJson["action"] = "long2short";
                requestJson["long_url"] = longUrl;
                string body = requestJson.ToJson(false);

                HttpTools req = new HttpTools(url);
                returnInfo = req.Post(body);

                JsonData responseJson = JsonMapper.ToObject(returnInfo);
                if (responseJson["errcode"].ToString() == "0")
                {
                    return responseJson["short_url"].ToString();
                }
            }
            catch (Exception ex)
            {
                this.TxLogError("长转短链接错误消息[]", ex);
            }
            return returnInfo;
        }

        #endregion

        #region 获取扫码支付二维码url

        public string GetScanPayQrUrl(string productId, out string returnInfo)
        {
            returnInfo = "";
            try
            {
                string _url = "weixin://wxpay/bizpayurl?";

                Parameters _scanQrParams = new Parameters();

                _scanQrParams.Add("appid", this.m_accountInfo.AppId);
                _scanQrParams.Add("mch_id", this.m_accountInfo.MchId);
                _scanQrParams.Add("product_id", productId);
                _scanQrParams.Add("time_stamp", (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
                _scanQrParams.Add("nonce_str", HttpTools.GetBoundary());
                _scanQrParams.Sort();

                string _keyStr = _scanQrParams.BuildQueryString(false) + "&key=" + this.m_accountInfo.MchSecret;
                string _sign = Txooo.Text.EncryptHelper.MD5(_keyStr).ToUpper();
                _scanQrParams.Add("sign", _sign);

                string _qr = _url + _scanQrParams.BuildQueryString(false);

                string _shortUrl = Url2Short(_qr, out returnInfo);
                return _shortUrl;
            }
            catch (Exception ex)
            {
                this.TxLogError("生成扫码支付二维码url错误消息[]", ex);
            }
            return returnInfo;
        }

        #endregion

        #region 从账户信息中获取证书内容
        /// <summary>
        /// 获取证书,如果当前证书不存在,则从账户信息中获取证书内容,如果也没有则从本地计算机账户下的个人存储区读取证书
        /// </summary>
        /// <param name="cert"></param>
        /// <returns></returns>
        private X509Certificate GetCert(X509Certificate cert)
        {
            #region Bytes读取证书
            if (cert == null)
            {
                if (this.m_accountInfo.Cert != null && this.m_accountInfo.Cert.Length > 0)
                {
                    cert = new X509Certificate(this.m_accountInfo.Cert, this.m_accountInfo.MchId);
                }
                else
                {
                    this.TxLogInfo(string.Format("数据库中没有获取到accountId={0}的证书", this.m_accountInfo.AccountId));
                }
            }
            #endregion

            #region X509Store证书 存储在本地计算机个人证书中
            if (cert == null && this.m_accountInfo.CertName != null)
            {
                X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                x509Store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, this.m_accountInfo.CertName, false);
                if (x509Certificate2Collection.Count > 0)
                {
                    cert = x509Certificate2Collection[0];
                }
            }
            else
            {
                this.TxLogInfo(string.Format("本地计算机账户下的个人存储区中没有获取到accountId={0},certName={1}的证书", this.m_accountInfo.AccountId, this.m_accountInfo.CertName ?? ""));
            }

            #endregion
            if (cert == null)
            {
                throw new Exception("Api证书为空");
            }
            return cert;
        }
        #endregion

        #region 企业付款到零钱
        /// <summary>
        /// 企业转账
        /// 文档地址https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2
        /// </summary>
        /// <param name="preParam">参数,除mchid，mch_appid，nonce_str，sign之外的其他参数集合</param>
        /// <param name="returnInfo">付款同步返回结果</param>
        /// <param name="cert">微信商户证书,默认为空,表示从账户存储中使用证书</param>
        /// <returns>是否付款成功</returns>
        public bool Transfers(Parameters preParam, out string returnInfo, X509Certificate cert = null)
        {
            cert = GetCert(cert);
            string _url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
            //后台默认参数调用时不传
            preParam.Add("mchid", this.m_accountInfo.MchId);
            preParam.Add("mch_appid", this.m_accountInfo.AppId);
            preParam.Add("nonce_str", HttpTools.GetBoundary());
            preParam.Sort();

            string _keyStr = preParam.BuildQueryString(false) + "&key=" + this.m_accountInfo.MchSecret;
            string _sign = Txooo.Text.EncryptHelper.MD5(_keyStr).ToUpper();

            preParam.Add("sign", _sign);

            XElement _root = new XElement("xml");
            XElement _param;
            XCData _cData;
            foreach (var item in preParam.Items)
            {
                _cData = new XCData(item.Value);
                _param = new XElement(item.Key, _cData);
                _root.Add(_param);
            }
            var _sendInfo = new XDocument(_root);

            HttpTools http = new HttpTools(_url);
            returnInfo = http.Post(_sendInfo.ToString(), cert);

            XDocument _returnXml = XDocument.Parse(returnInfo);

            return _returnXml.Root.Element("return_code")?.Value == "SUCCESS" &&
                _returnXml.Root.Element("result_code")?.Value == "SUCCESS";
        }
        /// <summary>
        /// 查询企业付款，询企业付款API只支持查询30天内的订单，30天之前的订单请登录商户平台查询。
        /// 文档地址https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2
        /// </summary>
        /// <param name="partnerTradeNo">商户调用企业付款API时使用的商户订单号</param>
        /// <returns>付款操作详细</returns>
        public string GetTransferInfo(string partnerTradeNo)
        {
            string _url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gettransferinfo";
            Parameters preParam = new Parameters();
            //后台默认参数调用时不传
            preParam.Add("nonce_str", HttpTools.GetBoundary());
            preParam.Add("mch_id", this.m_accountInfo.MchId);
            preParam.Add("appid", this.m_accountInfo.AppId);
            preParam.Add("partner_trade_no", partnerTradeNo);
            preParam.Sort();

            string _keyStr = preParam.BuildQueryString(false) + "&key=" + this.m_accountInfo.MchSecret;
            string _sign = Txooo.Text.EncryptHelper.MD5(_keyStr).ToUpper();

            preParam.Add("sign", _sign);

            XElement _root = new XElement("xml");
            XElement _param;
            XCData _cData;
            foreach (var item in preParam.Items)
            {
                _cData = new XCData(item.Value);
                _param = new XElement(item.Key, _cData);
                _root.Add(_param);
            }
            var _sendInfo = new XDocument(_root);

            HttpTools http = new HttpTools(_url);
            return http.Post(_sendInfo.ToString());
        }

        #endregion

        #region 申请退款
        /// <summary>
        /// 申请退款
        /// 文档地址：https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=9_4&index=6
        /// </summary>
        /// <param name="preParam">参数,appid，mch_id，nonce_str，sign之外的其他参数集合</param>
        /// <param name="returnInfo">付款同步返回xml结果信息</param>
        /// <param name="cert">微信商户证书,默认为空,表示从账户存储中使用证书</param>
        /// <returns>退款是否成功</returns>
        public bool Refund(Parameters preParam, out string returnInfo, X509Certificate cert = null)
        {
            cert = GetCert(cert);
            string _url = "https://api.mch.weixin.qq.com/secapi/pay/refund";

            preParam.Add("appid", this.m_accountInfo.AppId);//公众账号ID
            preParam.Add("mch_id", this.m_accountInfo.MchId);//商户号
            preParam.Add("nonce_str", HttpTools.GetBoundary());//随机字符串           
            preParam.Sort();

            string _keyStr = preParam.BuildQueryString(false) + "&key=" + this.m_accountInfo.MchSecret;
            string _sign = Txooo.Text.EncryptHelper.MD5(_keyStr).ToUpper();

            preParam.Add("sign", _sign);

            XElement _root = new XElement("xml");
            XElement _param;
            XCData _cData;
            foreach (var item in preParam.Items)
            {
                _cData = new XCData(item.Value);
                _param = new XElement(item.Key, _cData);
                _root.Add(_param);
            }
            var _sendInfo = new XDocument(_root);

            HttpTools http = new HttpTools(_url);
            returnInfo = http.Post(_sendInfo.ToString(), cert);

            XDocument _returnXml = XDocument.Parse(returnInfo);

            return _returnXml.Root.Element("return_code")?.Value == "SUCCESS" &&
                _returnXml.Root.Element("result_code")?.Value == "SUCCESS";
        }

        #endregion
    }
}
