using Iwenli.Push.JPush.Common;
using Iwenli.Push.JPush.PushApi;
using Iwenli.Push.Notify;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Iwenli.Push
{
    /// <summary>
    /// app消息
    /// </summary>
    public class AppMessageService
    {
        public AppMessage AppMessage { get; set; }

        public AppMessageService()
        {
            AppMessage = new AppMessage();
            AppMessage.OnSendStart += S_OnSendStart;
            AppMessage.OnSendEnd += S_OnSendEnd;
            AppMessage.SendException += S_SendException;
        }

        private void S_OnSendStart(AppMessage arg1)
        {
            if (arg1.Extras.IndexOf("{") == -1)
            {
                arg1.Extras = "{" + arg1.Extras + "}";
            }
        }
        private void S_OnSendEnd(AppMessage arg1, MessageResult arg2)
        {
            PushDAL.InsertPushReportReceived(arg2.msg_id);
            PushDAL.InsertPushHistroy(arg1.PushId, arg1.PushAPI.PushContent.Alert, string.Join(",", arg1.UserIds), arg2.msg_id);
        }

        private void S_SendException(AppMessage arg1, APIRequestException ex)
        {
            this.LogError(string.Format("APP推送消息错误，用户{0},代码：{1}，信息：{2}", string.Join(",", arg1.UserIds), ex.ErrorCode, ex));

            //if (ex.ErrorCode == 1011)
            //{
            //    Task.Run(() =>
            //    {
            //        try
            //        {
            //            string _url = string.Format("{0}/Serve/Passport/RegistJPush?uids={1}", SiteConfig.WebUrl, string.Join(",", arg1.UserIds));
            //            this.LogDebug(string.Format("注册极光：{0}", _url));
            //            new WebClient().DownloadData(_url);
            //        }
            //        catch (Exception ex2)
            //        {
            //            this.LogError(string.Format("再次注册极光异常：{0}", ex2.Message), ex2);
            //        }
            //    });
            //}
        }
    }
}
