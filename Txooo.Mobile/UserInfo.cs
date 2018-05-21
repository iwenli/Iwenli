using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    /// <summary>
    /// 第三方平台用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 归属平台信息
        /// </summary>
        public PlatformType Platform
        {
            set;
            get;
        }
        /// <summary>
        /// 普通用户的标识，对当前公众号唯一
        /// </summary>
        public string OpenId
        {
            set;
            get;
        }

        public long AccountId
        {
            set;
            get;
        }
        public long UserMapId
        {
            set;
            get;
        }

        /// <summary>
        /// 订阅状态
        /// </summary>
        public bool Status
        {
            set;
            get;
        }
        /// <summary>
        /// 分组ID
        /// </summary>
        public long GroupId
        {
            set;
            get;
        }

        public string Remark
        {
            set;
            get;
        }

        public UserInfo()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openId"></param>
        public UserInfo(PlatformType type, string openId)
        {
            Platform = type;
            OpenId = openId;
        }

        #region 用户基本属性

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public string Subscribe
        {
            set;
            get;
        }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string Nickname
        {
            set;
            get;
        }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string Sex
        {
            set;
            get;
        }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country
        {
            set;
            get;
        }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province
        {
            set;
            get;
        }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City
        {
            set;
            get;
        }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language
        {
            set;
            get;
        }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0", 
        /// </summary>
        public string HeadimgUrl
        {
            set;
            get;
        }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string SubscribeTime
        {
            set;
            get;
        }

        #endregion

        #region 记录消息数据

        /// <summary>
        /// 记录用户请求消息
        /// </summary>
        /// <param name="reqmsg"></param>
        public void NoteUserReqInfo(Platform.ReqMsg reqmsg)
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    //string _jsonStr = JsonMapper.ToJson(reqmsg);
                    JavaScriptSerializer jscvt = new JavaScriptSerializer();
                    string _jsonStr = jscvt.Serialize(reqmsg);

                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = UserMapId;
                    helper.SpFileValue["@is_send"] = 0;
                    helper.SpFileValue["@message_type"] = (int)reqmsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;

                    //冗余字段方便查询
                    helper.SpFileValue["@account_id"] = this.AccountId;
                    helper.SpFileValue["@nickname"] = this.Nickname;
                    helper.SpFileValue["@platform"] = this.Platform;

                    helper.SpGetReturnValue("SP_Service_MrLee_InsertMessage");
                }
                this.TxLogInfo("记录用户请求消息成功");
            }
            catch (Exception ex)
            {
                this.TxLogError("记录用户请求消息错误：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 记录签到用户的请求消息
        /// </summary>
        /// <param name="reqMsg"></param>
        public void NoteSignUserReqInfo(ReqMsg reqMsg, int isSin, string openid, int ischeck = 1)
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    JavaScriptSerializer jscvt = new JavaScriptSerializer();
                    string _jsonStr = jscvt.Serialize(reqMsg);

                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = UserMapId;
                    helper.SpFileValue["@is_send"] = 0;
                    helper.SpFileValue["@message_type"] = (int)reqMsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;

                    //冗余字段方便查询
                    helper.SpFileValue["@account_id"] = this.AccountId;
                    helper.SpFileValue["@nickname"] = this.Nickname;
                    helper.SpFileValue["@platform"] = this.Platform;
                    helper.SpFileValue["@issign"] = isSin;
                    helper.SpFileValue["@ischeck"] = ischeck;

                    helper.SpFileValue["@openid"] = openid;


                    helper.SpGetReturnValue("SP_Service_GT_InsertSignMessage");
                }
                this.TxLogInfo("记录签到消息成功");
            }
            catch (Exception ex)
            {
                this.TxLogError("记录签到消息错误：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 记录向用户发送的消息
        /// </summary>
        /// <param name="resmsg"></param>
        public void NoteUserResMsg(Platform.ResMsg resmsg)
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    //string _jsonStr = LitJson.JsonMapper.ToJson(resmsg);
                    JavaScriptSerializer jscvt = new JavaScriptSerializer();
                    string _jsonStr = jscvt.Serialize(resmsg);


                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = UserMapId;
                    helper.SpFileValue["@is_send"] = 1;
                    helper.SpFileValue["@message_type"] = (int)resmsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;

                    //冗余字段方便查询
                    helper.SpFileValue["@account_id"] = this.AccountId;
                    helper.SpFileValue["@nickname"] = this.Nickname;
                    helper.SpFileValue["@platform"] = this.Platform;

                    helper.SpFileValue["@restype"] = resmsg.ResType;

                    helper.SpGetReturnValue("SP_Service_MrLee_InsertMessage");
                }
                this.TxLogInfo("记录向用户发送的消息成功");
            }
            catch (Exception ex)
            {
                this.TxLogError("记录向用户发送的消息错误：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resmsg"></param>
        /// <param name="messageId">请求信息id</param>
        public void NoteUserResMsg(Platform.ResMsg resmsg, long messageId)
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    //string _jsonStr = LitJson.JsonMapper.ToJson(resmsg);
                    JavaScriptSerializer jscvt = new JavaScriptSerializer();
                    string _jsonStr = jscvt.Serialize(resmsg);


                    //获取用户ID
                    helper.SpFileValue["@user_map_id"] = UserMapId;
                    helper.SpFileValue["@is_send"] = 1;
                    helper.SpFileValue["@message_type"] = (int)resmsg.MsgType;
                    helper.SpFileValue["@json_countent"] = _jsonStr;

                    //冗余字段方便查询
                    helper.SpFileValue["@account_id"] = this.AccountId;
                    helper.SpFileValue["@nickname"] = this.Nickname;
                    helper.SpFileValue["@platform"] = this.Platform;

                    helper.SpFileValue["@restype"] = resmsg.ResType;
                    helper.SpFileValue["@replyid"] = messageId;

                    helper.SpGetReturnValue("SP_Service_GT_InsertMessageOfReqMsgId");
                }
                this.TxLogInfo("记录向用户发送的消息成功");
            }
            catch (Exception ex)
            {
                this.TxLogError("记录向用户发送的消息错误：" + ex.Message, ex);
            }
        }

        #endregion

        #region 关注与取消关于


        /// <summary>
        /// 关注公众账号
        /// </summary>
        /// <returns></returns>
        public long EnSubscribe(ReqEventMsg reqMsg)
        {
            long _userMapId = 0;
            try
            {
                int _qrCode = string.IsNullOrEmpty(reqMsg.EventKey) ? 0 : int.Parse(reqMsg.EventKey);
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    //获取用户ID
                    string _sql = "UPDATE [dbo].[Platform_AccountUserMap] SET [qr_code]=" + _qrCode + ",[subscribe_time]=getdate(),[status] = 1 WHERE [user_map_id]=" + UserMapId;
                    helper.SqlExecute(_sql);

                    UpdateScanTotalCount(_qrCode);
                }

            }
            catch (Exception ex)
            {
                this.TxLogInfo("更新usermap信息错误：" + ex.Message);
            }
            return _userMapId;
        }

        /// <summary>
        /// 更新二维码被扫描的总次数
        /// </summary>
        /// <param name="qrCode"></param>
        public void UpdateScanTotalCount(int qrCode)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                string updateScanCountSql =
                        "update [dbo].[Platform_Account_QrCode] set scan_count=scan_count+1 where qr_number=" + qrCode;
                helper.SqlExecute(updateScanCountSql);
            }
        }

        /// <summary>
        /// 更新最新一次的扫描时间
        /// </summary>
        public void UpdateLastScanTime()
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                string sql =
                    "update [TxoooMobile].[dbo].[Platform_AccountUserMap] set last_scan_time=getdate() where user_map_id=" +
                    UserMapId;
                helper.SqlExecute(sql);
            }
        }


        /// <summary>
        /// 取消关注公众账号
        /// </summary>
        public void UnSubscribe()
        {
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    //获取用户ID
                    string _sql = "UPDATE [dbo].[Platform_AccountUserMap] SET [unsubscribe_time] = getdate(),[status] = 0 WHERE [user_map_id]=" + UserMapId;
                    helper.SqlExecute(_sql);
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

        /// <summary>
        /// 获取用户信息，根据用户id
        /// </summary>
        /// <param name="user_map_id"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfoByUserMapId(long user_map_id)
        {
            UserInfo _info = new UserInfo();
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string _sql = "SELECT * FROM [dbo].[View_V0_Server_MrLee_UserInfo] WHERE [user_map_id]=" + user_map_id;

                    DataTable _dt = helper.SqlGetDataTable(_sql);
                    if (_dt.Rows.Count == 1)
                    {
                        _info.Platform = (PlatformType)int.Parse(_dt.Rows[0]["platform"].ToString());
                        _info.AccountId = long.Parse(_dt.Rows[0]["account_id"].ToString());
                        _info.UserMapId = long.Parse(_dt.Rows[0]["user_map_id"].ToString());
                        _info.Status = bool.Parse(_dt.Rows[0]["status"].ToString());
                        _info.GroupId = long.Parse(_dt.Rows[0]["group_id"].ToString());

                        _info.OpenId = _dt.Rows[0]["openid"].ToString();
                        _info.Nickname = _dt.Rows[0]["nickname"].ToString();
                        _info.Sex = _dt.Rows[0]["sex"].ToString();
                        _info.City = _dt.Rows[0]["city"].ToString();
                        _info.Country = _dt.Rows[0]["country"].ToString();
                        _info.Province = _dt.Rows[0]["province"].ToString();
                        _info.Language = _dt.Rows[0]["language"].ToString();
                        _info.HeadimgUrl = _dt.Rows[0]["headimgurl"].ToString();
                        _info.Remark = _dt.Rows[0]["remark"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _info;
        }

        /// <summary>
        /// 获取用户信息，根据openid
        /// </summary>
        /// <param name="user_map_id"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfoByOpenId(string openId)
        {
            UserInfo _info = new UserInfo();
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string _sql = "SELECT * FROM [dbo].[View_V0_Server_MrLee_UserInfo] WHERE [openid]='" + openId + "'";

                    DataTable _dt = helper.SqlGetDataTable(_sql);
                    if (_dt.Rows.Count == 1)
                    {
                        _info.Platform = (PlatformType)int.Parse(_dt.Rows[0]["platform"].ToString());
                        _info.AccountId = long.Parse(_dt.Rows[0]["account_id"].ToString());
                        _info.UserMapId = long.Parse(_dt.Rows[0]["user_map_id"].ToString());
                        _info.Status = bool.Parse(_dt.Rows[0]["status"].ToString());
                        _info.GroupId = long.Parse(_dt.Rows[0]["group_id"].ToString());

                        _info.OpenId = _dt.Rows[0]["openid"].ToString();
                        _info.Nickname = _dt.Rows[0]["nickname"].ToString();
                        _info.Sex = _dt.Rows[0]["sex"].ToString();
                        _info.City = _dt.Rows[0]["city"].ToString();
                        _info.Country = _dt.Rows[0]["country"].ToString();
                        _info.Province = _dt.Rows[0]["province"].ToString();
                        _info.Language = _dt.Rows[0]["language"].ToString();
                        _info.HeadimgUrl = _dt.Rows[0]["headimgurl"].ToString();
                        _info.Remark = _dt.Rows[0]["remark"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _info;
        }

        /// <summary>
        /// 获取用户MapId
        /// </summary>
        /// <param name="account"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static long GetUserMapId(AccountInfo account, string openId)
        {
            long _userMapId = 0;
            try
            {
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    helper.SpFileValue["@account_id"] = account.AccountId;
                    helper.SpFileValue["@platform"] = (int)account.Platform;
                    helper.SpFileValue["@openid"] = openId;
                    _userMapId = helper.SpGetReturnValue("SP_Service_MrLee_GetUserMapId");
                }
            }
            catch (Exception ex)
            {
            }
            return _userMapId;
        }

        #endregion

        #region 更新用户最后一次坐标
        /// <summary>
        /// 更新用户最后一次地理位置信息
        /// </summary>
        /// <param name="openId"></param>
        public static void UpdateUserLocation(ReqEventMsg message)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                //获取用户ID
                string _sql = "UPDATE [dbo].[Platform_User] SET [longitude]=@longitude,[latitude] = @latitude,[last_location_time]=getdate() WHERE [openid]=@openid";
                helper.SpFileValue["@longitude"] = message.Longitude;
                helper.SpFileValue["@latitude"] = message.Latitude;
                helper.SpFileValue["@openid"] = message.FromUserName;
                helper.SqlExecute(_sql, helper.SpFileValue);
            }
        }
        #endregion
    }
}
