using FluentValidation;
using NopFramework.Admin.Models.Messages;
using NopFramework.Core.Domains.Messages;
using NopFramework.Data;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.Messages
{
    public partial class EmailAccountValidator: BaseNopFrameworkValidator<EmailAccountModel>
    {
        public EmailAccountValidator(IDbContext dbContext)
        {
            RuleFor(x=>x.Email).NotEmpty().EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(x => x.DisplayName).NotEmpty();
            SetStringPropertiesMaxLength<EmailAccount>(dbContext);
        }
    }
}