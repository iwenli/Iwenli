#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：Request
 *  所属项目：Iwenli.Org.Ajax
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/28 13:59:10
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

using Iwenli.Extension;
using Iwenli.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Org.Ajax
{
    public class OpenRequest : SiteAjaxBase
    {
        /// <summary>
        /// 请求中转 
        /// 参数：
        /// method: get post
        /// url: 请求地址
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Jsonp(HttpContext context)
        {
            //允许所有域请求
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            var response = string.Empty;
            var method = context.Request.HttpMethod;

            var url = context.Request["call"] ?? "";

            if (url.IsNullOrEmpty())
            {
                return JsonHelper.Json(false, "参数[call]不能为空.");
            }

            try
            {
                var param = context.Request.QueryString;
                var http = new HttpTools(HttpUtility.UrlDecode(url));
                if (method.Equals("get", StringComparison.InvariantCultureIgnoreCase))
                {
                    //get
                    response = http.Get();
                }
                else
                {
                    //post
                    param = context.Request.Form;
                    foreach (var key in param.AllKeys)
                    {
                        http.Parameters.Add(key, param[key]);
                    }
                    response = http.Post();
                }
            }
            catch (Exception ex)
            {
                return JsonHelper.Json(false, ex.Message);
            }
            return response.Replace("\r\n", "");
        }
    }
}
