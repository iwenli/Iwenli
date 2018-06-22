#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：DefaultHandler
 *  所属项目：Iwenli.Mobile.Msgx
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/6/11 9:53:40
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using Iwenli.Mobile.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using Iwenli.Extension;
using System.Collections;

namespace Iwenli.Mobile.Msgx
{
    /// <summary>
    /// 默认消息处理类
    /// </summary>
    public class DefaultHandler : MessageHandler
    {
        #region 提取请求消息

        /// <summary>
        /// 提取请求消息
        /// </summary>
        /// <returns></returns>
        public override Platform.ReqMsg GetRequestMessage()
        {
            if (m_requestBody.Contains("<MsgType><![CDATA[text]]></MsgType>"))
            {
                #region 文本消息

                ReqTextMsg _msg = new ReqTextMsg(PlatformType.Weixin);
                //_msg.MsgId = m_requestBody.SearchStringTag("<MsgId>", "</MsgId>", 0, false).ToInt64();
                //_msg.ToUserName = m_requestBody.SearchStringTag("<ToUserName><![CDATA[", "]]></ToUserName>", 0, false);
                //_msg.FromUserName = m_requestBody.SearchStringTag("<FromUserName><![CDATA[", "]]></FromUserName>", 0, false);
                //_msg.CreateTime = m_requestBody.SearchStringTag("<CreateTime>", "</CreateTime>", 0, false);
                _msg.GetBaseInfo(m_requestBody);
                _msg.Content = m_requestBody.SearchStringTag("<Content><![CDATA[", "]]></Content>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[image]]></MsgType>"))
            {
                #region 图片消息

                ReqImageMsg _msg = new ReqImageMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);
                _msg.PicUrl = m_requestBody.SearchStringTag("<PicUrl><![CDATA[", "]]></PicUrl>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 语音消息

                ReqVoiceMsg _msg = new ReqVoiceMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);
                _msg.MediaId = m_requestBody.SearchStringTag("<MediaId><![CDATA[", "]]></MediaId>", 0, false);
                _msg.Format = m_requestBody.SearchStringTag("<Format><![CDATA[", "]]></Format>", 0, false);
                _msg.Recognition = m_requestBody.SearchStringTag("<Recognition><![CDATA[", "]]></Recognition>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 视频消息
                ReqVideoMsg _msg = new ReqVideoMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);
                _msg.MediaId = m_requestBody.SearchStringTag("<MediaId><![CDATA[", "]]></MediaId>", 0, false);
                _msg.ThumbMediaId = m_requestBody.SearchStringTag("<ThumbMediaId><![CDATA[", "]]></ThumbMediaId>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[location]]></MsgType>"))
            {
                #region 地理位置消息
                ReqLocationMsg _msg = new ReqLocationMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);
                _msg.Location_X = m_requestBody.SearchStringTag("<Location_X>", "</Location_X>", 0, false);
                _msg.Location_Y = m_requestBody.SearchStringTag("<Location_Y>", "</Location_Y>", 0, false);
                _msg.Scale = m_requestBody.SearchStringTag("<Scale>", "</Scale>", 0, false);
                _msg.Label = m_requestBody.SearchStringTag("<Label><![CDATA[", "]]></Label>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[link]]></MsgType>"))
            {
                #region 链接消息
                ReqLinkMsg _msg = new ReqLinkMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);
                _msg.Title = m_requestBody.SearchStringTag("<Title><![CDATA[", "]]></Title>", 0, false);
                _msg.Description = m_requestBody.SearchStringTag("<Description><![CDATA[", "]]></Description>", 0, false);
                _msg.Url = m_requestBody.SearchStringTag("<Url><![CDATA[", "]]></Url>", 0, false);
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[event]]></MsgType>"))
            {
                #region 事件消息处理
                ReqEventMsg _msg = new ReqEventMsg(PlatformType.Weixin);
                _msg.GetBaseInfo(m_requestBody);

                string _event = m_requestBody.SearchStringTag("<Event><![CDATA[", "]]></Event>", 0, false);
                switch (_event.ToLower())
                {
                    case "subscribe":
                        {
                            _msg.EventType = ReqEventType.Subscribe;
                            if (m_requestBody.Contains("<EventKey><![CDATA["))
                            {
                                _msg.EventKey = m_requestBody.SearchStringTag("<EventKey><![CDATA[", "]]></EventKey>", 0, false);
                                if (!string.IsNullOrEmpty(_msg.EventKey))
                                {
                                    _msg.EventType = ReqEventType.ScanSubscribe;
                                }
                                _msg.Ticket = m_requestBody.SearchStringTag("<Ticket><![CDATA[", "]]></Ticket>", 0, false);
                            }
                        }
                        break;
                    case "scan":
                        {
                            _msg.EventType = ReqEventType.Scan;
                            _msg.EventKey = m_requestBody.SearchStringTag("<EventKey><![CDATA[", "]]></EventKey>", 0, false);
                            _msg.Ticket = m_requestBody.SearchStringTag("<Ticket><![CDATA[", "]]></Ticket>", 0, false);
                        }
                        break;
                    case "location":
                        {
                            _msg.EventType = ReqEventType.Location;
                            _msg.Latitude = m_requestBody.SearchStringTag("<Latitude>", "</Latitude>", 0, false);
                            _msg.Longitude = m_requestBody.SearchStringTag("<Longitude>", "</Longitude>", 0, false);
                            _msg.Precision = m_requestBody.SearchStringTag("<Precision>", "</Precision>", 0, false);
                        }
                        break;
                    case "click":
                        {
                            _msg.EventType = ReqEventType.Click;
                            _msg.EventKey = m_requestBody.SearchStringTag("<EventKey><![CDATA[", "]]></EventKey>", 0, false);
                        }
                        break;
                    case "view":
                        {
                            _msg.EventType = ReqEventType.View;
                            _msg.EventKey = m_requestBody.SearchStringTag("<EventKey><![CDATA[", "]]></EventKey>", 0, false);
                        }
                        break;
                    case "unsubscribe":
                        _msg.EventType = ReqEventType.UnSubscribe;
                        break;
                    case "masssendjobfinish":
                        {
                            _msg.EventType = ReqEventType.MassSend;
                            ((ReqMassSendEventMsg)_msg).Status = m_requestBody.SearchStringTag("<Status><![CDATA[", "]]></Status>", 0, false);
                            ((ReqMassSendEventMsg)_msg).TotalCount = m_requestBody.SearchStringTag("<TotalCount>", "</TotalCount>", 0, false);
                            ((ReqMassSendEventMsg)_msg).FilterCount = m_requestBody.SearchStringTag("<FilterCount>", "</FilterCount>", 0, false);
                            ((ReqMassSendEventMsg)_msg).SentCount = m_requestBody.SearchStringTag("<SentCount>", "</SentCount>", 0, false);
                            ((ReqMassSendEventMsg)_msg).ErrorCount = m_requestBody.SearchStringTag("<ErrorCount>", "</ErrorCount>", 0, false);
                        }
                        break;
                    case "templatesendjobfinish":
                        {
                            _msg.EventType = ReqEventType.TempSend;
                            _msg.Status = m_requestBody.SearchStringTag("<Status><![CDATA[", "]]></Status>", 0, false);
                        }
                        break;
                    default:
                        break;
                }
                return _msg;

                #endregion
            }
            return null;
        }

        #endregion

        protected ReplyConfig m_replyConfig;
        protected UserInfo m_userInfo;

        /// <summary>
        /// 处理请求消息
        /// </summary>
        public override void ProcessRequestMessage()
        {
            //获取回复模板信息
            m_replyConfig = ReplyConfig.GetReplyConfigByIdFromCache(m_account.ReplyTemplateId);
            //获取当前用户信息
            m_userInfo = UserFactory.GetUserInfo(m_account, m_requestMessage.FromUserName);

            base.ProcessRequestMessage();
        }

        /// <summary>
        /// 处理回复消息
        /// </summary>
        public override void SynReplyMessage()
        {
            base.SynReplyMessage();
        }

        /// <summary>
        /// 异步处理回复消息
        /// </summary>
        public override void AsynReplyMessage()
        {
            base.AsynReplyMessage();

            //记录请求消息
            if (m_requestMessage != null)
            {
                m_userInfo.NoteUserReqInfo(m_requestMessage);
            }
            if (m_responseMessage != null)
            {
                //记录回复消息
                foreach (ResMsg item in m_responseMessage)
                {
                    m_userInfo.NoteUserResMsg(item);
                }
            }
        }

        #region 默认回复消息

        /// <summary>
        /// 默认回复消息
        /// </summary>
        /// <returns></returns>
        protected override ResMsg[] GetDefaultResponseMessage(ReqMsg message)
        {
            if (m_replyConfig != null)
            {
                //回复默认
                ResMsg _msg = m_replyConfig.DefaultResMsg.Clone() as ResMsg;
                _msg.ToUserName = m_requestMessage.FromUserName;
                _msg.FromUserName = m_requestMessage.ToUserName;

                _msg.ResType = 1;
                return new ResMsg[] { _msg };
            }
            return null;
        }

        #endregion

        #region 文本消息处理

        /// <summary>
        /// 文本消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerTextMessage(ReqTextMsg message)
        {
            ResMsg[] _resMsgList = null;

            //处理系统命令
            _resMsgList = HandlerSystemCmd(message);
            if (_resMsgList != null && _resMsgList.Length > 0)
            {
                _resMsgList = _resMsgList.Select(a =>
                {
                    a.ResType = 1;
                    return a;
                }).ToArray();
                return _resMsgList;
            }

            #region 关键词回复处理
            ResMsg[] _list = m_replyConfig.GetResMsg(((ReqTextMsg)m_requestMessage).Content);
            if (_list.Length != 0)
            {
                //回复关键词
                ResMsg[] _returnList = new ResMsg[_list.Length];
                for (int i = 0; i < _list.Length; i++)
                {
                    ResMsg _msg = _list[i].Clone() as ResMsg;
                    _msg.ToUserName = m_requestMessage.FromUserName;
                    _msg.FromUserName = m_requestMessage.ToUserName;

                    _msg.ResType = 1;

                    _returnList[i] = _msg;
                }
                return _returnList;
            }
            #endregion
            return GetDefaultResponseMessage(m_requestMessage);
        }

        protected static Hashtable m_userTripModeInfo = new Hashtable();
        /// <summary>
        /// 处理自定义命令
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual ResMsg[] HandlerSystemCmd(ReqTextMsg message)
        {
            string _key = m_account.AccountId + "_" + m_requestMessage.FromUserName;

            ResTextMsg _msg = null;

            string cmd = message.Content.ToUpper();

            //if (cmd == "TLBX")
            //{
            //    _msg = new ResTextMsg(m_requestMessage);
            //    m_userTripModeInfo[_key] = BaiduMapApiMode.walking;
            //    _msg.Content = @"您好，您选择的出行方式为“步行”，请发送您的位置";
            //}
            //if (cmd == "TLGJ")
            //{
            //    _msg = new ResTextMsg(m_requestMessage);
            //    m_userTripModeInfo[_key] = BaiduMapApiMode.transit;
            //    _msg.Content = @"您好，您选择的出行方式为“公交”，请发送您的位置";
            //}
            //if (cmd == "TLJC")
            //{
            //    _msg = new ResTextMsg(m_requestMessage);
            //    m_userTripModeInfo[_key] = BaiduMapApiMode.driving;
            //    _msg.Content = @"您好，您选择的出行方式为“驾车”，请发送您的位置";
            //}

            //最后一次位置消息
            ReqLocationMsg _lastLocationMsg = m_lastLocationMessage[_key] as ReqLocationMsg;
            if (_lastLocationMsg != null)
            {
                return HandlerLocationMessage(_lastLocationMsg);
            }

            //选择模式
            if (_msg != null)
            {
                _msg.ResType = 1;
                return new ResMsg[] { _msg };
            }
            return null;
        }

        #endregion

        #region 关注事件处理

        /// <summary>
        /// 处理关注事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerSubscribeEvent(ReqEventMsg message)
        {
            m_userInfo.EnSubscribe(message);
            if (m_replyConfig != null)
            {
                //回复默认
                ResMsg _msg = m_replyConfig.SubResMsg.Clone() as Platform.ResMsg;
                _msg.ToUserName = m_requestMessage.FromUserName;
                _msg.FromUserName = m_requestMessage.ToUserName;

                _msg.ResType = 2;
                return new ResMsg[] { _msg };
            }

            return GetDefaultResponseMessage(m_requestMessage);
        }

        /// <summary>
        /// 处理取消关注事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerUnSubscribeEvent(ReqEventMsg message)
        {
            m_userInfo.UnSubscribe();
            return base.HandlerUnSubscribeEvent(message);
        }

        #endregion

        #region 扫描关注事件处理

        /// <summary>
        /// 处理扫描关注事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerScanSubscribeEvent(ReqEventMsg message)
        {
            this.LogInfo("处理扫描关注事件：" + message.EventKey);
            long _id = message.EventKey.ToInt64();
            message.EventKey = message.EventKey.Replace("qrscene_", "");

            m_userInfo.EnSubscribe(message);
            m_userInfo.UpdateScanTotalCount((int)_id);//更新该二维码扫描的总次数
            m_userInfo.UpdateLastScanTime();//更新最近的扫描时间

            ResMsg[] _resmsg = HandlerScan(message);
            if (_resmsg != null)
            {
                _resmsg = _resmsg.Select(a =>
                {
                    a.ResType = 2;
                    return a;
                }).ToArray();
                return _resmsg;
            }

            if (m_replyConfig != null)
            {
                //回复默认
                Platform.ResMsg _msg = m_replyConfig.SubResMsg.Clone() as Platform.ResMsg;
                _msg.ToUserName = m_requestMessage.FromUserName;
                _msg.FromUserName = m_requestMessage.ToUserName;
                _msg.ResType = 2;
                return new ResMsg[] { _msg };
            }
            return GetDefaultResponseMessage(m_requestMessage);
        }
        #endregion

        #region 扫描带参数事件
        protected static Hashtable m_lastScanInfo = new Hashtable();
        /// <summary>
        /// 已关注扫描带参数二维码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerScanEvent(ReqEventMsg message)
        {
            long _id = message.EventKey.ToInt64();

            m_userInfo.UpdateScanTotalCount((int)_id);//更新该二维码扫描的总次数
            m_userInfo.UpdateLastScanTime();//更新最近的扫描时间
            #region 重复扫描处理,间隔5秒

            if (m_lastScanInfo[message.FromUserName] != null)
            {
                KeyValuePair<long, DateTime> _scanInfo = (KeyValuePair<long, DateTime>)m_lastScanInfo[message.FromUserName];
                if (_scanInfo.Key == _id && _scanInfo.Value.AddSeconds(5) > DateTime.Now)
                {
                    //ResTextMsg _nullMsg;
                    //_nullMsg = new ResTextMsg(message);
                    //_nullMsg.Content = "";
                    //return new ResMsg[] { _nullMsg };
                    return null;
                }
            }
            m_lastScanInfo[message.FromUserName] = new KeyValuePair<long, DateTime>(_id, DateTime.Now);

            #endregion

            ResMsg[] _resmsg = HandlerScan(message);
            if (_resmsg != null)
            {
                _resmsg = _resmsg.Where(a => a != null).Select(a =>
                {
                    a.ResType = 2;
                    return a;
                }).ToArray();
                return _resmsg;
            }

            return base.HandlerScanEvent(message);
        }

        /// <summary>
        /// 封装扫描带参数二维码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerScan(ReqEventMsg message)
        {
            this.LogInfo("处理扫描带参数事件：【" + message.FromUserName + "】" + message.EventKey);
            long _id = message.EventKey.ToInt64();

            #region 处理活动二维码
            if (_id > 99 && _id < 999)
            {
                this.LogInfo("处理活动二维码：【" + message.FromUserName + "】" + message.EventKey);

            }
            #endregion

            //处理微餐饮的桌位二维码
            if (_id >= 20000 && _id < 49999)
            {
                this.LogInfo("处理微餐饮二维码：【" + message.FromUserName + "】" + message.EventKey);
            }

            #region 处理登录临时二维码
            if (_id >= 80000)
            {
                //string key = m_account.AccountId + "_" + _id;
                //if (QrFactory.TempQrList.ContainsKey(key))
                //{
                //    string json = QrFactory.TempQrList[key];
                //    var data = JsonMapper.ToObject(json);
                //    data["status"] = 1;
                //    QrFactory.TempQrList[key] = data.ToJson(false);
                //}
                //else
                //{
                //    JsonData data = new JsonData();
                //    data["qrid"] = _id;
                //    data["ticket"] = message.Ticket;
                //    data["createtime"] = DateTime.Now.ToString();
                //    data["status"] = 1;
                //    string value = data.ToJson(false);
                //    QrFactory.TempQrList[key] = value;
                //}
            }
            #endregion

            #region 处理wifi二维码
            if (_id == 99)
            {
                ////var device = AccountDevice.GetDeviceInfoByAccountId(m_account.AccountId);
                ////if (device != null)
                ////{
                //var resMsg = new ResTextMsg(message);
                //resMsg.Content = "欢迎使用商机讲堂无线，请开启WLAN，并连接商机讲堂WIFI热点，登录密码：1234567890。";
                //resMsg.ResType = 2;
                //return new ResMsg[] { resMsg };
                ////}
            }
            #endregion

            #region 获取wifi密码
            if (_id == 98)
            {
                ////var device = AccountDevice.GetDeviceInfoByAccountId(m_account.AccountId);
                ////if (device != null)
                ////{
                //var resMsg = new ResNewsMsg(message);
                //var article = new ResArticle()
                //{
                //    Title = "点击消息，即可登录天下商机免费WI-FI",
                //    Discription = "简介：点击消息，可免费使用天下商机上网服务，请自觉遵守国家相关法律法规。禁止发布危害国家统一、民族团结等一切违法言论。禁止浏览黄色、赌博等违法网站。为了保证基本浏览速度，禁止使用P2P下载，请不要在此下载大量数据。",
                //    PicUrl = "http://img.txooo.com/2014/06/26/818d0f8d68dda4b30739c6016f2d1e32.jpg",
                //    Url = "http://wifi.txooo.com/nowifi.html?mode=scan_98_" + this.Account.BrandId//"http://mobile.txooo.com:689/template/phone/index.html?mode=scan_98&brandid=" + this.Account.BrandId
                //};
                //resMsg.Articles.Add(article);
                //resMsg.ResType = 2;
                //return new ResMsg[] { resMsg };

                ////}
            }
            #endregion

            //处理精准邀请的临时二维码
            if (_id >= 1200 && _id <= 19999)
            {
                //int receiptId;
                //InviteInfo info = InviteReceipt.GetReceiptInfoByQrNumber(_id, m_account.AccountId, out receiptId);

                //ResTextMsg msg = new ResTextMsg(message);
                //string sn =
                //    Txooo.Text.EncryptHelper.AESEncrypt(info.InviteId + "|" + receiptId + "|1");
                //msg.Content = "点击链接，填写回执信息：" + "http://" + info.BrandId + ".app.txooo.com/Invite/inviteIndex.html?sn=" + sn + "&openId=" + message.FromUserName;
                ////ResNewsMsg msg = new ResNewsMsg(message);
                ////msg.Articles.Add(new ResArticle()
                ////{
                ////    PicUrl = info.InviteCover,
                ////    Title = info.InviteTitle,
                ////    Discription = info.InviteDescription,
                ////    Url = "http://"+info.BrandId+".app.txooo.com/Invite/Receipt.html?inviteId="+info.InviteId+"&receiptId="+receiptId+"&type=1"
                ////});

                //return new ResMsg[] { msg };
            }
            //处理邀请的永久二维码
            if (_id > 1000 && _id <= 1099)
            {
                //int inviteId = InviteInfo.GetInviteIdByQrNumber(_id, m_account.AccountId);
                //InviteInfo info = InviteInfo.GetInviteInfoByInviteId(inviteId);

                //ResNewsMsg msg = new ResNewsMsg(message);
                //msg.Articles.Add(new ResArticle()
                //{
                //    PicUrl = info.InviteCover,
                //    Title = info.InviteTitle,
                //    Discription = info.InviteDescription,
                //    Url = "http://" + info.BrandId + ".app.txooo.com/Invite/inviteIndex.html?inviteId=" + info.InviteId + "&type=1&openId=" + message.FromUserName
                //});

                //return new ResMsg[] { msg };
            }

            //处理签到的微信二维码
            if (_id >= 1100 && _id < 1199)
            {
                //SignQrCode signQrCodeInfo = SignQrCode.GetSignQrCodeInfoByQrNumber((int)_id, m_account.AccountId);
                //SignInfo signInfo = SignInfo.GetSignInfoBySignId(signQrCodeInfo.SignId);
                //string sn = EncryptHelper.AESEncrypt(signInfo.SignId + "|1");
                //ResNewsMsg msg = new ResNewsMsg(message);
                //msg.Articles.Add(new ResArticle()
                //{
                //    PicUrl = signInfo.Cover,
                //    Title = signInfo.SignName,
                //    Discription = signInfo.Remark,
                //    Url = "http://" + signInfo.BrandId + ".app.txooo.com/Invite/signIndex.html?sn=" + sn + "&openId=" + message.FromUserName
                //});
                //return new ResMsg[] { msg };
            }

            //处理互动墙的二维码
            if (_id >= 51000 && _id <= 51999)
            {
                //this.TxLogInfo(string.Format("记录互动墙用户：{0},{1}", message.FromUserName, _id));

                //Hashtable _hashtable = new Hashtable();
                //_hashtable["@QrCode"] = _id;
                //_hashtable["@AccountId"] = m_account.AccountId;

                //WallIndex _wallInfo =
                //    WallIndex.GetWallIndexByWhere(" and account_id=@AccountId and qr_code=@QrCode ", _hashtable).FirstOrDefault();

                //if (_wallInfo != null)
                //{
                //    if (DateTime.Now >= _wallInfo.StartTime && DateTime.Now <= _wallInfo.EndTime)
                //    {
                //        if (!WallUser.IsExistWallUser(message.FromUserName, (int)_id))
                //        {
                //            //入库
                //            WallUser.AddWallUser(message.FromUserName, (int)_id, m_userInfo);
                //        }
                //    }
                //}
            }

            return null;
        }

        #endregion

        #region 菜单点击事件
        /// <summary>
        /// 处理菜单点击事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerClickEvent(ReqEventMsg message)
        {
            ResMsg _msg = null;

            this.LogInfo("处理菜单点击事件：" + message.EventKey);

            var eventParams = message.EventKey.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            switch (eventParams[0])
            {
                case "text":
                    {
                        ResTextMsg _txtMsg = new ResTextMsg(message);
                        _txtMsg.Content = eventParams[1];
                        _msg = _txtMsg;
                        _msg.ResType = 2;
                    }
                    break;
                case "image":
                    {
                        ResImageMsg _imgMsg = new ResImageMsg(message.FromUserName);
                        _imgMsg.LocalUrl = eventParams[1];
                        _msg = _imgMsg;
                        _msg.ResType = 2;
                    }
                    break;
                case "article":
                    {
                        //从回复信息中获取多图文内容
                        KeywordReply reply = new KeywordReply();
                        reply.ContentType = 4;
                        reply.ReplyContent = eventParams[1];
                        var _newsMsg = reply.GetArticles();
                        _newsMsg.ToUserName = message.FromUserName;
                        _newsMsg.FromUserName = message.ToUserName;
                        _msg = _newsMsg;
                        _msg.ResType = 2;
                    }
                    break;
                default:
                    break;
            }
            return new ResMsg[] { _msg };
        }

        #endregion

        #region 群发事件处理
        /// <summary>
        /// 群发事件处理
        /// </summary>
        /// <param name="reqMassSendEventMsg"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerMassSendEvent(ReqEventMsg reqMassSendEventMsg)
        {
            var _msg = (ReqMassSendEventMsg)reqMassSendEventMsg;


            return base.HandlerMassSendEvent(reqMassSendEventMsg);
        }

        /// <summary>
        /// 模板消息发送事件处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerTempSendEvent(ReqEventMsg message)
        {
            return null;
        }
        #endregion

        #region 自动获取地理位置事件

        protected override ResMsg[] HandlerLocationEvent(ReqEventMsg message)
        {
            UserInfo.UpdateUserLocation(message);
            return base.HandlerLocationEvent(message);
        }

        #endregion

        #region 位置消息处理

        protected static Hashtable m_lastLocationMessage = new Hashtable();

        protected override ResMsg[] HandlerLocationMessage(ReqLocationMsg message)
        {
            return base.HandlerLocationMessage(message);
            ////腾讯地图定位接口http://apis.map.qq.com/uri/v1/marker?marker=coord:39.892326,116.342763;title:超好吃冰激凌;addr:手帕口桥北铁路道口;tel:010-63521235&coord_type=1
            ////腾讯地图导航接口http://apis.map.qq.com/uri/v1/routeplan?type=bus&from=我的家&fromcoord=39.980683,116.302&to=中关村&tocoord=39.9836,116.3164&policy=1&referer=tengxun

            //string _key = m_account.AccountId + "_" + m_requestMessage.FromUserName;
            //m_lastLocationMessage[_key] = message;

            //if (m_userTripModeInfo[_key] == null)
            //{
            //    ResTextMsg _msg = new ResTextMsg(m_requestMessage);
            //    _msg.Content = @"您好，获取路径导航，请先选择出行方式：
            //                        步行请回复：TLBX
            //                        公交请回复：TLGJ
            //                        驾车请回复：TLJC
            //                        ";
            //    _msg.ResType = 2;
            //    return new ResMsg[] { _msg };
            //}
            //else
            //{
            //    var _listMap = m_account.GetCoordinates();
            //    if (_listMap != null)
            //    {
            //        //设置用户当前起点
            //        Api.MapLocation _origin = new Api.MapLocation();
            //        _origin.Location_X = message.Location_X;
            //        _origin.Location_Y = message.Location_Y;
            //        _origin.Label = string.IsNullOrEmpty(message.Label) ? "我的位置" : message.Label;
            //        //_origin.Region = "北京";

            //        var _resMsg = GetDirectionPath((BaiduMapApiMode)m_userTripModeInfo[_key], _origin, _listMap[0]);
            //        _resMsg = _resMsg.Select(a =>
            //        {
            //            a.ResType = 2;
            //            return a;
            //        }).ToArray();
            //        return _resMsg;
            //    }
            //}
            //return null;
        }

        #endregion

        #region 获取路径信息

        ///// <summary>
        ///// 获取路径信息
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="origin"></param>
        ///// <param name="destination"></param>
        ///// <returns></returns>
        //public virtual ResMsg[] GetDirectionPath(BaiduMapApiMode mode, MapLocation origin, MapLocation destination)
        //{
        //    string _directionInfo;
        //    List<string> _directionSteps;

        //    if (Api.BaiduMapApiHelper.GetDirectionPath(mode, origin, destination, out _directionInfo, out _directionSteps))
        //    {
        //        //if (_directionInfo.Length > 600)
        //        {
        //            List<ResMsg> _msgList = new List<ResMsg>();
        //            string _info = "";
        //            int _i = 0;
        //            if (mode == BaiduMapApiMode.transit)
        //            {
        //                #region 公交方案

        //                foreach (string item in _directionSteps)
        //                {
        //                    if (_info.Length > 500)
        //                    {
        //                        ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //                        _msg.Content = _info;
        //                        _msgList.Add(_msg);
        //                        //
        //                        _info = "";
        //                    }
        //                    else
        //                    {
        //                        string _filter = Regex.Replace(item, @"</[^>]*>", ">");
        //                        _filter = Regex.Replace(_filter, @"<[^>]*>", "<").Replace("<<", "[").Replace(">>", "]");

        //                        //item.Replace("<b>", "<").Replace("</b>", ">").Replace("<font color=\"0x000000\">", "<").Replace("</font>", ">").Replace("<<", "[").Replace(">>", "]") + "\r\n";

        //                        if (item.Contains("方案"))
        //                        {
        //                            _i = 0;
        //                            _info += "\r\n" + _filter + "\r\n";
        //                        }
        //                        else
        //                        {
        //                            _info += _i++ + "：" + _filter + "\r\n";
        //                        }
        //                    }
        //                }
        //                {
        //                    ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //                    _msg.Content = _info;
        //                    _msgList.Add(_msg);
        //                }

        //                #endregion
        //            }
        //            else
        //            {
        //                #region 驾车方案

        //                foreach (string item in _directionSteps)
        //                {
        //                    if (_info.Length > 450)
        //                    {
        //                        ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //                        _msg.Content = _info;
        //                        _msgList.Add(_msg);
        //                        //
        //                        _info = "";
        //                    }
        //                    else
        //                    {
        //                        _info += _i++ + "：" + item.Replace("<b>", "<").Replace("</b>", ">").Replace("<font color=\"0x000000\">", "<").Replace("</font>", ">") + "\r\n\r\n";
        //                    }
        //                }
        //                {
        //                    ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //                    _msg.Content = _info;
        //                    _msgList.Add(_msg);
        //                }

        //                #endregion
        //            }
        //            _msgList = _msgList.Select(a =>
        //            {
        //                a.ResType = 2;
        //                return a;
        //            }).ToList();
        //            return _msgList.ToArray();
        //        }
        //        //else
        //        //{
        //        //    ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //        //    _msg.Content = _directionInfo;
        //        //    return new ResMsg[] { _msg };
        //        //}
        //    }
        //    else
        //    {
        //        ResTextMsg _msg = new ResTextMsg(m_requestMessage);
        //        _msg.Content = "获取路径错误";
        //        _msg.ResType = 2;
        //        return new ResMsg[] { _msg };
        //    }
        //}
        #endregion
    }
}
