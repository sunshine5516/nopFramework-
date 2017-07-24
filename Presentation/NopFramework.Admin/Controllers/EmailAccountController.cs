using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Messages;
using NopFramework.Core.Domains.Messages;
using NopFramework.Services.Configuration;
using NopFramework.Services.Messages;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class EmailAccountController : BaseAdminController
    {
        #region 实例
        private readonly IEmailAccountService _emailAccountService;
        //private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IEmailSender _emailSender;
        private readonly EmailAccountSettings _emailAccountSettings;
        #endregion
        #region 构造函数
        public EmailAccountController(IEmailAccountService emailAccountService,
                ISettingService settingService, IEmailSender emailSender,
            EmailAccountSettings emailAccountSettings)
        {
            this._emailAccountService = emailAccountService;

            this._emailAccountSettings = emailAccountSettings;
            this._emailSender = emailSender;
            this._settingService = settingService;
        }
        #endregion
        #region 方法
        public ActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
            //    return AccessDeniedView();
            return View();
        }
        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            //var emailAccountModels = _emailAccountService.GetAllEmailAccounts().ToList();
            var tempModel = _emailAccountService.GetAllPagedList(command.Page - 1, command.PageSize);
            var Total = tempModel.TotalCount;
            var emailAccountModels = tempModel.Select(x => x.ToModel()).ToList();
            foreach (var eam in emailAccountModels)
                eam.IsDefaultEmailAccount = eam.Id == _emailAccountSettings.DefaultEmailAccountId;

            var gridModel = new DataSourceResult
            {
                Data = emailAccountModels,
                Total = tempModel.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new EmailAccountModel();
            model.Port = 25;
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(EmailAccountModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var emailAccount = model.ToEntity();
                //set password manually
                emailAccount.Password = model.Password;
                _emailAccountService.InsertEmailAccount(emailAccount);
                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(id);
            if (emailAccount == null)
                //No email account found with the specified id
                return RedirectToAction("List");

            return View(emailAccount.ToModel());
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]

        public ActionResult Edit(EmailAccountModel model, bool continueEditing)
        {

            var emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                //No email account found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                emailAccount = model.ToEntity(emailAccount);
                _emailAccountService.UpdateEmailAccount(emailAccount);

                //SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult MarkAsDefaultEmail(int id)
        {
            var defaultEmailAccount = _emailAccountService.GetEmailAccountById(id);
            if (defaultEmailAccount != null)
            {
                _emailAccountSettings.DefaultEmailAccountId = defaultEmailAccount.Id;
                _settingService.SaveSetting(_emailAccountSettings);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                _emailAccountService.DeleteEmailAccounts(_emailAccountService.GetEmailAccountsByIds(selectedIds.ToArray()));
            }
            return Json(new { Result = true });
        }
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("sendtestemail")]
        public ActionResult SendTestEmail(EmailAccountModel model)
        {
            var emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            try
            {
                string subject = "Hello Mote";
                string body = "Email works fine.";
                if (emailAccount == null)
                    //No email account found with the specified id
                    return RedirectToAction("List");
                _emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName,
                    model.SendTestEmailTo, null);
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc.Message, false);
            }

            return View(model);
        }
        #endregion
        // GET: EmailAccount
        public ActionResult Index()
        {
            return View();
        }

    }
}