using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Settings
{
    public class AllSettingsListModel : BaseNopFrameworkEntityModel
    {
        public string SearchSettingName { get; set; }
        public string SearchSettingValue { get; set; }
    }
}