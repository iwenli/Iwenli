using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 第三方平台用户信息
    /// </summary>
    public class PlatformUserInfo
    {
        #region 第三方平台SN处理

        /// <summary>
        /// 加密用户SN
        /// </summary>
        /// <param name="type"></param>
        /// <param name="platformUserSn"></param>
        /// <returns></returns>
        public static string EncryptPlatformUserSn(PlatformType type, string platformUserSn)
        {
            return Txooo.Text.EncryptHelper.AESEncrypt(platformUserSn);
        }

        /// <summary>
        /// 解密用户ＳＮ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="platformUserSn"></param>
        /// <returns></returns>
        public static bool DecryptPlatformUserSn(PlatformType type, string platformUserSn,ref string sn)
        {
            try
            {
                sn = Txooo.Text.EncryptHelper.AESDecrypt(platformUserSn);
                if (!string.IsNullOrEmpty(sn))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }       

        #endregion
        
        public static long GetPlatformUserId(PlatformType type,string platformUserSn)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooPassport"))
            {
                helper.SpFileValue["@PlatformUserSn"] = platformUserSn;
                helper.SpFileValue["@PlatformUserType"] = (int)type;

                return helper.SpGetReturnValue("SP_V1_Mobile_GetPublicPlatformUserId");
            }
            return 0;
        }
    }
}
