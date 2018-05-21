using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Pulpit.Page
{
    /// <summary>
    /// 请柬页面
    /// </summary>
    public class CardPage : Txooo.Web.Htmx.HtmxHandler1
    {
        string m_platformUserSn;
        PlatformType m_platform;

        public override void ParsePageBegin()
        {
            m_platformUserSn = Request["platform_user_sn"];
            m_platform = PlatformType.Weixin;
            if (string.IsNullOrEmpty(m_platformUserSn)
                || !Enum.TryParse<PlatformType>(Request["platform"], out m_platform)
                || !PlatformUserInfo.DecryptPlatformUserSn(m_platform, Request["platform_user_sn"], ref m_platformUserSn)
                )
            {
                Response.Redirect("/Error.html");
            }
            else
            {
                ReceiptInfo _receipt = ReceiptInfo.GetReceiptInfoByOpenId(m_platform, m_platformUserSn);
                if (_receipt != null)
                {
                    //查询用户是否参加会
                    AddTempVariable("name", _receipt.Name);
                    AddTempVariable("day", (DateTime.Parse("2013-12-10") - DateTime.Now).Days.ToString());
                }
                else
                {
                    Response.Redirect("/Error.html");
                }
            }
        }
    }
}
