using NopFramework.Services.Seo;
using NopFramework.Services.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IUrlRecordService _urlService;
        public HomeController(IScheduleTaskService scheduleTaskService,IUrlRecordService _urlService)
        {
            _scheduleTaskService = scheduleTaskService;
            this._urlService = _urlService;
        }
        public ActionResult Index()
        {
            //_urlService.DeleteUrlRecord();
            //using (var data = new Data.FrameworkObjectContext("Data Source=115.159.191.144;Initial Catalog=nopFrameworkDb;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=55165736sun!@#"))
            //{
            //    Core.Domains.Logging.Log code = new Core.Domains.Logging.Log();
            //    //var CodeFirst = data.Set<CodeFirst>().Find(2);
            //    var codes = data.Set<Core.Domains.Logging.Log>().FirstOrDefault();
            //    data.SaveChanges();

            //    //Core.Domains.Logging.Log code = new Core.Domains.Logging.Log();
            //    //var codes = data.Set<Core.Domains.Logging.Log>().FirstOrDefault();
            //}
            //var list= _scheduleTaskService.GetAllTasks();
            //_urlService.GetAllUrlRecords();
            //var conStr = "Data Source=115.159.191.144;Initial Catalog=nopFrameworkDb;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=55165736sun!@#";
            //var data = new Data.FrameworkObjectContext(conStr);
            ////Core.Demains.ScheduleTask t = new Core.Demains.ScheduleTask();

            //var t = data.Set<Core.Demains.ScheduleTask>().Find(2);
            //t.Id = 11;
            //t.Name = "test";
            //t.Type = "test";

            //data.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}