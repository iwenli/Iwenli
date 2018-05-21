using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace System
{
    /// <summary>
    /// 常用工具类  暂未分类
    /// </summary>
    public class ToolHelper
    {
        /// <summary>
        /// 校验逗号分隔字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CheckStrSplit(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            int _str = 0;
            List<string> _list = new List<string>();
            var _idArray = str.Split(new char[] { ',', '，', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _idArray.Length; i++)
            {
                if (int.TryParse(_idArray[i], out _str))
                {
                    _list.Add(_idArray[i]);
                }
            }
            return string.Join(",", _list);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="CookieValue"></param>
        /// <returns></returns>
        public static string ConvertObjectToString(object info)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, info);
            byte[] result = new byte[ms.Length];
            result = ms.ToArray();
            string temp = System.Convert.ToBase64String(result);
            ms.Flush();
            ms.Close();
            return temp;
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static byte[] ConvertObjectToByte(object info)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, info);
            byte[] result = new byte[ms.Length];
            result = ms.ToArray();
            ms.Flush();
            ms.Close();
            return result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertStringToObject(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            byte[] b = System.Convert.FromBase64String(value);
            MemoryStream ms = new MemoryStream(b, 0, b.Length);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object ConvertByteToObject(byte[] buffer)
        {
            if (buffer.Length == 0)
                return null;
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }

        public static bool IsMobile(string userAgent = null)
        {
            userAgent = userAgent ?? HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od|ad)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回对象json字符串，带有错误代码
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="errcode"></param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        public static string Json(bool success, object msg, int errcode, dynamic otherInfo = null)
        {
            JObject _newJson = new JObject();
            _newJson["success"] = success;
            _newJson["errcode"] = errcode;
            _newJson["msg"] = msg.ToString();
            if (otherInfo != null)
            {
                Type _type = otherInfo.GetType();
                var _prop = _type.GetProperties();
                foreach (var _p in _prop)
                {
                    _newJson[_p.Name] = _p.GetValue(otherInfo);
                }
            }
            return _newJson.ToString();
        }
        /// <summary>
        /// 返回数组json字符串，接口访问自动转为对象json
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static string Json(bool success, object msg, params string[] other)
        {
            var p = string.Empty;
            if (other.Length > 0)
            {
                p = "," + string.Join(",", other);
            }
            var result = string.Format("{{\"success\":{0},\"msg\":\"{1}\"{2}}}", success.ToString().ToLower(), msg, p);
            return result;
        }


        /// <summary>
        /// 数据序列化
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SerializeObject(object list, Formatting format = Formatting.Indented)
        {
            var timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
            return JsonConvert.SerializeObject(list, format, timeFormat);
        }

        #region 地图两点间距离
        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 获得地图两点间距离
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        #endregion
    }
}
