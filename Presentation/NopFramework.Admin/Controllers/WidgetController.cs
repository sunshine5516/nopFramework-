using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public partial class WidgetController : Controller
    {
        // GET: Widget
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult WidgetsByZone(string widgetZone)
        {
            return View();
        }
    }
}