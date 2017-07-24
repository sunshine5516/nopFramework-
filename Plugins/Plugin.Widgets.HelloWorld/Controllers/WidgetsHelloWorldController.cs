
using Nop.Plugin.Widgets.HelloWorld.Models;
using NopFramework.Web.Framework.Controllers;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.HelloWorld.Controllers
{
    public class WidgetsHelloWorldController : BasePluginController
    {
        public WidgetsHelloWorldController() { }
        [ChildActionOnly]
       
        public ActionResult Configure()
        {
            return View("~/Plugins/Widgets.HelloWorld/Views/WidgetsHelloWorld/Configure.cshtml");
        }
        [HttpPost]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            return Configure();
        }
        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            return View("~/Plugins/Widgets.HelloWorld/Views/WidgetsHelloWorld/PublicInfo.cshtml", null);
        }
    }
}
