using LitJson;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 易信接口
    /// </summary>
    public class YixinApiHelper : ApiHelper
    {
        
        #region 静态构造函数

        /// <summary>
        /// 接口错误消息
        /// </summary>
        static Dictionary<int, string> m_errorCode;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static YixinApiHelper()
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
            //m_errorCode.Add(40029, "不合法的oauth_code");
            //m_errorCode.Add(40030, "不合法的refresh_token");
            //m_errorCode.Add(40031, "不合法的openid列表");
            //m_errorCode.Add(40032, "不合法的openid列表长度");
            //m_errorCode.Add(40033, "不合法的请求字符，不能包含\\uxxxx格式的字符");
            //m_errorCode.Add(40034, "");
            //m_errorCode.Add(40035, "不合法的参数");
            //m_errorCode.Add(40036, "");
            //m_errorCode.Add(40037, "");
            //m_errorCode.Add(40038, "不合法的请求格式");
            //m_errorCode.Add(40039, "不合法的URL长度");
            //m_errorCode.Add(40050, "不合法的分组id");
            //m_errorCode.Add(40051, "分组名字不合法");

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
        public YixinApiHelper(AccountInfo account)
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
                        if (GetAccessTokenByRemote(out token))
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
                        if (GetAccessTokenByRemote(out token))
                        {
                            JsonData json = JsonMapper.ToObject(token);
                            m_accountInfo.AccessToken = json["access_token"].ToString();                           
                            m_accountInfo.AccessTokenTime = DateTime.Now;
                            m_accountInfo.UpdateAccessTokenToDatabase();
                        }
                        m_isGetAccessToken = false;
                    }
                }
            }
        }

        /// <summary>
        /// 远程获取访问Token
        /// </summary>
        /// <returns></returns>
        public override bool GetAccessTokenByRemote(out string returnInfo)
        {
            returnInfo = string.Empty;
            try
            {
                string _url = "https://api.yixin.im/cgi-bin/token?grant_type=client_credential&appid=" + m_accountInfo.AppId + "&secret=" + m_accountInfo.AppSecret + "";

                returnInfo = new WebClient().DownloadString(_url);
                return returnInfo.IndexOf("errcode") == -1;
            } 
            catch (Exception ex)
            {
                this.TxLogError("远程获取访问Token错误：" + ex.Message, ex);
            }
            return false;
        }


        #endregion
        
        public override bool SendMessage(Platform.ResMsg msg, out string errorInfo)
        {
            errorInfo = "";
            try
            {
                GetAccessToken();
                string _url = "https://api.yixin.im/cgi-bin/message/custom/send?access_token=" + m_accountInfo.AccessToken;

                HttpTools req = new HttpTools(_url);
                string body = GetSendJson(msg);

                string returnInfo = req.Post(body);
                JsonData obj = JsonMapper.ToObject(returnInfo);

                return obj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }

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
                    string _url = "https://api.yixin.im/cgi-bin/user/get?access_token=" + m_accountInfo.AccessToken + "&next_openid=" + _nextOpenId;
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
                this.TxLogError("获取易信用户列表错误[]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        public override bool GetUserInfo(string openId, ref UserInfo userInfo, out string errorInfo)
        {        
            errorInfo = "";
            try
            {
                GetAccessToken();

                string _url = "https://api.yixin.im/cgi-bin/user/info?access_token=" + m_accountInfo.AccessToken + "&openid=" + openId;

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
                this.TxLogError("获取易信用户信息错误[" + openId + "]", ex);
                errorInfo = "程序错误：" + ex.Message;
            }
            return false;
        }

        public override bool DeleteMenu(out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool PostMenu(Platform.MenuInfo[] menu, out string errorInfo)
        {
            errorInfo = "";
            try
            {
                GetAccessToken();
                string _url = "https://api.yixin.im/cgi-bin/menu/create?access_token=" + m_accountInfo.AccessToken;

                string _body = GetMenuJsonIncludeSub(menu);
                HttpTools req = new HttpTools(_url);
                //主菜单最少2个，子菜单最少2个 否则报错
                string returnInfo = req.Post(_body);

                var jsonObj = LitJson.JsonMapper.ToObject(returnInfo);
                return jsonObj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                this.TxLogError("发送客服消息[]", ex);
                errorInfo = "程序错误：" + ex.Message; 
                //throw;
            }
            return false;       
        }

        public override bool GetLimitQrcode(int id, out string ticket, out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            try
            {
                m_isGetAccessToken = true;

                GetAccessToken();
                string _url = "https://api.yixin.im/cgi-bin/qrcode/create?access_token=" + m_accountInfo.AccessToken;

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
                this.TxLogError("获取易信永久二维码错误", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false; 
        }

        public override bool GetTempQrcode(int sceneId, int expireSeconds, out string ticket, out string errorInfo)
        {
            errorInfo = "";
            ticket = "";
            try
            {
                GetAccessToken();
                string _url = "https://api.yixin.im/cgi-bin/qrcode/create?access_token=" + m_accountInfo.AccessToken;

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
                else
                {
                    JsonData retObj = JsonMapper.ToObject(returnInfo);
                    ticket = retObj["ticket"].ToString();
                }
                return issucess;

            }
            catch (Exception ex)
            {
                this.TxLogError("获取易信临时二维码出错[]", ex);
                errorInfo = "程序错误：" + ex.Message;
                //throw;
            }
            return false;
        }
     
        public override bool GetUserGroup(out List<UserGroupInfo> userGroup, out string errorInfo)
        {
            errorInfo = "";
            userGroup = new List<UserGroupInfo>();
            try
            {
                GetAccessToken();
                HttpTools req = new HttpTools("https://api.yixin.im/cgi-bin/groups/get?access_token=" + m_accountInfo.AccessToken);

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

                string _url = "https://api.yixin.im/cgi-bin/groups/create?access_token=" + m_accountInfo.AccessToken;
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

                string _url = "https://api.yixin.im/cgi-bin/groups/update?access_token=" + m_accountInfo.AccessToken;

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

                string _url = " https://api.yixin.im/cgi-bin/groups/members/update?access_token=" + m_accountInfo.AccessToken;
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

        /// <summary>
        /// 格式化消息数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetSendJson(ResMsg msg)
        {
            JsonData json = new JsonData();
            json["touser"] = msg.ToUserName;
            var msgtype = msg.MsgType.ToString().ToLower();
            json["msgtype"] = msgtype;
            json[msgtype] = new JsonData();

            switch (msg.MsgType)
            {
                case ResMsgType.Text:
                    json[msgtype]["content"] = ((ResTextMsg)msg).Content;
                    break;

                case ResMsgType.Image:
                    {
                        var img = (ResImageMsg)msg;
                        HttpTools req = new HttpTools("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + m_accountInfo.AccessToken + "&type=image");
                        Files files = new Files();
                        //缺点必须jpeg格式
                        //new UploadFile(img.LocalUrl, new Random().Next(10000, 99999).ToString() + ".jpg", "image/jpeg"));
                        files.Add("media", new UploadFile(img.LocalUrl));
                        var returnJson = req.PostFile(files);
                        var obj = JsonMapper.ToObject(returnJson);
                        json[msgtype]["media_id"] = obj["media_id"].ToString();
                    }
                    break;

                case ResMsgType.Voice:
                    {
                        var voice = (ResVoiceMsg)msg;
                        HttpTools req = new HttpTools("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + m_accountInfo.AccessToken + "&type=voice");
                        Files files = new Files();
                        files.Add("media", new UploadFile(voice.LocalUrl));
                        var returnJson = req.PostFile(files);
                        var obj = JsonMapper.ToObject(returnJson);
                        json[msgtype]["media_id"] = obj["media_id"].ToString();
                    }
                    break;

                case ResMsgType.Video:
                    {
                        var video = (ResVideoMsg)msg;
                        HttpTools req = new HttpTools("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + m_accountInfo.AccessToken + "&type=video");
                        Files files = new Files();
                        files.Add("media", new UploadFile(video.LocalUrl));
                        var returnJson = req.PostFile(files);
                        var obj = JsonMapper.ToObject(returnJson);
                        json[msgtype]["media_id"] = obj["media_id"].ToString();
                        json[msgtype]["title"] = video.Title;
                        json[msgtype]["description"] = video.Description;
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

                case ResMsgType.News:
                    json[msgtype]["articles"] = new JsonData();
                    ResNewsMsg _news = (ResNewsMsg)msg;
                    JsonData articles;
                    foreach (ResArticle item in _news.Articles)
                    {
                        articles = new JsonData();
                        articles["title"] = item.Title;
                        articles["description"] = item.Discription;
                        articles["url"] = string.IsNullOrEmpty(item.Url) ? "http://w.txooo.com/showmsg.html?material=" + item.ID : item.Url;
                        articles["picurl"] = item.PicUrl;
                        json[msgtype]["articles"].Add(articles);
                    }
                    break;
            }

            return json.ToJson(false);
        }

        public override bool UploadMedia(ResMsg resMsg, out string returnInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByOpenId(string openIds, ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByGroup(long groupId, ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }
    }
}
