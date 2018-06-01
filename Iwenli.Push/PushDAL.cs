#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：PushDAL
 *  所属项目：Iwenli.Push
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/31 16:36:56
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using Iwenli.Push.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Iwenli.Push.JPush.ReportApi.ReceivedResult;

namespace Iwenli.Push
{
    /// <summary>
    /// 消息推送记录
    /// </summary>
    class PushDAL
    {
        #region 消息推送记录msg_id
        /// <summary>
        /// 消息推送记录用于统计送达数据
        /// </summary>
        /// <param name="msgId"></param>
        public static void InsertPushReportReceived(long msgId)
        {
            using (DataHelper helper = DataHelper.GetDataHelper("IwenliPush"))
            {
                string _sql = " INSERT INTO [dbo].[app_jpush_report] ([msg_id]) VALUES (@msg_id) ";
                helper.SpFileValue["@msg_id"] = msgId;
                helper.SqlExecute(_sql, helper.SpFileValue);
            }
        }
        /// <summary>
        /// 消息推送统计送达数据
        /// </summary>
        /// <param name="received"></param>
        public static void SetPushReportReceived(Received received)
        {
            using (DataHelper helper = DataHelper.GetDataHelper("IwenliPush"))
            {
                string _sql = " UPDATE [dbo].[app_jpush_report] SET [android_received]=@android_received , [ios_apns_sent]=@ios_apns_sent where [msg_id]=@msg_id ";
                helper.SpFileValue["@msg_id"] = received.msg_id;
                helper.SpFileValue["@android_received"] = received.android_received == null ? "" : received.android_received;
                helper.SpFileValue["@ios_apns_sent"] = received.ios_apns_sent == null ? "" : received.ios_apns_sent;
                helper.SqlExecute(_sql, helper.SpFileValue);
            }
        }

        #endregion

        #region 推送历史记录
        /// <summary>
        /// 推送历史记录入库
        /// </summary>
        /// <param name="pushId">模板id</param>
        /// <param name="content">内容</param>
        /// <param name="pushTag">用户</param>
        /// <param name="msg_id">返回的消息</param>
        public static void InsertPushHistroy(long pushId, string pushContent, string pushTag, long msgId)
        {
            using (DataHelper helper = DataHelper.GetDataHelper("IwenliPush"))
            {
                string _sql = " INSERT INTO [dbo].[app_pushhistroy] ([push_id] ,[push_content] ,[push_tag] ,[msg_id] ) VALUES (@push_id ,@push_content ,@push_tag ,@msg_id ) ";

                helper.SpFileValue["@push_id"] = pushId;
                helper.SpFileValue["@push_content"] = pushContent;
                helper.SpFileValue["@push_tag"] = pushTag;
                helper.SpFileValue["@msg_id"] = msgId;
                helper.SqlExecute(_sql, helper.SpFileValue);
            }
        }
        #endregion

        #region 发送系统消息
        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="userIds">本消息接收人id集合</param>
        /// <param name="message">消息实体</param>
        public static void InsertSystemMessage(List<long> userIds, SystemMessageInfo message)
        {
            using (DataHelper helper = DataHelper.GetDataHelper("IwenliPush"))
            {
                string _sql = "INSERT INTO [dbo].[sales_mch_sysmsg] ([user_id],[title],[content],[msg_type],[only_id]) VALUES (@user_id,@title,@msg_content,@msg_type,@only_id)";
                helper.SpFileValue["@only_id"] = message.OnlyId;
                helper.SpFileValue["@user_id"] = message.UserId;
                helper.SpFileValue["@title"] = message.Title;
                helper.SpFileValue["@msg_content"] = "{" + message.Content + "}";
                helper.SpFileValue["@msg_type"] = message.MsgType;
                foreach (var item in userIds)
                {
                    helper.SpFileValue["@user_id"] = item;
                    helper.SqlExecute(_sql, helper.SpFileValue);
                }
            }
        }
        #endregion
    }
}
