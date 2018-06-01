using Iwenli.Push.Notify;
using System;

namespace Iwenli.Push
{
    /// <summary>
    /// 短信通知
    /// </summary>
    public class SmsMessageService
    {
        public SmsMessage SmsMessage { get; set; }

        public SmsMessageService(PlatformEnum platform,string appId,string appKey,string sign)
        {
            if (SmsMessage == null)
            {
                SmsMessage = new SmsMessage();
            }

            SmsMessage.Platform = platform;
            SmsMessage.PlatformKey = appId;
            SmsMessage.PlatformSecret = appKey;
            SmsMessage.Sign = sign;
            SmsMessage.SendResult += SendResult;
            SmsMessage.SendException += SendException;
        }
        /// <summary>
        /// 每次发送短信-调用
        /// </summary>
        /// <param name="t1">发送短信的实例</param>
        /// <param name="t2">手机号</param>
        /// <param name="t3">返回结果</param>
        private void SendResult(SmsMessage t1, string t2, string t3)
        {
            this.LogDebug(string.Format("平台：【{0}】，发送号码：【{1}】，返回：{2}", t1.Platform, t2, t3));
            switch (t1.Platform)
            {
                case PlatformEnum.YuntongCloud:
                    break;
                case PlatformEnum.AliCloud:
                    break;
                case PlatformEnum.QCloud:
                    break;
            }
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="ex"></param>
        private void SendException(SmsMessage t1, Exception ex)
        {
            this.LogError(string.Format("平台：【{0}】，发送号码：【{1}】，异常：{2}", t1.Platform, string.Join(",", t1.Mobiles), ex.Message));
        }
    }
}
