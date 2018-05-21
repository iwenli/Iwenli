using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Pulpit.Ajax
{
    class PulpitData : Txooo.Web.Ajax.AjaxHandler
    {
        public string SendInfo()
        {
            bool _start = false;
            string _info = "";

            int _employeeId,_brandId;
            string _employeeName = Request["employee_name"];
            string _brandName = Request["brand_name"];
            string _name = Request["name"];
            string _mobile = Request["mobile"];
            string _post = Request["post"];
            PlatformType _platform = PlatformType.Weixin;
            string _platformUserSn = Request["platform_user_sn"];

            if (int.TryParse(Request["employee_id"], out _employeeId)
                && int.TryParse(Request["brand_id"], out _brandId)
                && !string.IsNullOrEmpty(_employeeName)
                && !string.IsNullOrEmpty(_brandName)
                && !string.IsNullOrEmpty(_name)
                && !string.IsNullOrEmpty(_mobile)
                && !string.IsNullOrEmpty(_post)
                && Enum.TryParse<PlatformType>(Request["platform"], out _platform)
                && PlatformUserInfo.DecryptPlatformUserSn(_platform, Request["platform_user_sn"], ref _platformUserSn)
               )
            {
                try
                {
                    ReceiptInfo _receipt = ReceiptInfo.GetReceiptInfoByOpenId(_platform, _platformUserSn);
                    if (_receipt != null)
                    {
                        _start = false;
                        _info = "您的参会回执已经提交，感谢您的支持！";
                    }
                    else
                    {

                        #region 入库
                        using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooMobile"))
                        {
                            string _sql = @"INSERT INTO [dbo].[Tx_Receipt]
           ([openId]
           ,[platform]
           ,[employee_id]
           ,[employee_name]
           ,[brand_id]
           ,[brand_name]
           ,[name]
           ,[mobile]
           ,[post])
     VALUES
           (@openId
           ,@platform
           ,@employee_id
           ,@employee_name
           ,@brand_id
           ,@brand_name
           ,@name
           ,@mobile
           ,@post)";
                            helper.SpFileValue["@openId"] = _platformUserSn;
                            helper.SpFileValue["@platform"] = (int)_platform;
                            helper.SpFileValue["@employee_id"] = _employeeId;
                            helper.SpFileValue["@employee_name"] = _employeeName;
                            helper.SpFileValue["@brand_id"] = _brandId;
                            helper.SpFileValue["@brand_name"] = _brandName;
                            helper.SpFileValue["@name"] = _name;
                            helper.SpFileValue["@mobile"] = _mobile;
                            helper.SpFileValue["@post"] = _post;

                            helper.SqlExecute(_sql, helper.SpFileValue);
                        }
                        #endregion

                        _start = true;
                    }
                }
                catch (Exception ex)
                {
                    this.TxLogError("邀请函入库失败：" + ex.Message);
                    _start = false;
                    _info = "服务器错误，请稍后再试！";
                }
            }
            else
            {
                _start = false;
                _info = "参数错误";
            }
            return "{'start':" + _start.ToString().ToLower() + ",'info':'" + _info + "'}";

        }
    }
}
