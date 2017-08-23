using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Iwenli.Data.Entity
{
    /// <summary>
    /// 数据行类型的实体类，字段属性
    /// </summary>
    public class DataRowEntityFileAttribute : Attribute
    {
        /// <summary>
        /// 用于对数据类型的实体类字段，进行数据行字段属性标注
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="type">字段类型</param>
        public DataRowEntityFileAttribute(string name, Type type)
        {
            this._fieldName = name;
            this._fieldTitle = name;
            this._toStringFormate = string.Empty;
            this._fieldType = type;
        }
        /// <summary>
        /// 用于对数据类型的实体类字段，进行数据行字段属性标注
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <param name="title">字段标注</param>
        /// <param name="fieldType">字段类型</param>
        public DataRowEntityFileAttribute(string name, string title, Type type)
        {
            this._fieldName = name;
            this._fieldTitle = title;
            this._toStringFormate = string.Empty;
            this._fieldType = type;
        }

        private string _fieldName;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return this._fieldName; }
            set { this._fieldName = value; }
        }

        private string _fieldTitle;
        /// <summary>
        /// 字段说明
        /// </summary>
        public string FieldTitle
        {
            get { return _fieldTitle; }
            set { _fieldTitle = value; }
        }

        private string _toStringFormate;
        /// <summary>
        /// 字段格式化模板
        /// </summary>
        public string ToStringFormate
        {
            get { return _toStringFormate; }
            set { _toStringFormate = value; }
        }


        private Type _fieldType;
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType
        {
            get { return this._fieldType; }
            set { this._fieldType = value; }
        }
    }
}
