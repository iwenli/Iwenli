using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Web;

namespace Txooo.Mobile.Msgx
{
    /*
     * 请求路径格式：
     * http://weixin.mobile.txooo.com/MessageReceive.aspx/txooo
     * http://mobile.txooo.com/Txooo_Mobile_Msgx_MessageHandler/weixin.msg
     * http://mobile.txooo.com/0/weixin.msg
     * 
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
                long _accountId = 0;
                AccountInfo _accountInfo = new AccountInfo();
                if (long.TryParse(_pathInfo[0], out _accountId))
                {                    
                    _accountInfo = AccountInfo.GetAccountInfoByIdFromCache(_accountId);

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
