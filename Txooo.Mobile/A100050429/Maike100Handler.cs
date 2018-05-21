/*----------------------------------------------------------------
 *  Copyright (C) 2015 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：Maike100Handler
 *  所属项目：Txooo.Mobile.A100050429 -- Maike100Handler
 *  创建用户：FN
 *  创建时间：2018年3月27日 星期二 上午 08:44:21
 *  
 *  功能描述：
 *          1、
 *          2、 
 * 
 *  修改标识：  
 *  修改描述：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Mobile.Msgx;
using Txooo.Mobile.Platform;

namespace Txooo.Mobile.A100050429
{
    public class Maike100Handler : DefaultHandler
    {
        protected override ResMsg[] HandlerScanSubscribeEvent(ReqEventMsg message)
        {
            this.TxLogInfo("卖客壹佰处理扫描带参数事件：" + message.EventKey);
            
            message.EventKey = message.EventKey.Replace("qrscene_", "");

            int _brandId = 0, _storeId = 0;
            int.TryParse(message.EventKey.Split(',')[0], out _brandId);
            int.TryParse(message.EventKey.Split(',')[1], out _storeId);

            List<ResMsg> _msgList = new List<ResMsg>();

            #region 处理活动二维码
            if (_brandId > 0 && _storeId > 0)
            {
                var resMsg = new ResNewsMsg(message);
                ResArticle article = new ResArticle();

                article.Url =
                    string.Format("https://wxapp.txooo.com/vmall/index.html?brandid={0}&storeid={1}",
                       _brandId, _storeId);
                article.PicUrl = "http://img.txooo.com/2017/11/17/0b79c46bdb318aa3de3f7bbe7e283428.png";
                article.Discription = "点击完成注册，开启不一样的购物体验";
                article.Title = "立即完成注册，即可刷脸开门！";

                resMsg.Articles.Add(article);

                _msgList.AddRange(new ResMsg[] { resMsg });
            }
            #endregion
            
            return _msgList.Count > 0 ? _msgList.ToArray() : GetDefaultResponseMessage(m_requestMessage);
        }
        
    }
}
