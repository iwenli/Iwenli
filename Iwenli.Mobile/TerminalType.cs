#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：TerminalType
 *  所属项目：Iwenli.Mobile
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 15:59:19
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile
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
