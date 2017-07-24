using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Messages;
using NopFramework.Core.Domains.Messages;
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
    public class QueuedEmailController : BaseAdminController
    {
        #region 声明实例
        IQueuedEmailService _queuedEmailService;
        //private readonly IDateTimeHelper _dateTimeHelper;
        #endregion
        #region 构造函数
        public QueuedEmailController(IQueuedEmailService queuedEmailService)
        {
            this._queuedEmailService = queuedEmailService;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(DataSourceRequest command)
        {
            var model = new QueuedEmailListModel
            {
                //default value
                SearchMaxSentTries = 10
            };
            return View(model);
            //return View();
        }
        [HttpPost]
        public ActionResult QueuedEmailList(DataSourceRequest command, QueuedEmailListModel model)
        {
            DateTime? startDateValue = model.SearchStartDate;
            DateTime? endDateValue = model.SearchEndDate;
            //DateTime? startDateValue = (model.SearchStartDate == null) ? null
            //               : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SearchStartDate.Value, _dateTimeHelper.CurrentTimeZone);

            //DateTime? endDateValue = (model.SearchEndDate == null) ? null
            //                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SearchEndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            //var queuedEmails = _queuedEmailService.GetAllEmails();
            var queuedEmails = _queuedEmailService.SearchEmails(model.SearchFromEmail, model.SearchToEmail,
                startDateValue, endDateValue,model.SearchLoadNotSent,false,
                model.SearchMaxSentTries,true, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = queuedEmails,
                Total = queuedEmails.Count()
            };
            return Json(gridModel);
        }
        public ActionResult Edit(int id)
        {
            var email = _queuedEmailService.GetQueuedEmailById(id);
            if (email == null)
                return RedirectToAction("List");
            var model = email.ToModel();
            //model.PriorityName = email.Priority.GetLocalizedEnum(_localizationService, _workContext);
            //model.CreatedOn = _dateTimeHelper.ConvertToUserTime(email.CreatedOnUtc, DateTimeKind.Utc);
            //if (email.SentOnUtc.HasValue)
            //    model.SentOn = _dateTimeHelper.ConvertToUserTime(email.SentOnUtc.Value, DateTimeKind.Utc);
            //if (email.DontSendBeforeDateUtc.HasValue)
            //    model.DontSendBeforeDate = _dateTimeHelper.ConvertToUserTime(email.DontSendBeforeDateUtc.Value, DateTimeKind.Utc);
            //else model.SendImmediately = true;
            return View(model);
        }
        /// <summary>
        /// 从客户端()中检测到有潜在危险的 Request.Form 值 解决方法 -- [ValidateInput(false)]或者在字段前添加[AllowHtml]
        /// </summary>
        /// <param name="model"></param>
        /// <param name="continueEditing"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
       
        public ActionResult Edit(QueuedEmailModel model, bool continueEditing)
        {
            var email = _queuedEmailService.GetQueuedEmailById(model.Id);
            if (email == null)
                //No email found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                email = model.ToEntity(email);
                _queuedEmailService.UpdateQueuedEmail(email);
                SuccessNotification("更新邮件成功");
                return continueEditing ? RedirectToAction("Edit", new { id = email.Id }) : RedirectToAction("List");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var email = _queuedEmailService.GetQueuedEmailById(id);
            if (email == null)
                //No email found with the specified id
                return RedirectToAction("List");
            _queuedEmailService.DeleteQueuedEmail(email);
            return RedirectToAction("List");
        }
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                _queuedEmailService.DeleteQueuedEmails(_queuedEmailService.GetQueuedEmailsByIds(selectedIds.ToArray()));
            }
            return Json(new { Result = true });
        }
        [HttpPost, ActionName("List")]
        [FormValueRequired("delete-all")]
        public ActionResult DeleteAll()
        {
            _queuedEmailService.DeleteAllEmails();
            return RedirectToAction("List");

        }
        [HttpPost,ActionName("Edit"),FormValueRequired("requeue")]
        public ActionResult Requeue(QueuedEmailModel queuedEmailModel)
        {
            var queuedEmail = _queuedEmailService.GetQueuedEmailById(queuedEmailModel.Id);
            if (queuedEmail == null)
                //No email found with the specified id
                return RedirectToAction("List");
            var requeuedEmail = new QueuedEmail
            {
                PriorityId = queuedEmail.PriorityId,
                From = queuedEmail.From,
                FromName = queuedEmail.FromName,
                To = queuedEmail.To,
                ToName = queuedEmail.ToName,
                ReplyTo = queuedEmail.ReplyTo,
                ReplyToName = queuedEmail.ReplyToName,
                CC = queuedEmail.CC,
                Bcc = queuedEmail.Bcc,
                Subject = queuedEmail.Subject,
                Body = queuedEmail.Body,
                AttachmentFilePath = queuedEmail.AttachmentFilePath,
                AttachmentFileName = queuedEmail.AttachmentFileName,
                AttachedDownloadId = queuedEmail.AttachedDownloadId,
                CreatedOn = DateTime.Now,
                EmailAccountId = queuedEmail.EmailAccountId,
                //DontSendBeforeDateUtc = (queuedEmailModel.SendImmediately || !queuedEmailModel.DontSendBeforeDate.HasValue) ?
                //   null : (DateTime?)_dateTimeHelper.ConvertToUtcTime(queuedEmailModel.DontSendBeforeDate.Value)
            };
            _queuedEmailService.InsertQueuedEmail(requeuedEmail);
            SuccessNotification("修改邮件队列成功");
            return RedirectToAction("Edit", new { id = requeuedEmail.Id });
        }

        #endregion
        // GET: QueuedEmail
        public ActionResult Index()
        {
            return View();
        }
    }
}