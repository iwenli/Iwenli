using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iwenli
{
    /// <summary>
    /// Config通用接口
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 从XML节点中读取配置信息
        /// </summary>
        /// <param name="node">XML节点</param>
        void Load(XmlElement node);
    }
}
