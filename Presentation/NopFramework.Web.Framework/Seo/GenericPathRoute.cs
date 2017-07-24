using NopFramework.Web.Framework.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web;
using NopFramework.Core.Data;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Seo;

namespace NopFramework.Web.Framework.Seo
{
    /// <summary>
    /// 提供定义SEO友好路由的属性和方法，以及获取有关路由的信息。
    /// </summary>
    public partial class GenericPathRoute : LocalizedRoute
    {
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">路由的URL模式。</param>
        /// <param name="routeHandler">处理路由请求的对象.</param>
        public GenericPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">路由的URL模式</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值.</param>
        /// <param name="routeHandler">处理路由请求的对象</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">路由的URL模式</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值</param>
        /// <param name="constraints">指定URL参数有效值的正则表达式。</param>
        /// <param name="routeHandler">处理路由请求的对象.</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="url">路由的URL模式</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值</param>
        /// <param name="constraints">指定URL参数有效值的正则表达式。</param>
        /// <param name="dataTokens">传递给路由处理程序但不用于确定路由是否匹配特定URL模式的自定义值。 路由处理程序可能需要这些值来处理请求。</param>
        /// <param name="routeHandler">处理路由请求的对象.</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }
        #endregion
        #region 方法
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData data = base.GetRouteData(httpContext);
            if (data != null&& DataSettingsHelper.DatabaseIsInstalled())
            {
                ///
                var urlRecordService= EngineContext.Current.Resolve<IUrlRecordService>();
                var slug= data.Values["generic_se_name"] as string;//获取标识值
                var urlRecord = urlRecordService.GetBySlugCached(slug);
                if (urlRecord == null)
                {
                    //no URL record found

                    //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    //var response = httpContext.Response;
                    //response.Status = "302 Found";
                    //response.RedirectLocation = webHelper.GetStoreLocation(false);
                    //response.End();
                    //return null;

                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                    return data;
                }
                switch (urlRecord.EntityName.ToLowerInvariant())
                {
                    case "product":
                        {
                            data.Values["controller"] = "Product";
                            data.Values["action"] = "ProductDetails";
                            data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    default:
                        {
                            //no record found
                            //generate an event this way developers could insert their own types
                            //EngineContext.Current.Resolve<IEventPublisher>()
                            //    .Publish(new CustomUrlRecordEntityNameRequested(data, urlRecord));
                        }
                        break;
                }
                RouteData result = null;
                string requestURL = httpContext.Request.AppRelativeCurrentExecutionFilePath;
                //{
                data.Values["controller"] = "Home";
                data.Values["action"] = "Index";
                data.Values["productid"] = "1";
                data.Values["SeName"] = "hello";
            }

            return data;
            //return base.GetRouteData(httpContext);
        }
        #endregion
    }
}
