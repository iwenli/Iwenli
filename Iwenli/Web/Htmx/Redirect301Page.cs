namespace Iwenli.Web.Htmx
{
    /// <summary>
    /// 页面永久重定向
    /// </summary>
    public class Redirect301Page : HtmxHandler1
    {
        public override void LoadMainTemplate()
        {
            string url = System.Text.RegularExpressions.Regex.Replace(Url.Current.AbsolutePath, _wlPage.MappedRegexStr, _wlPage.TemplateRegexStr);

            Response.StatusCode = 301;
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", url);
            Response.End();
        }
    }
}
