using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Settings
{
    public partial class SettingModel: BaseNopFrameworkEntityModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

    }
}