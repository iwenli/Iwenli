using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Iwenli.Data.Entity
{
    /// <summary>
    /// 数据行类型的实体类
    /// </summary>
    public abstract class DataRowEntityBase
    {
        /// <summary>
        /// 原始数据行
        /// </summary>
        protected DataRow m_originalDataRow;

        /// <summary>
        /// 初始化实体类,为提高速度，最好重载此函数，直接赋值
        /// </summary>
        /// <param name="row"></param>
        public virtual void InitEntity(DataRow row)
        {
            m_originalDataRow = row;
            DataEntityHelper.DataRowToEntity(this, row);
        }

        /// <summary>
        /// 获取数据行中对应数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual object this[string name]
        {
            get
            {
                if (m_originalDataRow.Table.Columns.Contains(name))
                {
                    return m_originalDataRow[name];
                }
                return null;
            }
        }

        #region 加载和保存当前对象

        #region 加载实体

        /// <summary>
        /// 加载实体
        /// </summary>
        /// <param name="str">文件路径或XML文本</param>
        /// <param name="isFile">是否为文件路径</param>
        /// <returns></returns>
        public virtual object LoadFromXml(string str, bool isFile)
        {
            if (isFile)
            {
                using (XmlReader writer = XmlReader.Create(str))
                {
                    return LoadFromXml(writer);
                }
            }
            else
            {
                using (XmlReader writer = XmlReader.Create(new System.IO.StringReader(str)))
                {
                    return LoadFromXml(writer);
                }
            }
        }

        /// <summary>
        /// 加载实体
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual object LoadFromXml(XmlReader reader)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
            return serializer.Deserialize(reader);
        }

        #endregion

        #region 写入

        /// <summary>
        /// 将当前实体XML格式写入文件
        /// </summary>
        /// <param name="filePath"></param>
        public virtual void WriteToXml(string filePath)
        {
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                WriteToXml(writer);
            }
        }

        /// <summary>
        /// 将当前实体XML格式写入XmlWriter
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteToXml(XmlWriter writer)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(String.Empty, String.Empty);
            serializer.Serialize(writer, this, xmlns);
        }

        /// <summary>
        /// 获取当前实体的XML格式
        /// </summary>
        /// <returns></returns>
        public virtual string GetXml()
        {
            StringBuilder _str = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(_str))
            {
                WriteToXml(writer);
            }
            return _str.ToString();
        }

        #endregion

        #endregion

    }
}
