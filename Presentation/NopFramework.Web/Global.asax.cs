using NopFramework.Core.Data;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Task;
using NopFramework.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NopFramework.Web
{
    public class MvcApplication : HttpApplication
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //注册自定义的路由规则及插件相关路由
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);
            //routes.MapRoute(
            //    "HomePage",
            //    "",
            //    new { controller = "Home", action = "Index" },
            //    new[] { "NopFramework.Web.Controllers" }
            //    );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "NopFramework.Web.Controllers" }
            );

        }
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            EngineContext.Initialize(false);

            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();

            //启动数据库中定时任务
            //初始化所有数据库中创建的定时任务
            if (databaseInstalled)
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }

            //注册常用的MVC物件，包括 Area ,Route
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //TaskManager.Instance.Initialize();
            //TaskManager.Instance.Start();
        }
    }
}
