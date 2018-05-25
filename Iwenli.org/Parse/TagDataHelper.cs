#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：TagDataHelper
 *  所属项目：Iwenli.Org.Parse
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 14:47:59
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

using Iwenli.Web.Parse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.Parse
{
    public class TagDataHelper
    {
        public TagEntity Tag { get; set; }
        public Tag pTag { get; set; }

        private int m_totalCount;
        public int TotalCount
        {
            get { return m_totalCount; }
            set { m_totalCount = value; }
        }


        public TagDataHelper(TagEntity tag, Tag pTag)
        {
            this.Tag = tag;
            this.pTag = pTag;
        }

        public DataTable GetDataTable(string methodName)
        {
            MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance
                 | BindingFlags.IgnoreCase
                 | BindingFlags.Public);

            var fun = (Func<DataTable>)method.CreateDelegate(typeof(Func<DataTable>), this);

            return fun();
        }
    }
}
