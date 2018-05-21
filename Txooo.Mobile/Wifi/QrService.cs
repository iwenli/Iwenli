using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LitJson;

namespace Txooo.Mobile.Wifi
{
    public class QrService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        //public  static readonly IDictionary<string, string> TempQrList =  new Dictionary<string, string>();
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";

            try
            {
                string methodName = context.Request.PathInfo.Replace("/", "");
                System.Reflection.MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance
                        | BindingFlags.IgnoreCase
                        | BindingFlags.NonPublic);

                var fun = (Action<HttpContext>)method.CreateDelegate(typeof(Action<HttpContext>), this);

                fun(context);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        void Forever(HttpContext context)
        {
            //var param = Txooo.Text.EncryptHelper.AESDecrypt(context.Request["p"]);
            //var paramAry = param.Split(',');

            int _qrId, _accountId;
            if (int.TryParse(context.Request["qrid"], out _qrId)
                && int.TryParse(context.Request["accountid"], out _accountId))
            {
                var _helper = Txooo.Mobile.AccountInfo.GetAccountInfoByIdFromCache(_accountId).ApiHelper;

                string _ticket, _errorInfo;

                if (_helper.GetLimitQrcode(_qrId, out _ticket, out _errorInfo))
                {
                    //context.Response.Write(_ticket);
                    if (string.IsNullOrEmpty(context.Request["out"]))
                        context.Response.Write(_ticket);
                    else
                        context.Response.Redirect("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + _ticket);
                }
                else
                {
                    context.Response.Write("错误：" + _errorInfo);
                }
            }
        }

        void Temp(HttpContext context)
        {
            int _qrId, _accountId;
            if (int.TryParse(context.Request["qrid"], out _qrId)
                && int.TryParse(context.Request["accountid"], out _accountId))
            {
                var _helper = Txooo.Mobile.AccountInfo.GetAccountInfoByIdFromCache(_accountId).ApiHelper;

                string _ticket, _errorInfo;

                if (_helper.GetTempQrcode(_qrId, 1800, out _ticket, out _errorInfo))
                {
                    if (string.IsNullOrEmpty(context.Request["out"]))
                        context.Response.Write(_ticket);
                    else
                        context.Response.Redirect("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + _ticket);
                }
                else
                {
                    context.Response.Write("错误：" + _errorInfo);
                }
            }
        }

        void TempStatus(HttpContext context)
        {
            int _qrId, _accountId;
            int _status = 0;

            if (int.TryParse(context.Request["qrid"], out _qrId)
                && int.TryParse(context.Request["accountid"], out _accountId))
            {
                string key = _accountId + "_" + _qrId;

                if (QrFactory.TempQrList.ContainsKey(key))
                {
                    _status = 1;

                    var data = JsonMapper.ToObject(QrFactory.TempQrList[key]);                     
                    if (DateTime.Parse(data["createtime"].ToString()).AddSeconds(1700) < DateTime.Now)
                    {
                        QrFactory.TempQrList.Remove(key);
                        _status = 2;
                    }                  
                }
            }
            context.Response.Write(_status);
        }

        public bool IsReusable { get { return false; } }
    }

    public static class QrFactory
    {
        public static readonly IDictionary<string, string> TempQrList = new Dictionary<string, string>();      
    }
}
