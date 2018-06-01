#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：EmallMessage
 *  所属项目：Iwenli.Push.Notify
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/31 17:25:36
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

namespace Iwenli.Push.Notify
{
    /// <summary>
    /// 邮箱消息
    /// </summary>
    public class EmallMessage : MessageBase
    {
        public override long PushId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Temp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool Send()
        {
            throw new NotImplementedException();
        }
    }
}
