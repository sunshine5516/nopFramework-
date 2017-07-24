using NopFramework.Admin.Models.Tasks;
using NopFramework.Core.Demains;
using NopFramework.Services.Task;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    /// <summary>
    /// 计划任务控制器
    /// </summary>
    public class ScheduleTaskController : BaseAdminController
    {
        #region 实例对象
        IScheduleTaskService _scheduleTaskService;
        #endregion
        #region 构造函数
        public ScheduleTaskController(IScheduleTaskService scheduleTaskService)
        {
            this._scheduleTaskService = scheduleTaskService;
        }
        #endregion
        #region 方法
        // GET: ScheduleTask
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            var models = _scheduleTaskService.GetAllTasks(true).Select(PrepareScheduleTaskModel).ToList();
            //var models = _scheduleTaskService.GetAllTasks(true).ToList();
            var gridModel = new DataSourceResult
            {
                Data = models,
                Total = models.Count
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult TaskUpdate(ScheduleTaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = "" });
            }
            var scheduleTask = _scheduleTaskService.GetTaskById(task.Id);
            if (scheduleTask == null)
                return Content("Schedule task cannot be loaded");
            scheduleTask.Name = task.Name;
            scheduleTask.Seconds = task.Seconds;
            scheduleTask.StopOnError = task.StopOnError;
            scheduleTask.Enable = task.Enabled;
            _scheduleTaskService.UpdateTask(scheduleTask);
            return new JsonResult();
        }
        /// <summary>
        /// 执行计划任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RunNow(int id)
        {
            try
            {

                var scheduleTask = _scheduleTaskService.GetTaskById(id);
                if (scheduleTask == null)
                    throw new Exception("Schedule task cannot be loaded");
                var task=new Task(scheduleTask);
                task.Enabled = true;
                task.Execute(true, false, false);
                SuccessNotification("计划任务执行成功");
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
            }
            return RedirectToAction("List");
        }
        #endregion
        #region 辅助方法
        [NonAction]
        protected virtual ScheduleTaskModel PrepareScheduleTaskModel(ScheduleTask task)
        {
            var model = new ScheduleTaskModel
            {
                Id = task.Id,
                Name = task.Name,
                Seconds = task.Seconds,
                Enabled = task.Enable,
                StopOnError = task.StopOnError,
                LastStart = task.LastStartUtc,
                LastEnd = task.LastEndUtc,
                LastSuccess = task.LastSuccessUtc
                //LastStart = task.LastStartUtc.HasValue ? _dateTimeHelper.ConvertToUserTime(task.LastStartUtc.Value, DateTimeKind.Utc).ToString("G") : "",
                //LastEnd = task.LastEndUtc.HasValue ? _dateTimeHelper.ConvertToUserTime(task.LastEndUtc.Value, DateTimeKind.Utc).ToString("G") : "",
                //LastSuccess = task.LastSuccessUtc.HasValue ? _dateTimeHelper.ConvertToUserTime(task.LastSuccessUtc.Value, DateTimeKind.Utc).ToString("G") : "",
            };
            return model;
        }
        #endregion


    }
}