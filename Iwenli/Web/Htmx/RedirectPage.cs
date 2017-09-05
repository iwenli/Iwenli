namespace Iwenli.Web.Htmx
{
    /// <summary>
    /// 跳转页面
    /// </summary>
    public class RedirectPage : HtmxHandler1
    {
        public override void LoadMainTemplate()
        {
            string url = System.Text.RegularExpressions.Regex.Replace(Url.Current.AbsolutePath, _wlPage.MappedRegexStr, _wlPage.TemplateRegexStr);
            url = url.Replace("{#QueryString#}", Request.QueryString.ToString());
            Response.Redirect(url);
            Response.End();
        }
    }
}
