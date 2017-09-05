using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web.Security
{
    /// <summary>
    /// 用户对象的基本功能
    /// </summary>
    public abstract class Principal: IPrincipal
    {
        protected Identity _identity;

        //public Principal(string username)
        //{
        //    _identity = new Identity(username);
        //}

        #region IPrincipal 成员

        /// <summary>
        /// 获取当前用户的标识，接口成员（实现IIdentity接口）
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        /// <summary>
        /// 确定当前用户是否属于指定的角色，接口成员
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual bool IsInRole(string str)
        {
            string[] _list = str.Split(';');

            foreach (string item in _list)
            {
                string[] _item = item.Split(':');
                string _type = _item[0];
                //判断是否要执行否操作
                bool _ibool = false;
                if (_item[0].Length == 2)
                {
                    _ibool = true;
                    _type = _item[0].Substring(1, 1);
                }

                //计算表达式的值
                bool _expresserValue = false;

                #region 计算表达式的值

                if (_type == "R")
                {//角色认证
                    string[] _role = _item[1].Split(',');
                    foreach (string role in _role)
                    {
                        if (IsInWlRole(role))
                        {
                            _expresserValue = true;
                            break;
                        }
                    }
                }
                else if (_type == "P")
                {//权限认证
                    string[] _permission = _item[1].Split(',');
                    foreach (string permission in _permission)
                    {
                        if (IsInWlPermission(permission))
                        {
                            _expresserValue = true;
                            break;
                        }
                    }
                }
                else if (_type == "G")
                {//部门认证
                    string[] _group = _item[1].Split(',');
                    foreach (string group in _group)
                    {
                        if (IsInWlGroup(group))
                        {
                            _expresserValue = true;
                            break;
                        }
                    }
                }

                #endregion

                if (_ibool)
                {//执行否操作
                    _expresserValue = !_expresserValue;
                }

                if (_expresserValue)
                {
                    //通过验证，返回TRUE
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Wl角色认证
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public abstract bool IsInWlRole(string role);
        /// <summary>
        /// Wl权限认证
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public abstract bool IsInWlPermission(string permission);
        /// <summary>
        /// Wl部门认证
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public abstract bool IsInWlGroup(string role);
        /// <summary>
        /// Wl服务认证
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public abstract bool IsInWlService(string service);
        /// <summary>
        /// 目录认证
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract bool IsBrowseUrl(Url url);
    }
}
