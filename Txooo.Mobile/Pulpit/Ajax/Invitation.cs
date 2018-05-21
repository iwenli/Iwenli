using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Txooo.Mobile.Pulpit.Ajax
{
    /// <summary>
    /// 获取邀请函页面
    /// </summary>
    public class Invitation : Txooo.Web.Ajax.AjaxHandler
    {
        public string GetUrl()
        {
            string _errorInfo = "参数错误";

            if (!string.IsNullOrEmpty(Request["c"]))
            {
                string[] _info = Request["c"].Split(',');
                long _employeeId;
                long _manId;
                if (_info.Length == 2
                    && long.TryParse(_info[0], out _employeeId)
                    && long.TryParse(_info[1], out _manId)                    
                    )
                {
                    try
                    {
                        OA.OAUserInfo _userInfo = OA.OAUserInfo.GetUserInfoByUserId(_employeeId);
                        if (_userInfo != null)
                        {
                            using (Txooo.TxDataHelper helper = Txooo.TxDataHelper.GetDataHelper("TxoooCRM2013"))
                            {
                                string _sql = "SELECT [ContactmanId],[BrandId],[CustomersName],[name],[position],[mobile] FROM [dbo].[View_V4_OA_MFC_ContactMan] WHERE [ContactmanId]=" + _manId;
                                DataTable _table = helper.SqlGetDataTable(_sql);
                                if (_table.Rows.Count == 1)
                                {
                                    InviteInfo _inviteInfo = new InviteInfo(_employeeId);
                                    _inviteInfo.EmployeeName = _userInfo.Username;
                                    _inviteInfo.BrandId = int.Parse(_table.Rows[0][1].ToString());
                                    _inviteInfo.BrandName = _table.Rows[0][2].ToString();
                                    _inviteInfo.Name = _table.Rows[0][3].ToString();
                                    _inviteInfo.Position = _table.Rows[0][4].ToString();
                                    _inviteInfo.Mobile = _table.Rows[0][5].ToString();

                                    if (string.IsNullOrEmpty(_inviteInfo.Name)  || _inviteInfo.Name.Length < 2 )
                                    {
                                        _errorInfo = "请填写正确的姓名";
                                    }
                                    else if(string.IsNullOrEmpty(_inviteInfo.Position) || _inviteInfo.Position.Length < 2 )
                                    {
                                        _errorInfo = "请填写正确的职务";
                                    }
                                    else if (string.IsNullOrEmpty(_inviteInfo.Mobile) || !Txooo.TxStringHelper.IsMobile(_inviteInfo.Mobile) )
                                    {
                                        _errorInfo = "请填写正确的手机";
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(_inviteInfo.Name)
                                            && !string.IsNullOrEmpty(_inviteInfo.Position)
                                            && !string.IsNullOrEmpty(_inviteInfo.Mobile)
                                            && _inviteInfo.Name.Length > 1
                                            && Txooo.TxStringHelper.IsMobile(_inviteInfo.Mobile)
                                            )
                                        {
                                            long QrcodeId = _inviteInfo.GetQrcodeId();
                                            if (QrcodeId > 0)
                                            {
                                                Response.Redirect("http://pulpit.txooo.com/Invitation.htm?id=" + Txooo.Text.EncryptHelper.AESEncrypt(QrcodeId.ToString("000000")));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorInfo = "服务器错误，请稍后再试！";
                    }
                }
            }

            return _errorInfo;
        }
    }
}
