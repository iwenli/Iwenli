using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Txooo.Mobile.Platform;

namespace Txooo.Mobile.Msgx
{    
    /// <summary>
    /// 消息处理事件
    /// </summary>
    public class MessageHandlerEventArgs : EventArgs
    {
        /// <summary>
        /// 账号信息
        /// </summary>
        protected AccountInfo m_account;
        /// <summary>
        /// 账号信息
        /// </summary>
        public AccountInfo Account
        {
            get { return m_account; }
            set { m_account = value; }
        }
        /// <summary>
        /// 接受到的消息主体
        /// </summary>
        private string m_requestBody;
        /// <summary>
        /// 接受到的消息主体
        /// </summary>
        public string RequestBody
        {
            get { return m_requestBody; }
            set { m_requestBody = value; }
        }
        /// <summary>
        /// 接受到的消息
        /// </summary>
        private ReqMsg m_requestMessage;
        /// <summary>
        /// 接受到的消息
        /// </summary>
        public ReqMsg RequestMessage
        {
            get { return m_requestMessage; }
            set { m_requestMessage = value; }
        }
        /// <summary>
        /// 返回消息
        /// </summary>
        private ResMsg[] m_responseMessage;
        /// <summary>
        /// 返回消息
        /// </summary>
        public ResMsg[] ResponseMessage
        {
            get { return m_responseMessage; }
            set { m_responseMessage = value; }
        }
    }
}
