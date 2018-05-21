using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Msgx
{
    public class WeixinHandler : DefaultHandler
    {
        #region 获取请求消息

        public override Platform.ReqMsg GetRequestMessage()
        {
            if (m_requestBody.Contains("<MsgType><![CDATA[text]]></MsgType>"))
            {
                #region 文本消息

                ReqTextMag _msg = new ReqTextMag(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");
                _msg.Content = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Content><![CDATA[", "]]></Content>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[image]]></MsgType>"))
            {
                #region 图片消息

                ReqImageMsg _msg = new ReqImageMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");
                _msg.PicUrl = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<PicUrl><![CDATA[", "]]></PicUrl>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 语音消息

                ReqVoiceMsg _msg = new ReqVoiceMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");

                _msg.MediaId = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MediaId><![CDATA[", "]]></MediaId>");
                _msg.Format = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Format><![CDATA[", "]]></Format>");
                _msg.Recognition = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Recognition><![CDATA[", "]]></Recognition>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 视频消息

                ReqVideoMsg _msg = new ReqVideoMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");

                _msg.MediaId = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MediaId><![CDATA[", "]]></MediaId>");
                _msg.ThumbMediaId = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ThumbMediaId><![CDATA[", "]]></ThumbMediaId>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[location]]></MsgType>"))
            {
                #region 地理位置消息

                ReqLocationMsg _msg = new ReqLocationMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");

                _msg.Location_X = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Location_X>", "</Location_X>");
                _msg.Location_Y = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Location_Y>", "</Location_Y>");
                _msg.Scale = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Scale>", "</Scale>");
                _msg.Label = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Label>", "</Label>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[link]]></MsgType>"))
            {
                #region 链接消息

                ReqLinkMsg _msg = new ReqLinkMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");

                _msg.Title = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Title><![CDATA[", "]]></Title>");
                _msg.Description = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Description><![CDATA[", "]]></Description>");
                _msg.Url = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Url><![CDATA[", "]]></Url>");
                return _msg;

                #endregion
            }
            if (m_requestBody.Contains("<MsgType><![CDATA[event]]></MsgType>"))
            {
                #region 事件消息处理

                ReqEventMsg _msg = new ReqEventMsg(PlatformType.Weixin);
                //_msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<CreateTime>", "</CreateTime>");

                string _event = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Event><![CDATA[", "]]></Event>");
                switch (_event.ToLower())
                {
                    case "subscribe":
                        {
                            _msg.EventType = ReqEventType.Subscribe;
                            if (m_requestBody.Contains("<Ticket><![CDATA["))
                            {
                                _msg.EventType = ReqEventType.ScanSubscribe;
                                _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<EventKey><![CDATA[", "]]></EventKey>");
                                _msg.Ticket = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Ticket><![CDATA[", "]]></Ticket>");
                            }
                        }
                        break;
                    case "scan":
                        {
                            _msg.EventType = ReqEventType.Scan;
                            _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<EventKey><![CDATA[", "]]></EventKey>");
                            _msg.Ticket = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Ticket><![CDATA[", "]]></Ticket>");
                        }
                        break;
                    case "location":
                        {
                            _msg.EventType = ReqEventType.Location;
                            _msg.Latitude = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Latitude>", "</Latitude>");
                            _msg.Longitude = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Longitude>", "</Longitude>");
                            _msg.Precision = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<Precision>", "</Precision>");
                        }
                        break;
                    case "click":
                        {
                            _msg.EventType = ReqEventType.Click;
                            _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(m_requestBody, "<EventKey><![CDATA[", "]]></EventKey>");
                        }
                        break;
                    case "unsubscribe":
                        _msg.EventType = ReqEventType.UnSubscribe;
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

        protected override void ReplyTextMessage(ResTextMsg message)
        {
            Response.Write(message.GetWeixinResXml());
        }

        protected override ResMsg[] HandlerSubscribeEvent(ReqEventMsg message)
        {
            //订阅事件，获取用户数据并记录数据库            
            
            //
            return base.HandlerSubscribeEvent(message);
        }
        protected override ResMsg[] HandlerScanEvent(ReqEventMsg message)
        {
            return base.HandlerScanEvent(message);
        }

        protected override ResMsg[] HandlerScanSubscribeEvent(ReqEventMsg message)
        {
            return base.HandlerScanSubscribeEvent(message);
        }


    }
}
