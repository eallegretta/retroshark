using System.Web.Mvc;
using RetroShark.Application.Backend.NHibernate;

namespace RetroShark.Application.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NHibernateFilter());
        }
    }
}