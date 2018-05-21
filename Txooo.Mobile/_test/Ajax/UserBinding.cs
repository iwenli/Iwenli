using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Ajax
{
    public class UserBinding : Txooo.Web.Ajax.AjaxHandler
    {
        public string Info()
        {
            return "1.1";
        }

        #region 获取验证码

        public string GetCode()
        {
            bool _start = false;
            string _info = "";

            string _type = Request["type"];
            PlatformType _platform = PlatformType.Weixin;
            string _platformUserSn = Request["platform_user_sn"];
            string _mobile = Request["mobile"];
            if (string.IsNullOrEmpty(_type)
                || string.IsNullOrEmpty(_platformUserSn)
                || !Enum.TryParse<PlatformType>(Request["platform"], out _platform)
                || !PlatformUserInfo.DecryptPlatformUserSn(_platform, Request["platform_user_sn"], ref _platformUserSn)
                )
            {
                _start = false;
                _info = "参数错误";
            }
            else
            {
                if (!Txooo.TxStringHelper.IsMobile(_mobile))
                {
                    _start = false;
                    _info = "请填写正确的手机号码！";
                }
                else
                {
                    if (_type == "oa")
                    {

                        if (OA.OAUserInfo.GetUserInfoByPlatformUserSn(_platform, _platformUserSn) != null)
                        {
                            _start = false;
                            _info = "此微信号已经绑定账号！";
                        }
                        else if (OA.OAUserInfo.GetUserInfoByMobile(_mobile) == null)
                        {
                            _start = false;
                            _info = "不存在此手机号码，请确定你的安全手机号！";
                        }
                        else
                        {
                            string _code = new Random().Next(100000, 999999).ToString();
                            Context.Session[_mobile + "_oa_code"] = _code;
                            //发送验证码
                            SysSmsService.SysSmsServiceSoapClient _c = new SysSmsService.SysSmsServiceSoapClient();
                            _c.SendOfficeSms(_mobile, "你的账号绑定验证码为：" + _code);
                            _start = true;
                        }
                    }
                    else if (_type == "web")
                    {
                        //if (UserHelper.WebUserIfBinding(_platform, _platformUserSn, _mobile))
                        //{
                        //    _start = false;
                        //    _info = "此手机号码已经绑定账号！";
                        //}
                        //else
                        //{
                        //    //发送验证码
                        //    //SysSmsService.SysSmsServiceSoapClient _c = new SysSmsService.SysSmsServiceSoapClient();
                        //    //_c.SendOfficeSms(_mobile, "微信测试验证码");
                        //    _start = true;
                        //}
                    }
                    else
                    {
                        _start = false;
                        _info = "账户类型错误！";
                    }
                }
            }
            return "{'start':" + _start.ToString().ToLower() + ",'info':'" + _info + "'}";
        }
        #endregion

        #region 绑定账号

        public string BindingUser()
        {
            bool _start = false;
            string _info = "";

            string _type = Request["type"];
            string _platformUserSn = Request["platform_user_sn"];
            PlatformType _platform = PlatformType.Weixin;
            string _mobile = Request["mobile"];
            string _code = Request["code"];
            if (string.IsNullOrEmpty(_type)
                || string.IsNullOrEmpty(_platformUserSn)
                || string.IsNullOrEmpty(_code)
                || !Enum.TryParse<PlatformType>(Request["platform"], out _platform)
                || !PlatformUserInfo.DecryptPlatformUserSn(_platform, Request["platform_user_sn"], ref _platformUserSn)
                )
            {
                _start = false;
                _info = "参数错误";
            }
            else
            {
                if (_type == "oa")
                {
                    if (Context.Session[_mobile + "_oa_code"] != null
                        && Context.Session[_mobile + "_oa_code"].ToString() == _code)
                    {
                        _start = OA.OAUserInfo.OAUserBinding(_platform, _platformUserSn, _mobile);
                        _info = "账户绑定成功，你可以使用相关服务了！";
                    }
                    else
                    {
                        _start = false;
                        _info = "验证码错误";
                    }
                }
                else
                {

                }
            }
            return "{'start':" + _start.ToString().ToLower() + ",'info':'" + _info + "'}";
        }

        #endregion
    }
}
