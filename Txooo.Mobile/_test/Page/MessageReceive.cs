using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Page
{
    /// <summary>
    /// 消息接受页面
    /// </summary>
    public class MessageReceive : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            PlatformType _type = PlatformType.Touch;
            for (int i = 0; i < 8; i++)
            {
                if (context.Request.Url.Host.ToLower().Contains(((PlatformType)i).ToString().ToLower()))
                {
                    _type = (PlatformType)i;
                }
            }
            //string _pathInfo = context.Request.PathInfo.Substring(1);

            //URL:      http://weixin.mobile.txooo.com/MessageReceive.aspx
            //Token :   535A32744558486A75484F36536F32502B6B317962513D3D

            switch (_type)
            {
                case PlatformType.Weixin:
                    if (CheckWeixinSignature(context))
                    {
                        context.Response.Write(new Weixin.RequestHelper().GetResponseMessage(context));
                    }
                    break;
                case PlatformType.Yixin:
                    if (CheckYixinSignature(context))
                    {
                        context.Response.Write(new Yixin.RequestHelper().GetResponseMessage(context));
                    }
                    break;
                case PlatformType.Feixin:
                    break;
                case PlatformType.Laiwan:
                    break;
                case PlatformType.Alipay:
                    break;
                default:
                    context.Response.Write("error");
                    break;
            }
        }

        #region 验证签名

        #region 验证微信签名

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CheckWeixinSignature(System.Web.HttpContext context)
        {
            string _pathInfo = context.Request.PathInfo.Substring(1);
            string _signature = context.Request.QueryString["signature"];

            string _timestamp = context.Request.QueryString["timestamp"];
            string _nonce = context.Request.QueryString["nonce"];
            string _token = Txooo.Text.EncryptHelper.MD5("tx_weixin_"+_pathInfo);

            List<string> _list = new List<string>();
            _list.Add(_token);
            _list.Add(_timestamp);
            _list.Add(_nonce);
            _list.Sort();
            string _sortInfo = "";
            foreach (var item in _list)
            {
                _sortInfo += item;
            }
            string _sha1 = Txooo.Text.EncryptHelper.SHA1(_sortInfo);
            if (_sha1 == _signature)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 验证易信签名

        /// <summary>
        /// 验证易信签名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CheckYixinSignature(System.Web.HttpContext context)
        {
            string _pathInfo = context.Request.PathInfo.Substring(1);
            string _signature = context.Request.QueryString["signature"];

            string _timestamp = context.Request.QueryString["timestamp"];
            string _nonce = context.Request.QueryString["nonce"];
            string _token = Txooo.Text.EncryptHelper.MD5("tx_yixin_" + _pathInfo);

            List<string> _list = new List<string>();
            _list.Add(_token);
            _list.Add(_timestamp);
            _list.Add(_nonce);
            _list.Sort();
            string _sortInfo = "";
            foreach (var item in _list)
            {
                _sortInfo += item;
            }
            string _sha1 = Txooo.Text.EncryptHelper.SHA1(_sortInfo);
            if (_sha1 == _signature)
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion
    }
}
