using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 新浪微博API
    /// </summary>
    public class WeiboApiHelper : ApiHelper
    {
        private AccountInfo m_accountInfo;

        const string APPKEY = "3621462661";
        const string APPSECRET = "89115de51d0315595f198749d0abcf97";

        public WeiboApiHelper(AccountInfo info)
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
                    HttpTools req = new HttpTools("https://api.weibo.com/2/statuses/update.json");
                    req.Parameters.Add("access_token", m_accountInfo.AccessToken);
                    req.Parameters.Add("status", ((ResTextMsg)msg).Content);

                    errorInfo = req.Post();
                }
                else if (msg.MsgType == ResMsgType.News)
                {
                    HttpTools req = new HttpTools("https://upload.api.weibo.com/2/statuses/upload.json");
                    req.Parameters.Add("access_token", m_accountInfo.AccessToken);
                    req.Parameters.Add("status", ((ResNewsMsg) msg).Articles[0].Title);

                    Files files = new Files();
                    files.Add("pic", new UploadFile(((ResNewsMsg) msg).Articles[0].PicUrl));
                    errorInfo = req.PostFile(files);
                }
                else if (msg.MsgType == ResMsgType.Image)
                {
                    HttpTools req = new HttpTools("https://upload.api.weibo.com/2/statuses/upload.json");
                    req.Parameters.Add("access_token", m_accountInfo.AccessToken);
                    req.Parameters.Add("status", "分享图片：");//((ResImageMsg)msg).Articles[0].Title

                    Files files = new Files();
                    files.Add("pic", new UploadFile(((ResImageMsg)msg).LocalUrl));
                    errorInfo = req.PostFile(files);
                }

                return errorInfo.IndexOf("error_code") == -1;
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
