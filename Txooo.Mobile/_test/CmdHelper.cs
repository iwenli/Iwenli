using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile
{
    internal class CmdHelper
    {
        public ResMsg GetReturnInfo(ReqMsg reqmsg)
        {
            ResTextMsg _resmsg;
            _resmsg = new ResTextMsg(reqmsg);         
            _resmsg.Content = "你好,有什么可以为您服务的？";


            if (reqmsg.MsgType == ReqMsgType.Text)
            {
                ReqTextMag _reqTextmsg = reqmsg as ReqTextMag;

                OA.OAUserInfo _oaUser = OA.OAUserInfo.GetUserInfoByPlatformUserSn(reqmsg.Platform, reqmsg.FromUserName);

                #region 判断用户是否已经绑定

                if (_oaUser == null)
                {
                    if (_reqTextmsg.Content == "办公" || _reqTextmsg.Content.ToLower() == "oa")
                    {
                        #region 用户绑定接口

                        string _url = "http://weixin.mobile.txooo.com/UserBinding.htm?type=oa&platform=" + (int)_reqTextmsg.Platform + "&platform_user_sn=" + PlatformUserInfo.EncryptPlatformUserSn(_reqTextmsg.Platform, _reqTextmsg.FromUserName);

                        //返回数据
                        ResNewsMsg resMsg = new ResNewsMsg(_reqTextmsg);
                        resMsg.Articles.Add(new ResArticle()
                        {
                            Title = "你好，如需完成相关操作，请先进行账户绑定！",
                            Discription = "你还没有完成账户绑定，如需查询相关数据，请先完成账户绑定！",
                            PicUrl = "",
                            Url = _url
                        });

                        return resMsg;

                        #endregion
                    }
                }
                else
                {
                    //已经绑定用户
                    _resmsg.Content = _oaUser.Username + ",你好,办公命令如下:\r\n 1、品牌id＋效果＋天数\r\n 2、品牌id＋恢复＋天数";

                    if (_reqTextmsg.Content == "我的品牌")
                    {
                        _resmsg.Content = OAGetUserBrandInfo(_oaUser);
                    }
                    else if (_reqTextmsg.Content.Contains("效果"))
                    {
                        int _brandId, _day;
                        if (int.TryParse(_reqTextmsg.Content.Substring(0, _reqTextmsg.Content.IndexOf("效果")), out _brandId)
                            && int.TryParse(_reqTextmsg.Content.Substring(_reqTextmsg.Content.IndexOf("效果") + 2), out _day)
                            )
                        {
                            _resmsg.Content = OAGetUserXG(_oaUser, _brandId, DateTime.Now.Date.AddDays(0 - (_day)));
                        }
                        else
                        {
                            _resmsg.Content = "命令错误,正确命令格式，品牌ＩＤ＋效果＋天数，如查询９９品牌前天的效果，发送：９９效果２";
                        }
                    }
                    else if (_reqTextmsg.Content.Contains("恢复"))
                    {
                        int _brandId, _day;
                        if (int.TryParse(_reqTextmsg.Content.Substring(0, _reqTextmsg.Content.IndexOf("恢复")), out _brandId)
                            && int.TryParse(_reqTextmsg.Content.Substring(_reqTextmsg.Content.IndexOf("恢复") + 2), out _day)
                            )
                        {
                            _resmsg.Content = OARegainBrandLY(_oaUser, _brandId, DateTime.Now.Date.AddDays(0 - (_day)));
                        }
                        else
                        {
                            _resmsg.Content = "命令错误,正确命令格式，品牌ＩＤ＋效果＋天数，如查询９９品牌前天的效果，发送：９９效果２";
                        }
                    }
                    else if (_reqTextmsg.Content.Contains("邀请函"))
                    {
                        //邀请函,口袋鸡排,何总,总经理,13581952323
                        //InviteInfo _info = new InviteInfo(_reqTextmsg.Content.Replace("邀请函", _oaUser.UserId + "," + _oaUser.Username + ",0"));
                        //return _info.GetResMsg(_reqTextmsg);

                        //return Page.TxoooHandler.GetPulpit(_oaUser.UserId, _reqTextmsg.Content, _reqTextmsg);                       
                    }
                }

                #endregion
            }
            
            return _resmsg;
        }

        #region 获得用户品牌列表

        string OAGetUserBrandInfo(OA.OAUserInfo oaUser)
        {
            string _str = "";
            try
            {
                string[] _nexList = { "竞价", "效果", "联展", "7158", "999178" };
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooCRM2013"))
                {
                    string _sql = @"SELECT * From (
SELECT A.[brand_id],B.[customers_name],A.[end_time],c.[Mobile],c.[Name] 
	FROM(SELECT * FROM [dbo].[Fn_Service_OnlineBrand] ({0})) AS A
		INNER JOIN [dbo].[Fn_Service_CustomerInfo] ({0}) AS B
		ON A.[brand_id]=B.[brand_id]
		INNER JOIN [dbo].[Fn_Service_EmployeeMobilesInfo]() AS C
		ON C.[ID]=B.[employee_id]
		) as AA WHERE AA.Name='"+oaUser.Username+"'	Order by end_time";
                    //) as AA WHERE AA.Name='" + oaUser.Username + "' Order by end_time";
                    for (int i = 0; i < _nexList.Length; i++)
                    {
                        DataTable _table = helper.SqlGetDataTable(string.Format(_sql, i));

                        _str += _nexList[i] + "平台项目," + _table.Rows.Count + "个：\r\n";
                        foreach (DataRow item in _table.Rows)
                        {
                            _str += "[" + int.Parse(item["brand_id"].ToString()).ToString("00000") + "][" + DateTime.Parse(item["end_time"].ToString()).AddDays(-1).ToString("MM-dd") + "]" + item["customers_name"].ToString() + "\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.TxLogError("获得用户品牌列表错误：" + ex.Message, ex);
                _str = "查询错误，请稍后再试！";
            }
            _str = _str.Replace("色", "*");
            return _str;
        }

        #endregion

        #region 查询品牌效果

        string OAGetUserXG(OA.OAUserInfo oaUser ,long brandId,DateTime date)
        {
            string _str = "";
            try
            {
                if (CheckEmployeeBrand(oaUser, brandId))
                {
                    #region 查询效果信息

                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("IUP_DWLZC"))
                    {
                        string _sql = @"SELECT [BrandID],[BrandName],[all_xg],[ly],[p400],[huihu],[kf],[kfxs],[kfxs_wm],[kfxs_kh],[pv] From [dbo].[View_LGZ_ItemKPI]   
                                WHERE vyear =" + date.Year + " and vmonth = " + date.Month + " and vday = " + date.Day + " and [BrandID]=" + brandId;

                        DataTable _table = helper.SqlGetDataTable(_sql);
                        if (_table.Rows.Count == 1)
                        {
                            _str += string.Format("{2}[{0}]\r\n[{1}]\r\n总效果线索【{3}】条\r\n具体数据如下：\r\n\t◇留言：{4}\r\n\t◇热线：{5}\r\n\t◇回呼：{6}\r\n\t◇TQ沟通：{7}\r\n\t◇TQ有效：{8}\r\n\t◇TQ网民：{9}\r\n\t◇TQ客户：{10}\r\n\t◇PV：{11}"
                                , _table.Rows[0]["BrandID"].ToString()
                                , date.ToString("yyyy-MM-dd")
                                , _table.Rows[0]["BrandName"].ToString()
                                , _table.Rows[0]["all_xg"].ToString()
                                , _table.Rows[0]["ly"].ToString()
                                , _table.Rows[0]["p400"].ToString()
                                , _table.Rows[0]["huihu"].ToString()
                                , _table.Rows[0]["kf"].ToString()
                                , _table.Rows[0]["kfxs"].ToString()
                                , _table.Rows[0]["kfxs_wm"].ToString()
                                , _table.Rows[0]["kfxs_kh"].ToString()
                                , _table.Rows[0]["pv"].ToString()
                                );
                        }
                        else
                        {
                            _str = "无此品牌[" + date.ToString("yyyy-MM-dd") + "]效果数据";
                        }
                    }

                    #endregion
                }
                else
                {
                    _str = "权限审核错误，请稍后再试！";
                }
            }
            catch (Exception ex)
            {
                this.TxLogError("查询品牌效果错误[" + brandId + "][" + oaUser.Username + "][" + date.ToString() + "]：" + ex.Message, ex);

                _str = "查询错误，请稍后再试！";
            }

            return _str;
        }

        #endregion

        #region 恢复留言

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oaUser"></param>
        /// <param name="brandId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        string OARegainBrandLY(OA.OAUserInfo oaUser, long brandId, DateTime date)
        {
            string _str = "";
            try
            {
                if (CheckEmployeeBrand(oaUser, brandId))
                {
                    #region 恢复留言

                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooLY"))
                    {
                        string _sql = @"UPDATE [TxoooLY].[dbo].[LY_Work] SET [isDel]=0,[isOut]=0
                                WHERE [ly_id] IN ( 
                                    SELECT [ly_id] FROM [TxoooLY].[dbo].[LY_Original]  
                                        WHERE [ly_id] IN (SELECT [ly_id]  FROM [TxoooLY].[dbo].[LY_Assign]  WHERE [brand_id]=" + brandId + @" AND [is_ok]=1
                                ) AND [vyear]=" + date.Year + " AND [vmonth] = " + date.Month + " AND [vday] = " + date.Day + ")";

                        int _num = helper.SqlExecute(_sql);
                        _str = "恢复品牌【" + brandId + "】\r\n【" + date.ToString("yyyy-MM-dd") + "】\r\n留言数据共【" + _num + "】条";
                    }

                    #endregion
                }
                else
                {
                    _str = "权限审核错误，请稍后再试！";
                }
            }
            catch (Exception ex)
            {
                this.TxLogError("恢复留言错误[" + brandId + "][" + oaUser.Username + "][" + date.ToString() + "]：" + ex.Message, ex);

                _str = "恢复错误，请稍后再试！";
            }

            return _str;
        }

        #endregion

        #region 查询员工品牌归属

        bool CheckEmployeeBrand(OA.OAUserInfo oaUser, long brandId)
        {
            if (oaUser.Username == "李磊" || oaUser.Username == "李国增" || oaUser.Username == "喻增友")
            {
                return true;
            }
            else
            {
                try
                {
                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooCRM2013"))
                    {
                        string _sql = @"SELECT employee_id FROM dbo.Customer_Info RIGHT OUTER JOIN dbo.Customer_Employee_Map ON dbo.Customer_Info.customers_id = dbo.Customer_Employee_Map.customers_id 
                                        WHERE  brand_id=" + brandId + " AND employee_id=" + oaUser.UserId;

                        if (helper.SqlGetDataTable(_sql).Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.TxLogError("查询员工品牌归属错误：[" + brandId + "][" + oaUser.Username + "]：" + ex.Message, ex);
                }
            }
            return false;
        }

        #endregion

    }
}
