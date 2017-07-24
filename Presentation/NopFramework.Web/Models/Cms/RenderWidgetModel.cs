using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace NopFramework.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseNopFrameworkEntityModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}