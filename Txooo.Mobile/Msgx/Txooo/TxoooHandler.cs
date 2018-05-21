using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Msgx
{
    public class TxoooHandler : MessageHandler
    {
        protected override Platform.ResMsg HandlerTextMessage(Platform.ReqTextMag message)
        {
            return new CmdHelper().GetReturnInfo(message);
        }

        #region 扫描事件

        /// <summary>
        /// 用户已关注时的事件推送
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override Platform.ResMsg HandlerScanEvent(Platform.ReqEventMsg message)
        {
            int _id;
            int.TryParse(message.EventKey, out _id);
            if (_id > 0)
            {
                return InviteInfo.GetInviteInfoById(_id).GetResMsg(message);
            }
            return base.HandlerScanEvent(message);
        }

        /// <summary>
        /// 如果用户还未关注公众号，则用户可以关注公众号，关注后微信会将带场景值关注事件推送给开发者。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override Platform.ResMsg HandlerScanSubscribeEvent(Platform.ReqEventMsg message)
        {
            int _id;
            int.TryParse(message.EventKey.Replace("qrscene_", ""), out _id);
            if (_id > 0)
            {
                return InviteInfo.GetInviteInfoById(_id).GetResMsg(message);
            }
            return base.HandlerScanSubscribeEvent(message);
        }

        #endregion
    }
}
