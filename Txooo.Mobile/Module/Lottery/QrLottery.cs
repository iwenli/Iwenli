using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Txooo.Data.Entity;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Module
{
    /// <summary>
    /// 投票处理类
    /// </summary>
    public class QrLottery
    {
        static List<string> m_notPrizeInfo = new List<string>();
        static Random m_random = new Random();
        static object m_lock = new object();

        static QrLottery()
        {
            m_notPrizeInfo.Add("太可惜了，这次没中奖！");
            m_notPrizeInfo.Add("差一点就中奖了！");
            m_notPrizeInfo.Add("多多微笑，阴霾天谨防情绪感冒！继续努力！");
            m_notPrizeInfo.Add("锄禾日当午，啥都不靠谱！又没中！");
            m_notPrizeInfo.Add("咳！该说的说，不该说的小声说，没中奖就别说了！");
            m_notPrizeInfo.Add("你扫或不扫，奖品就在那里，继续！");
            m_notPrizeInfo.Add("山外青山楼外楼，这次不中你别愁！");
        }

        ReqMsg m_reqMsg;

        LotteryInfo m_lotteryInfo;
        IList<LotteryPrizeInfo> m_prizeList;

        Hashtable m_stateData;

        public QrLottery(ReqMsg msg, LotteryInfo lotteryInfo)
        {
            m_reqMsg = msg;
            m_lotteryInfo = lotteryInfo;

            //初始化消息(已废)
            //m_msgList = LotteryMessage.GetMessageList(" AND ActivityType=5 AND ActivityId=" + m_lotteryInfo.ActivityId);
        }


        public Platform.ResMsg[] GetResMsg()
        {
            lock (m_lock)
            {
                //初始化奖项信息
                m_prizeList = LotteryPrizeInfo.GetPrizeListByLotteryID(" AND ActivityType=5 AND ActivityID=" + m_lotteryInfo.ActivityId);

                //限制数据
                using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                {
                    helper.SpFileValue["@activity_id"] = m_lotteryInfo.ActivityId;
                    helper.SpFileValue["@activity_type"] = 5;
                    helper.SpFileValue["@byip"] = 0;
                    helper.SpFileValue["@search"] = m_reqMsg.FromUserName;

                    var dt = helper.SpGetDataTable("SP_PLATFORM_ZDL_LotteryLimit");
                    m_stateData = new Hashtable();
                    m_stateData["TotalTimes"] = dt.Rows[0][0];//该用户已经参加的总次数
                    m_stateData["TodayTimes"] = dt.Rows[0][1];//该用户今天已经参加的次数
                    m_stateData["TodayPrizes"] = dt.Rows[0][2];//今天已经出奖次数 
                    m_stateData["Current"] = Convert.ToInt32(dt.Rows[0][3]) + 1;//第几位参与此活动
                    m_stateData["IsWined"] = dt.Rows[0][4];//该用户本次活动已经获奖次数
                }

                var _now = DateTime.Now;
                if (_now < m_lotteryInfo.StartTime)
                {
                    ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                    _txtmsg.Content = "很抱歉,活动还未开始";
                    //+ "，活动时间:(" + m_lotteryInfo.StartTime.ToString("yyyy-MM-dd") + " 至 " + m_lotteryInfo.EndTime.ToString("yyyy-MM-dd") + ")，欢迎您届时参与活动";
                    return new ResMsg[] { _txtmsg };
                }
                if (_now > m_lotteryInfo.EndTime)
                {
                    ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                    _txtmsg.Content = "很遗憾,您来晚了,活动已结束";
                    return new ResMsg[] { _txtmsg };
                }

                #region 处理次数限制
                {
                    ResTextMsg _txtmsg = UserTimesLimit();
                    if (_txtmsg != null) return new ResMsg[] { _txtmsg };
                }
                #endregion

                try
                {
                    var _sn = m_random.Next(10000000, 99999999).ToString();

                    var _mode = m_prizeList[0].SetDrawState;
                    LotteryPrizeInfo _winPrize = null;

                    if ((int)m_stateData["IsWined"] < m_lotteryInfo.LimitCount)//限制二次中奖
                        _winPrize = _mode == 1 ? WholeMode() : PrizeMode();

                    var _isWin = _winPrize == null ? false : true;

                    int _newID = 0;
                    using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                    {
                        helper.SpFileValue["@join_name"] = m_reqMsg.FromUserName;
                        helper.SpFileValue["@openid"] = m_reqMsg.FromUserName;
                        helper.SpFileValue["@mobile"] = System.DBNull.Value;
                        helper.SpFileValue["@activity_id"] = m_lotteryInfo.ActivityId;
                        helper.SpFileValue["@activity_type"] = (int)ActivityType.QRLOTTERY;
                        helper.SpFileValue["@source"] = "微信";
                        helper.SpFileValue["@ip"] = System.DBNull.Value;
                        helper.SpFileValue["@join_mode"] = 1;
                        helper.SpFileValue["@sn"] = _sn;
                        helper.SpFileValue["@is_win"] = _isWin;
                        if (_isWin)
                        {
                            helper.SpFileValue["@prize_name"] = _winPrize.PrizeName;
                            helper.SpFileValue["@prize_id"] = _winPrize.PrizeId;
                        }

                        _newID = Convert.ToInt32(helper.SpScalar("SP_PLATFORM_ZDL_AddLotteryScan"));
                    }

                    if (_isWin && _newID > 0)
                    {
                        ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                        _txtmsg.Content = string.Format("恭喜，您中奖了！您获得了【{0}】，兑换码：{1}，请妥善保管兑换码并及时到店内兑换奖品", _winPrize.PrizeName, _sn);
                        return new ResMsg[] { _txtmsg };

                    }
                    else
                    {
                        ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                        int _rand = m_random.Next(0, 7);
                        _txtmsg.Content = m_notPrizeInfo[_rand];
                        return new ResMsg[] { _txtmsg };
                    }
                }
                catch (Exception ex)
                {
                    TxLogHelper.GetLogger("QrLottery").Error("抽奖错误：" + ex.Message, ex);
                    ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                    _txtmsg.Content = "系统繁忙！请再试一次";
                    return new ResMsg[] { _txtmsg };
                }
            }
        }

        /// <summary>
        /// 按总体配奖方式
        /// </summary>
        /// <returns></returns>
        private LotteryPrizeInfo WholeMode()
        {
            LotteryPrizeInfo _prize = null;

            var _way = m_prizeList[0].WinWayState;

            switch (_way)
            {
                case 3://按时间
                    {
                        var _date = DateTime.Now;

                        var _times = JsonMapper.ToObject(m_prizeList[0].PrizeTime);
                        foreach (JsonData item in _times)
                        {
                            var _t = item["Time"].ToString().Split(new string[] { "至" }, StringSplitOptions.RemoveEmptyEntries);
                            DateTime _startTime = DateTime.Parse(_t[0]), _endTime = DateTime.Parse(_t[1]);

                            if (_date > _startTime && _date < _endTime)
                            {
                                _prize = RandomPrizeByTime();
                                break;
                            }
                        }
                    }
                    break;
                case 1://按自动中奖率 暂时先按顺序处理                   
                case 2://按手动中奖率 暂时先按顺序处理
                    {
                        var current = (int)m_stateData["Current"];

                        current = m_prizeList[0].Rate >= 1 ? current % 100 : current = current % 1000;
                       
                        var _orders = m_prizeList[0].PrizeOrder.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (_orders.Contains(current.ToString()))
                        {
                            _prize = RandomPrize();
                        }
                    }
                    break;
                case 4://按顺序
                    {
                        var current = (int)m_stateData["Current"];

                        if (m_prizeList[0].OrderState == 2)
                        {
                            var _orders = m_prizeList[0].PrizeOrder.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (_orders.Contains(current.ToString()))
                            {
                                _prize = RandomPrize();
                            }
                        }
                        else
                        {
                            var _orders = m_prizeList[0].PrizeOrder;
                            current = current % 10;
                            if (current == int.Parse(_orders))
                            {
                                _prize = RandomPrize();
                            }
                        }
                    }
                    break;
            }
            return _prize;
        }

        /// <summary>
        /// 时间段内随机奖品
        /// </summary>      
        /// <returns></returns>
        private LotteryPrizeInfo RandomPrizeByTime()
        {
            var _time = JsonMapper.ToObject(m_prizeList[0].PrizeTime)[0];
            int _ltdCount = int.Parse(_time["prizeTimeCount"].ToString());
            var _t = _time["Time"].ToString().Split(new string[] { "至" }, StringSplitOptions.RemoveEmptyEntries);

            int[] _prizeNum = new int[m_prizeList.Count];
            for (int i = 0; i < _prizeNum.Length; i++)
            {
                _prizeNum[i] = m_random.Next(0, m_prizeList.Count);
            }


            foreach (int i in _prizeNum)
            {
                int _winCount = GetTimeWinCount(m_prizeList[i], DateTime.Parse(_t[0]), DateTime.Parse(_t[1]));
                if (_winCount < _ltdCount && m_prizeList[i].RemainCount > 0)
                {
                    return m_prizeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取某个奖品时间段内已经中将的数量
        /// </summary>
        /// <param name="prize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private int GetTimeWinCount(LotteryPrizeInfo prize, DateTime startTime, DateTime endTime)
        {
            int _count = 0;

            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
            {
                string _sql = "SELECT COUNT(*) FROM View_PLATFORM_ZDL_JoinUser WHERE PrizeID=@PrizeID AND IsWin=1 AND AddTime Between @StartTime AND @EndTime";

                helper.SpFileValue["@PrizeID"] = prize.PrizeId;
                helper.SpFileValue["@StartTime"] = startTime;
                helper.SpFileValue["@EndTime"] = endTime;

                _count = Convert.ToInt32(helper.SqlScalar(_sql, helper.SpFileValue));
            }
            return _count;
        }

        /// <summary>
        /// 随机选择一个奖品
        /// </summary>
        /// <returns></returns>
        private LotteryPrizeInfo RandomPrize()
        {
            //剩余最多的先派
            var _maxCountPrize = m_prizeList.OrderByDescending(p => { return p.RemainCount; });
            var _prize = _maxCountPrize.ToArray()[0];
            return _prize.RemainCount > 0 ? _prize : null;

            //随机
            //var _remainList = m_prizeList.Where(prize => { return prize.RemainCount > 0; });
            //var _count = _remainList.Count();
            //if (_count > 0)
            //{
            //    var _rand = m_random.Next(0, _count);
            //    return _remainList.ToArray()[_rand];
            //}
            //return null;
        }

        /// <summary>
        /// 按奖品单独配奖方式
        /// </summary>
        /// <returns></returns>
        public LotteryPrizeInfo PrizeMode()
        {
            LotteryPrizeInfo _prize = null;

            var _way = m_prizeList[0].WinWayState;

            switch (_way)
            {
                case 3://按时间
                    {
                        bool _isfind = false;
                        var _date = DateTime.Now;
                        foreach (var item in m_prizeList)
                        {
                            var _times = JsonMapper.ToObject(item.PrizeTime);
                            foreach (JsonData time in _times)
                            {
                                var _t = time["Time"].ToString().Split(new string[] { "至" }, StringSplitOptions.RemoveEmptyEntries);
                                DateTime _startTime = DateTime.Parse(_t[0]), _endTime = DateTime.Parse(_t[1]);

                                if (_date > _startTime && _date < _endTime)
                                {
                                    int _timeCount = GetTimeWinCount(item, _startTime, _endTime);

                                    var _count = time["prizeTimeCount"].ToString();
                                    if (_timeCount < int.Parse(_count) && item.RemainCount > 0)
                                    {
                                        _prize = item;
                                    }
                                    _isfind = true;
                                    break;
                                }
                            }
                            if (_isfind) break;
                        }
                    }
                    break;
                case 2://按手动中奖率
                    _prize = Calculate();
                    break;
                case 4://按顺序
                    {
                        var current = (int)m_stateData["Current"];

                        foreach (var item in m_prizeList)
                        {
                            if (item.OrderState == 2)
                            {
                                var _orders = item.PrizeOrder.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (_orders.Contains(current.ToString()))
                                {
                                    if (item.RemainCount > 0)
                                    {
                                        _prize = item;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                var _orders = item.PrizeOrder;
                                if (current % 10 == int.Parse(_orders))
                                {
                                    if (item.RemainCount > 0)
                                    {
                                        _prize = item;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
            return _prize;
        }

        /// <summary>
        /// 暂定
        /// </summary>
        /// <returns></returns>
        private LotteryPrizeInfo Calculate()
        {
            int[] rate_count = new int[m_prizeList.Count];
            m_prizeList = m_prizeList.OrderBy(p => { return p.Rate; }).ToList();

            for (int i = 0; i < m_prizeList.Count; i++)
            {
                if (i != 0)
                {
                    rate_count[i] = rate_count[i - 1] + (int)(m_prizeList[i].Rate * 10);
                }
                else
                {
                    rate_count[i] = rate_count[i] + (int)(m_prizeList[i].Rate * 10);
                }
            }

            var _code = m_random.Next(1, 1001);

            for (int i = 0; i < rate_count.Length; i++)
            {
                if (_code < rate_count[i])
                {
                    return m_prizeList[i].RemainCount > 0 ? m_prizeList[i] : null;
                }
            }
            return null;
        }

        /// <summary>
        /// 抽奖次数判定
        /// </summary>
        /// <returns></returns>
        private ResTextMsg UserTimesLimit()
        {
            if (m_lotteryInfo.Totalcount > 0 && (int)m_stateData["TotalTimes"] >= m_lotteryInfo.Totalcount)
            {
                ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                _txtmsg.Content = "您抽奖次数已经到达上限，欢迎下次继续关注我们";
                return _txtmsg;
            }

            if (m_lotteryInfo.Countforoneday > 0 && (int)m_stateData["TodayTimes"] >= m_lotteryInfo.Countforoneday)
            {
                ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                _txtmsg.Content = "您今天抽奖次数已经超过当日最高抽奖次数，欢迎下次再来";
                return _txtmsg;
            }

            if (m_lotteryInfo.Mostawardcount > 0 && (int)m_stateData["TodayPrizes"] >= m_lotteryInfo.Mostawardcount)
            {
                ResTextMsg _txtmsg = new ResTextMsg(m_reqMsg.FromUserName);
                _txtmsg.Content = "今天的抽奖活动已经结束，欢迎下次再来";
                return _txtmsg;
            }

            return null;
        }
    }
}
