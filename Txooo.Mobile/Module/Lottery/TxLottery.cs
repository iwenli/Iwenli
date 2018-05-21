using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Module
{
    /// <summary>
    /// 商机讲堂活动抽奖程序
    /// </summary>
    public class TxLottery
    {
        static Random m_random = new Random();
        static object m_lockObj = new object();
        static List<string> m_notPrizeInfo;

        static TxLottery()
        {
            //没有中奖信息
            m_notPrizeInfo = new List<string>();
            #region 调侃的话

            m_notPrizeInfo.Add("太可惜了，这次没中奖！");
            m_notPrizeInfo.Add("差一点就中奖了！");
            m_notPrizeInfo.Add("多多微笑，阴霾天谨防情绪感冒！继续努力！");
            m_notPrizeInfo.Add("锄禾日当午，啥都不靠谱！又没中！");
            m_notPrizeInfo.Add("咳！该说的说，不该说的小声说，没中奖就别说了！");
            m_notPrizeInfo.Add("你扫或不扫，奖品就在那里，继续！");
            m_notPrizeInfo.Add("山外青山楼外楼，这次不中你别愁！");

            m_notPrizeInfo.Add("太可惜了，这次没中奖！");
            m_notPrizeInfo.Add("差一点就中奖了！");
            m_notPrizeInfo.Add("多多微笑，阴霾天谨防情绪感冒！继续努力！");
            m_notPrizeInfo.Add("锄禾日当午，啥都不靠谱！又没中！");
            m_notPrizeInfo.Add("咳！该说的说，不该说的小声说，没中奖就别说了！");
            m_notPrizeInfo.Add("你扫或不扫，奖品就在那里，继续！");
            m_notPrizeInfo.Add("山外青山楼外楼，这次不中你别愁！");


            m_notPrizeInfo.Add("太可惜了，这次没中奖！");
            m_notPrizeInfo.Add("差一点就中奖了！");
            m_notPrizeInfo.Add("多多微笑，阴霾天谨防情绪感冒！继续努力！");
            m_notPrizeInfo.Add("锄禾日当午，啥都不靠谱！又没中！");
            m_notPrizeInfo.Add("咳！该说的说，不该说的小声说，没中奖就别说了！");
            m_notPrizeInfo.Add("你扫或不扫，奖品就在那里，继续！");
            m_notPrizeInfo.Add("山外青山楼外楼，这次不中你别愁！");


            m_notPrizeInfo.Add("太可惜了，这次没中奖！");
            m_notPrizeInfo.Add("差一点就中奖了！");
            m_notPrizeInfo.Add("多多微笑，阴霾天谨防情绪感冒！继续努力！");
            m_notPrizeInfo.Add("锄禾日当午，啥都不靠谱！又没中！");
            m_notPrizeInfo.Add("咳！该说的说，不该说的小声说，没中奖就别说了！");
            m_notPrizeInfo.Add("你扫或不扫，奖品就在那里，继续！");
            m_notPrizeInfo.Add("山外青山楼外楼，这次不中你别愁！");

            #endregion
        }

        
        #region 更新抽奖信息

        /// <summary>
        /// 初始化抽奖信息
        /// </summary>
        /// <param name="startNumber"></param>
        /// <param name="endNumber"></param>
        /// <param name="prizeInfo"></param>
        public static bool InitLotterInfo(int startNumber, int endNumber, List<KeyValuePair<string, string>> prizeInfo, List<string> notPrizeInfo)
        {
            try
            {
                lock (m_lockObj)
                {
                    Dictionary<int, string> _lotterList = new Dictionary<int, string>();

                    #region 随机分配奖品数据

                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                    {
                        //随机抽取
                        while (prizeInfo.Count > 0)
                        {
                            int _prizeIndex = m_random.Next(0, prizeInfo.Count);
                            int _lotterIndex = m_random.Next(startNumber, endNumber);
                            if (!_lotterList.ContainsKey(_lotterIndex))
                            {
                                helper.SpFileValue["@number"] = _lotterIndex;
                                helper.SpFileValue["@ok"] = 1;
                                helper.SpFileValue["@name"] = prizeInfo[_prizeIndex].Key;
                                helper.SpFileValue["@info"] = prizeInfo[_prizeIndex].Value;
                                helper.SpExecute("SP_Temp_UpdateLotteryInfo_MrLee");
                                _lotterList.Add(_lotterIndex, prizeInfo[_prizeIndex].Value);
                                prizeInfo.RemoveAt(_prizeIndex);
                            }
                        }
                    }

                    #endregion

                    #region 随机分配活动口号

                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                    {
                        for (int i = startNumber; i < endNumber; i++)
                        {
                            if (!_lotterList.ContainsKey(i))
                            {
                                helper.SpFileValue["@number"] = i;
                                helper.SpFileValue["@ok"] = 0;
                                helper.SpFileValue["@name"] = "";
                                helper.SpFileValue["@info"] = notPrizeInfo[m_random.Next(0, notPrizeInfo.Count)];
                                helper.SpExecute("SP_Temp_UpdateLotteryInfo_MrLee");
                            }
                        }
                    }

                    #endregion

                }
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        #endregion

        #region 更新固定二维码信息

        /// <summary>
        /// 更新固定二维码信息
        /// </summary>
        /// <param name="startNumber"></param>
        /// <param name="endNumber"></param>
        public static void UpdateQrcodeInfo(int startNumber, int endNumber)
        {
            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                for (int i = startNumber; i < endNumber; i++)
                {
                    helper.SpFileValue["@number"] = i;

                    //二维码数据
                    Txooo.Mobile.WeixinApiHelper _helper = AccountInfo.GetAccountInfoByIdFromCache(100000000).ApiHelper as Txooo.Mobile.WeixinApiHelper;
                    //Txooo.Mobile.Weixin.WeixinHelper _helper = new Txooo.Mobile.Weixin.WeixinHelper("wx17b0bfd50aa4117b", "89c6eab9b6243fd229b28baa03e6ab6f");
                    string _ticket, _errorInfo;
                    while (!_helper.GetLimitQrcode(i, out _ticket, out _errorInfo))
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    string _ticketUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + _ticket;
                    helper.SpFileValue["@info"] = "";
                    helper.SpFileValue["@ticket"] = _ticket;
                    helper.SpFileValue["@url"] = _ticketUrl;

                    helper.SpExecute("SP_Temp_UpdateQrCodeInfo_MrLee");
                }
            }
        }

        #endregion

        #region 中奖信息发送

        /// <summary>
        /// 扫描信息处理
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ResMsg ScanInfoHandler(ReqMsg msg, long key)
        {
            ResTextMsg _resMsg = new ResTextMsg(msg);
            try
            {
                lock (m_lockObj)
                {
                    TxLogHelper.GetLogger("TxLottery").Error("抽奖验证【" + key + "】【" + msg.FromUserName + "】");

                    string _content = "";
                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                    {
                        string _sql = "SELECT * FROM [dbo].[Tx_Lottery] WHERE [lottery_number]=" + key;

                        System.Data.DataTable _table = helper.SqlGetDataTable(_sql);
                        if (_table.Rows.Count == 1)
                        {
                            if (_table.Rows[0]["lottery_ok"].ToString().ToLower() == "true")
                            {//中奖
                                TxLogHelper.GetLogger("TxLottery").Error("抽奖验证数据");

                                if (_table.Rows[0]["is_scan"].ToString().ToLower() == "true")
                                {
                                    TxLogHelper.GetLogger("TxLottery").Error("已经扫描");

                                    //已经扫描
                                    _content = "很遗憾，别人下手比你快！";
                                    TxLogHelper.GetLogger("TxLottery").Error("抽奖验证【" + key + "】【" + msg.FromUserName + "】：已经开启");
                                }
                                else
                                {
                                    TxLogHelper.GetLogger("TxLottery").Error("没有扫描");

                                    //判断是否中奖2次
                                    if (helper.SqlGetDataTable("SELECT * FROM [dbo].[Tx_Winners] WHERE [winners_open_id]='" + msg.FromUserName + "'").Rows.Count > 1)
                                    {
                                        TxLogHelper.GetLogger("TxLottery").Error("二次限制");

                                        #region 限制中奖次数

                                        _content = m_notPrizeInfo[m_random.Next(0, m_notPrizeInfo.Count - 1)];

                                        #endregion
                                    }
                                    else
                                    {
                                        TxLogHelper.GetLogger("TxLottery").Error("不是二次限制");

                                        TxLogHelper.GetLogger("TxLottery").Error("抽奖验证【" + key + "】【" + msg.FromUserName + "】：中奖");

                                        //第一次扫描
                                        _content = _table.Rows[0]["lottery_info"].ToString();
                                        //插入中奖人信息
                                        helper.SpFileValue["@number"] = _table.Rows[0]["lottery_number"].ToString();
                                        helper.SpFileValue["@name"] = _table.Rows[0]["lottery_name"].ToString();
                                        helper.SpFileValue["@code"] = m_random.Next(9, 99999).ToString("00000");
                                        helper.SpFileValue["@openid"] = msg.FromUserName;
                                        int _codeID = helper.SpGetReturnValue("SP_Temp_InsertWinnersInfoInfo_MrLee");
                                        _content += "\r\n兑换码：【" + _codeID.ToString() + "】";

                                        //更新数据
                                        TxLogHelper.GetLogger("TxLottery").Error("更新扫描数据");

                                        _sql = "UPDATE [dbo].[Tx_Lottery] SET [is_scan] = 1,[scan_number]=[scan_number]+1 WHERE [lottery_number]=" + key;
                                        helper.SqlExecute(_sql);

                                        TxLogHelper.GetLogger("TxLottery").Error("抽奖验证【" + key + "】【" + msg.FromUserName + "】：" + _content);
                                    }
                                }
                            }
                            else
                            {
                                //没有中奖
                                _content = _table.Rows[0]["lottery_info"].ToString();
                                TxLogHelper.GetLogger("TxLottery").Error("抽奖验证【" + key + "】【" + msg.FromUserName + "】：没有中奖");
                            }
                        }
                    }
                    _resMsg.Content = _content;
                }
            }
            catch (Exception ex)
            {
                _resMsg.Content = "系统繁忙，请再试一次！";
                TxLogHelper.GetLogger("TxLottery").Error("抽奖错误：" + ex.Message, ex);
            }
            return _resMsg;
        }

        #endregion

        #region 领取奖品

        /// <summary>
        /// 领取奖品
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResMsg ReceiveLottery(ReqTextMag msg,long code)
        {
            ResTextMsg _resMsg = new ResTextMsg(msg);
            try
            {
                string _content = "";    
                {
                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                    {
                        string _sql = "SELECT * FROM [dbo].[Tx_Winners] WHERE [receive_code]=" + code;

                        System.Data.DataTable _table = helper.SqlGetDataTable(_sql);
                        if (_table.Rows.Count > 0)
                        {
                            if (_table.Rows[0]["is_receive"].ToString().ToLower() == "true")
                            {
                                _content = "此兑换码已经使用！";
                            }
                            else
                            {
                                _content = "此兑换码对应奖品为：\r\n[" + code + "]\r\n" + _table.Rows[0]["lottery_name"].ToString();

                                _sql = "UPDATE [dbo].[Tx_Winners] SET [is_receive]=1 ,[receive_user]= '" + msg.FromUserName + "'  ,[receive_time]=getdate() WHERE [receive_code]=" + code;
                                helper.SqlExecute(_sql);
                            }
                        }
                        else
                        {
                            _content = "没有此兑换码！";
                        }
                    }
                }
                _resMsg.Content = _content;
            }
            catch (Exception ex)
            {
                _resMsg.Content = "查询中奖信息失败，请稍后再试！";
            }
            return _resMsg;
        }

        #endregion

        #region 查询中奖情况

        /// <summary>
        /// 查询中奖情况
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ResMsg SelectLottery(ReqMsg msg)
        {
            ResTextMsg _resMsg = new ResTextMsg(msg);
            try
            {
                string _content = "";
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    string _sql = "SELECT * FROM [dbo].[Tx_Winners] WHERE [is_receive]=0 AND [winners_open_id]='" + msg.FromUserName + "'";

                    System.Data.DataTable _table = helper.SqlGetDataTable(_sql);
                    if (_table.Rows.Count > 0)
                    {
                        _content = "您有【" + _table.Rows.Count + "】件奖品可领取，\r\n请凭此信息到领奖台办理：\r\n";
                        foreach (System.Data.DataRow item in _table.Rows)
                        {
                            _content += "[" + item["receive_code"].ToString() + "][" + item["lottery_name"].ToString() + "]\r\n";
                        }
                    }
                    else
                    {
                        _content = "很遗憾，没有发现您的中奖信息哦！";
                    }
                }
                _resMsg.Content = _content;
            }
            catch (Exception ex)
            {
                _resMsg.Content = "查询中奖信息失败，请稍后再试！";
            }
            return _resMsg;
        }


        #endregion

    }
}
