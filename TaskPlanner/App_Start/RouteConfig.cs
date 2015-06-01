using System.Web.Mvc;
using System.Web.Routing;

namespace TaskPlanner
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Task", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}