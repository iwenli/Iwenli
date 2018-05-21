using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Mobile.Msgx;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile.A100050429
{
    /// <summary>
    /// 分销公众号处理类
    /// </summary>
    public class SalesHandler : DefaultHandler
    {
        /// <summary>
        /// 关注事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerSubscribeEvent(ReqEventMsg message)
        {
            var _hongbao = new ResNewsMsg(message);
            ResArticle article = new ResArticle();

            article.Url = "http://sales.7518.cn/hongbao/index.html";
            article.PicUrl = "http://img.txooo.com/2015/07/14/f9926f0629f9bb918176b243f2d85a65.jpg";
            article.Discription = "恭喜您获得新手红包";
            article.Title = "恭喜您获得新手红包";
            _hongbao.Articles.Add(article);

            if (m_replyConfig != null)
            {
                //回复默认
                Platform.ResMsg _msg = m_replyConfig.SubResMsg.Clone() as Platform.ResMsg;
                _msg.ToUserName = m_requestMessage.FromUserName;
                _msg.FromUserName = m_requestMessage.ToUserName;
                _msg.ResType = 2;

                return new ResMsg[] { _msg, _hongbao };
            }           

            return GetDefaultResponseMessage(m_requestMessage);
        }
        /// <summary>
        /// 扫描关注
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override ResMsg[] HandlerScanSubscribeEvent(ReqEventMsg message)
        {
            this.TxLogInfo("分销处理扫描关注事件：" + message.EventKey);
            long _id;
            long.TryParse(message.EventKey, out _id);
            message.EventKey = message.EventKey.Replace("qrscene_", "");

            List<ResMsg> _msgList = new List<ResMsg>();

            var _hongbao = new ResNewsMsg(message);
            ResArticle article = new ResArticle();

            article.Url = "http://sales.7518.cn/hongbao/index.html";
            article.PicUrl = "http://img.txooo.com/2015/07/14/f9926f0629f9bb918176b243f2d85a65.jpg";
            article.Discription = "恭喜您获得新手红包";
            article.Title = "恭喜您获得新手红包";
            _hongbao.Articles.Add(article);

            if (m_replyConfig != null)
            {
                //回复默认
                Platform.ResMsg _msg = m_replyConfig.SubResMsg.Clone() as Platform.ResMsg;
                _msg.ToUserName = m_requestMessage.FromUserName;
                _msg.FromUserName = m_requestMessage.ToUserName;
                _msg.ResType = 2;
                _msgList.Add(_msg);
            }

            _msgList.Add(_hongbao);
         
            ResMsg[] _resmsg = HandlerScan(message);
            if (_resmsg != null)
            {                
                var _temp = _resmsg.Select(a =>
                {
                    a.ResType = 2;
                    return a;
                });

                _msgList.AddRange(_temp);
            }

            return _msgList.Count > 0 ? _msgList.ToArray() : GetDefaultResponseMessage(m_requestMessage);            
        }
        /// <summary>
        /// 带参数二维码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override Platform.ResMsg[] HandlerScan(Platform.ReqEventMsg message)
        {
            this.TxLogInfo("分销账号处理扫描带参数事件：【" + message.FromUserName + "】" + message.EventKey);
            long _id;
            long.TryParse(message.EventKey, out _id);

            #region 处理活动二维码
            if (_id >10000)
            {
                this.TxLogInfo("处理分销个人推广二维码：【" + message.FromUserName + "】" + message.EventKey);
              
                   
                    var resMsg = new ResNewsMsg(message);
                    ResArticle article = new ResArticle();

                    article.Url =
                        string.Format("http://passport.7518.cn/login.html?agent_id={0}",
                           _id - 10000);
                    article.PicUrl = "http://img.txooo.com/2015/07/14/9b95f980439bfff32ee043a29dfe8cc7.jpg";
                    article.Discription = "点击快速成为创业者";
                    article.Title = "大家都来创业吧";

                    resMsg.Articles.Add(article);

                    return new ResMsg[] { resMsg };        
            }
            #endregion
            return GetDefaultResponseMessage(m_requestMessage);
        }
    }
}
