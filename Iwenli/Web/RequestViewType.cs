using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web
{
    /// <summary>
    /// 页面请求视图，由页面后缀名决定
    /// </summary>
    public enum RequestViewType
    {
        /// <summary>
        /// 发布（正常）试图
        /// </summary>
        Put = 0,
        /// <summary>
        /// 编辑视图,[.htme]
        /// </summary>
        Edit = 1,
        /// <summary>
        /// 更新页面,[.htmx]
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除页面,[.htmd]
        /// </summary>
        Delete = 3,
        /// <summary>
        /// 保存视图,[.htms]
        /// </summary>
        Save = 4,
        /// <summary>
        /// 清空,[.htmn]
        /// </summary>
        Null = 5,
        /// <summary>
        /// 缓存页面，[.htmc]
        /// </summary>
        Cache = 6
    }
}
