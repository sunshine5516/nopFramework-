using NopFramework.Core.Caching;
using NopFramework.Services.Cms;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Infrastructure.Cache;
using NopFramework.Web.Models.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NopFramework.Web.Controllers
{
    public class WidgetController : BaseController
    {
        #region 声明实例
        private readonly IWidgetService _widgetService;
        private readonly ICacheManager _cacheManager;
        #endregion
        #region 构造函数
        public WidgetController(IWidgetService widgetPlugin, ICacheManager cachemanager)
        {
            this._widgetService = widgetPlugin;
            this._cacheManager = cachemanager;
        }
        #endregion
        #region 方法
        // GET: Widget
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult WidgetsByZone(string widgetZone, object additionalData = null)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.WIDGET_MODEL_KEY, widgetZone);
            var cacheModel = _cacheManager.Get(cacheKey, () =>
            {
                //model
                var model = new List<RenderWidgetModel>();

                var widgets = _widgetService.LoadActiveWidgetsByWidgetZone(widgetZone);
                foreach (var widget in widgets)
                {
                    var widgetModel = new RenderWidgetModel();

                    string actionName;
                    string controllerName;
                    RouteValueDictionary routeValues;
                    widget.GetDisplayWidgetRoute(widgetZone, out actionName, out controllerName, out routeValues);
                    widgetModel.ActionName = actionName;
                    widgetModel.ControllerName = controllerName;
                    widgetModel.RouteValues = routeValues;

                    model.Add(widgetModel);
                }
                return model;
            });

            //如果没有Model返回为空字符串
            if (!cacheModel.Any())
                return Content("");

            //"RouteValues" property of widget models depends on "additionalData".
            //We need to clone the cached model before modifications (the updated one should not be cached)
            var clonedModel = new List<RenderWidgetModel>();
            foreach (var widgetModel in cacheModel)
            {
                var clonedWidgetModel = new RenderWidgetModel();
                clonedWidgetModel.ActionName = widgetModel.ActionName;
                clonedWidgetModel.ControllerName = widgetModel.ControllerName;
                if (widgetModel.RouteValues != null)
                    clonedWidgetModel.RouteValues = new RouteValueDictionary(widgetModel.RouteValues);

                if (additionalData != null)
                {
                    if (clonedWidgetModel.RouteValues == null)
                        clonedWidgetModel.RouteValues = new RouteValueDictionary();
                    clonedWidgetModel.RouteValues.Add("additionalData", additionalData);
                }

                clonedModel.Add(clonedWidgetModel);
            }

            return PartialView(clonedModel);
        }
        #endregion

    }
}