using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace Txooo.Mobile.Invite
{
    public class InviteInfo : DataRowEntityBase
    {
        #region 字段

        private int m_inviteId;
        private string m_inviteName;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private string m_inviteKeyword;
        private string m_inviteCover;
        private string m_inviteDescription;
        private string m_inviteNickname;
        private string m_mainContent;
        private string m_timeLine;
        private string m_relationPerson;
        private string m_fieldConfig;
        private string m_footLink;
        private int m_isNotify;
        private int m_accountId;
        private string m_templateStyle;
        private DateTime m_addTime;
        private int m_state;

        #endregion

        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int InviteId
        {
            get { return m_inviteId; }
            set { m_inviteId = value; }
        }

        public int BrandId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InviteName
        {
            get { return m_inviteName; }
            set { m_inviteName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            get { return m_startTime; }
            set { m_startTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            get { return m_endTime; }
            set { m_endTime = value; }
        }

        /// <summary>
        /// 邀请关键字
        /// </summary>
        public string InviteKeyword
        {
            get { return m_inviteKeyword; }
            set { m_inviteKeyword = value; }
        }

        /// <summary>
        /// 邀请的图文的背景图
        /// </summary>
        public string InviteCover
        {
            get { return m_inviteCover; }
            set { m_inviteCover = value; }
        }

        /// <summary>
        /// 邀请的图文的描述
        /// </summary>
        public string InviteDescription
        {
            get { return m_inviteDescription; }
            set { m_inviteDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string InviteNickname
        {
            get { return m_inviteNickname; }
            set { m_inviteNickname = value; }
        }

        /// <summary>
        /// 邀请正文
        /// </summary>
        public string MainContent
        {
            get { return m_mainContent; }
            set { m_mainContent = value; }
        }

        /// <summary>
        /// 日程
        /// </summary>
        public string TimeLine
        {
            get { return m_timeLine; }
            set { m_timeLine = value; }
        }

        /// <summary>
        /// 邀请关联人物
        /// </summary>
        public string RelationPerson
        {
            get { return m_relationPerson; }
            set { m_relationPerson = value; }
        }

        /// <summary>
        /// 回执需要填写的信息 json
        /// </summary>
        public string FieldConfig
        {
            get { return m_fieldConfig; }
            set { m_fieldConfig = value; }
        }

        /// <summary>
        /// 底部链接
        /// </summary>
        public string FootLink
        {
            get { return m_footLink; }
            set { m_footLink = value; }
        }

        /// <summary>
        /// 预留字段 是否短信提醒
        /// </summary>
        public int IsNotify
        {
            get { return m_isNotify; }
            set { m_isNotify = value; }
        }

        /// <summary>
        /// 微信公众号
        /// </summary>
        public int AccountId
        {
            get { return m_accountId; }
            set { m_accountId = value; }
        }

        /// <summary>
        /// 模板样式
        /// </summary>
        public string TemplateStyle
        {
            get { return m_templateStyle; }
            set { m_templateStyle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        public string AdvertiseImg { get; set; }

        public string InviteTitle { get; set; }
        #endregion

        public static InviteInfo GetInviteInfoByInviteId(int inviteId)
        {
            InviteInfo info = null;
            using (TxDataHelper helper = TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql =
                    "select invite_cover, invite_description,invite_title,brand_id from [Invite_Info] where invite_id=@InviteId";
                helper.SpFileValue["@InviteId"] = inviteId;

                DataRowCollection rows = helper.SqlGetDataTable(sql, helper.SpFileValue).Rows;
                if (rows.Count > 0)
                {
                    info = new InviteInfo()
                    {
                        InviteCover = rows[0]["invite_cover"].ToString(),
                        InviteDescription = rows[0]["invite_description"].ToString(),
                        InviteTitle = rows[0]["invite_title"].ToString(),
                        InviteId = inviteId,
                        BrandId = Convert.ToInt32(rows[0]["brand_id"])
                    };
                }
                return info;
            }

        }

        /// <summary>
        /// 根据二维码id获取邀请函id
        /// </summary>
        public static int GetInviteIdByQrNumber(long qrNumber,long accountId)
        {
            using (TxDataHelper helper = TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql = "select invite_id from [Invite_Info] where qr_number=@QrNumber and account_id=@AccountId";
                helper.SpFileValue["@QrNumber"] = qrNumber;
                helper.SpFileValue["@AccountId"] = accountId;

                return Convert.ToInt32(helper.SqlScalar(sql, helper.SpFileValue));
            }
        }
    }
}
