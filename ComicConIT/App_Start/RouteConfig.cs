using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComicConIT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Index",
               url: "Index",
               defaults: new { controller = "Comics", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Home2",
              url: "",
              defaults: new { controller = "Comics", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Best",
                url: "Best",
                defaults: new { controller = "Comics", action = "Index", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name:"New",
                url: "New",
                defaults: new { controller = "Comics", action = "New", id = UrlParameter.Optional }
                );
            routes.MapRoute(
               name: "My",
               url: "My",
               defaults: new { controller = "Comics", action = "My", id = UrlParameter.Optional }
               );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
