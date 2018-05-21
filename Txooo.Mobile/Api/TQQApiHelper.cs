using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 腾讯微博API
    /// </summary>
    public class TQQApiHelper : ApiHelper
    {
        private AccountInfo m_accountInfo;

        public TQQApiHelper(AccountInfo info)
        {
            // TODO: Complete member initialization
            this.m_accountInfo = info;
        }

        public override bool SendMessage(Platform.ResMsg msg, out string errorInfo)
        {
            errorInfo = string.Empty;
            try
            {
                if (msg.MsgType == ResMsgType.Text)
                {
                    HttpTools req = new HttpTools("https://open.t.qq.com/api/t/add");
                    req.Parameters.Add("appid", "801449036");
                    req.Parameters.Add("openid", m_accountInfo.AppId);
                    req.Parameters.Add("openkey", m_accountInfo.AppSecret);
                    if (HttpContext.Current != null)
                    {
                        req.Parameters.Add("clientip", HttpContext.Current.Request.UserHostAddress);
                    }
                    else
                    {
                        req.Parameters.Add("clientip", "127.0.0.1");
                    }
                    req.Parameters.Add("reqtime", DateTime.Now.ToString());
                    req.Parameters.Add("wbversion", "1");

                    //check sign error
                    string _oauth_signature = HttpTools.HMACSHA1Signature(
                    "2acc8c1f0948932696b09c4c2889551e" + "&", "POST", "/t/add", req.Parameters);

                    req.Parameters.Add("format", "json");
                    req.Parameters.Add("content", ((ResTextMsg)msg).Content);
                    //parameters.Add("longitude", "");
                    //parameters.Add("latitude", "");
                    //parameters.Add("syncflag", "0");
                    req.Parameters.Add("sig", _oauth_signature);

                    errorInfo = req.Post();
                }
                else if (msg.MsgType == ResMsgType.News)
                {
                    HttpTools req = new HttpTools("https://open.t.qq.com/api/t/add_pic");
                    req.Parameters.Add("appid", "801449036");
                    req.Parameters.Add("openid", m_accountInfo.AppId);
                    req.Parameters.Add("openkey", m_accountInfo.AppSecret);
                    if (HttpContext.Current != null)
                    {
                        req.Parameters.Add("clientip", HttpContext.Current.Request.UserHostAddress);
                    }
                    else
                    {
                        req.Parameters.Add("clientip", "127.0.0.1");
                    }
                    req.Parameters.Add("reqtime", DateTime.Now.ToString());
                    req.Parameters.Add("wbversion", "1");

                    string _oauth_signature = HttpTools.HMACSHA1Signature(
                    "2acc8c1f0948932696b09c4c2889551e" + "&", "POST", "/t/add_pic", req.Parameters);

                    req.Parameters.Add("format", "json");
                    req.Parameters.Add("content", ((ResNewsMsg)msg).Articles[0].Title);
                    req.Parameters.Add("sig", _oauth_signature);

                    Files files = new Files();
                    files.Add("pic", new UploadFile(((ResNewsMsg)msg).Articles[0].PicUrl));

                    errorInfo = req.PostFile(files);
                }
                else if (msg.MsgType == ResMsgType.Image)
                {
                    HttpTools req = new HttpTools("https://open.t.qq.com/api/t/add_pic");
                    req.Parameters.Add("appid", "801449036");
                    req.Parameters.Add("openid", m_accountInfo.AppId);
                    req.Parameters.Add("openkey", m_accountInfo.AppSecret);
                    if (HttpContext.Current != null)
                    {
                        req.Parameters.Add("clientip", HttpContext.Current.Request.UserHostAddress);
                    }
                    else
                    {
                        req.Parameters.Add("clientip", "127.0.0.1");
                    }
                    req.Parameters.Add("reqtime", DateTime.Now.ToString());
                    req.Parameters.Add("wbversion", "1");

                    string _oauth_signature = HttpTools.HMACSHA1Signature(
                    "2acc8c1f0948932696b09c4c2889551e" + "&", "POST", "/t/add_pic", req.Parameters);

                    req.Parameters.Add("format", "json");
                    req.Parameters.Add("content", "分享图片：");//((ResNewsMsg)msg).Articles[0].Title
                    req.Parameters.Add("sig", _oauth_signature);

                    Files files = new Files();
                    files.Add("pic", new UploadFile(((ResImageMsg)msg).LocalUrl));

                    errorInfo = req.PostFile(files);
                }

                var jsonObj = JsonMapper.ToObject(errorInfo);
                return jsonObj["errcode"].ToString() == "0";
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public override bool GetAllUserList(out List<string> userList, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool GetUserInfo(string openId, ref UserInfo userInfo, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteMenu(out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool PostMenu(Platform.MenuInfo[] menu, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool GetLimitQrcode(int id, out string ticket, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool GetTempQrcode(int sceneId, int expireSeconds, out string ticket, out string errorInfo)
        {
            throw new NotImplementedException();
        }     

        public override bool GetUserGroup(out List<UserGroupInfo> userGroup, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool CreateGroup(UserGroupInfo groupInfo, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool ModifyGroupname(UserGroupInfo groupInfo, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool MoveUserToGroup(string openId, int toGroupID, out string errorInfo)
        {
            throw new NotImplementedException();
        }


        public override bool UploadMedia(ResMsg resMsg, out string returnInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByOpenId(string openIds, ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByGroup(long groupId, ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool GetAccessTokenByRemote(out string returnInfo)
        {
            throw new NotImplementedException();
        }
    }
}
