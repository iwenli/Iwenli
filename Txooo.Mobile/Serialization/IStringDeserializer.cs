using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile.Serialization
{
    public interface IStringDeserializer
    {
        To ParseToObj<To>(string serializedText);
        //object Parse(string serializedText, Type type);
    }
}
