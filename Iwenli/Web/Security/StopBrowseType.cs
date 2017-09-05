using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web.Security
{
    /// <summary>
    /// 终止请求浏览类型
    /// </summary>
    public enum StopBrowseType : int
    {
        /// <summary>
        /// 重定向到登录页<LoginUrl>
        /// </summary>
        Redirect = 0,
        /// <summary>
        /// 终止请求，返回空
        /// </summary>
        Stop = 1,
        /// <summary>
        /// 抛出异常
        /// </summary>
        Exception = 2,
        /// <summary>
        /// 输出信息<StopInfo>
        /// </summary>
        Info = 3
    }
}
