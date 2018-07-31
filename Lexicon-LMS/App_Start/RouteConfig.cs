using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lexicon_LMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //This is suppose to remove the need of writing /Index but it seems to work anyway :)
            //routes.MapRoute("Course", "Course/{id}",
            //             new { controller = "Course", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Courses", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Notif",
                url: "Notificaitons/{action}/{id}",
                defaults: new { action = "Create", id = UrlParameter.Optional }
            );
        }


    }
}
