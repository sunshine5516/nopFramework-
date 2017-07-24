using FluentValidation.Attributes;
using NopFramework.Admin.Validators.Tasks;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Tasks
{
    [Validator(typeof(ScheduleTaskValidator))]
    public class ScheduleTaskModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("名称")]
        public string Name { get; set; }
        [DisplayNameAttribute("时间间隔")]
        public int Seconds { get; set; }
        [DisplayNameAttribute("是否可用")]
        public bool Enabled { get; set; }
        [DisplayNameAttribute("发生错误时停止")]
        public bool StopOnError { get; set; }
        [DisplayNameAttribute("最后开始日期")]
        public DateTime? LastStart { get; set; }
        [DisplayNameAttribute("最后结束日期")]
        public DateTime? LastEnd { get; set; }
        [DisplayNameAttribute("	最后成功日期")]
        public DateTime? LastSuccess { get; set; }
    }
}