using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Pulpit.Page
{
    public class PulpitPage : Txooo.Web.Htmx.HtmxHandler1
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
                //查询用户是否参加会
                ReceiptInfo _receipt = ReceiptInfo.GetReceiptInfoByOpenId(m_platform, m_platformUserSn);
                if (_receipt == null)
                {
                    AddTempVariable("platform", m_platform.ToString());
                    AddTempVariable("platform_user_sn", PlatformUserInfo.EncryptPlatformUserSn(m_platform, m_platformUserSn));

                    int _inviteId;
                    if (int.TryParse(Request["rqcode"], out _inviteId))
                    {
                        InviteInfo _info = InviteInfo.GetInviteInfoById(_inviteId);

                        AddTempVariable("employee_id", _info.EmployeeId.ToString());
                        AddTempVariable("employee_name", _info.EmployeeName);
                        AddTempVariable("brand_id", _info.BrandId.ToString());
                        AddTempVariable("brand_name", _info.BrandName);
                        AddTempVariable("post", _info.Position);
                        AddTempVariable("name", _info.Name);
                        AddTempVariable("mobile", _info.Mobile);
                    }
                }
                else
                {
                    Response.Redirect("/Card.htm?platform=" + (int)m_platform + "&platform_user_sn=" + PlatformUserInfo.EncryptPlatformUserSn(m_platform, m_platformUserSn));
                }

            }
        }
    }
}
