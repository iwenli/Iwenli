#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：AES
 *  所属项目：Iwenli.Text
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/6/22 13:38:54
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
    /// AES加密解密
    /// </summary>
    class AES
    {
        public AES(string _key, string _iv)
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
                    m_key = key.MD5();
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
                    m_iv = iv.MD5().Substring(0, 16);
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
        /// <param name="decryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public string AESDecrypt(string decryptStr)
        { 
            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                byte[] bKey = Encoding.Default.GetBytes(Key);
                byte[] bIV = Encoding.Default.GetBytes(IV);
                byte[] byteArray = decryptStr.ConvertBase64ToBytes();

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
