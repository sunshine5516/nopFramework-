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
    [Validator(typeof(EmailAccountValidator))]
    public partial class EmailAccountModel: BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("邮件")]
        [AllowHtml]
        public string Email { get; set; }

        [DisplayNameAttribute("邮件友好名称")]
        [AllowHtml]
        public string DisplayName { get; set; }

        [DisplayNameAttribute("主机")]
        [AllowHtml]
        public string Host { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Port")]
        [DisplayNameAttribute("端口")]
        public int Port { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Username")]
        [DisplayNameAttribute("用户名")]
        [AllowHtml]
        public string Username { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Password")]
        [DisplayNameAttribute("密码")]
        [AllowHtml]
        public string Password { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.EnableSsl")]
        [DisplayNameAttribute("SSL")]
        public bool EnableSsl { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.UseDefaultCredentials")]
        [DisplayNameAttribute("端口")]
        public bool UseDefaultCredentials { get; set; }

        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.IsDefaultEmailAccount")]
        [DisplayNameAttribute("是否默认")]
        public bool IsDefaultEmailAccount { get; set; }


        //[NopResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SendTestEmailTo")]
        [DisplayNameAttribute("发送测试邮件")]
        [AllowHtml]
        public string SendTestEmailTo { get; set; }

    }
}