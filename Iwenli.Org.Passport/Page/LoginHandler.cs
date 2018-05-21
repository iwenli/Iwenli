///*----------------------------------------------------------------
// *  Copyright (C) 2017 天下商机（txooo.com）版权所有
// * 
// *  文 件 名：LoginHandler
// *  所属项目：
// *  创建用户：张玉龙(HouWeiya)
// *  创建时间：2018/4/11 15:03:37
// *  
// *  功能描述：
// *          1、
// *          2、 
// * 
// *  修改标识：  
// *  修改描述：
// *  待 完 善：
// *          1、 
//----------------------------------------------------------------*/

//using Iwenli.Web.Htmx;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Iwenli.Org.Passport.Page
//{
//    public class LoginHandler : HtmxHandler1
//    {
//        public override void LoadMainTemplateBegin()
//        {
//            #region 返回地址
//            string _domainCode = "0";
//            string _returnUrl = string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) ? SiteConfig.WebUrl : Request.QueryString["ReturnUrl"];
//            var _math = SiteConfig.DomainRegex.Match(_returnUrl);
//            if (_math.Success) { _domainCode = _math.Groups[2].Value; }

//            AddTempVariable("ReturnUrl", _returnUrl);

//            #endregion

//            #region 已登陆状态则返回
//            //var _txUser = HttpContext.Current.User.Identity as TxUserIdentity;
//            //if (_txUser != null)
//            //{
//            //    Response.Redirect(_returnUrl);
//            //    Response.End();
//            //}
//            #endregion

//            #region 产品推广类型
//            if (!string.IsNullOrEmpty(_returnUrl) && _returnUrl.IndexOf("/shop/") > -1)
//            {
//                TxCookie.SaveCookie("ShareSource", "2", 1);
//            }
//            string _shareSource = TxCookie.GetCookie("ShareSource");
//            AddTempVariable("ShareSource", _shareSource == "" ? "1" : _shareSource);

//            #endregion

//            #region 邀请码

//            _domainCode = _domainCode == "0" ? "" : _domainCode;
//            AddTempVariable("ShareCode", string.IsNullOrEmpty(Request.QueryString["sharecode"]) ? _domainCode : Request.QueryString["sharecode"]);

//            #endregion

//            #region 微信登陆
//            if (Context.Request.UserAgent.ToLower().Contains("micromessenger"))
//            {
//                Response.Redirect("/wxlogin.html?ReturnUrl=" + Uri.EscapeDataString(_returnUrl));
//                Response.End();
//            }
//            #endregion

//            base.LoadMainTemplateBegin();
//        }
//    }
//}
