using System.Text;
using System.Text.RegularExpressions;

namespace Iwenli.Web.Parse
{
    /// <summary>
    /// 解析表情数据，抽象类
    /// </summary>
    public abstract class ParserBase : IParser
    {
        protected string _tagRegexStr;
        /// <summary>
        /// 标签正则
        /// </summary>
        public string TagRegexStr
        {
            get { return _tagRegexStr; }
        }
        
        public virtual void Parse(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode)
        {
            if (string.IsNullOrEmpty(_tagRegexStr))
            {
                throw new WlException("解析表情出错，当前类没有标签匹配字符串串，请给[TagRegexStr]赋值");
            }

            //查询的正则表达式
            Regex re = new Regex(_tagRegexStr, RegexOptions.IgnoreCase);
            MatchCollection matches = re.Matches(pageCode.ToString());
            foreach (Match var in matches)
            {
                Tag tag = new Tag(var.Value, urlInfo);
                ParseTag(urlInfo, site, skin, page, ref pageCode, tag);
            }
        }

        public abstract void ParseTag(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode, Tag tag);

        public virtual void Load(System.Xml.XmlElement node)
        {

        }
    }
}
