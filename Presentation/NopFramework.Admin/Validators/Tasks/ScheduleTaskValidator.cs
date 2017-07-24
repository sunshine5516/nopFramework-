using FluentValidation;
using NopFramework.Admin.Models.Tasks;
using NopFramework.Data;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.Tasks
{
    public class ScheduleTaskValidator : BaseNopFrameworkValidator<ScheduleTaskModel>
    {
        public ScheduleTaskValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(s => s.Seconds).GreaterThan(0).WithMessage("间隔时间大于0");
        }
    }
}