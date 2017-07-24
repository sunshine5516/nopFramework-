using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Plugins
{
    public partial class PluginModel: BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("分组")]
        public string Group { get; set; }
        [DisplayNameAttribute("友好名")]
        public string FriendlyName { get; set; }
        [DisplayNameAttribute("系统名称")]
        public string SystemName { get; set; }
        [DisplayNameAttribute("版本")]
        public string Version { get; set; }
        [DisplayNameAttribute("作者")]
        public string Author { get; set; }
        [DisplayNameAttribute("显示序号")]
        public int DisplayOrder { get; set; }
        [DisplayNameAttribute("配置URL")]
        public string ConfigurationUrl { get; set; }
        [DisplayNameAttribute("是否加载")]
        public bool Installed { get; set; }
        [DisplayNameAttribute("是否可用")]
        public string Description { get; set; }
        [DisplayNameAttribute("是否可修改")]
        public bool CanChangeEnabled { get; set; }
        [DisplayNameAttribute("是否可用")]
        public bool IsEnabled { get; set; }

        [DisplayNameAttribute("Logo")]
        public string LogoUrl { get; set; }
    }
}