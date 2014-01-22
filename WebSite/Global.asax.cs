using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Cinchcast.Framework.Commands;
using Microsoft.AspNet.SignalR;
using RetroShark.Application.App_Start;
using RetroShark.Application.Backend.Autofac;
using RetroShark.Application.Backend.NHibernate;
using RetroShark.Application.Backend.Services;

namespace RetroShark.Application
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfiguration.Initialize();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            foreach(var cmdType in Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(Command).IsAssignableFrom(x)))
            {
                ModelBinders.Binders.Add(cmdType, new DependencyResolverModelBinder());
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            DependencyResolver.Current.GetService<IAuthenticationService>().SetPrincipal();
        }
    }
}