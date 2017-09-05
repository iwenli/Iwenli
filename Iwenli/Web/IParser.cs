using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Web
{
    /// <summary>
    /// 解析容器接口,负责将相应的标签转到相应的类进行处理
    /// </summary>
    public interface IParser : IConfig
    {
        void Parse(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode);
    }
}
