using Iwenli.Extension;
using Iwenli.Push.JPush.ReportApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Iwenli.Push.JPush.Util
{
    public class JsonTool
    {
        //// 从一个对象信息生成Json串        
        //public static string ObjectToJson(object obj)        
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    MemoryStream stream = new MemoryStream();
        //    serializer.WriteObject(stream, obj);
        //    byte[] dataBytes = new byte[stream.Length];
        //    stream.Position = 0;
        //    stream.Read(dataBytes, 0, (int)stream.Length);
        //    return Encoding.UTF8.GetString(dataBytes).Replace("\\", "");
        //}

        //// 从一个Json串生成对象信息        
        //public static T JsonToObject<T>(string jsonString)
        //{
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //    MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        //    return serializer.ReadObject(mStream);
        //}

        //// 从一个对象信息生成Json串        
        //public static string DictionaryToJson(Dictionary<String, Object> dict)
        //{
        //    StringBuilder json = new StringBuilder();
            
        //    foreach (KeyValuePair<String, Object> pair in dict) 
        //    {
        //        json.Append("\"").Append(pair.Key).Append("\"").Append(":").Append(ValueToJson(pair.Value)).Append(",");            
        //    }
        //    //Console.WriteLine("json String ******"+json);
        //    if (json.Length > 0) 
        //    {
        //        json.Remove(json.Length -1, 1);            
        //    }
        //    json.Append("}");
        //    json.Insert(0, "{");
            
        //    return json.ToString();
        //}

        //public static List<ReceivedResult.Received> JsonList(string jsonString)
        //{
        //    JavaScriptSerializer Serializer = new JavaScriptSerializer();
        //    List<ReceivedResult.Received> jsonclassList = Serializer.Deserialize<List<ReceivedResult.Received>>(jsonString);
        //    return jsonclassList;
        //}
        //从dictionary 的value中解析出字符串
        //private static string ValueToJson(object value)
        //{
        //    Type type = value.GetType();
        //    if(type==typeof(int)){

        //        return value.ToString();
        //    }else if(type==typeof(string)){

        //        return "\""+value+"\"";

        //    }else if(type==typeof(List<int>)||type==typeof(List<string>))
        //    {
        //        //return ObjectToJson(value);
        //        return JsonHelper.SerializeObject(value);
        //    }
        //    else if(type==typeof(Dictionary<string,object>))
        //    {
        //        //return JsonTool.DictionaryToJson((Dictionary<string, object>)value);
        //        return JsonHelper.SerializeObject((Dictionary<string, object>)value);
        //    }
        //    else{
        //        Debug.WriteLine("Type in Dictionary is error!");
        //        return "type erro";
        //    }
        //}
    }
}
