using System.Web.Mvc;
using System.Web.Routing;

namespace RetroShark.Application.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHubs();

            routes.MapRoute(
                   name: "Retrospective",
                   url: "{code}",
                   defaults: new { controller = "Retrospective", action = "Index" });

            routes.MapRoute(
                    name: "RetrospectiveController",
                    url: "r/{action}",
                    defaults: new {controller = "Retrospective"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}