using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web
{
    /// <summary>
    /// 页面保存方法
    /// </summary>
    public enum PageSaveType
    {
        /// <summary>
        /// 没有配置
        /// </summary>
        Null,
        /// <summary>
        /// 静态化处理页面
        /// </summary>
        Save,
        /// <summary>
        /// 不带参数，缓存页面
        /// </summary>
        Cache,
        /// <summary>
        /// 不做处理
        /// </summary>
        None
    }
}
