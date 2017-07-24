using NopFramework.Web.Framework.Localization;
using NopFramework.Web.Framework.Mvc.Routes;
using NopFramework.Web.Framework.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace NopFramework.Web.Infrastructure
{
    /// <summary>
    /// 注册路由事件，注册一些友好路由
    /// </summary>
    public partial class GenericUrlRouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return -1000000;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapGenericPathRoute("GenericUrl",
                "{generic_se_name}",
                new { controller = "Common", action = "GenericUrl" },
                new[] { "NopFramework.Web.Controllers" });
            routes.MapLocalizedRoute(
               "Category",
               "{SeName}",
               new { controller = "Home", action = "Index" },
               new[] { "NopFramework.Web.Controllers" }
               );
        }
    }
}