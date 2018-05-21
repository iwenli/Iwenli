using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Serialization
{
    public interface IStringSerializer
    {
        string ParseToStr<TFrom>(TFrom from);
    }
}
