using Iwenli.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.Passport.BLL
{
    class UserBLL
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public string Login(string userName, string passWord)
        {
            //更新缓存
            //SalesUserCache.UpdateCache(userId, "update");
            //var _userInfo = SalesUserCache.GetUser(userId);
            //if (_userInfo.IsLock)
            //{
            //    ToolHelper.Json(false, "您的账户存在违规操作，已被屏蔽，请联系管理员解除");
            //}
            //UserLogHelper.WriteUserLog("sales_log_passport", userId.ToString(), "web登入创业赚钱", 1, "TxoooAgent");
            Iwenli.Web.Security.SecurityHelper.SetAuthCookie(string.Format("{0}|{1}|{2}|{3}", userName, 10163, 0, 1), true);
            
            return JsonHelper.Json(true, "登陆成功");
        }
    }
}
