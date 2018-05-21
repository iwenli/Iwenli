#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：XmlExtensionMethods
 *  所属项目：System
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 16:00:53
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace System
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XmlExtensionMethods
    {
        /// <summary>
        /// 获得对应名称属性的值
        /// </summary>
        /// <param name="node">当前的节点</param>
        /// <param name="attName">属性名</param>
        /// <returns>属性值</returns>
        public static string GetAttributeValue(this XmlNode node, string attName)
        {
            if (node == null)
                return null;

            return node.Attributes[attName].SelectValue(s => s.Value);
        }
    }
}
