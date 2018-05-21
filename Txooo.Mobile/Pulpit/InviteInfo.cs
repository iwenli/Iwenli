using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Pulpit
{
    /// <summary>
    /// 邀请信息
    /// </summary>
    class InviteInfo
    {
        long m_employeeId;

        public long EmployeeId
        {
            get { return m_employeeId; }
            set { m_employeeId = value; }
        }

        public string EmployeeName { set; get; }


        long m_brandId;

        public long BrandId
        {
            get { return m_brandId; }
            set { m_brandId = value; }
        }

        public string BrandName { set; get; }
        public string Position { set; get; }
        public string Name { set; get; }
        public string Mobile { set; get; }

        #region 构造函数

        public InviteInfo(long employeeId)
        {
            m_employeeId = employeeId;
            OA.OAUserInfo _user = OA.OAUserInfo.GetUserInfoByUserId(m_employeeId);
            if (_user != null)
            {
                EmployeeName = _user.Username;
            }
        }

        public InviteInfo(string info)
        {
            #region 全局数据

            if (!string.IsNullOrEmpty(info))
            {
                //员工id,品牌Id,口袋鸡排,总经理,何总,13581952323
                string[] _info = info.Replace("，", ",").Split(',');

                if (_info.Length > 0)
                {
                    long.TryParse(_info[0], out m_employeeId);
                }
                if (_info.Length > 1)
                {
                    EmployeeName = _info[1];
                }
                if (_info.Length > 2)
                {
                    long.TryParse(_info[2], out m_brandId);
                }
                if (_info.Length > 3)
                {
                    BrandName = _info[3];
                }
                if (_info.Length > 4)
                {
                    Position = _info[4];
                }
                if (_info.Length > 5)
                {
                    Name = _info[5];
                }
                if (_info.Length > 6)
                {
                    Mobile = _info[6];
                }
            }

            #endregion
       }

        #endregion

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", m_employeeId, EmployeeName, m_brandId, BrandName, Position, Name, Mobile);
        }

        #region 获取二维码ID

        /// <summary>
        /// 获取二维码ID
        /// </summary>
        /// <returns></returns>
        public int GetQrcodeId()
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    helper.SpFileValue["@Qrcode_Info"] = ToString();
                    helper.SpFileValue["@Qrcode_sn"] = Txooo.Text.EncryptHelper.MD5(ToString());
                    return helper.SpGetReturnValue("SP_Temp_GetTxTempQrcodeId_MrLee");
                }
            }
            catch (Exception ex)
            {
               
            }
            return 0;
        }

        #endregion

        #region 获取发送的消息

        public Platform.ResMsg GetResMsg(Platform.ReqMsg msg)
        {
            string _title = "您好，天下商机诚邀您参加“第九届商机讲堂”";
            string _url = "http://pulpit.txooo.com/Receipt.htm?platform=" + (int)msg.Platform + "&platform_user_sn=" + PlatformUserInfo.EncryptPlatformUserSn(msg.Platform, msg.FromUserName) + "&rqcode=" + GetQrcodeId();
            if (!string.IsNullOrEmpty(Name))
            {
                _title = Name + "，您好，天下商机诚邀您参加“第九届商机讲堂”";
            }

            #region 邀请函

            //返回数据
            Platform.ResNewsMsg resMsg = new Platform.ResNewsMsg(msg);
            resMsg.Articles.Add(new Platform.ResArticle()
            {
                Title = _title,
                Discription = @"移动互联网时代，如何把您的公司开到消费者的手机里？
移动营销  共赢天下
 -- 从“项目时代”走向“品牌时代”
天下商机诚邀您参加第九届商机讲堂
",
                PicUrl = "",
                Url = _url
            });
            return resMsg;

            #endregion
        }

        #endregion

        public static InviteInfo GetInviteInfoById(long id)
        {
            InviteInfo _info = new InviteInfo(0);
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string _sql = "SELECT [qrcode_info] FROM [dbo].[Tx_TempQrcode] WHERE [qrcode_id]="+id;
                    _info = new InviteInfo(helper.SqlScalar(_sql).ToString());
                }
            }
            catch (Exception ex)
            {

            }
            return _info;
        }

    }
}
