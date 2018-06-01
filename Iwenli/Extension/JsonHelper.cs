#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：Json
 *  所属项目：Iwenli.Extension
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 16:12:13
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Extension
{
    /// <summary>
    /// Json相关操作扩展
    /// </summary>
    public class JsonHelper
    {
        #region 序列化
        /// <summary>
        /// 全局序列化设置
        /// </summary>
        static JsonHelper()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                //空值处理
                setting.NullValueHandling = NullValueHandling.Ignore;

                ////高级用法九中的Bool类型转换 设置
                //setting.Converters.Add(new BoolConvert("是,否"));
                return setting;
            });
        }

        /// <summary>
        /// 序列化对象到json字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// json字符串转为对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// json字符串转为对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }
        #endregion

        #region json返回格式统一
        /// <summary>
        /// 操作失败定义错误码
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static string Json(int code)
        {
            return Json(false, "failed", code);
        }
        /// <summary>
        /// 操作成功或者失败
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static string Json(bool success)
        {
            return Json(success, success ? "success" : "failed");
        }
        /// <summary>
        /// 操作成功返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Json(object data)
        {
            return Json(true, "success", 1, data);
        }

        /// <summary>
        /// 操作成功失败和提示消息
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string Json(bool success, string msg)
        {
            return Json(success, msg, success ? 1 : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Json(bool success, string msg, int code)
        {
            return Json(success, msg, code, null);
        }

        /// <summary>
        /// 完整的json返回
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Json(bool success, string msg, int code, object data)
        {
            var _result = new
            {
                success = success,
                msg = msg,
                code = code,
                data = data
            };
            return SerializeObject(_result);
        }
        #endregion
    }
}
