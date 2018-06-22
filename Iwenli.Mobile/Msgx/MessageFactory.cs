#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：MessageFactory
 *  所属项目：Iwenli.Mobile.Msgx
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/6/11 17:19:44
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Iwenli.Mobile.Msgx
{
    /*
     * 请求路径格式：
     * http://weixin.mobile.txooo.com/MessageReceive.aspx/txooo
     * http://mobile.txooo.com/Txooo_Mobile_Msgx_MessageHandler/weixin.msg
     * http://mobile.txooo.com/0/weixin.msg
     * 
     * 
      <add name="Msg" path="*.msg" verb="*" type="Txooo.Mobile.Msgx.MessageFactory" modules="ManagedPipelineHandler" scriptProcessor="" resourceType="Unspecified" requireAccess="Script" allowPathInfo="false" preCondition="integratedMode" responseBufferLimit="4194304"/>
     */

    /// <summary>
    /// 被动消息处理工厂
    /// </summary>
    public class MessageFactory : IHttpHandlerFactory
    {
        #region 接口工厂

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="pathTranslated"></param>
        /// <returns></returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            // /0/weixin.msg/get
            string[] _pathInfo = context.Request.FilePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (_pathInfo.Length == 2)
            {
                var _accountId = _pathInfo[0].ToInt64();
                if (_accountId > 0)
                {
                    var _accountInfo = AccountInfo.GetAccountInfoByIdFromCache(_accountId);

                    string _key = _pathInfo[1].Split('.')[0];
                    string _handleType = _accountInfo.GetMessageHandler(_key);

                    Type _type = System.Web.Compilation.BuildManager.GetType(_handleType, false, true);
                    if (_type != null)
                    {
                        MessageHandler _handler = Activator.CreateInstance(_type) as MessageHandler;
                        _handler.Account = _accountInfo;
                        return _handler;
                    }
                }
            }
            return new DefaultHandler();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {

        }

        #endregion
    }
}
