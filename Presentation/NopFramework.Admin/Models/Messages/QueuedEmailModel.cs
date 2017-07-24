using FluentValidation.Attributes;
using NopFramework.Admin.Validators.Messages;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Messages
{
    [Validator(typeof(QueuedEmailValidator))]
    public partial class QueuedEmailModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("ID")]
        public override int Id { get; set; }
        [DisplayNameAttribute("消息优先权")]
        public string PriorityName { get; set; }
        [DisplayNameAttribute("来自")]
        public string From { get; set; }
        [DisplayNameAttribute("来自 姓名")]
        public string FromName { get; set; }
        [DisplayNameAttribute("收件人")]
        public string To { get; set; }
        [DisplayNameAttribute("收件人 姓名")]
        public string ToName { get; set; }
        [DisplayNameAttribute("回复到")]
        public string ReplyTo { get; set; }
        [DisplayNameAttribute("回复到 姓名")]
        public string ReplyToName { get; set; }
        [DisplayNameAttribute("转发邮件")]
        public string CC { get; set; }
        [DisplayNameAttribute("私密发送邮件")]
        public string Bcc { get; set; }
        [DisplayNameAttribute("主题")]
        [AllowHtml]
        public string Subject { get; set; }
        [DisplayNameAttribute("正文")]
        [AllowHtml]
        public string Body { get; set; }
        [DisplayNameAttribute("附件路径")]
        public string AttachmentFilePath { get; set; }
        [DisplayNameAttribute("创建于")]
        public DateTime CreatedOn { get; set; }
        [DisplayNameAttribute("是否立即发送")]
        public bool SendImmediately { get; set; }
        [DisplayNameAttribute("")]
        public DateTime? DontSendBeforeDate { get; set; }
        [DisplayNameAttribute("发送尝试次数")]
        public int SentTries { get; set; }
        [DisplayNameAttribute("发送于")]
        public DateTime? SentOn { get; set; }
        [DisplayNameAttribute("电子邮件账号")]
        public string EmailAccountName { get; set; }
    }
}