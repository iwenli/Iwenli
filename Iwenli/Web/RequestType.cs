using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web
{
    /// <summary>
    /// 请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Htm页面
        /// </summary>
        HTM,
        /// <summary>
        /// 图片
        /// </summary>
        PIC,
        /// <summary>
        /// 样式文件
        /// </summary>
        CSS,
        /// <summary>
        /// JS脚本文件
        /// </summary>
        JS,
        /// <summary>
        /// Falsh文件
        /// </summary>
        Falsh,
        /// <summary>
        /// ASPX页面
        /// </summary>
        ASPX,
        /// <summary>
        /// 其他文件
        /// </summary>
        Other,
        /// <summary>
        /// Wap手机页面
        /// </summary>
        Wap
    }
}
