using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Iwenli.Push.Notify
{
    public class SmsMessage : MessageBase
    {
        /// <summary>
        /// 信息模板id
        /// </summary>
        public override long PushId { get; set; }
        /// <summary>
        /// 短信模板ID
        /// </summary>
        public override string Temp { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string[] Mobiles { get; set; }

        /// <summary>
        /// 短信发送平台
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 短信发送平台key
        /// </summary>
        public string PlatformKey { get; set; }

        /// <summary>
        /// 短信发送平台密钥
        /// </summary>
        public string PlatformSecret { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 请求后的结果
        /// </summary>
        public event Action<SmsMessage, string, string> SendResult;

        /// <summary>
        /// 发送消息异常处理
        /// </summary>
        public event Action<SmsMessage, Exception> SendException;

        public override bool Send()
        {
            try
            {
                //string _returnMsg = string.Empty;
                //if (string.IsNullOrEmpty(Sign)) { Sign = "纵横商海"; }

                //TxNoticeService.NoticeServiceSoapClient _client = new TxNoticeService.NoticeServiceSoapClient();

                //var _param = new TxNoticeService.ArrayOfString();
                //Dictionary<string, string> _dic = new Dictionary<string, string>();
                //foreach (string key in this.m_ReplaceObj.Keys)
                //{
                //    _param.Add(this.m_ReplaceObj[key]);
                //    _dic.Add(key, m_ReplaceObj[key]);
                //}
                //switch (Platform)
                //{
                //    case PlatformEnum.YuntongCloud:
                //        {
                //            foreach (string mobile in Mobiles)
                //            {
                //                if (Regex.IsMatch(mobile, @"^1[0-9]{10}$", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                //                {
                //                    _returnMsg = _client.SendZhuanNotice(Temp, mobile, _param);
                //                }
                //                else
                //                {
                //                    _returnMsg = _client.SendZhuanNoticeByTXID(Temp, long.Parse(mobile), _param);
                //                }
                //                SendResult?.Invoke(this, mobile, _returnMsg);
                //            }
                //        }
                //        break;
                //    case PlatformEnum.AliCloud:
                //        {
                //            string _mobiles = string.Join(",", Mobiles);
                //            _returnMsg = _client.SendAliTplSMS(_mobiles, Temp, Sign, PlatformKey, PlatformSecret, JsonConvert.SerializeObject(_dic, Formatting.None));
                //            SendResult?.Invoke(this, _mobiles, _returnMsg);
                //        }
                //        break;
                //    case PlatformEnum.QCloud:
                //        {
                //            string _mobiles = string.Join(",", Mobiles);
                //            //_returnMsg = _client.SendAliTplSMS(_mobiles, Temp, Sign, JsonConvert.SerializeObject(m_ReplaceObj.Keys, Formatting.None));
                //            //SendResult?.Invoke(this, _mobiles, _returnMsg);
                //        }
                //        break;
                //}


                return true;
            }
            catch (Exception ex)
            {
                SendException?.Invoke(this, ex);
                return false;
            }
        }
    }

    /// <summary>
    /// 短信平台
    /// </summary>
    public enum PlatformEnum
    {
        YuntongCloud = 0,
        AliCloud = 1,
        QCloud = 2
    }
}
