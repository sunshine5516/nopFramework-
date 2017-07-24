using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace NopFramework.Web.Framework.Mvc.Routes
{
    public interface IRoutePublisher
    {
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        void RegisterRoutes(RouteCollection routes);
    }
}
