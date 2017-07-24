using NopFramework.Web.Framework.Localization;
using NopFramework.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace NopFramework.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapLocalizedRoute(
                "HomePage",
                "",
                new { controller = "Home", action = "Index" },
                new[] { "NopFramework.Web.Controllers" }
                );
            
            //routes.MapLocalizedRoute(
            //    "Category",
            //    "{SeName}",
            //    new { controller = "Home", action = "Index" },
            //    new[] { "NopFramework.Web.Controllers" });
        }
    }
}