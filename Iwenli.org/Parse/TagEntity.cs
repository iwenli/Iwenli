#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：TagEntity
 *  所属项目：Iwenli.Org.Parse
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 14:30:27
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

namespace Iwenli.Org.Parse
{
    /// <summary>
    /// 
    /// </summary>
    public class TagEntity
    {
        /// <summary>
        /// 通用不满足时可以指定独立方法命
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 数据库节点名
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// 表、视图、函数名称
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 提取字段
        /// </summary>
        public string Filed { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        public TagEnum DataType { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 输出方式
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// 页面类型 手机 PC
        /// </summary>
        public string PageType { get; set; }
    }
}
