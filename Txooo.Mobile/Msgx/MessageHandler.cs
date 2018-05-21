using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;

using Txooo.Mobile.Platform;
using System.Web.SessionState;

namespace Txooo.Mobile.Msgx
{
    /// <summary>
    /// 消息处理类
    /// </summary>
    public abstract class MessageHandler : System.Web.IHttpHandler, IRequiresSessionState
    {
        #region Http对象

        /// <summary>
        /// 当前请求HttpContext
        /// </summary>
        protected HttpContext Context;
        /// <summary>
        /// 当前请HttpRequest对象
        /// </summary>
        protected HttpRequest Request;
        /// <summary>
        /// 当前请HttpResponse对象
        /// </summary>
        protected HttpResponse Response;

        #endregion

        /// <summary>
        /// 账号信息
        /// </summary>
        protected AccountInfo m_account;
        /// <summary>
        /// 账号信息
        /// </summary>
        public AccountInfo Account
        {
            get
            {
                return m_account;
            }
            set
            {
                m_account = value;
            }
        }
        /// <summary>
        /// 接受到的消息主体
        /// </summary>
        protected string m_requestBody;
        /// <summary>
        /// 接受到的消息
        /// </summary>
        protected ReqMsg m_requestMessage;
        /// <summary>
        /// 返回消息
        /// </summary>
        protected ResMsg[] m_responseMessage;

        #region 0、请求处理

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public virtual void ProcessRequest(System.Web.HttpContext context)
        {
            Context = context;
            Request = context.Request;
            Response = context.Response;

            //验证请求签名
            if (CheckSignature())
            {
                //接口验证处理
                VerificationHandler();

                #region 1、接受消息

                //获取接受到的消息主题
                using (StreamReader responseReader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    m_requestBody = responseReader.ReadToEnd();
                }
                this.TxLogInfo("接受消息：" + m_requestBody);

                #endregion

                #region 2、处理消息

                //处理消息
                m_requestMessage = GetRequestMessage();
                if (m_requestMessage != null)
                {
                    if (!m_account.IsAsyn)
                    {
                        //处理请求消息
                        ProcessRequestMessage();
                        //同步回复处理
                        SynReplyMessage();

                        //转发客服
                        //TransferCS();
                    }
                    else
                    {
                        //异步处理
                        System.Threading.Thread _t = new System.Threading.Thread(new System.Threading.ThreadStart(AsynReplyMessage));
                        _t.Start();

                        //TransferCS();

                        Response.Write("");
                    }
                }
                #endregion
            }
        }

        private void TransferCS()
        {

            string msgType = "transfer_customer_service";
            string _str = "";

            _str += "<xml>";
            _str += "<ToUserName><![CDATA[" + m_requestMessage.FromUserName + "]]></ToUserName>";
            _str += "<FromUserName><![CDATA[" + m_requestMessage.ToUserName + "]]></FromUserName>";
            _str += "<CreateTime>" + m_requestMessage.CreateTime + "</CreateTime>";
            _str += "<MsgType><![CDATA[" + msgType + "]]></MsgType>";
            _str += "</xml>";

            Response.Write(_str);
        }

        #endregion

        #region 1、验证签名

        /// <summary>
        /// 验证请求签名
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckSignature()
        {
            if (Account != null)
            {
                string _signature = Request.QueryString["signature"];
                string _timestamp = Request.QueryString["timestamp"];
                string _nonce = Request.QueryString["nonce"];
                string _token = Account.TxApiToken;

                List<string> _list = new List<string>();
                _list.Add(_token);
                _list.Add(_timestamp);
                _list.Add(_nonce);
                _list.Sort();
                string _sortInfo = "";
                foreach (var item in _list)
                {
                    _sortInfo += item;
                }
                string _sha1 = Txooo.Text.EncryptHelper.SHA1(_sortInfo);
                if (_sha1 == _signature)
                {
                    //this.TxLogInfo("验证签名成功:" + Request.Url.ToString());
                    return true;
                }
                else
                {
                    m_account.ChangeAccountStart(0, "验证签名失败：" + Request.Url.ToString());
                }
            }
            this.TxLogInfo("验证签名失败:" + Request.Url.ToString());
            return false;
        }

        #endregion

        #region 2、接口验证处理

        /// <summary>
        /// 接口验证处理
        /// </summary>
        public virtual void VerificationHandler()
        {
            string _echostr = Request.QueryString["echostr"];
            if (!string.IsNullOrEmpty(_echostr))
            {
                //成功开通
                if (m_account.AccountType == 1)
                {
                    m_account.ChangeAccountStart(1, "");
                }
                this.TxLogInfo("接口绑定成功:" + Account.AccountId);
                Response.Write(_echostr);
                Response.End();
            }
        }

        #endregion

        /// <summary>
        /// 3、提取请求消息
        /// </summary>
        /// <returns></returns>
        public abstract ReqMsg GetRequestMessage();

        #region 4、处理请求消息

        /// <summary>
        /// 处理请求消息
        /// </summary>
        public virtual void ProcessRequestMessage()
        {
            switch (m_requestMessage.MsgType)
            {
                case ReqMsgType.Event:
                    {
                        switch (((ReqEventMsg)m_requestMessage).EventType)
                        {
                            case ReqEventType.Subscribe:
                                m_responseMessage = HandlerSubscribeEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.UnSubscribe:
                                m_responseMessage = HandlerUnSubscribeEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.Scan:
                                m_responseMessage = HandlerScanEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.ScanSubscribe:
                                m_responseMessage = HandlerScanSubscribeEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.Location:
                                m_responseMessage = HandlerLocationEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.Click:
                                m_responseMessage = HandlerClickEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.View:
                                //m_responseMessage = HandlerViewEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.MassSend:
                                m_responseMessage = HandlerMassSendEvent((ReqEventMsg)m_requestMessage);
                                break;
                            case ReqEventType.TempSend:
                                m_responseMessage = HandlerTempSendEvent((ReqEventMsg)m_requestMessage);
                                break;
                        }
                    }
                    break;
                case ReqMsgType.Text:
                    m_responseMessage = HandlerTextMessage((ReqTextMag)m_requestMessage);
                    break;
                case ReqMsgType.Image:
                    m_responseMessage = HandlerImageMessage((ReqImageMsg)m_requestMessage);
                    break;
                case ReqMsgType.Voice:
                    m_responseMessage = HandlerVoiceMessage((ReqVoiceMsg)m_requestMessage);
                    break;
                case ReqMsgType.Video:
                    m_responseMessage = HandlerVideoMessage((ReqVideoMsg)m_requestMessage);
                    break;
                case ReqMsgType.Location:
                    m_responseMessage = HandlerLocationMessage((ReqLocationMsg)m_requestMessage);
                    break;
                case ReqMsgType.Link:
                    m_responseMessage = HandlerLinkMessage((ReqLinkMsg)m_requestMessage);
                    break;
            }
            //if (m_responseMessage == null && eventType != ReqEventType.Location)
            //{
            //    m_responseMessage = GetDefaultResponseMessage(m_requestMessage);
            //}
        }

        #endregion

        #region 5、消息回复

        /// <summary>
        /// 同步处理回复消息
        /// </summary>
        public virtual void SynReplyMessage()
        {
            this.TxLogInfo("同步处理回复消息");
            if (m_responseMessage != null && m_responseMessage.Length > 0)
            {
                //同步回复只回复第一条消息
                ResMsg _responseMessage = m_responseMessage[0];
                switch (_responseMessage.MsgType)
                {
                    case ResMsgType.Text:
                        ReplyTextMessage((ResTextMsg)_responseMessage);
                        break;
                    case ResMsgType.Image:
                        ReplyImageMessage((ResImageMsg)_responseMessage);
                        break;
                    case ResMsgType.Voice:
                        ReplyVoiceMessage((ResVoiceMsg)_responseMessage);
                        break;
                    case ResMsgType.Video:
                        ReplyVideoMessage((ResVideoMsg)_responseMessage);
                        break;
                    case ResMsgType.Music:
                        ReplyMusicMessage((ResMusicMsg)_responseMessage);
                        break;
                    case ResMsgType.News:
                        ReplyNewsMessage((ResNewsMsg)_responseMessage);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 异步消息处理
        /// </summary>
        public virtual void AsynReplyMessage()
        {
            ProcessRequestMessage();

            if (m_responseMessage != null)
            {
                this.TxLogInfo("异步消息处理");
                string _errorInfo;
                foreach (ResMsg item in m_responseMessage)
                {
                    int _number = 0;
                    bool _isOk = false;
                    do
                    {
                        _number++;
                        _isOk = m_account.ApiHelper.SendMessage(item, out _errorInfo);
                        if (!_isOk)
                        {
                            this.TxLogInfo("异步发送文本消息错误:" + _errorInfo);
                        }
                    } while (!_isOk && _number < 10);
                }
            }
        }

        #endregion

        /// <summary>
        /// 默认回复消息
        /// </summary>
        /// <returns></returns>
        protected abstract ResMsg[] GetDefaultResponseMessage(ReqMsg message);

        #region A、各种消息处理方法

        /// <summary>
        /// 处理文本消息
        /// </summary>
        protected virtual ResMsg[] HandlerTextMessage(ReqTextMag message)
        {
            return null;
        }
        /// <summary>
        /// 处理图片消息
        /// </summary>
        protected virtual ResMsg[] HandlerImageMessage(ReqImageMsg message)
        {
            return null;
        }
        /// <summary>
        /// 处理语言消息
        /// </summary>
        protected virtual ResMsg[] HandlerVoiceMessage(ReqVoiceMsg message)
        {
            return null;
        }
        /// <summary>
        /// 处理视频消息
        /// </summary>
        protected virtual ResMsg[] HandlerVideoMessage(ReqVideoMsg message)
        {
            return null;
        }
        /// <summary>
        /// 处理地理位置消息
        /// </summary>
        protected virtual ResMsg[] HandlerLocationMessage(ReqLocationMsg message)
        {
            return null;
        }
        /// <summary>
        /// 处理链接消息
        /// </summary>
        protected virtual ResMsg[] HandlerLinkMessage(ReqLinkMsg message)
        {
            return null;
        }

        #endregion

        #region B、各种事件消息处理方法

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerSubscribeEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerUnSubscribeEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 扫描事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerScanEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 扫描订阅事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerScanSubscribeEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerLocationEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 点击自定义菜单事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerClickEvent(ReqEventMsg message)
        {
            return null;
        }
        /// <summary>
        /// 群发事件
        /// </summary>
        /// <param name="reqMassSendEventMsg"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerMassSendEvent(ReqEventMsg reqMassSendEventMsg)
        {
            return null;
        }

        /// <summary>
        /// 模板消息发送时间处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual ResMsg[] HandlerTempSendEvent(ReqEventMsg message)
        {
            return null;
        }

        #endregion

        #region C、各种回复消息处理方法

        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyTextMessage(ResTextMsg message)
        {
            Response.Write(message.GetWeixinResXml());
        }


        /// <summary>
        /// 回复图片消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyImageMessage(ResImageMsg message)
        {

        }
        /// <summary>
        /// 回复语音消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyVoiceMessage(ResVoiceMsg message)
        {

        }
        /// <summary>
        /// 回复视频消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyVideoMessage(ResVideoMsg message)
        {

        }

        /// <summary>
        /// 回复音乐消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyMusicMessage(ResMusicMsg message)
        {

        }

        /// <summary>
        /// 回复图文消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void ReplyNewsMessage(ResNewsMsg message)
        {
            Response.Write(message.GetWeixinResXml());
        }
        #endregion



    }
}
