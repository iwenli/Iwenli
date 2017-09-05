using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Text
{
    /// <summary>
    /// 字符加密处理
    /// </summary>
    public static class EncryptHelper
    {
        const string iKey = @")O[N-]6*^NKI,YF}+efc{}JK#JH8>Z'e9M";
        const string iIV = @"L+\~fUF($#:KL*C4,I+)bkf";

        #region Base64编码解码

        /// <summary>
        /// Base64编码（UTF-8编码）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64Encode(this string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
        /// <summary>
        /// Base64解码（UTF-8编码）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64Decode(this string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        #endregion

        #region MD5加密

        /// <summary>
        /// MD5加密函数
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(this string str)
        {
            byte[] bt = System.Text.UTF8Encoding.UTF8.GetBytes(str);//UTF8需要对Text的引用
            System.Security.Cryptography.MD5CryptoServiceProvider objMD5;
            objMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] output = objMD5.ComputeHash(bt);
            return BitConverter.ToString(output).Replace("-", "");
        }
        #endregion

        #region 16进制转化

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBase16(string str)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in Encoding.Default.GetBytes(str))
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base16ToString(string str)
        {
            try
            {
                byte[] inputByteArray = new byte[str.Length / 2];
                for (int x = 0; x < str.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(str.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                return Encoding.Default.GetString(inputByteArray);
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        #endregion

        #region AES加密解密方法

        /// <summary>
        /// 使用默认密钥加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns></returns>
        public static string AESEncrypt(this string plainStr)
        {
            return AESEncrypt(plainStr, iKey, iIV);
        }


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="key">蜜钥</param>
        /// <param name="iv">向量</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(this string plainStr, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainStr))
            {
                return string.Empty;
            }

            AES caes = new AES(key, iv);
            string encrypt = caes.AESEncrypt(plainStr);//加密
            if (string.IsNullOrEmpty(encrypt))
            {
                return string.Empty;
            }
            else
            {
                return StringToBase16(encrypt);//转换
            }
        }

        /// <summary>
        /// 使用默认密钥解密
        /// </summary>
        /// <param name="decryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(this string decryptStr)
        {
            return AESDecrypt(decryptStr, iKey, iIV);
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">密文字符串</param>
        /// <param name="key">蜜钥</param>
        /// <param name="iv">向量</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(this string decryptStr, string key, string iv)
        {
            decryptStr = Base16ToString(decryptStr);//16进制转换
            if (string.IsNullOrEmpty(decryptStr))
            {
                return string.Empty;
            }
            AES caes = new AES(key, iv);
            string decrypt = caes.AESDecrypt(decryptStr);//解密
            if (string.IsNullOrEmpty(decrypt))
            {
                return string.Empty;
            }
            else
            {
                return decrypt;//返回
            }
        }

        #endregion

        #region HMACSHA1 签名

        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HMACSHA1(string key, string data)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(key);

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 获取字符串SHA1加密串，兼容 php sha1()
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(this string str)
        {
            byte[] StrRes = Encoding.Default.GetBytes(str);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        #endregion
    }
}
