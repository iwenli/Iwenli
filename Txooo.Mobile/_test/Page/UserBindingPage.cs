using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Page
{
    /// <summary>
    /// 用户账号绑定页面
    /// </summary>
    public class UserBindingPage : Txooo.Web.Htmx.HtmxHandler1
    {
        protected bool m_isBinding;

        string m_type;
        string m_platformUserSn;
        PlatformType m_platform;

        public override void ParsePageBegin()
        {            
            m_type = Request["type"];
            m_platformUserSn = Request["platform_user_sn"];
            m_platform = PlatformType.Weixin;
            if (string.IsNullOrEmpty(m_type)
                || string.IsNullOrEmpty(m_platformUserSn)
                || !Enum.TryParse<PlatformType>(Request["platform"], out m_platform)
                || !PlatformUserInfo.DecryptPlatformUserSn(m_platform, Request["platform_user_sn"], ref m_platformUserSn)
                )
            {
                Response.Write("错误");
                Response.End();
            }
            else
            {
                if (m_type=="oa")
                {
                    if (OA.OAUserInfo.GetUserInfoByPlatformUserSn(m_platform, m_platformUserSn) != null)
                    {
                        Response.Redirect("/OA/Help.html?platform=" + (int)PlatformType.Weixin + "&platform_user_sn=" + PlatformUserInfo.EncryptPlatformUserSn(m_platform, m_platformUserSn));
                    }
                }
                if (m_type == "web")
                {
                    if (OA.WebUserInfo.GetUserInfoByUserId(m_platform, m_platformUserSn) != null)
                    {
                        Response.Redirect("/Web/Help.html");
                    }
                }
                AddTempVariable("type", m_type);
                AddTempVariable("platform", m_platform.ToString());
                AddTempVariable("platform_user_sn", PlatformUserInfo.EncryptPlatformUserSn(m_platform, m_platformUserSn));
            }
            base.ParsePageBegin();
        }
    }
}
