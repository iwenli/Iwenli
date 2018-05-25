using System;

namespace Iwenli.Web.Security
{
    /// <summary>
    /// 用户票证
    /// </summary>
    public sealed class UserTicket
    {
        #region 票证信息

        /// <summary>
        /// 加密的字符
        /// </summary>
        private string _encryptValue;
        /// <summary>
        /// 解密后的字符
        /// </summary>
        private string _decryptValue;

        /// <summary>
        /// 用户名
        /// </summary>
        private string _username;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
        }

        /// <summary>
        /// 客户最后访问IP
        /// </summary>
        private string _ip;
        /// <summary>
        /// 最后更新IP
        /// </summary>
        public string IP
        {
            get
            {
                return _ip;
            }
        }

        /// <summary>
        /// 客户最后访问时间
        /// </summary>
        private DateTime _time;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return _time;
            }
        }

        /// <summary>
        /// 票证过期的时间
        /// </summary>
        private DateTime _expiration;
        /// <summary>
        /// 票证过期的时间
        /// </summary>
        public DateTime Expiration
        {
            get
            {
                if (_slidingExpiration)
                {//调用过期时间
                    _expiration = DateTime.Now.AddMinutes((double)_timeout);
                }
                return _expiration;
            }
        }

        /// <summary>
        /// 票证是否已过期
        /// </summary>
        public bool Expired
        {
            get
            {
                return DateTime.Now > Expiration;
            }
        }

        private bool _slidingExpiration = false;
        /// <summary>
        /// 指定是否启用可调过期时间。可调过期将 Cookie 的当前身份验证时间重置为在单个会话期间收到每个请求时过期。
        /// </summary>
        public bool SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
        }

        private bool _createPersistentCookie = false;
        /// <summary>
        /// 若要创建持久 Cookie（跨浏览器会话保存的 Cookie），则为 true；否则为 false。
        /// </summary>
        public bool CreatePersistentCookie
        {
            get
            {
                return _createPersistentCookie;
            }
        }
        private int _timeout;
        /// <summary>
        /// 指定 Cookie 过期前逝去的时间（以整数分钟为单位）。
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">票证代码</param>
        public UserTicket(string code)
        {
            _decryptValue = RBACAES.AESDecrypt(code);
            string[] valueArrage = _decryptValue.Split('|');
            _username = RBACAES.AESDecrypt(valueArrage[0]);//用户名
            _ip = valueArrage[1];//最后IP
            _expiration = DateTime.Parse(valueArrage[2]);//过期时间
            _time = DateTime.Parse(valueArrage[3]);//最后更新时间
            _slidingExpiration = bool.Parse(valueArrage[4]);//是否调用过期
            _createPersistentCookie = bool.Parse(valueArrage[5]);//是否调用过期
            _timeout = int.Parse(valueArrage[6]);//过期时间间隔
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="outTime">过期时间</param>
        public UserTicket(string username, int outTime)
        {
            _username = username;
            _timeout = outTime;
            _expiration = DateTime.Now.AddMinutes((double)outTime);
            _ip = WebUtility.GetIP();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="outTime">过期时间</param>
        /// <param name="slidingExpiration">是否启用可调过期时间</param>
        /// <param name="createPersistentCookie">若要创建持久 Cookie（跨浏览器会话保存的 Cookie），则为 true；否则为 false</param>
        public UserTicket(string username, int outTime, bool slidingExpiration, bool createPersistentCookie)
        {
            _username = username;
            _timeout = outTime;
            _expiration = DateTime.Now.AddMinutes((double)outTime);
            _slidingExpiration = slidingExpiration;
            _createPersistentCookie = createPersistentCookie;
            _ip = WebUtility.GetIP();
        }

        /// <summary>
        /// 获得票证代码
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string _value = RBACAES.AESEncrypt(_username) + "|" + _ip + "|" + Expiration.ToString() + "|" + DateTime.Now.ToString() + "|" + _slidingExpiration.ToString() + "|" + _createPersistentCookie.ToString() + "|" + _timeout.ToString();
            return RBACAES.AESEncrypt(_value);
        }

        #region 用户票证加密解密

        /// <summary>
        /// 加密解密类
        /// </summary>
        class RBACAES
        {
            #region AES

            const string key = @")O[N-]6,YF}+efcaj{+oESb9d8>Z'e9M";
            const string iv = @"L+\~f4,I+)b$=pkf";

            private static string _key;
            private static string Key
            {
                get
                {
                    if (string.IsNullOrEmpty(_key))
                    {
                        _key = (key + SecurityConfig.Instance.Key).MD5();
                    }
                    return _key;
                }
            }

            private static string _iv;
            private static string IV
            {
                get
                {
                    if (string.IsNullOrEmpty(_iv))
                    {
                        _iv = (iv + SecurityConfig.Instance.Key).MD5().Substring(0, 16);
                    }
                    return _iv;
                }
            } 
            #endregion

            #region 加密解密方法

            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="plainStr">明文字符串</param>
            /// <returns>密文</returns>
            internal static string AESEncrypt(string plainStr)
            {
                if (string.IsNullOrEmpty(plainStr))
                {
                    return string.Empty;
                }
                string encrypt = plainStr.AESEncrypt(Key, IV);
                if (string.IsNullOrEmpty(encrypt))
                {
                    return string.Empty;
                }
                else
                {
                    return encrypt.Base16Encode();
                }
            }

            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="encryptStr">密文字符串</param>
            /// <returns>明文</returns>
            internal static string AESDecrypt(string decryptStr)
            {
                decryptStr = decryptStr.Base16Decode();
                if (string.IsNullOrEmpty(decryptStr))
                {
                    return string.Empty;
                }
                string decrypt = decryptStr.AESDecrypt(Key, IV);
                if (string.IsNullOrEmpty(decrypt))
                {
                    return string.Empty;
                }
                else
                {
                    return decrypt;
                }
            }

            #endregion
        }

        #endregion
    }
}
