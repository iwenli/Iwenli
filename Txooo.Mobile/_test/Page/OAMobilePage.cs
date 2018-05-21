using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Page
{
    public class OAMobilePage : Txooo.Web.Htmx.HtmxHandler1
    { 
        string m_platformUserSn;
        PlatformType m_platform;

        OA.OAUserInfo m_userInfo;

        public override void LoadMainTemplateBegin()
        {            
            base.ParsePageBegin();
            m_platform = PlatformType.Weixin;
            m_platformUserSn = Request["platform_user_sn"];
            if (string.IsNullOrEmpty(m_platformUserSn)
                || !Enum.TryParse<PlatformType>(Request["platform"], out m_platform)
                || !PlatformUserInfo.DecryptPlatformUserSn(m_platform, Request["platform_user_sn"], ref m_platformUserSn)
                )
            {
                Response.Write("错误");
                Response.End();
            }
            else
            {

                m_userInfo = OA.OAUserInfo.GetUserInfoByPlatformUserSn(m_platform, m_platformUserSn);
                if (m_userInfo == null)
                {
                    Response.Redirect("/UserBinding.htm?type=oa&platform=" + (int)m_platform + "&platform_user_sn=" + m_platformUserSn);
                }
                else
                {
                    AddTempVariable("platform_user_sn", m_platformUserSn);
                    AddTempVariable("platform", m_platform.ToString());
                    AddTempVariable("username", m_userInfo.Username.ToString());
                }
            }
        }
    }
}
