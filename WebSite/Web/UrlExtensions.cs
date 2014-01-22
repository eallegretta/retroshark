
namespace System.Web.Mvc
{
    public static class UrlExtensions
    {
        public static string Absolute(this UrlHelper url, string relativeOrAbsolute)
        {
            var uri = new Uri(relativeOrAbsolute, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri)
            {
                return relativeOrAbsolute;
            }



            string hostHeader = url.RequestContext.HttpContext.Request.Headers["host"];

            return new Uri(string.Format("{0}://{1}{2}",
     url.RequestContext.HttpContext.Request.Url.Scheme,
     hostHeader,
     VirtualPathUtility.ToAbsolute(relativeOrAbsolute))).ToString();

        }
    }
}