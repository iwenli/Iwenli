using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Push.JPush.Util
{
    class Preconditions
    {
        public static void CheckArgument(bool expression)
        {
            if (!expression)
            {
                throw new ArgumentNullException();
            }
        }
        public static void CheckArgument(bool expression, object errorMessage)
        {
            if (!expression)
            {
                throw new ArgumentException(errorMessage.ToString());
            }
        }
    }
}
