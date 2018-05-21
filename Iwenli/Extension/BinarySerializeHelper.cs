#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：BinarySerializeHelper
 *  所属项目：System
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/21 15:58:06
 *  
 *  功能描述：
 *          1、
 * 
 *  修改标识：  
 *  修改描述：
 *  修改时间：
 *  待 完 善：
 *          1、 
----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 二进制序列化辅助类
    /// </summary>
    public static class BinarySerializeHelper
    {

        /// <summary>
        /// 从文件中反序列化对象
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns>原对象</returns>
        public static object DeserializeFromFile(string FileName)
        {
            using (System.IO.FileStream stream = new FileStream(FileName, FileMode.Open))
            {
                object res = stream.DeserializeFromStream();
                stream.Dispose();
                return res;
            }
        }

        /// <summary>
        /// 从字节数组中反序列化
        /// </summary>
        /// <param name="array">字节数组</param>
        /// <returns>序列化结果</returns>
        public static object DeserialzieFromBytes(byte[] array)
        {
            object result = null;
            if (array == null || array.Length == 0)
                return result;

            using (var ms = new System.IO.MemoryStream())
            {
                ms.Write(array, 0, array.Length);
                ms.Seek(0, SeekOrigin.Begin);

                result = ms.DeserializeFromStream();
                ms.Dispose();
            }

            return result;
        }

        /// <summary>
		/// 序列化对象到文件
		/// </summary>
		/// <param name="ObjectToSerilize">要序列化的对象</param>
		/// <param name="FileName">保存到的文件路径</param>
		public static void SerializeToFile(this object ObjectToSerialize, string FileName)
        {
            if (ObjectToSerialize == null || String.IsNullOrEmpty(FileName))
                return;

            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                SerializeToStream(ObjectToSerialize, stream);
                stream.Dispose();
            }
        }

        /// <summary>
        /// 序列化对象到字节数组
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <returns>返回创建后的字节数组</returns>
        public static byte[] SerializeToBytes(this object objectToSerialize)
        {
            byte[] result = null;
            if (objectToSerialize == null)
                return result;

            using (var ms = new MemoryStream())
            {
                objectToSerialize.SerializeToStream(ms);
                ms.Dispose();
                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// 序列化对象到流
        /// </summary>
        /// <param name="objectToSerialize">要序列化的对象</param>
        /// <param name="stream">保存对象信息的流</param>
        public static void SerializeToStream(this object objectToSerialize, Stream stream)
        {
            if (objectToSerialize == null || stream == null)
                return;

            BinaryFormatter xso = new BinaryFormatter();
            xso.Serialize(stream, objectToSerialize);
        }

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>反序列化的对象</returns>
        public static object DeserializeFromStream(this Stream stream)
        {
            object result = null;
            if (stream == null)
                return result;

            BinaryFormatter xso = new BinaryFormatter();
            result = xso.Deserialize(stream);

            return result;
        }
    }
}
