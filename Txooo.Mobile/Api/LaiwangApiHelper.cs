using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 来往接口
    /// </summary>
    public class LaiwangApiHelper:ApiHelper
    {
        private AccountInfo accountInfo;

        public LaiwangApiHelper(AccountInfo accountInfo)
        {
            // TODO: Complete member initialization
            this.accountInfo = accountInfo;
        }
        public override bool GetAccessTokenByRemote(out string returnInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendMessage(Platform.ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
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

        public override bool UploadMedia(Platform.ResMsg resMsg, out string returnInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByOpenId(string openIds, Platform.ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }

        public override bool SendAllByGroup(long groupId, Platform.ResMsg msg, out string errorInfo)
        {
            throw new NotImplementedException();
        }
    }
}
