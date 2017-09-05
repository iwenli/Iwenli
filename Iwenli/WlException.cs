using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli
{
    /// <summary>
    /// 异常基础类
    /// </summary>
    public class WlException : Exception
    {
        public WlException(string message)
            : base(message)
        {

        }
    }
}
