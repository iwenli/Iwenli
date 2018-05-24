#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：AccountInfo
 *  所属项目：Iwenli.Mobile
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 16:53:04
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

using Iwenli.Mobile.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile
{
    /// <summary>
    /// 获取账号授权信息委托
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public delegate string GetAccountAccessTokenDelegate(AccountInfo info);

    /// <summary>
    /// 账号数据
    /// </summary>
    public class AccountInfo : Iwenli.Data.Entity.DataRowEntityBase
    {
        #region 基本属性

        long m_brandId;
        /// <summary>
        /// 品牌ID
        /// </summary>
        public long BrandId
        {
            get { return m_brandId; }
            set { m_brandId = value; }
        }
        long m_accountId;
        /// <summary>
        /// 账号ID
        /// </summary>
        public long AccountId
        {
            get { return m_accountId; }
            set { m_accountId = value; }
        }
        string m_name;
        /// <summary>
        /// 账号名称
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        AccountStatus m_status;
        /// <summary>
        /// 账号状态
        /// </summary>
        public AccountStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }
        PlatformType m_platform;
        /// <summary>
        /// 平台
        /// </summary>
        public PlatformType Platform
        {
            get { return m_platform; }
            set { m_platform = value; }
        }
        string m_qrcodeUrl;
        /// <summary>
        /// 二维码地址
        /// </summary>
        public string QrcodeUrl
        {
            get { return m_qrcodeUrl; }
            set { m_qrcodeUrl = value; }
        }
        string m_txApiUrl;
        /// <summary>
        /// API接口地址
        /// </summary>
        public string TxApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(m_txApiUrl))
                {
                    m_txApiUrl = string.Format("http://mobile.txooo.com/{0}/{1}.msg", m_accountId.ToString("000000"), m_platform.ToString());
                }
                return m_txApiUrl;
            }
            set { m_txApiUrl = value; }
        }
        string m_txApiToken;
        /// <summary>
        /// APIToken
        /// </summary>
        public string TxApiToken
        {
            get
            {
                if (string.IsNullOrEmpty(m_txApiToken))
                {
                    string _str = m_brandId.ToString("000000") + m_accountId.ToString();
                    m_txApiToken = _str.AESEncrypt().MD5();
                }
                return m_txApiToken;
            }
            set { m_txApiToken = value; }
        }
        string m_appId;
        /// <summary>
        /// 第三方AppId
        /// </summary>
        public string AppId
        {
            get { return m_appId; }
            set { m_appId = value; }
        }
        string m_appSecret;
        /// <summary>
        /// 第三方AppSecret
        /// </summary>
        public string AppSecret
        {
            get { return m_appSecret; }
            set { m_appSecret = value; }
        }

        string m_appPubKey;
        /// <summary>
        /// 支付宝app公钥
        /// </summary>
        public string AppPubKey
        {
            get { return m_appPubKey; }
            set { m_appPubKey = value; }
        }

        string m_userName;
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string UserName
        {
            get { return m_userName; }
            set { m_userName = value; }
        }
        string m_password;
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        string m_defaultHandler;
        /// <summary>
        /// 默认消息处理类
        /// </summary>
        public string DefaultHandler
        {
            get { return m_defaultHandler; }
            set { m_defaultHandler = value; }
        }

        long m_replyTemplateId;
        /// <summary>
        /// 回复模板ＩＤ
        /// </summary>
        public long ReplyTemplateId
        {
            get { return m_replyTemplateId; }
            set { m_replyTemplateId = value; }
        }

        long m_menuTemplateId;
        /// <summary>
        /// 菜单模板ＩＤ
        /// </summary>
        public long MenuTemplateId
        {
            get { return m_menuTemplateId; }
            set { m_menuTemplateId = value; }
        }
        int m_accountType;
        /// <summary>
        /// 账号类型
        /// </summary>
        public int AccountType
        {
            get { return m_accountType; }
            set { m_accountType = value; }
        }
        /// <summary>
        /// 是否异步处理消息
        /// </summary>
        public bool IsAsyn
        {
            get
            {
                if (m_platform == PlatformType.Weixin && m_accountType == 0)
                {
                    return true;
                }
                if (m_platform == PlatformType.Yixin) //out of limit 应该和易信认证有关 粉丝必须大于500才可
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region 访问Token管理

        string m_accessToken;
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken
        {
            get { return m_accessToken; }
            set { m_accessToken = value; }
        }
        DateTime m_accessTokenTime;
        /// <summary>
        /// 访问Token获取时间（过期时间更具不同账户类型确定）
        /// </summary>
        public DateTime AccessTokenTime
        {
            get { return m_accessTokenTime; }
            set { m_accessTokenTime = value; }
        }

        /// <summary>
        /// 从数据库读取AccessToken
        /// </summary>
        public void GetAccessTokenFromDatabase()
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _sql = "SELECT [access_token],[access_token_time] FROM [dbo].[Platform_Account] WHERE [account_id]=" + m_accountId;
                    DataTable _table = helper.SqlGetDataTable(_sql);
                    if (_table.Rows.Count == 1)
                    {
                        m_accessToken = _table.Rows[0]["access_token"].ToString();
                        DateTime.TryParse(_table.Rows[0]["access_token_time"].ToString(), out m_accessTokenTime);
                    }
                }
                this.LogInfo("从数据库读取AccessToken");
            }
            catch (Exception ex)
            {
                this.LogError("从数据库读取AccessToken错误：", ex);
            }
        }
        /// <summary>
        /// 更新AccessToken到数据库
        /// </summary>
        public void UpdateAccessTokenToDatabase()
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _sql = @"UPDATE [dbo].[Platform_Account] SET [access_token] = @access_token,[access_token_time] = @access_token_time WHERE [account_id]=@account_id";
                    helper.SpFileValue["@access_token"] = m_accessToken;
                    helper.SpFileValue["@access_token_time"] = m_accessTokenTime;
                    helper.SpFileValue["@account_id"] = m_accountId;
                    helper.SqlExecute(_sql, helper.SpFileValue);
                }
                this.LogInfo("更新AccessToken到数据库");
            }
            catch (Exception ex)
            {
                this.LogError("更新AccessToken到数据库错误", ex);
            }
        }

        /// <summary>
        /// 更改账号状态
        /// </summary>
        /// <param name="status"></param>
        public void ChangeAccountStart(int status, string remark)
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _sql = @"UPDATE [dbo].[Platform_Account] SET [status]=@status,[remark]=@remark WHERE [account_id]=@account_id";
                    helper.SpFileValue["@status"] = status;
                    helper.SpFileValue["@remark"] = remark;
                    helper.SpFileValue["@account_id"] = m_accountId;
                    helper.SqlExecute(_sql, helper.SpFileValue);
                }
                this.LogInfo("更改账号【" + m_accountId + "】状态到数据库");
            }
            catch (Exception ex)
            {
                this.LogError("更改账号【" + m_accountId + "】状态到数据库错误：" + ex.Message, ex);
            }
        }

        #endregion

        #region 微信支付商户属性

        private string m_mchId;
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId
        {
            get { return m_mchId; }
            set { m_mchId = value; }
        }

        private string m_mchSecret;
        /// <summary>
        /// 商户平台的API密钥
        /// </summary>
        public string MchSecret
        {
            get { return m_mchSecret; }
            set { m_mchSecret = value; }
        }

        private byte[] m_cert;
        /// <summary>
        /// 微信api证书
        /// </summary>
        public byte[] Cert
        {
            get { return m_cert; }
            set { m_cert = value; }
        }

        private string m_cert_name;
        /// <summary>
        /// 微信api证书名称
        /// </summary>
        public string CertName
        {
            get { return m_cert_name; }
            set { m_cert_name = value; }
        }
        #endregion

        #region 获取账户Api操作接口

        ApiHelper m_apiHelper;
        /// <summary>
        /// 获取账户Api操作接口
        /// </summary>
        public ApiHelper ApiHelper
        {
            get
            {
                if (m_apiHelper == null)
                {
                    switch (m_platform)
                    {
                        case PlatformType.Default:
                            break;
                        case PlatformType.Weixin:
                            m_apiHelper = new WeixinApiHelper(this);
                            break;
                        case PlatformType.Yixin:
                           // m_apiHelper = new YixinApiHelper(this);
                            break;
                        case PlatformType.Laiwan:
                            //m_apiHelper = new LaiwangApiHelper(this);
                            break;
                        case PlatformType.Feixin:
                            break;
                        case PlatformType.Alipay:
                            break;
                        case PlatformType.Weibo:
                            //m_apiHelper = new WeiboApiHelper(this);
                            break;
                        case PlatformType.TQQ:
                            //m_apiHelper = new TQQApiHelper(this);
                            break;
                        case PlatformType.MiniProgram:
                            m_apiHelper = new WeixinApiHelper(this);
                            break;
                        default:
                            break;
                    }
                }
                return m_apiHelper;
            }
        }

        #endregion

        #region 构造函数

        public AccountInfo()
        {

        }

        #endregion

        #region 获取处理程序

        static Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"weixin","Iwenli.Mobile.Msgx.DefaultHandler"},
            {"yixin","Iwenli.Mobile.Msgx.YiXinHandler"},
            {"laiwang","Iwenli.Mobile.Msgx.LaiWangHandler"}
        };

        /// <summary>
        /// 获取消息处理类
        /// </summary>
        /// <returns></returns>
        public string GetMessageHandler(string key)
        {
            if (string.IsNullOrEmpty(m_defaultHandler))
            {
                return dictionary[key];
            }
            return m_defaultHandler;
        }

        #endregion

        #region 群发消息

        public void SendMessage(Platform.ResMsg message, out int okCount, out int errorCount)
        {
            SendMessage(message, -1, -1, "", out okCount, out errorCount);
        }

        /// <summary>
        /// 群发消息，24小时内的用户
        /// </summary>
        /// <param name="message"></param>
        /// <param name="groupId">全部用户，请使用-1</param>
        /// <param name="okCount"></param>
        /// <param name="errorCount"></param>
        public void SendMessage(Platform.ResMsg message, int sex, int groupId, string userList, out int okCount, out int errorCount)
        {
            okCount = 0;
            errorCount = 0;
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _errorInfo;
                    string _sql = "SELECT [openid] FROM [dbo].[View_V0_Service_MrLee_Message] WHERE [status]=1 AND [account_id]=" + AccountId + " AND dateadd(day,1,addtime)>GETDATE() ";
                    if (groupId > 0)
                    {
                        _sql += " AND [group_id]=" + groupId;
                    }
                    if (sex > 0)
                    {
                        _sql += " AND [sex]=" + sex;
                    }
                    if (!string.IsNullOrEmpty(userList))
                    {
                        _sql += " AND [user_id] in (" + userList + ")";
                    }
                    {
                        _sql += " GROUP BY [openid]";
                    }

                    foreach (DataRow item in helper.SqlGetDataTable(_sql).Rows)
                    {
                        message.ToUserName = item[0].ToString();
                        if (ApiHelper.SendMessage(3, message, out _errorInfo))
                        {
                            okCount++;
                        }
                        else
                        {
                            errorCount++;
                            this.LogInfo("消息群发错误：" + _errorInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 工厂方法

        static Hashtable m_accountList = new Hashtable();

        /// <summary>
        /// 更具账号ID获取账户信息, 从缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AccountInfo GetAccountInfoByIdFromCache(long id)
        {
            return GetAccountInfoByIdFromDatabase(id);

            //AccountInfo _account = m_accountList[id] as AccountInfo;
            //if (_account == null)
            //{
            //    _account = GetAccountInfoByIdFromDatabase(id);
            //    if (_account != null)
            //    {
            //        m_accountList[id] = _account;
            //    }
            //}
            //return _account;
        }

        /// <summary>
        /// 更具账号ID获取账户信息, 从数据库
        /// </summary>
        /// <param name="id"></param> 
        /// <returns></returns>
        static AccountInfo GetAccountInfoByIdFromDatabase(long id)
        {
            List<AccountInfo> _list = Iwenli.Data.Entity.DataEntityHelper.GetDataRowEntityList<AccountInfo>("IwenliMobile", "View_V0_Service_MrLee_AccountInfo", "　AND [AccountId]=" + id);
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }

        #endregion

        #region 获取账户信息
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static AccountInfo GetAccountInfoByWhere(string where)
        {
            List<AccountInfo> _list = Iwenli.Data.Entity.DataEntityHelper.GetDataRowEntityList<AccountInfo>("IwenliMobile", "View_V0_Service_MrLee_AccountInfo", where);
            if (_list.Count == 1)
            {
                return _list[0];
            }
            return null;
        }
        #endregion
    }

    /// <summary>
    /// 账号状态
    /// </summary>
    public enum AccountStatus : int
    {
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0,
        /// <summary>
        /// 开启
        /// </summary>
        Open = 1,
        /// <summary>
        /// 授权异常
        /// </summary>
        Expire = 2,
        /// <summary>
        /// 账户错误
        /// </summary>
        Error = 3
    }
}
