using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Platform
{
    public class ReqEventMsg : ReqMsg
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public ReqEventType EventType { set; get; }

        /// <summary>
        /// subscribe：事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// scan：事件KEY值，是一个32位无符号整数
        /// Click：事件KEY值，是一个32位无符号整数
        /// </summary>
        public string EventKey { set; get; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { set; get; }

        /// <summary>
        /// 请求消息状态
        /// </summary>
        public string Status { get; set; }

        #region 地理位置

        /// <summary>
        /// 地理位置纬度 
        /// </summary>
        public string Latitude { set; get; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { set; get; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { set; get; }

        #endregion
        
        public ReqEventMsg(PlatformType type)
        {
            Platform = type;
            MsgType = ReqMsgType.Event;
        }
    }

    public class ReqMassSendEventMsg : ReqEventMsg
    {
        public ReqMassSendEventMsg(PlatformType type) : base(type) { }
        /// <summary>
        /// 群发状态群发的结构，为“send success”或“send fail”或“err(num)”。但send success时，也有可能因用户拒收公众号的消息、系统错误等原因造成少量用户接收失败。err(num)是审核失败的具体原因，可能的情况如下：err(10001), //涉嫌广告 err(20001), //涉嫌政治 err(20004), //涉嫌社会 err(20002), //涉嫌色情 err(20006), //涉嫌违法犯罪 err(20008), //涉嫌欺诈 err(20013), //涉嫌版权 err(22000), //涉嫌互推(互相宣传) err(21000), //涉嫌其他
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public string TotalCount { get; set; }
        /// <summary>
        /// 过滤（过滤是指特定地区、性别的过滤、用户设置拒收的过滤，用户接收已超4条的过滤）后，准备发送的粉丝数，原则上，FilterCount = SentCount + ErrorCount
        /// </summary>
        public string FilterCount { get; set; }
        /// <summary>
        /// 成功粉丝数
        /// </summary>
        public string SentCount { get; set; }
        /// <summary>
        /// 失败粉丝数
        /// </summary>
        public string ErrorCount { get; set; }
    }

    public enum ReqEventType : int
    {
        /// <summary>
        /// 订阅
        /// </summary>
        Subscribe = 0,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UnSubscribe = 1,
        /// <summary>
        /// 扫描
        /// </summary>
        Scan = 2,
        /// <summary>
        /// 扫描订阅
        /// </summary>
        ScanSubscribe = 3,
        /// <summary>
        /// 地理位置
        /// </summary>
        Location = 4,
        /// <summary>
        /// 点击自定义菜单事件
        /// </summary>
        Click = 5,
        /// <summary>
        /// 点击菜单跳转事件
        /// </summary>
        View = 6,
        /// <summary>
        /// 群发响应
        /// </summary>
        MassSend = 7,
        /// <summary>
        /// 模板消息响应
        /// </summary>
        TempSend = 8
    }
}
