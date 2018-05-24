#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：UserGroupInfo
 *  所属项目：Iwenli.Mobile
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 15:58:05
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
    /// 用户分组信息
    /// </summary>
    public class UserGroupInfo
    {
        /// <summary>
        /// 分组ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Groupname { get; set; }

        /// <summary>
        /// 分组包含用户数
        /// </summary>
        public int UserCount { get; set; }
    }
}
