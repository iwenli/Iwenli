using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 终端类型
    /// </summary>
    public enum TerminalType : int
    {
        Pc = 0,
        /// <summary>
        /// Wap
        /// </summary>
        Wap = 1,
        /// <summary>
        /// 彩屏
        /// </summary>
        Color = 2,
        /// <summary>
        /// 触屏
        /// </summary>
        Touch = 3,
        /// <summary>
        /// 平板
        /// </summary>
        Pad = 4,
        /// <summary>
        /// 电视
        /// </summary>
        Tv = 5
    }
}
