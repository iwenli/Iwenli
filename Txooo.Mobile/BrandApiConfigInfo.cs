using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Txooo.Data.Entity;

namespace Txooo.Mobile
{
    /// <summary>
    /// 品牌API配置信息
    /// </summary>
    public class BrandApiConfigInfo : DataRowEntityBase
    {
        #region 属性

        long m_configId;
        /// <summary>
        /// 配置ID
        /// </summary>
        public long ConfigId
        {
            get { return m_configId; }
            set { m_configId = value; }
        }
        long m_brandId;
        /// <summary>
        /// 品牌ID
        /// </summary>
        public long BrandId
        {
            get { return m_brandId; }
            set { m_brandId = value; }
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
        string m_username;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        string m_password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        string m_appId;
        /// <summary>
        /// 开发者凭据 API ID
        /// </summary>
        public string AppId
        {
            get { return m_appId; }
            set { m_appId = value; }
        }
        string m_appSecret;
        /// <summary>
        /// 开发者凭据 App Secret
        /// </summary>
        public string AppSecret
        {
            get { return m_appSecret; }
            set { m_appSecret = value; }
        }
        string m_txAppToken;
        /// <summary>
        /// 数据接收接口Token
        /// </summary>
        public string TxAppToken
        {
            get { return m_txAppToken; }
            set { m_txAppToken = value; }
        }
        string m_txAppUrl;
        /// <summary>
        /// 数据接收接口地址
        /// </summary>
        public string TxAppUrl
        {
            get { return m_txAppUrl; }
            set { m_txAppUrl = value; }
        }

        #endregion
        
        #region 工厂方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static List<BrandApiConfigInfo> GetAllBrandApiConfigInfo(long brandId)
        {
            List<BrandApiConfigInfo> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<BrandApiConfigInfo>("TxoooMobile", "View_V1_PlatformApiConfigInfo_MrLee", " AND BrandId=" + brandId);
            return _list;
        }

        /// <summary>
        /// 获取品牌API配置数据
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static BrandApiConfigInfo GetBrandApiConfigInfo(long brandId,PlatformType platform)
        {
            List<BrandApiConfigInfo> _list = Txooo.Data.Entity.DataEntityHelper.GetDataRowEntityList<BrandApiConfigInfo>("TxoooMobile", "View_V1_PlatformApiConfigInfo_MrLee", " AND [platform]=" + (int)platform + " AND BrandId=" + brandId);
            if (_list.Count > 0)
            {
                return _list[0];
            }
            return null;
        }

        #endregion
    }
}
