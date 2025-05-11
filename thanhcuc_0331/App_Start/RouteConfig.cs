using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace thanhcuc_0331
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Books",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Books", action = "ListBooks", id = UrlParameter.Optional }
            );
        }
    }
}
