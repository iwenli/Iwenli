using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Msgx
{
    /// <summary>
    /// 来往消息处理类
    /// </summary>
    public class LaiWangHandler : MessageHandler
    {
        public override Platform.ReqMsg GetRequestMessage()
        {
            throw new NotImplementedException();
        }

        protected override Platform.ResMsg[] GetDefaultResponseMessage(Platform.ReqMsg message)
        {
            throw new NotImplementedException();
        }
    }
}
