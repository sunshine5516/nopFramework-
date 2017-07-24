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
    public partial class QueuedEmailValidator: BaseNopFrameworkValidator<QueuedEmailModel>
    {
        public QueuedEmailValidator(IDbContext dbContext)
        {
            RuleFor(x => x.From).NotEmpty().WithMessage("发件人不能为空");
            RuleFor(x => x.To).NotEmpty().WithMessage("收件人不能为空");
            RuleFor(x => x.SentTries).NotNull().WithMessage("不能为空")
                                    .InclusiveBetween(0, 99999).WithMessage("在此范围内");
            SetStringPropertiesMaxLength<QueuedEmail>(dbContext);
        }
    }
}