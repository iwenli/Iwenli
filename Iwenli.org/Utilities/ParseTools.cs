#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：ParseTools
 *  所属项目：Iwenli.Org.Utilities
 *  创建用户：iwenli(HouWeiya)
 *  创建时间：2018/5/25 14:42:47
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

using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Iwenli.Org.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class ParseTools
    {
        public static Regex _valueTagRegex = new Regex(@"\{\$(.*?)(\{(.*?)\})?\}",
                RegexOptions.Compiled
                | RegexOptions.IgnoreCase
                | RegexOptions.Singleline);
        static Regex m_ifTagRegex = new Regex(@"when(.*?)then(.*?)");

        /// <summary>
        /// 解析模板标签
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ParseDataTable(DataTable dt, string tmp)
        {
            StringBuilder _sb = new StringBuilder();
            var _matches = _valueTagRegex.Matches(tmp);

            foreach (DataRow dr in dt.Rows)
            {
                string _tempCode = tmp;
                foreach (Match item in _matches)
                {
                    string _valueTagStr = item.Groups[0].Value;

                    //逻辑标签
                    //if (m_ifTagRegex.IsMatch(_valueTagStr))
                    //{
                    //    _tempCode = _tempCode.Replace(_valueTagStr, FormatIfTag(_valueTagStr, dr));
                    //    continue;
                    //}

                    string _formatStr = item.Groups[2].Value;
                    string _colName = item.Groups[1].Value;
                    if (dt.Columns.Contains(_colName))
                    {
                        string _colValue = dr[_colName].ToString();

                        if (_formatStr == "")
                        {
                            _tempCode = _tempCode.Replace(_valueTagStr, _colValue);
                        }
                        else
                        {
                            _tempCode = _tempCode.Replace(_valueTagStr, FormatTag(item.Groups[3].Value, _colValue, dr));
                        }
                    }
                }
                _sb.Append(_tempCode);
            }
            return _sb.ToString();
        }

        public static string ParseArrayList<T>(IList<T> list, string tmp)
        {
            if (list == null || list.Count == 0)
                return "";
            PropertyInfo[] _propertys = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            StringBuilder _sb = new StringBuilder();
            var _matches = _valueTagRegex.Matches(tmp);

            foreach (T obj in list)
            {
                string _tempCode = tmp;
                foreach (Match item in _matches)
                {
                    string _valueTagStr = item.Groups[0].Value;
                    string _formatStr = item.Groups[2].Value;

                    string _properName = item.Groups[1].Value;
                    var _propInfo = _propertys.SingleOrDefault(p => { return p.Name == _properName; });

                    if (_propInfo != null)
                    {
                        string _properValue = Convert.ToString(_propInfo.GetValue(obj));

                        if (_formatStr == "")
                        {
                            _tempCode = _tempCode.Replace(_valueTagStr, _properValue);
                        }
                        else
                        {
                            _tempCode = _tempCode.Replace(_valueTagStr, FormatTag(item.Groups[3].Value, _properValue, obj));
                        }
                    }
                }
                _sb.Append(_tempCode);
            }
            return _sb.ToString();
        }

        public static void ParseJson(JsonData json, ref string _temp)
        {
            IDictionary dic = (IDictionary)json;

            foreach (DictionaryEntry item in dic)
            {
                _temp = _temp.Replace("{$" + item.Key.ToString() + "}", item.Value.ToString());
            }
        }

        public static string FormatTag(string formatStr, string colValue, object rowData)
        {
            NameValueCollection _formatParam = new NameValueCollection();
            var _formatAry = formatStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in _formatAry)
            {
                var _paramAry = item.Split('=');

                _formatParam[_paramAry[0]] = _paramAry[1];
            }

            string _formatType = _formatParam["type"];

            #region 替换待完善

            string _replaceField = _formatParam["field"];
            if (!string.IsNullOrEmpty(_replaceField) && rowData != null)
            {
                var _ifValue = _formatParam["map"].ToString().Split('|');
                if (rowData is DataRow)
                {
                    if (Convert.ToString(colValue) == _ifValue[0])
                    {
                        colValue = string.Format(_ifValue[1], ((DataRow)rowData)[_replaceField].ToString());
                    }
                }
                else
                {
                    if (Convert.ToString(colValue) == _ifValue[0])
                    {
                        PropertyInfo[] _propertys = rowData.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        var _propInfo = _propertys.SingleOrDefault(p => { return p.Name == _replaceField; });
                        colValue = string.Format(_ifValue[1], _propInfo.GetValue(rowData).ToString());
                    }
                }
            }
            #endregion

            //格式化日期
            if (_formatType == "date")
            {
                return DateTime.Parse(colValue).ToString(_formatParam["format"]);
            }

            //格式化字符串
            if (_formatType == "str")
            {
                int _len = int.Parse(_formatParam["len"]);
                if (colValue.Length > _len)
                {
                    if (_formatParam["style"] == "none") //截取
                        return colValue.Substring(0, _len);

                    if (_formatParam["style"] == "last") //截取后
                        return colValue.Substring((colValue.Length - _len), _len);

                    if (_formatParam["style"] == "dotted")//截取加省略号
                        return colValue.Substring(0, _len) + "...";
                }
            }
            //货币保留小数点后两位
            if (_formatType == "money")
            {
                if (string.IsNullOrEmpty(colValue))
                {
                    colValue = "0";
                }
                return Convert.ToDouble(colValue).ToString(string.Format("0.{0}", _formatParam["style"]));
            }
            //数字转中文
            if (_formatType == "zh")
            {
                var _mapList = _formatParam["map"].ToString().Split(',');

                foreach (string item in _mapList)
                {
                    var _itemSplit = item.Split('|');

                    if (colValue == _itemSplit[0])
                    {
                        return _itemSplit[1];
                    }
                }
            }
            //设置无数据时默认值（只有ctype=1的内容标签有效）
            if (_formatType == "def" && colValue == "")
            {
                colValue = _formatParam["val"].ToString();
            }

            return colValue;
        }

        static Regex m_calculateRegex = new Regex(@"[\>|\>\=|\=|\<|\<\=|\!\=]");
        /// <summary>
        /// 带完善未使用
        /// </summary>
        /// <param name="formatStr"></param>
        /// <param name="colValue"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private static string FormatIfTag(string valueTagStr, object rowData)
        {
            //valueTagStr = Txooo.TxStringHelper.GetMiddleStr(valueTagStr, "{$", "}");
            //string _else = null;
            //if (valueTagStr.IndexOf("else") > -1)
            //{
            //    _else = valueTagStr.Split(new string[] { "else" }, StringSplitOptions.None)[1].Trim();
            //}
            //Regex ifRegex = new Regex(@"when(.*?)then(.*?);");

            //var _matches = ifRegex.Matches(valueTagStr);

            //foreach (Match item in _matches)
            //{
            //    string _when = item.Groups[1].Value.Trim();
            //    var _graph = m_calculateRegex.Match(_when).Groups[0].Value;
            //    string _then = item.Groups[2].Value.Trim();

            //    switch (_graph)
            //    {
            //        case "=":
            //            string _left = _when.Split('=')[0].IndexOf("$")>-1?
            //            string _right = _when.Split('=')[1];
            //            break;

            //        case ">=":
            //            break;
            //        case ">":
            //            break;
            //        case "<":
            //            break;
            //        case "<=":
            //            break;
            //        case "!=":
            //            break;
            //    }


            //}
            return "";
        }

        private static string GetObjectvalue(string field, object rowData)
        {
            if (rowData is DataRow)
            {
                return ((DataRow)rowData)[field].ToString();
            }
            else
            {
                PropertyInfo[] _propertys = rowData.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                var _propInfo = _propertys.SingleOrDefault(p => { return p.Name == field; });
                return _propInfo.GetValue(rowData).ToString();
            }
        }
    }
}
