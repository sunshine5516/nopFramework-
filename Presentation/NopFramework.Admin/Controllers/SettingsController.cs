using NopFramework.Admin.Models.Settings;
using NopFramework.Services.Configuration;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class SettingsController : Controller
    {
        #region 声明实例
        private readonly ISettingService _settingService;
        #endregion
        #region 构造函数
        public SettingsController(ISettingService settingService)
        {
            this._settingService = settingService;
        }
        #endregion
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }
        #region 所有设置AllSettings
        public ActionResult AllSettings()
        {
            return View();
        }
        [HttpPost]
        //do not validate request token (XSRF)
        //for some reasons it does not work with "filtering" support
        public ActionResult AllSettings(DataSourceRequest command, AllSettingsListModel model)
        {
            var query = _settingService.GetAllSettings().AsQueryable();
            var settings = query.ToList().Select(
                x =>
                {
                    var settingModel = new SettingModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Value = x.Value
                    };
                    return settingModel;
                }
                ).AsQueryable();
            var gridModel = new DataSourceResult
            {
                Data = settings.ToList(),
                Total = settings.Count()
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult SettingAdd([Bind(Exclude = "Id")] SettingModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                //return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            _settingService.SetSetting(model.Name, model.Value);
            return new JsonResult();
        }
        [HttpPost]
        public ActionResult SettingUpdate(SettingModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                //return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var setting = _settingService.GetSettingById(model.Id);
            if (setting == null)
                return Content("No setting could be loaded with the specified ID");

            _settingService.SetSetting(model.Name, model.Value);
            return new JsonResult();
        }
        [HttpPost]
        public ActionResult SettingDelete(int id)
        {

            var setting = _settingService.GetSettingById(id);
            if (setting == null)
                throw new ArgumentException("No setting found with the specified id");
            _settingService.DeleteSetting(setting);
            return new JsonResult();
        }
        #endregion
    }
}