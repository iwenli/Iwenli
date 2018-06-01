using Iwenli.Push.JPush.PushApi;
using Iwenli.Push.JPush.PushApi.Entity;
using Iwenli.Push.JPush.PushApi.Notification;
using Iwenli.Push.JPush.ReportApi;
using LitJson;
using System.Collections;

namespace Iwenli.Push.JPush
{
    #region 用法
    /*
     JPushAPI _client = new JPushAPI("7eafdc23bf08ec9dd1e8f3f3", "d05256c81fe955bba002f8df");

            _client.PushContent = new PushInfo()
            {
                Alert = "测试推送",
                Title = "快来看哈！",
                Extras = new JsonData()
            };

            _client.PushIOS();
    */
    #endregion

    public class PushInfo
    {
        public string Title { get; set; }
        public string Alert { get; set; }
        public int TimeToLive { get; set; }

        private JsonData _extras = new JsonData();
        public JsonData Extras
        {
            get { return _extras; }
            set { _extras = value; }
        }
    }
    public class JPushAPI
    {
        private JPushClient Client;
        public PushInfo PushContent { get; set; }
        public JPushAPI(string appKey, string appSecret)
        {
            Client = new JPushClient(appKey, appSecret);
        }

        /// <summary>
        /// 推送安卓和IOS平台
        /// </summary>
        /// <returns></returns>
        public MessageResult PushAndroid_IOS(params string[] userIds) 
        {
            PushPayload pushPayload = new PushPayload();
            //设置平台
            pushPayload.platform = Platform.android_ios();

            //设置接收用户
            if (userIds != null && userIds.Length > 0)
            {
                pushPayload.audience = Audience.s_tag(userIds);
            }
            else
            {
                pushPayload.audience = Audience.all();
            }

            //设置内容
            var notification = new Notification().setAlert(PushContent.Alert);
            notification.AndroidNotification = new AndroidNotification();
            notification.AndroidNotification.setTitle(PushContent.Title);
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);

            //设置参数
            IDictionary dic = (IDictionary)PushContent.Extras;
            if (dic != null)
            {
                foreach (DictionaryEntry item in dic)
                {
                    notification.IosNotification.AddExtra(item.Key.ToString(), item.Value.ToString());
                    notification.AndroidNotification.AddExtra(item.Key.ToString(), item.Value.ToString());
                }
                notification.AndroidNotification.AddExtra("isJpush", "1");
            }

            pushPayload.notification = notification.Check();

            pushPayload.options.apns_production = true;
            pushPayload.options.time_to_live = PushContent.TimeToLive;

            return Client.SendPush(pushPayload);
        }

        /// <summary>
        /// 推送安卓
        /// </summary>
        /// <returns></returns>
        public MessageResult PushAndroid(params string[] userIds)
        {
            PushPayload pushPayload = new PushPayload();
            //设置平台
            pushPayload.platform = Platform.android();

            //设置接收用户
            if (userIds != null && userIds.Length > 0)
            {
                pushPayload.audience = Audience.s_tag(userIds);
            }
            else
            {
                pushPayload.audience = Audience.all();
            }

            //设置内容
            var notification = new Notification().setAlert(PushContent.Alert);
            notification.AndroidNotification = new AndroidNotification();
            notification.AndroidNotification.setTitle(PushContent.Title);

            //设置参数
            IDictionary dic = (IDictionary)PushContent.Extras;
            if (dic != null)
            {
                foreach (DictionaryEntry item in dic)
                {
                    notification.AndroidNotification.AddExtra(item.Key.ToString(), item.Value.ToString());
                }
                notification.AndroidNotification.AddExtra("isJpush", "1");
            }

            pushPayload.notification = notification.Check();
            pushPayload.options.time_to_live = PushContent.TimeToLive;

            return Client.SendPush(pushPayload);
        }
        /// <summary>
        /// 推送IOS
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public MessageResult PushIOS(params string[] userIds)
        {
            PushPayload pushPayload = new PushPayload();
            //设置平台
            pushPayload.platform = Platform.ios();

            //设置接收用户
            if (userIds != null && userIds.Length > 0)
            {
                pushPayload.audience = Audience.s_tag(userIds);
            }
            else
            {
                pushPayload.audience = Audience.all();
            }

            //设置内容
            var notification = new Notification().setAlert(PushContent.Alert);
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);

            //设置参数
            IDictionary dic = (IDictionary)PushContent.Extras;

            if (dic != null)
            {
                foreach (DictionaryEntry item in dic)
                {
                    notification.IosNotification.AddExtra(item.Key.ToString(), item.Value.ToString());
                }
                notification.AndroidNotification.AddExtra("isJpush", "1");
            }

            pushPayload.notification = notification.Check();

            pushPayload.options.apns_production = true;
            pushPayload.options.time_to_live = PushContent.TimeToLive;

            return Client.SendPush(pushPayload);
        }

        /// <summary>
        /// 送达统计
        /// </summary>
        /// <param name="msg_ids"></param>
        /// <returns></returns>
        public ReceivedResult ReportReceived(string msg_ids)
        {
            var _result = Client.getReceivedApi_v3(msg_ids);
            foreach (var item in _result.ReceivedList)
            {
                PushDAL.SetPushReportReceived(item);
            }
            return _result;
        }
    }
}
