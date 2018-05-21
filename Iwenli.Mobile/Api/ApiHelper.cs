#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：ApiHelper
 *  所属项目：Iwenli.Mobile.Api
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 17:01:32
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

using Iwenli.Mobile.Platform;
using Iwenli.Mobile.Platform.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Mobile.Api
{
    /// <summary>
    /// 第三方接口处理类
    /// </summary>
    public abstract class ApiHelper
    {
        #region 获取远程access_token
        /// <summary>
        ///  获取远程access_token
        /// </summary>
        /// <param name="returnInfo"></param> 
        /// <returns></returns>
        public abstract bool GetAccessTokenByRemote(out string returnInfo);

        #endregion

        #region 发送客服消息

        /// <summary>
        /// 发送客服消息,如果报错，重复发送
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool SendMessage(int resetNumber, ResMsg msg, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = SendMessage(msg, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 向用户发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool SendMessage(ResMsg msg, out string errorInfo);
        /// <summary>
        /// 根据openid群发
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool SendAllByOpenId(string openIds, Platform.ResMsg msg, out string errorInfo);
        /// <summary>
        /// 根据分组群发
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="msg"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool SendAllByGroup(long groupId, Platform.ResMsg msg, out string errorInfo);

        #endregion

        #region 获取用户列表

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="userList"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool GetAllUserList(int resetNumber, out List<string> userList, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = GetAllUserList(out userList, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool GetAllUserList(out List<string> userList, out string errorInfo);

        #endregion

        #region 获取用户信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="openId"></param>
        /// <param name="userInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool GetUserInfo(int resetNumber, string openId, ref UserInfo userInfo, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = GetUserInfo(openId, ref userInfo, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool GetUserInfo(string openId, ref UserInfo userInfo, out string errorInfo);

        #endregion

        #region 删除菜单数据

        /// <summary>
        /// 删除菜单数据
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool DeleteMenu(int resetNumber, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = DeleteMenu(out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 删除菜单数据
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool DeleteMenu(out string errorInfo);

        #endregion

        #region 推送菜单数据

        /// <summary>
        /// 推送菜单数据
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="menu"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool PostMenu(int resetNumber, MenuInfo[] menu, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = PostMenu(menu, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 推送菜单数据
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool PostMenu(MenuInfo[] menu, out string errorInfo);

        #endregion

        #region 获取永久二维码

        /// <summary>
        /// 获取永久二维码
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool GetLimitQrcode(int resetNumber, int id, out string ticket, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = GetLimitQrcode(id, out ticket, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 获取永久二维码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool GetLimitQrcode(int id, out string ticket, out string errorInfo);

        #endregion

        #region 获取临时二维码

        /// <summary>
        /// 获取临时二维码,可以重复次数
        /// </summary>
        /// <param name="resetNumber">重复发送次数</param>
        /// <param name="sceneId"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool GetTempQrcode(int resetNumber, int sceneId, int expireSeconds, out string ticket, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = GetTempQrcode(sceneId, expireSeconds, out ticket, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }
        /// <summary>
        /// 获取临时二维码
        /// </summary>
        /// <param name="sceneId"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="ticket"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool GetTempQrcode(int sceneId, int expireSeconds, out string ticket, out string errorInfo);

        #endregion

        #region 用户分组操作

        /// <summary>
        /// 获取用户分组信息
        /// </summary>
        /// <param name="resetNumber"></param>
        /// <param name="userGroup"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool GetUserGroup(int resetNumber, out List<UserGroupInfo> userGroup, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = GetUserGroup(out userGroup, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }

        /// <summary>
        /// 获取用户分组信息
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool GetUserGroup(out List<UserGroupInfo> userGroup, out string errorInfo);


        /// <summary>
        /// 创建用户分组信息
        /// </summary>
        /// <param name="resetNumber"></param>
        /// <param name="userGroup"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool CreateGroup(int resetNumber, UserGroupInfo groupInfo, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = CreateGroup(groupInfo, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }

        /// <summary>
        /// 创建用户分组
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool CreateGroup(UserGroupInfo groupInfo, out string errorInfo);

        /// <summary>
        /// 修改用户分组名称
        /// </summary>
        /// <param name="resetNumber"></param>
        /// <param name="groupInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool ModifyGroupname(int resetNumber, UserGroupInfo groupInfo, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = ModifyGroupname(groupInfo, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }

        /// <summary>
        /// 修改分组名称
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool ModifyGroupname(UserGroupInfo groupInfo, out string errorInfo);

        /// <summary>
        /// 移动用户到分组
        /// </summary>
        /// <param name="resetNumber"></param>
        /// <param name="openId"></param>
        /// <param name="toGroupID"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public virtual bool MoveUserToGroup(int resetNumber, string openId, int toGroupID, out string errorInfo)
        {
            int _number = 0;
            bool _isOk = false;
            do
            {
                _number++;
                _isOk = MoveUserToGroup(openId, toGroupID, out errorInfo);
            } while (!_isOk && _number < resetNumber);
            return _isOk;
        }

        /// <summary>
        /// 移动用户到分组
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="groupID"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public abstract bool MoveUserToGroup(string openId, int toGroupID, out string errorInfo);


        /// <summary>
        /// 上传素材媒体
        /// </summary>
        /// <param name="resMsg"></param>
        /// <returns></returns>
        public abstract bool UploadMedia(ResMsg resMsg, out string returnInfo);

        #endregion

        #region 设置备注名

        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId">openid</param>
        /// <param name="remark">备注名</param>
        /// <param name="errorInfo">输出参数，返回的错误消息</param>
        /// <returns></returns>
        public virtual bool UpdateRemark(string openId, string remark, out string errorInfo)
        {
            errorInfo = "";
            return false;
        }
        #endregion

        #region 发送模板消息
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="returnInfo"></param>
        /// <returns></returns>
        public virtual bool SendTempMsg(ResTemplateMsg templateMsg, out string returnInfo)
        {
            returnInfo = "";
            return false;
        }
        #endregion
    }
}
