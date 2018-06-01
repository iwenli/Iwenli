using System.Collections.Specialized;

namespace Iwenli.Push.Notify
{
    /// <summary>
    /// 消息通知基类
    /// </summary>
    public abstract class MessageBase
    {
        protected NameValueCollection m_ReplaceObj = new NameValueCollection();
        /// <summary>
        /// 模板id
        /// </summary>
        public abstract long PushId { get; set; }
        /// <summary>
        /// 模板内容（微信和短信传id）
        /// </summary>
        public abstract string Temp { get; set; }

        /// <summary>
        /// 添加模板替换参数
        /// </summary>
        public virtual string this[string key]
        {
            get { return m_ReplaceObj[key]; }
            set { m_ReplaceObj[key] = value; }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <returns></returns>
        public abstract bool Send();
        
    }
}
