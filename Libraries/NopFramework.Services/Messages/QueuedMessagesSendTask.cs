using NopFramework.Services.Logging;
using NopFramework.Services.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Messages
{
    /// <summary>
    /// 发送电子邮件任务
    /// </summary>
    public partial class QueuedMessagesSendTask : ITask
    {
        #region 实例
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        #endregion
        #region 构造函数
        public QueuedMessagesSendTask(IQueuedEmailService _queuedEmailService, ILogger _logger,
            IEmailSender _emailSender)
        {
            this._queuedEmailService = _queuedEmailService;
            this._logger = _logger;
            this._emailSender = _emailSender;
        }
        #endregion
        /// <summary>
        /// execute task
        /// </summary>
        public void Execute()
        {
            var maxTries = 3;
            var queuedEmails = _queuedEmailService.SearchEmails(null, null, null, null,
                true, true, maxTries, false, 0, 500);
            foreach (var queuedEmail in queuedEmails)
            {
                var bcc = String.IsNullOrWhiteSpace(queuedEmail.Bcc) ? null :
                    queuedEmail.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cc = String.IsNullOrWhiteSpace(queuedEmail.CC)
                            ? null
                            : queuedEmail.CC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    _emailSender.SendEmail(queuedEmail.EmailAccount,
                       queuedEmail.Subject,
                       queuedEmail.Body,
                      queuedEmail.From,
                      queuedEmail.FromName,
                      queuedEmail.To,
                      queuedEmail.ToName,
                      queuedEmail.ReplyTo,
                      queuedEmail.ReplyToName,
                      bcc,
                      cc,
                      queuedEmail.AttachmentFilePath,
                      queuedEmail.AttachmentFileName,
                      queuedEmail.AttachedDownloadId);
                    queuedEmail.SentOn = DateTime.Now;
                }
                catch (Exception exc)
                {
                    _logger.Error(string.Format("Error sending e-mail. {0}", exc.Message), exc);
                }
                finally
                {
                    queuedEmail.SentTries = queuedEmail.SentTries + 1;
                    _queuedEmailService.UpdateQueuedEmail(queuedEmail);
                }
            }
        }
    }
}
