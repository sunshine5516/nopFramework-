using NopFramework.Core.Infrastructure;
using NopFramework.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace NopFramework.Web.Framework.Mvc.Routes
{
    /// <summary>
    /// 实现接口IRoutePublisher，通过typeFinder.FindClassesOfType查找项目中所有实现了接口IRouteProvider的类，并依次注册其里面的路由。
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        protected readonly ITypeFinder typeFinder;
        #region 构造函数
        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }
        #endregion
        /// <summary>
        /// 根据位于其程序集中的某个类型找到一个插件描述符
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;
                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }
            return null;
        }
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            //通过typeFinder找出所有（包括插件）实现了路由接口IRouteProvider相关的类型
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                var plugin = FindPlugin(providerType);
                if (plugin != null && !plugin.Installed)
                    continue;
                //采用反射动态创建IRouteProvider的具体类的实例
                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
        }
    }
}
