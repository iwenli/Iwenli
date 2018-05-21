using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 公众平台类型
    /// </summary>
    public enum PlatformType : int
    {
        /// <summary>
        /// Wap
        /// </summary>
        Txooo = 0,
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
