using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class CommonController : Controller
    {
        #region 方法
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            return View();
        }
        #endregion
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
    }
}