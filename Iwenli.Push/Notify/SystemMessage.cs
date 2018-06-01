using System;
using System.Collections.Generic;

namespace Iwenli.Push.Notify
{
    /// <summary>
    /// 系统消息发送处理类
    /// </summary>
    public class SystemMessage : MessageBase
    {
        /// <summary>
        /// 信息模板id
        /// </summary>
        public override long PushId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public override string Temp { get; set; }

        /// <summary>
        /// 推送用户ID
        /// </summary>
        public List<long> UserIds { get; set; }

        public virtual SystemMessageInfo Message { get; set; }

        public override bool Send()
        {
            try
            {
                if (UserIds == null || UserIds.Count == 0) UserIds = new List<long>() { Message.UserId };
                PushDAL.InsertSystemMessage(UserIds, Message);
                PushDAL.InsertPushHistroy(Message.PushId, Message.HistoryContent, string.Join(",", UserIds), 0);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 系统消息实体
    /// </summary>
    public class SystemMessageInfo
    {
        #region 属性


        private long m_sysId;

        /// <summary>
        /// sys_id
        /// </summary>
        public long SysId
        {
            get { return m_sysId; }
            set { m_sysId = value; }
        }


        private long m_userId;

        /// <summary>
        /// user_id
        /// </summary>
        public long UserId
        {
            get { return m_userId; }
            set { m_userId = value; }
        }


        private string m_title;

        /// <summary>
        /// title
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }


        private string m_content;

        /// <summary>
        /// content
        /// </summary>
        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }


        private DateTime m_addTime;

        /// <summary>
        /// add_time
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }


        private bool m_isOpen;

        /// <summary>
        /// is_open
        /// </summary>
        public bool IsOpen
        {
            get { return m_isOpen; }
            set { m_isOpen = value; }
        }


        private string m_msgType;

        /// <summary>
        /// 消息类型映射 sales_template 别名(msg_type:team 团队发展，task 任务提醒，购物通知调AddUserOrderMsg,money 个人资产，person 个人信息)
        /// </summary>
        public string MsgType
        {
            get { return m_msgType; }
            set { m_msgType = value; }
        }


        private long m_onlyId;

        /// <summary>
        /// 当前消息类型下的标记id
        /// </summary>
        public long OnlyId
        {
            get { return m_onlyId; }
            set { m_onlyId = value; }
        }
        private string m_contentExtra;

        /// <summary>
        /// 内容参数
        /// </summary>
        public string ContentExtra
        {
            get { return m_contentExtra; }
            set { m_contentExtra = value; }
        }
        #endregion

        #region 外加属性
        /// <summary>
        /// 推送内容id
        /// </summary>
        public long PushId { get; set; }
        /// <summary>
        /// 历史记录内容
        /// </summary>
        public string HistoryContent { get; set; }
        #endregion
    }
}
