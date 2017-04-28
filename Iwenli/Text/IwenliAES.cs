/*----------------------------------------------------------------
 *  Copyright (C) 2017 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：IwenliAES
 *  所属项目：
 *  创建用户：张玉龙
 *  创建时间：2017/4/25 15:22:51
 *  
 *  功能描述：
 *          1、
 *          2、
 * 
 *  修改标识：
 *  修改描述：
 *  待 完 善：
 *          1、
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Text
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    class IwenliAES
    {
        public IwenliAES(string _key, string _iv)
        {
            key = _key;
            iv = _iv;
        }

        #region AES

        private string key;
        private string m_key;
        private string Key
        {
            get
            {
                if (string.IsNullOrEmpty(m_key))
                {
                    m_key = EncryptHelper.MD5(key);
                }
                return m_key;
            }
        }

        private string iv;
        private string m_iv;
        private string IV
        {
            get
            {
                if (string.IsNullOrEmpty(m_iv))
                {
                    m_iv = EncryptHelper.MD5(iv).Substring(0, 16);
                }
                return m_iv;
            }
        }


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public string AESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.Default.GetBytes(Key);
            byte[] bIV = Encoding.Default.GetBytes(IV);
            byte[] byteArray = Encoding.Default.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.Default.GetBytes(Key);
            byte[] bIV = Encoding.Default.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.Default.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        #endregion
    }
}
