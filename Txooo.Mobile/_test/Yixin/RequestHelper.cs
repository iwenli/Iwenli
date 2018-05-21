using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Yixin
{
    class RequestHelper
    {
        public string GetResponseMessage(System.Web.HttpContext context)
        {
            #region 不能修改 echostr — 微信应用提交验证

            string echostr = context.Request.QueryString["echostr"];
            if (!string.IsNullOrEmpty(echostr))
            {
                return echostr;
            }

            #endregion

            return "";
        }
    }
}
