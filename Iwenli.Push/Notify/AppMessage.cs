using Iwenli.Push.JPush;
using Iwenli.Push.JPush.Common;
using Iwenli.Push.JPush.PushApi;
using System;

namespace Iwenli.Push.Notify
{
    /// <summary>
    /// 安卓和IOS推送信息
    /// </summary>
    public class AppMessage : MessageBase
    {
        /// <summary>
        /// 安卓用到的通知标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 信息模板id
        /// </summary>
        public override long PushId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public override string Temp { get; set; }

        /// <summary>
        /// 推送平台0全部 1安卓 2IOS
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 推送用户ID
        /// </summary>
        public string[] UserIds { get; set; }

        /// <summary>
        /// 附加参数（app收到通知后根据参数做相应的操作)
        /// </summary>
        public string Extras { get; set; }

        private int m_timeToLive = 86400;
        /// <summary>
        /// 离线消息保留时长
        /// </summary>
        public int TimeToLive
        {
            get { return m_timeToLive; }
            set { m_timeToLive = value; }
        }

        /// <summary>
        /// 极光app
        /// </summary>
        public JPushAPI PushAPI { get; set; }

        public MessageResult MessageResult { get; set; }

        /// <summary>
        /// 发送消息前
        /// </summary>
        public event Action<AppMessage> OnSendStart;
        /// <summary>
        /// 发送后的事件
        /// </summary>
        public event Action<AppMessage, MessageResult> OnSendEnd;
        /// <summary>
        /// 发送消息异常处理
        /// </summary>
        public event Action<AppMessage, APIRequestException> SendException;

        public override bool Send()
        {
            try
            {
                OnSendStart?.Invoke(this);

                var _pushInfo = new PushInfo();
                _pushInfo.Alert = this.Temp.FormatDic(this.m_ReplaceObj);
                _pushInfo.Title = this.Title.FormatDic(this.m_ReplaceObj);
                _pushInfo.Extras = LitJson.JsonMapper.ToObject(this.Extras);
                _pushInfo.TimeToLive = this.TimeToLive;

                this.PushAPI.PushContent = _pushInfo;

                switch (this.Platform)
                {
                    case 0:
                        MessageResult = PushAPI.PushAndroid_IOS(this.UserIds);
                        break;
                    case 1:
                        MessageResult = PushAPI.PushAndroid(this.UserIds);
                        break;
                    case 2:
                        MessageResult = PushAPI.PushIOS(this.UserIds);
                        break;
                    default:
                        break;
                }

                if (OnSendEnd != null)
                    OnSendEnd(this, MessageResult);

                return true;
            }
            catch (APIRequestException ex)
            {
                SendException?.Invoke(this, ex);
                //this.TxLogDebug(string.Format("APP推送消息错误：{0}", string.Join(",", UserIds)));
                return false;
            }
        }

    }
}
