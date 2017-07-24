using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Messages
{
    public partial class QueuedEmailListModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }
        [DisplayNameAttribute("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }
        [DisplayNameAttribute("来自 地址	")]
        [AllowHtml]
        public string SearchFromEmail { get; set; }
        [DisplayNameAttribute("收件人")]
        [AllowHtml]
        public string SearchToEmail { get; set; }
        [DisplayNameAttribute("仅调入未发送的电子邮件")]
        public bool SearchLoadNotSent { get; set; }
        [DisplayNameAttribute("失败重发次数")]
        public int SearchMaxSentTries { get; set; }
        [DisplayNameAttribute("直接到电子邮件 #")]
        public int GoDirectlyToNumber { get; set; }
    }
}