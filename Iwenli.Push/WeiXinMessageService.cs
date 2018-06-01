using Iwenli.Mobile;
using Iwenli.Push.Notify;
using System.Collections.Specialized;

namespace Iwenli.Push
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeiXinMessageService
    {
        public WeiXinMessage WeiXinMessage { get; set; }

        public WeiXinMessageService(AccountInfo account)
        {
            WeiXinMessage = new WeiXinMessage(account);
            WeiXinMessage.OnSendEnd += S_OnSendEnd;
        }

        private void S_OnSendEnd(WeiXinMessage arg1, NameValueCollection arg2, string returnInfo)
        {
            PushDAL.InsertPushHistroy(arg1.PushId, arg2["first"], string.Join(",", arg1.OpenIds), 0);
        }
    }
}
