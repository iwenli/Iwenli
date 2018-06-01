using Iwenli.Mobile;
using Iwenli.Mobile.Platform.Entity;
using System;
using System.Collections.Specialized;

namespace Iwenli.Push.Notify
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeiXinMessage : MessageBase
    {
        private AccountInfo _accountInfo;
        public WeiXinMessage()
        { }
        public WeiXinMessage(AccountInfo accountInfo)
        {
            _accountInfo = accountInfo;
        }
        /// <summary>
        /// 信息模板id
        /// </summary>
        public override long PushId { get; set; }
        /// <summary>
        /// 公众号消息模板ID
        /// </summary>
        public override string Temp { get; set; }

        /// <summary>
        /// 消息地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 账号ID（Mobile中）
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string[] OpenIds { get; set; }

        /// <summary>
        /// 发送后的事件
        /// </summary>
        public event Action<WeiXinMessage, NameValueCollection, string> OnSendEnd;

        public override bool Send()
        {
            try
            {
                string _returnInfo = string.Empty;
                AccountInfo _accountInfo = this._accountInfo ?? Iwenli.Mobile.AccountInfo.GetAccountInfoByIdFromCache(AccountId);

                var _tempMsg = new ResTemplateMsg();
                _tempMsg.TemplateId = this.Temp;
                _tempMsg.Url = this.Url;
                foreach (string key in this.m_ReplaceObj)
                {
                    _tempMsg.AddTemplateData(this.m_ReplaceObj[key], key, "#173177");
                }

                foreach (string openid in OpenIds)
                {
                    _tempMsg.ToUserName = openid;
                    _accountInfo.ApiHelper.SendTempMsg(_tempMsg, out _returnInfo);
                }

                if (OnSendEnd != null)
                    OnSendEnd(this, m_ReplaceObj, _returnInfo);

                return true;
            }
            catch// (Exception ex)
            {
                return false;
            }
        }
    }
}
