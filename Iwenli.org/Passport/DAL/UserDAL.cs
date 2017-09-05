using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Org.Passport.DAL
{
    class UserDAL
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static int Regist(string userName, string passWord)
        {
            //using (DataHelper helper = DataHelper.GetDataHelper("Iwenli"))
            //{
            //    //helper.SpFileValue["@user_name"] = userInfo.UserName;
            //    //helper.SpFileValue["@password"] = userInfo.Password;
            //    //helper.SpFileValue["@pay_password"] = userInfo.Password;
            //    //helper.SpFileValue["@security_phone"] = userInfo.UserName;
            //    //helper.SpFileValue["@show_phone"] = userInfo.ShowPhone;
            //    //helper.SpFileValue["@nick_name"] = userInfo.NickName;
            //    //helper.SpFileValue["@source"] = userInfo.Source;
            //    //helper.SpFileValue["@source_channel"] = userInfo.SourceChannel;
            //    //helper.SpFileValue["@reg_ip"] = userInfo.RegIp;
            //    //helper.SpFileValue["@recommend_share_code"] = userInfo.RecommendShareCode;
            //    //helper.SpFileValue["@app_key"] = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            //    //helper.SpFileValue["@user_type"] = userInfo.UserType;

            //    return helper.SpGetReturnValue("SP_V1_MFC_Sales_RegistUser");
            //}
            return 1;
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public static int Login(string userName, string passWord)
        {
            //using (DataHelper helper = DataHelper.GetDataHelper("Iwenli"))
            //{
            //    helper.SpFileValue["@user_name"] = userName;
            //    helper.SpFileValue["@password"] = passWord;
            //    return helper.SpGetReturnValue("SP_V1_MFC_Sales_LoginUser");
            //}
            return 1;
        }
    }
}
