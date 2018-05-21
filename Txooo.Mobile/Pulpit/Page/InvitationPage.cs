using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Pulpit.Page
{
    /// <summary>
    /// 邀请函
    /// </summary>
    class InvitationPage : Txooo.Web.Htmx.HtmxHandler1
    {
        public override void ParsePageBegin()
        {
            InviteInfo _info = new InviteInfo(Request["c"]);

            Txooo.Mobile.Weixin.WeixinHelper _helper = new Txooo.Mobile.Weixin.WeixinHelper("wx17b0bfd50aa4117b", "89c6eab9b6243fd229b28baa03e6ab6f");
            string _ticket, _errorInfo;

            if (_helper.GetTempQrcode(_info.GetQrcodeId(), 1800, out _ticket, out _errorInfo))
            {
                AddTempVariable("ticket", _ticket);
            }
        }
    }
}
