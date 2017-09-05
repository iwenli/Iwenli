using System.Text;

namespace Iwenli.Web.Parse
{
    /// <summary>
    /// 模版标签处理
    /// </summary>
    public class TemplateParser : ParserBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TemplateParser()
        {
            _tagRegexStr = @"<!--Wl_TMP{(.*?)}-->";
        }

        public override void ParseTag(Url urlInfo, ISite site, ISkin skin, Page page, ref StringBuilder pageCode, Tag tag)
        {
            //模板名称
            string _tName = tag["tmp"];
            string _templateStr;
            if (!skin.TryGetTemplate(_tName, out _templateStr))
            {
                _templateStr = "<!--获取模板文件[" + _tName + "]出错-->";
                this.LogError("获取模板文件[" + _tName + "]出错，【" + urlInfo.ToString() + "】");
            }
            pageCode = pageCode.Replace(tag.TagStr, _templateStr);
        }
    }
}
