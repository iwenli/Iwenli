using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Invite
{
    public class SignInfo
    {
        #region 字段

        private int m_signId;

        private string m_signName;

        private string m_inviteId;

        private int m_accountId;

        private int m_sceneId;

        private string m_cover;

        private string m_remark;

        private string m_fieldConfig;

        private string m_signTip;

        private int m_templateId;

        private int m_state;

        private DateTime m_addTime;

        #endregion

        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int SignId
        {
            get { return m_signId; }
            set { m_signId = value; }
        }

        public int BrandId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignName
        {
            get { return m_signName; }
            set { m_signName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string InviteId
        {
            get { return m_inviteId; }
            set { m_inviteId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AccountId
        {
            get { return m_accountId; }
            set { m_accountId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SceneId
        {
            get { return m_sceneId; }
            set { m_sceneId = value; }
        }

        /// <summary>
        /// 扫码回复图文的背景图
        /// </summary>
        public string Cover
        {
            get { return m_cover; }
            set { m_cover = value; }
        }

        /// <summary>
        /// 回复图文的description
        /// </summary>
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }

        /// <summary>
        /// 用户需要填写的字段信息
        /// </summary>
        public string FieldConfig
        {
            get { return m_fieldConfig; }
            set { m_fieldConfig = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SuccessTip
        {
            get { return m_signTip; }
            set { m_signTip = value; }
        }

        public string ErrorTip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TemplateId
        {
            get { return m_templateId; }
            set { m_templateId = value; }
        }

        /// <summary>
        /// 签到状态 0：已关闭 1：进行中  
        /// </summary>
        public int State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime
        {
            get { return m_addTime; }
            set { m_addTime = value; }
        }

        public string SceneName { get; set; }

        public int TableCount { get; set; }

        public int TabelPerson { get; set; }

        public string TableAlias { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        #endregion

        /// <summary>
        /// 根据签到id获取签到信息
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public static SignInfo GetSignInfoBySignId(int signId)
        {
            SignInfo info = null;
            using (TxDataHelper helper = TxDataHelper.GetDataHelper("TxoooBrands"))
            {
                string sql =
                    "select sign_id,brand_id, sign_name, invite_id, account_id, cover, remark, field_config, success_tip, error_tip, add_time, table_person, table_count, table_alias, scene_name,start_time,end_time from [Invite_SignInfo] where sign_id=@SignId";
                helper.SpFileValue["@SignId"] = signId;

                DataRowCollection rows = helper.SqlGetDataTable(sql, helper.SpFileValue).Rows;

                if (rows.Count > 0)
                {
                    DataRow row = rows[0];
                    info = new SignInfo()
                    {
                        SignId = Convert.ToInt32(row["sign_id"]),
                        BrandId = Convert.ToInt32(row["brand_id"]),
                        SignName = row["sign_name"].ToString(),
                        InviteId = row["invite_id"].ToString(),
                        AccountId = Convert.ToInt32(row["account_id"]),
                        Cover = row["cover"].ToString(),
                        Remark = row["remark"].ToString(),
                        FieldConfig = row["field_config"].ToString(),
                        SuccessTip = row["success_tip"].ToString(),
                        ErrorTip = row["error_tip"].ToString(),
                        AddTime = Convert.ToDateTime(row["add_time"]),
                        TabelPerson = row["table_person"] == DBNull.Value ? 0 : Convert.ToInt32(row["table_person"]),
                        TableCount = row["table_count"] == DBNull.Value ? 0 : Convert.ToInt32(row["table_count"]),
                        TableAlias = row["table_alias"] == DBNull.Value ? "" : row["table_alias"].ToString(),
                        SceneName = row["scene_name"] == DBNull.Value ? "" : row["scene_name"].ToString(),
                        StartTime = Convert.ToDateTime(row["start_time"]),
                        EndTime = Convert.ToDateTime(row["end_time"])
                    };
                }
                return info;
            }
        }
    }
}
