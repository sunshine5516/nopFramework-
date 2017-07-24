using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Plugins;
using NopFramework.Core;
using NopFramework.Core.Plugins;
using NopFramework.Services;
using NopFramework.Services.Task;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class PluginController : BaseController
    {
        #region 声明实例
        private readonly IPluginFinder _pluginFinder;
        IScheduleTaskService _scheduleTaskService;
        private readonly IWebHelper _webHelper;
        #endregion
        #region 构造函数
        public PluginController(IPluginFinder pluginFinder, IScheduleTaskService scheduleTaskService
            , IWebHelper webHelper)
        {
            this._pluginFinder = pluginFinder;
            this._scheduleTaskService = scheduleTaskService;
            this._webHelper = webHelper;
        }
        #endregion
        #region 方法
        // GET: Plugin
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            var model = new PluginListModel();
            model.AvailableLoadModes = LoadPluginsMode.InstalledOnly.ToSelectList(false).ToList();
            foreach (var g in _pluginFinder.GetPluginGroups())
            {
                model.AvailableGroups.Add(new SelectListItem { Text = g, Value = g });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ListSelect(DataSourceRequest command, PluginListModel model)
        {
            var loadMode = (LoadPluginsMode)model.SearchLoadModeId;
            var models = _scheduleTaskService.GetAllTasks(true).ToList();
            var pluginDescriptors = _pluginFinder.GetPluginDescriptors(loadMode, model.SearchGroup).ToList();
            var gridModel = new DataSourceResult
            {
                Data = pluginDescriptors.Select(x => PreparePluginModel(x, false, false)),
                //.OrderBy(x => x.Group)
                //.ToList(),
                //Data = pluginDescriptors,
                Total = 1
            };
            //var models = _scheduleTaskService.GetAllTasks(true).ToList();
            var gridModels = new DataSourceResult
            {
                Data = models,
                Total = models.Count
            };
            return Json(gridModel);
            //return Json(gridModel);
        }
        [HttpPost, ActionName("List")]
        [FormValueRequired(FormValueRequirement.StartsWith, "install-plugin-link-")]
        [ValidateInput(false)]
        public ActionResult Install(FormCollection form)
        {
            try
            {
                string systemName = null;
                foreach (var formValue in form.AllKeys)
                    if (formValue.StartsWith("install-plugin-link-", StringComparison.InvariantCultureIgnoreCase))
                        systemName = formValue.Substring("install-plugin-link-".Length);
                var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);
                ///未发现插件
                if (pluginDescriptor == null)
                    return RedirectToAction("List");
                ///插件是否安装
                if (pluginDescriptor.Installed)
                    return RedirectToAction("List");
                //安装插件
                pluginDescriptor.Instance().Install();
                SuccessNotification("安装成功");
                ///重新启动
                _webHelper.RestartAppDomain();
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
            }
            return RedirectToAction("List");
        }
        [HttpPost, ActionName("List")]
        [FormValueRequired(FormValueRequirement.StartsWith, "uninstall-plugin-link-")]
        [ValidateInput(false)]
        public ActionResult Uninstall(FormCollection form)
        {
            try
            {
                string systemName = null;
                foreach (var formValue in form.AllKeys)
                    if (formValue.StartsWith("uninstall-plugin-link-", StringComparison.InvariantCultureIgnoreCase))
                        systemName = formValue.Substring("uninstall-plugin-link-".Length);
                var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("List");

                //插件是否加载
                if (!pluginDescriptor.Installed)
                    return RedirectToAction("List");
                pluginDescriptor.Instance().Uninstall();
                SuccessNotification("卸载成功");
                ///重新启动
                _webHelper.RestartAppDomain();
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
            }
            return RedirectToAction("List");
        }

        public ActionResult EditPopup(string systemName)
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return RedirectToAction("List");
            var model = PreparePluginModel(pluginDescriptor);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditPopup(string btnId, string formId, PluginModel model)
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(model.SystemName, LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                pluginDescriptor.FriendlyName = model.FriendlyName;
                pluginDescriptor.DisplayOrder = model.DisplayOrder;
                PluginFileParser.SavePluginDescriptionFile(pluginDescriptor);
                _pluginFinder.ReloadPlugins();

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            return View(model);
        }


        #endregion
        #region 辅助方法
        protected virtual PluginModel PreparePluginModel(PluginDescriptor pluginDescriptor,
          bool prepareLocales = true, bool prepareStores = true)
        {
            var pluginModel = pluginDescriptor.ToModel();
            pluginModel.LogoUrl = pluginDescriptor.GetLogUrl(_webHelper);
            //logo
            //pluginModel.LogoUrl = pluginDescriptor.GetLogoUrl(_webHelper);

            //if (prepareLocales)
            //{
            //    //locales
            //    AddLocales(_languageService, pluginModel.Locales, (locale, languageId) =>
            //    {
            //        locale.FriendlyName = pluginDescriptor.Instance().GetLocalizedFriendlyName(_localizationService, languageId, false);
            //    });
            //}

            return pluginModel;
        }
        #endregion

    }
}