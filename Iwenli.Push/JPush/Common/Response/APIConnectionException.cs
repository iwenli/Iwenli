using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Push.JPush.Common.Response
{
   public class APIConnectionException:Exception
    {
        public APIConnectionException(String message):base(message)
        {
            
        }
    }
}
