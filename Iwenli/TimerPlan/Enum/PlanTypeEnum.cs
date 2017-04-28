using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.TimerPlan.Enum
{
    /// <summary>
    /// 任务计划枚举 重复执行或者执行一次
    /// </summary>
    public enum PlanTypeEnum
    {
        /// <summary>
        /// 重复执行
        /// </summary>
        Repeat = 0,
        /// <summary>
        /// 执行一次
        /// </summary>
        One
    }
}
