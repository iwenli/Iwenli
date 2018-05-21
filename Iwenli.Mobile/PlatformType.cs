#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：PlatformType
 *  所属项目：Iwenli.Mobile
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 16:55:04
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
    /// 公众平台类型
    /// </summary>
    public enum PlatformType : int
    {
        /// <summary>
        /// Wap
        /// </summary>
        Default = 0,
        /// <summary>
        /// 微信
        /// </summary>
        Weixin = 1,
        /// <summary>
        /// 易信
        /// </summary>
        Yixin = 2,
        /// <summary>
        /// 来往
        /// </summary>
        Laiwan = 3,
        /// <summary>
        /// 飞信
        /// </summary>
        Feixin = 4,
        /// <summary>
        /// 支付宝
        /// </summary>
        Alipay = 5,
        /// <summary>
        /// 新浪微博
        /// </summary>
        Weibo = 6,
        /// <summary>
        /// 腾讯微博
        /// </summary>
        TQQ = 7,
        /// <summary>
        /// 微信小程序
        /// </summary>
        MiniProgram = 100,
        /// <summary>
        /// 支付宝生活号
        /// </summary>
        AliLive = 101
    }
}
