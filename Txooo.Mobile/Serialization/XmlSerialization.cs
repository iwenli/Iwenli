using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace Txooo.Mobile.Serialization
{
    public class XmlSerialization : IStringDeserializer, IStringSerializer
    {
        //public static string ToXml(object dest)
        //{
        //    try
        //    {
        //        XmlSerializer s = new XmlSerializer(dest.GetType());
        //        MemoryStream ms = new MemoryStream();
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Encoding = Encoding.UTF8;
        //        settings.Indent = true;
        //        settings.OmitXmlDeclaration = false;
        //        XmlWriter w = XmlWriter.Create(ms, settings);
        //        XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
        //        xmlSerializerNamespaces.Add("", "");
        //        s.Serialize(w, dest, xmlSerializerNamespaces);
        //        w.Close();
        //        ms.Seek(0, SeekOrigin.Begin);
        //        StreamReader r = new StreamReader(ms);
        //        string sXml = r.ReadToEnd();
        //        r.Close();
        //        ms.Close();
        //        return sXml;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //public static T FromXml<T>(string XmlString)
        //{
        //    try
        //    {
        //        XmlSerializer s = new XmlSerializer(typeof(T));
        //        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(XmlString));
        //        return (T)s.Deserialize(ms);
        //    }
        //    catch
        //    {
        //        return default(T);
        //    }
        //}


        public To ParseToObj<To>(string serializedText)
        {
            var type = typeof(To);
            return (To)Parse(serializedText, type);
        }

        public object Parse(string xml, Type type)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(xml);
                using (var reader = XmlDictionaryReader.CreateTextReader(bytes, new XmlDictionaryReaderQuotas()))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(type);
                    return serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("Error serializing object of type {0}", type.FullName), ex);
            }
        }

        public string ParseToStr<TFrom>(TFrom from)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (XmlWriter xw = new XmlTextWriter(ms, Encoding.UTF8))
                    {
                        var ser = new XmlSerializerWrapper(from.GetType());
                        ser.WriteObject(xw, from);
                        xw.Flush();
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("Error serializing object of type {0}", from.GetType().FullName), ex);
            }
        }
    }
}
