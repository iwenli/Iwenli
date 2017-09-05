using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web
{
    /// <summary>
    /// 域名信息
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// 域名
        /// </summary>
        private string _domainInfo;
        /// <summary>
        /// 域名
        /// </summary>
        public string DomainInfo
        {
            get { return _domainInfo; }
        }
        /// <summary>
        /// 顶级域名（top level domain，.com）
        /// </summary>
        private string _tdl;
        /// <summary>
        /// 顶级域名（top level domain）
        /// </summary>
        public string TDL
        {
            get { return _tdl; }
        }
        /// <summary>
        /// 主域（二级域名,iwenli.com）
        /// </summary>
        private string _mainDomain;
        /// <summary>
        /// 主域
        /// </summary>
        public string MainDomain
        {
            get { return _mainDomain; }
        }
        /// <summary>
        /// 二级域名
        /// </summary>
        private string _secondDomain;
        /// <summary>
        /// 二级域名
        /// </summary>
        public string SecondDomain
        {
            get { return _secondDomain; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="domain"></param>
        public Domain(string domain)
        {
            _domainInfo = domain.ToLower();
            string[] _domain = _domainInfo.Split('.');

            if (_domain.Length > 2)
            {
                if ((_domain[_domain.Length - 2] == "com") || (_domain[_domain.Length - 2] == "net") || (_domain[_domain.Length - 2] == "org"))
                {//类com.cn,net.cn,org.cn站点
                    _tdl = _domain[_domain.Length - 2] + "." + _domain[_domain.Length - 1];
                    _mainDomain = _domain[_domain.Length - 3] + "." + _domain[_domain.Length - 2] + "." + _domain[_domain.Length - 1];
                    _secondDomain = _domainInfo.Replace("." + _mainDomain, "");
                }
                else
                {
                    _tdl = _domain[_domain.Length - 1];
                    _mainDomain = _domain[_domain.Length - 2] + "." + _domain[_domain.Length - 1];
                    _secondDomain = _domainInfo.Replace("." + _mainDomain, "");
                }
            }
            else
            {
                _mainDomain = _domainInfo;
            }
        }
    }
}
