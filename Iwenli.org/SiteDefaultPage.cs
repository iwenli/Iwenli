#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：SiteDefaultPage
 *  所属项目：Iwenli.Org
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/31 11:10:19
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using Iwenli.Web.Authorization;
using Iwenli.Web.Htmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Org
{
    public class SiteDefaultPage : HtmxHandler1
    {
        public int m_domainCode = 0;
        public override void InitPage(HttpContext context, Web.Url url, Web.IWeb web, Web.ISite site, Web.ISkin skin, Web.Page page)
        {
            base.InitPage(context, url, web, site, skin, page);
        }

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        protected UserIdentity UserIdentity
        {
            get { return HttpContext.Current.User.Identity as UserIdentity; }
        }


        public override void LoadMainTemplateBegin()
        {
            base.LoadMainTemplateBegin();

            if (UserIdentity != null)
            {
                //UserRemainHelper.WriteRemain(new UserRemainInfo() { UserId = UserIdentity.UserID, AppType = (Request.UserAgent.ToLower().IndexOf("txooo.app zhuan") > -1 ? AppType.PersonAPP : AppType.PersonWap), RunIp = Txooo.Web.TxWebUtility.GetIP() });
                ////缓存中没有找到当前登陆用户，用户锁定状态
                //var _userInfo = SalesUserCache.GetUser(UserIdentity.UserID);
                //if (_userInfo == null || _userInfo.IsLock == true)
                //{
                //    Txooo.Web.Security.TxSecurityHelper.SignOut();
                //    TxCookie.ClearCookie("ShareCode");
                //    TxCookie.ClearCookie("ThirdUser");
                //    if (_userInfo.IsLock == true)
                //    {
                //        Response.Write("<script>alert('您的账户存在违规操作，已被屏蔽，请联系管理员解除');window.location.href='/index.html';</script>");
                //        Response.End();
                //    }
                //}
            }
        }

        public override void ParsePage()
        {
            AddTempVariable("SYS_SERVICE_TIME", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

            #region 通用参数解析
            foreach (var item in Request.QueryString)
            {
                if (item != null)
                {
                    AddTempVariable("U_QUERY_" + item.ToString().ToUpper(), Request.QueryString[item.ToString()]);
                }
            }
            #endregion

            #region 登陆用户信息
            AddTempVariable("IsAuth", "false");
            int _userId = int.Parse(Request.QueryString["shareuid"] ?? "0");//用户分享id
            if (UserIdentity != null)
            {
                AddTempVariable("IsAuth", "true");
                //用户分享id没有时赋值当前用户id
                _userId = UserIdentity.UserID;
            }
            //var _userList = SalesUserCache.GetUser(_userId);
            //if (_userList == null)
            //{
            //    _userList = new UserInfoV2();
            //}
            //AddTempVariable("UserId", _userList.UserId.ToString());
            //AddTempVariable("Username", _userList.ShowPhone);
            //AddTempVariable("UserSecurityPhone", _userList.SecurityPhone);
            //string _loginPage = TxSecurityConfig.Instance.LoginUrl + (TxSecurityConfig.Instance.LoginUrl.IndexOf("?") > -1 ? "&" : "?") + "ReturnUrl=";
            //AddTempVariable("ConfigLoginPageUrl", _loginPage);
            //AddTempVariable("ConfigWebUrl", SiteConfig.WebUrl);
            //AddTempVariable("ConfigPassportUrl", SiteConfig.PassportUrl);
            //AddTempVariable("ConfigPayWebUrl", SiteConfig.PayWebUrl);
            //AddTempVariable("ConfigOfficialWebUrl", SiteConfig.OfficialWebUrl);
            //AddTempVariable("ConfigHostDomain", SiteConfig.SettingConfig.HostDomain);
            #endregion

            #region 用户推广解析

            ////来源方式（判断是否提示关注公众号）
            //if (!string.IsNullOrEmpty(Request.QueryString["z"]))
            //{
            //    TxCookie.SaveCookie("z_type", Request.QueryString["z"] ?? "", 1);
            //}
            //AddTempVariable("z_source_type", Request.QueryString["z"] ?? "");

            #endregion

            #region 用户分享公共解析

            ////分享用户信息
            //AddTempVariable("UserNickName", _userList.NickName);
            //AddTempVariable("UserHeadPic", _userList.HeadPic);
            //AddTempVariable("UserAddTime", _userList.AddTime.ToString());

            ////分享内容
            //var _data = UserThirdCache.UserThirdDb;
            //var _r = new Random();

            //int _sign = _r.Next(0, _data.Rows.Count);
            //string _title = Request.QueryString["title"]?.Replace("\n", "").Replace("\r", "");
            //string _desc = Request.QueryString["desc"]?.Replace("\n", "").Replace("\r", "");
            //string _imgurl = Request.QueryString["imgurl"]?.Replace("\n", "").Replace("\r", "");

            //#region 客户端 title 和 desc base64编码发送，这里进行解码(先解URLDecode 再还原Base64)  
            ////APP端UrlEncdoe 编码会将+转换成%2B，而.net2+会将将%2B解码成空格  所以要替换一下
            //try
            //{
            //    if (!string.IsNullOrEmpty(_title))
            //    {
            //        _title = Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(_title).Replace(" ", "+")));
            //    }
            //    if (!string.IsNullOrEmpty(_desc))
            //    {
            //        _desc = Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(_desc).Replace(" ", "+")));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    TxLogHelper.TxLogError("SiteDefaultPage_ShareUrlAnalysis", ex.Message, ex);
            //}

            //#endregion

            //AddTempVariable("Thirdtitle", string.IsNullOrEmpty(_title) ? _data.Rows[_sign]["title"].ToString().Replace("##", _userList.NickName) : _title.Substring(0, _title.Length > 30 ? 30 : _title.Length));
            //AddTempVariable("Thirdcontent_info", string.IsNullOrEmpty(_desc) ? _data.Rows[_sign]["content_info"].ToString().Replace("##", _userList.NickName) : _desc.Substring(0, _desc.Length > 30 ? 30 : _desc.Length));
            //AddTempVariable("Thirdimg_url", string.IsNullOrEmpty(_imgurl) ? _data.Rows[_sign]["img_url"].ToString() : _imgurl);

            #endregion

            base.ParsePage();
        }
    }
}
