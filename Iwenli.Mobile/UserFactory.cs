#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：UserFactory
 *  所属项目：Iwenli.Mobile
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/22 15:58:55
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iwenli.Mobile
{
    public class UserFactory
    {

        #region 订阅处理事件

        static Queue<KeyValuePair<AccountInfo, UserInfo>> m_subscribeQueue = new Queue<KeyValuePair<AccountInfo, UserInfo>>();
        static Queue<KeyValuePair<AccountInfo, UserInfo>> m_unSubscribeUserQueue = new Queue<KeyValuePair<AccountInfo, UserInfo>>();

        public static void AddSubscribeUser(AccountInfo account, UserInfo user)
        {
            m_subscribeQueue.Enqueue(new KeyValuePair<AccountInfo, UserInfo>(account, user));
        }

        public static void AddUnSubscribeUser(AccountInfo account, UserInfo user)
        {
            m_unSubscribeUserQueue.Enqueue(new KeyValuePair<AccountInfo, UserInfo>(account, user));
        }

        #endregion

        #region 更新用户信息

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUserInfo(UserInfo user)
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    //获取用户ID
                    helper.SpFileValue["@platform"] = (int)user.Platform;
                    helper.SpFileValue["@openid"] = user.OpenId;
                    helper.SpFileValue["@nickname"] = System.Web.HttpUtility.HtmlEncode(user.Nickname);
                    helper.SpFileValue["@sex"] = user.Sex;
                    helper.SpFileValue["@city"] = user.City;
                    helper.SpFileValue["@country"] = user.Country;
                    helper.SpFileValue["@province"] = user.Province;
                    helper.SpFileValue["@language"] = user.Language;
                    helper.SpFileValue["@headimgurl"] = user.HeadimgUrl;

                    helper.SpGetReturnValue("SP_Service_MrLee_UpdateUserInfo");
                }
                //this.TxLogInfo("获取usermapid成功：" + _userMapId);
            }
            catch (Exception ex)
            {
                //this.TxLogInfo("获取usermapid错误：" + ex.Message);
            }
        }

        #endregion


        #region 记录用户消息

        /// <summary>
        /// 记录用户请求消息
        /// </summary>
        /// <param name="userMapId"></param>
        /// <param name="reqmsg"></param>
        public static void NoteUserReqInfo(long userMapId, Platform.ReqMsg reqmsg)
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _jsonStr = LitJson.JsonMapper.ToJson(reqmsg);
                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = userMapId;
                    helper.SpFileValue["@is_send"] = 0;
                    helper.SpFileValue["@message_type"] = (int)reqmsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;
                    helper.SpGetReturnValue("SP_Service_MrLee_InsertMessage");
                }
                //this.TxLogInfo("获取usermapid成功：" + _userMapId);
            }
            catch (Exception ex)
            {
                //this.TxLogInfo("获取usermapid错误：" + ex.Message);
            }
        }

        /// <summary>
        /// 记录向用户发送的消息
        /// </summary>
        /// <param name="userMapId"></param>
        /// <param name="resmsg"></param>
        public static void NoteUserResMsg(long userMapId, Platform.ResMsg resmsg)
        {
            try
            {
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    string _jsonStr = LitJson.JsonMapper.ToJson(resmsg);

                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = userMapId;
                    helper.SpFileValue["@is_send"] = 1;
                    helper.SpFileValue["@message_type"] = (int)resmsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;
                    helper.SpGetReturnValue("SP_Service_MrLee_InsertMessage");
                }
                //this.TxLogInfo("获取usermapid成功：" + _userMapId);
            }
            catch (Exception ex)
            {
                //this.TxLogInfo("获取usermapid错误：" + ex.Message);
            }
        }

        #endregion


        #region 工厂方法

        static Hashtable m_userInfoList = new Hashtable();
        public static UserInfo GetUserInfo(AccountInfo account, string openId)
        {
            string _key = account.Platform + "_" + account.AccountId + "_" + openId;
            UserInfo _userInfo = m_userInfoList[_key] as UserInfo;
            long _userMapId = 0;
            if (_userInfo == null)
            {
                //获取用户ID
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    helper.SpFileValue["@account_id"] = account.AccountId;
                    helper.SpFileValue["@platform"] = (int)account.Platform;
                    helper.SpFileValue["@openid"] = openId;
                    _userMapId = helper.SpGetReturnValue("SP_Service_MrLee_GetUserMapId");
                }
                //从数据库提取数据
                _userInfo = UserInfo.GetUserInfoByUserMapId(_userMapId);

                m_userInfoList[_key] = _userInfo;


                //if (string.IsNullOrEmpty(_userInfo.Nickname))
                //{
                //    //同步数据
                //    string _errorInfo;
                //    if (account.ApiHelper.GetUserInfo(3, openId, ref _userInfo, out _errorInfo))
                //    {
                //        Txooo.Mobile.UserFactory.UpdateUserInfo(_userInfo);
                //    }
                //}
            }
            else if (_userInfo.UserMapId == 0)
            {
                //获取用户ID
                using (DataHelper helper = DataHelper.GetDataHelper("IwenliMobile"))
                {
                    helper.SpFileValue["@account_id"] = account.AccountId;
                    helper.SpFileValue["@platform"] = (int)account.Platform;
                    helper.SpFileValue["@openid"] = openId;
                    _userMapId = helper.SpGetReturnValue("SP_Service_MrLee_GetUserMapId");
                }
                //从数据库提取数据
                _userInfo = UserInfo.GetUserInfoByUserMapId(_userMapId);

                m_userInfoList[_key] = _userInfo;
            }

            Thread _thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    string _errorInfo;
                    UserInfo _remoteUser = new UserInfo(account.Platform, openId);
                    if (account.ApiHelper.GetUserInfo(3, openId, ref _remoteUser, out _errorInfo))
                    {
                        if (_remoteUser.Nickname != _userInfo.Nickname || _remoteUser.HeadimgUrl != _userInfo.HeadimgUrl)
                        {
                            Iwenli.Mobile.UserFactory.UpdateUserInfo(_remoteUser);
                            m_userInfoList[_key] = _remoteUser;
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.GetLogger("Txooo.Mobile.UserFactory").Error("同步用户数据错误：" + ex.Message);
                }
            }));
            _thread.Start();

            return _userInfo;
        }

        #endregion
    }
}
