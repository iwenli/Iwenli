using Iwenli.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Iwenli
{
    /// <summary>
    /// 文本字符串处理
    /// </summary>
    public static class StringHelper
    {
        #region 获得两个字符串中间的字符串

        /// <summary>
        ///  获得两个字符串中间的字符串
        /// </summary>
        /// <param name="original">原始字符</param>
        /// <param name="startStr">开始字符</param>
        /// <param name="endStr">结束字符</param>
        /// <returns></returns>
        public static string GetMiddleStr(string original, string startStr, string endStr)
        {
            return GetMiddleStr(original, startStr, endStr, 0);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="original">原始字符串</param>
        /// <param name="startStr">开始字符串</param>
        /// <param name="endStr">结束字符串</param>
        /// <param name="include">是否包含前后字符串，0：不包含，1：包含前，2：包含后，3：包含前后</param>
        /// <returns></returns>
        public static string GetMiddleStr(string original, string startStr, string endStr, int include)
        {
            string _str = "";

            int s = original.IndexOf(startStr);
            if (s < 0)
            {
                return "";
            }
            original = original.Substring(s + startStr.Length);
            int e = original.IndexOf(endStr);
            if (e >= 0)
            {
                _str = original.Substring(0, e);
                switch (include)
                {
                    case 1:
                        _str = startStr + _str;
                        break;
                    case 2:
                        _str = _str + endStr;
                        break;
                    case 3:
                        _str = startStr + _str + endStr;
                        break;
                    default:
                        break;
                }
            }
            return _str;
        }

        #endregion

        #region 验证身份证号码

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard(string Id)
        {
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (Id.Length == 18)
            {
                long n = 0;
                if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false;//数字验证
                }
                if (address.IndexOf(Id.Remove(2)) == -1)
                {
                    return false;//省份验证
                }

                string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = Id.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                {
                    return false;//校验码验证
                }
                return true;//符合GB11643-1999标准

            }

            if (Id.Length == 15)
            {
                long n = 0;
                if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                {
                    return false;//数字验证
                }
                if (address.IndexOf(Id.Remove(2)) == -1)
                {
                    return false;//省份验证
                }
                string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证
                }
                return true;//符合15位身份证标准
            }

            return false;
        }

        #endregion

        #region 验证Ip地址

        /// <summary>
        /// 是否IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string str)
        {
            if (str == null || str == string.Empty || str.Length < 7 || str.Length > 15)
                return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str);
        }

        #endregion

        #region 是否是数字

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 是否是中文

        /// <summary>
        /// 判断是否是中文
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static bool IsChina(string CString)
        {
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;
                }
                else
                {
                    BoolValue = true;
                }
            }
            return BoolValue;
        }

        #endregion

        #region 是否手机

        /// <summary>
        /// 校验手机号码是否符合标准。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return false;

            return Regex.IsMatch(mobile, @"^(13|14|15|16|17|18|19)\d{9}$");
        }

        #endregion

        #region 生成验证码

        /// <summary>
        /// 创建随机字符
        /// </summary>
        /// <param name="length">创建随机字符</param>
        /// <returns></returns>
        public static string CreateRandomStr(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }


        #endregion

        #region 字符串字节数

        /// <summary>
        /// 字符串所占字节数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetByteLenght(this string str)
        {
            byte[] bytestr = System.Text.Encoding.Unicode.GetBytes(str);
            int j = 0;
            for (int i = 0; i < bytestr.GetLength(0); i++)
            {
                if (i % 2 == 0)
                {
                    j++;
                }
                else
                {
                    if (bytestr[i] > 0)
                    {
                        j++;
                    }
                }
            }
            return j;
        }

        #endregion

        #region 比较2个字符串相识度

        /// <summary>
        /// 比较2个字符的显示度，返回100内的整数（显示度50%返回50）【移位算法】
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int GetSemblance(string str1, string str2)
        {
            LevenshteinDistance _ld = new LevenshteinDistance(str1, str2);
            _ld.Compute();
            return (int)(double.Parse(_ld.ComputeResult.Rate) * 100);
        }

        #endregion

        #region unicode转中文

        /// <summary>
        /// unicode转中文
        /// </summary>
        /// <param name="unicodeStr"></param>
        /// <returns></returns>
        private static string Unicode2CHr(string unicodeStr)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(unicodeStr, "\\\\u([\\w]{4})");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    unicodeStr = unicodeStr.Replace(v, Encoding.Unicode.GetString(codes));
                }
            }
            return unicodeStr;
        }

        #endregion
    }
}
