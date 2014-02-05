
using System.Web.WebPages;
namespace System.Web.Mvc.Html
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper, bool excludePropertyErrors = true)
        {
            if (helper.ViewData.ModelState.IsValid)
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create("<div class=\"alert alert-danger alert-dismissable\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>" + helper.ValidationSummary(excludePropertyErrors, "Please review the errors in the page") + "</div>");
        }

        public static MvcHtmlString ViewModel(this WebPageContext pageContext)
        {
            string viewModelFile = pageContext.Page.VirtualPath.ToLowerInvariant().Replace("~", string.Empty).Replace(".cshtml", string.Empty);

            return MvcHtmlString.Create(string.Format("<script data-view-model type=\"plain/text\">{0}</script>", viewModelFile));
        }
    }
}