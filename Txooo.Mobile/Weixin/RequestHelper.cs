using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Weixin
{
    /// <summary>
    /// 请求处理
    /// </summary>
    public class RequestHelper
    {     
        public string GetResponseMessage(System.Web.HttpContext context)
        {
            #region 不能修改 echostr — 微信应用提交验证
            
            string echostr = context.Request.QueryString["echostr"];
            if (!string.IsNullOrEmpty(echostr))
            {
               return echostr;
            }

            #endregion

            #region 微信消息处理

            try
            {
                string reqmsg = string.Empty;
                using (StreamReader responseReader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    reqmsg = responseReader.ReadToEnd();
                }                
                if (string.IsNullOrEmpty(reqmsg))
                {
                    return "";
                }
                this.TxLogInfo("接受消息：" + reqmsg);
                
                //提前消息
                ReqMsg _reqmsg = GetReqMsg(reqmsg);

                if (_reqmsg != null)
                {
                    //根据请求消息提取返回消息
                    ResMsg _resmsg = new CmdHelper().GetReturnInfo(_reqmsg);
                    if (_resmsg != null)
                    {
                        if (_reqmsg.MsgType == ReqMsgType.Text)
                        {
                            return _resmsg.GetWeixinResString();
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("新消息类型无法处理,消息全文:{0}", reqmsg));
                }                
            }
            catch (Exception ex)
            {
                this.TxLogError("消息处理错误：" + ex.Message);
            }

            #endregion
            
            return "";
        }

        #region 根据请求字符串获取请求消息

        /// <summary>
        /// 根据请求字符串获取请求消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ReqMsg GetReqMsg(string msg)
        {
            if (msg.Contains("<MsgType><![CDATA[text]]></MsgType>"))
            {
                #region 文本消息

                ReqTextMag _msg = new ReqTextMag(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");
                _msg.Content = Txooo.TxStringHelper.GetMiddleStr(msg, "<Content><![CDATA[", "]]></Content>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[image]]></MsgType>"))
            {
                #region 图片消息

                ReqImageMsg _msg = new ReqImageMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");
                _msg.PicUrl = Txooo.TxStringHelper.GetMiddleStr(msg, "<PicUrl><![CDATA[", "]]></PicUrl>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 语音消息

                ReqVoiceMsg _msg = new ReqVoiceMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");

                _msg.MediaId = Txooo.TxStringHelper.GetMiddleStr(msg, "<MediaId><![CDATA[", "]]></MediaId>");
                _msg.Format = Txooo.TxStringHelper.GetMiddleStr(msg, "<Format><![CDATA[", "]]></Format>");
                _msg.Recognition = Txooo.TxStringHelper.GetMiddleStr(msg, "<Recognition><![CDATA[", "]]></Recognition>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[voice]]></MsgType>"))
            {
                #region 视频消息

                ReqVideoMsg _msg = new ReqVideoMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");

                _msg.MediaId = Txooo.TxStringHelper.GetMiddleStr(msg, "<MediaId><![CDATA[", "]]></MediaId>");
                _msg.ThumbMediaId = Txooo.TxStringHelper.GetMiddleStr(msg, "<ThumbMediaId><![CDATA[", "]]></ThumbMediaId>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[location]]></MsgType>"))
            {
                #region 地理位置消息

                ReqLocationMsg _msg = new ReqLocationMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");

                _msg.Location_X = Txooo.TxStringHelper.GetMiddleStr(msg, "<Location_X>", "</Location_X>");
                _msg.Location_Y = Txooo.TxStringHelper.GetMiddleStr(msg, "<Location_Y>", "</Location_Y>");
                _msg.Scale = Txooo.TxStringHelper.GetMiddleStr(msg, "<Scale>", "</Scale>");
                _msg.Label = Txooo.TxStringHelper.GetMiddleStr(msg, "<Label>", "</Label>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[link]]></MsgType>"))
            {
                #region 链接消息

                ReqLinkMsg _msg = new ReqLinkMsg(PlatformType.Weixin);
                _msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");

                _msg.Title = Txooo.TxStringHelper.GetMiddleStr(msg, "<Title><![CDATA[", "]]></Title>");
                _msg.Description = Txooo.TxStringHelper.GetMiddleStr(msg, "<Description><![CDATA[", "]]></Description>");
                _msg.Url = Txooo.TxStringHelper.GetMiddleStr(msg, "<Url><![CDATA[", "]]></Url>");
                return _msg;

                #endregion
            }
            if (msg.Contains("<MsgType><![CDATA[event]]></MsgType>"))
            {
                #region 事件消息处理

                ReqEventMsg _msg = new ReqEventMsg(PlatformType.Weixin);
                //_msg.MsgId = long.Parse(Txooo.TxStringHelper.GetMiddleStr(msg, "<MsgId>", "</MsgId>"));
                _msg.ToUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<ToUserName><![CDATA[", "]]></ToUserName>");
                _msg.FromUserName = Txooo.TxStringHelper.GetMiddleStr(msg, "<FromUserName><![CDATA[", "]]></FromUserName>");
                _msg.CreateTime = Txooo.TxStringHelper.GetMiddleStr(msg, "<CreateTime>", "</CreateTime>");

                string _event = Txooo.TxStringHelper.GetMiddleStr(msg, "<Event><![CDATA[", "]]></Event>");
                switch (_event.ToLower())
                {
                    case "subscribe":
                        {
                            _msg.EventType = ReqEventType.Subscribe;
                            if (msg.Contains("<EventKey><![CDATA["))
                            {
                                _msg.EventType = ReqEventType.ScanSubscribe;
                                _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(msg, "<EventKey><![CDATA[", "]]></EventKey>");
                                _msg.Ticket = Txooo.TxStringHelper.GetMiddleStr(msg, "<Ticket><![CDATA[", "]]></Ticket>");
                            }
                        }
                        break;
                    case "scan":
                        {
                            _msg.EventType = ReqEventType.Scan;
                            _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(msg, "<EventKey><![CDATA[", "]]></EventKey>");
                            _msg.Ticket = Txooo.TxStringHelper.GetMiddleStr(msg, "<Ticket><![CDATA[", "]]></Ticket>");
                        }
                        break;
                    case "location":
                        {
                            _msg.EventType = ReqEventType.Location;
                            _msg.Latitude = Txooo.TxStringHelper.GetMiddleStr(msg, "<Latitude>", "</Latitude>");
                            _msg.Longitude = Txooo.TxStringHelper.GetMiddleStr(msg, "<Longitude>", "</Longitude>");
                            _msg.Precision = Txooo.TxStringHelper.GetMiddleStr(msg, "<Precision>", "</Precision>");
                        }
                        break;
                    case "click":
                        {
                            _msg.EventType = ReqEventType.Click;
                            _msg.EventKey = Txooo.TxStringHelper.GetMiddleStr(msg, "<EventKey><![CDATA[", "]]></EventKey>");
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
    }
}
