using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Txooo.Mobile.Module.vWall
{
    public class WallUser
    {
        //user_id, open_id, qr_code, add_time

        public int UserId { get; set; }

        public string OpenId { get; set; }

        public int QrCode { get; set; }

        public int AccountId { get; set; }

        public string NickName { get; set; }

        public string HeadImg { get; set; }

        public DateTime AddTime { get; set; }

        public static bool AddWallUser(string openId, int qrCode, Txooo.Mobile.UserInfo userInfo)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrandShop"))
            {
                string sql = "insert into [vwall_user](open_id,qr_code,account_id,nickname,head_img) values(@OpenId,@QrCode,@AccountId,@NickName,@HeadImg)";

                helper.SpFileValue["@OpenId"] = openId;
                helper.SpFileValue["@QrCode"] = qrCode;
                helper.SpFileValue["@AccountId"] = userInfo.AccountId;
                helper.SpFileValue["@NickName"] = userInfo.Nickname;
                helper.SpFileValue["@HeadImg"] = userInfo.HeadimgUrl;

                return helper.SqlExecute(sql, helper.SpFileValue) > 0;
            }
        }

        public static bool IsExistWallUser(string openId, int qrCode)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrandShop"))
            {
                string sql = "select count(0) from [vwall_user] where open_id=@OpenId and qr_code=@QrCode";

                helper.SpFileValue["@OpenId"] = openId;
                helper.SpFileValue["@QrCode"] = qrCode;

                return Convert.ToInt32(helper.SqlScalar(sql, helper.SpFileValue)) > 0;
            }
        }

        public static List<WallUser> GetUserByWhere(string where, Hashtable hashtable)
        {
            List<WallUser> list = new List<WallUser>();
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrandShop"))
            {
                string sql = "select * from [vwall_user] where 1=1 " + where;

                DataRowCollection rows = helper.SqlGetDataTable(sql, hashtable).Rows;

                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        list.Add(new WallUser()
                        {
                            HeadImg = row["head_img"].ToString(),
                            NickName = row["nickname"].ToString(),
                            OpenId = row["open_id"].ToString(),
                            UserId = Convert.ToInt32(row["user_id"]),
                            QrCode = Convert.ToInt32(row["qr_code"]),
                            AccountId = Convert.ToInt32(row["account_id"]),
                            AddTime = DateTime.Parse(row["add_time"].ToString())
                        });
                    }
                }
                return list;
            }
        }

        //记录互动墙用户消息
        public static void NoteUserReqInfo(Platform.ReqMsg reqMsg, UserInfo userInfo, AccountInfo m_account)
        {
            try
            {
                var _now = DateTime.Now;
                var _wallList = WallIndex.GetWallIndexByWhere(" and account_id=@AccountId ", new Hashtable(){{"@AccountId",m_account.AccountId}}).Where((wall) => { return _now >= wall.StartTime && _now <= wall.EndTime; });

                if (_wallList.Count() > 0)
                {
                    foreach (var item in _wallList)
                    {
                        var _wallUser = GetUserByWhere("and account_id=@account_id and qr_code=@qr_code and open_id=@open_id ",
                            new Hashtable() { { "@account_id", item.AccountId }, { "@qr_code", item.QrCode }, { "@open_id", userInfo.OpenId } }).FirstOrDefault();

                        if (_wallUser != null)
                        {
                            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooBrandShop"))
                            {
                                //string _jsonStr = JsonMapper.ToJson(reqmsg);
                                JavaScriptSerializer jscvt = new JavaScriptSerializer();
                                string _jsonStr = jscvt.Serialize(reqMsg);

                                //获取用户ID
                                helper.SpFileValue["@user_map_id"] = userInfo.UserMapId;
                                helper.SpFileValue["@is_send"] = 0;
                                helper.SpFileValue["@message_type"] = (int)reqMsg.MsgType;
                                helper.SpFileValue["@json_content"] = _jsonStr;
                                helper.SpFileValue["@head_img"] = userInfo.HeadimgUrl;

                                //冗余字段方便查询
                                helper.SpFileValue["@open_id"] = userInfo.OpenId;
                                helper.SpFileValue["@account_id"] = m_account.AccountId;
                                helper.SpFileValue["@nickname"] = userInfo.Nickname;
                                helper.SpFileValue["@platform"] = (int)userInfo.Platform;

                                helper.SpFileValue["@qr_code"] = item.QrCode;

                                helper.SpGetReturnValue("SP_Wall_ZDL_InsertWxMsg");
                            }
                            TxLogHelper.GetLogger("Txooo.Mobile.Module.vWall.NoteUserReqInfo").TxLogInfo("记录互动墙用户信息成功");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxLogHelper.GetLogger("Txooo.Mobile.Module.vWall.NoteUserReqInfo").TxLogInfo("记录互动墙用户信息错误:" + ex.Message);
            }
        }
    }
}
