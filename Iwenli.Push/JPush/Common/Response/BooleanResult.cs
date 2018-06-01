using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Push.JPush.Common.Response
{
    public class BooleanResult : DefaultResult 
    {
	     public bool result;
         new public static BooleanResult fromResponse(ResponseWrapper responseWrapper)
         {
             BooleanResult tagListResult = new BooleanResult();
             if (responseWrapper.isServerResponse())
             {
                 tagListResult = JsonConvert.DeserializeObject<BooleanResult>(responseWrapper.responseContent);
             }
             tagListResult.ResponseResult = responseWrapper;
             return tagListResult;
         }
    }

}
