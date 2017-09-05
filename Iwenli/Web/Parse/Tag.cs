using System.Collections.Specialized;

namespace Iwenli.Web.Parse
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : NameValueCollection
    {
        /// <summary>
        /// 要解析的标签正则
        /// </summary>
        public const string TAG_REGEX = "<!--WL_(.*?){(.*?)}-->";

        #region 私有变量

        private Url _urlInfo;
        private string _tagName;
        private string _tagStr;
        private string _tagAttributeStr;

        #endregion

        #region 属性

        /// <summary>
        /// URL地址
        /// </summary>
        public Url UrlInfo
        {
            get { return _urlInfo; }
        }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName
        {
            get { return _tagName; }
        }

        /// <summary>
        /// 标签属性串
        /// </summary>
        public string TagAttributeStr
        {
            get { return _tagAttributeStr; }
        }

        /// <summary>
        /// 标签字符串
        /// </summary>
        public string TagStr
        {
            get { return _tagStr; }
        }


        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tagStr"></param>
        /// <param name="urlInfo"></param>
        public Tag(string tagStr, Url urlInfo)
        {
            _urlInfo = urlInfo;

            _tagStr = tagStr;
            _tagName = System.Text.RegularExpressions.Regex.Replace(tagStr, TAG_REGEX, "$1", System.Text.RegularExpressions.RegexOptions.IgnoreCase).ToUpper();
            //获得属性值
            _tagAttributeStr = System.Text.RegularExpressions.Regex.Replace(tagStr, TAG_REGEX, "$2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string[] tagArray = _tagAttributeStr.Trim().Split(';');
            foreach (string var in tagArray)
            {
                if (!string.IsNullOrEmpty(var)
                    && var.Contains("="))
                {
                    string _key = var.Substring(0, var.IndexOf("="));
                    string _value = var.Substring(var.IndexOf("=")+1);
                    this[_key] = _value;
                }
            }
            //获得动态属性
            if (!string.IsNullOrEmpty(this["regex"]))
            {
                string _url = urlInfo.AbsolutePath;
                if (this["regex"].Substring(0, 1) != "/")
                {//带域名匹配
                    _url = urlInfo.Domain.DomainInfo + "/" + urlInfo.AbsolutePath;
                }
                foreach (string var in this.AllKeys)
                {
                    if (this[var].Substring(0, 1) == "$")
                    {
                        this[var] = System.Text.RegularExpressions.Regex.Replace(_url, this["regex"], this[var]);
                    }
                }
            }
        }

        #endregion
    }
}
