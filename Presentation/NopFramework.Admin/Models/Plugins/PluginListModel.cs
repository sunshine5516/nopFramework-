using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Plugins
{
    public class PluginListModel
    {
        [DisplayNameAttribute("加载类型")]
        public IList<SelectListItem> AvailableLoadModes { get; set; }
        [DisplayNameAttribute("模块")]
        public IList<SelectListItem> AvailableGroups { get; set; }
        [DisplayNameAttribute("加载类型")]
        public int SearchLoadModeId { get; set; }
        [DisplayNameAttribute("模块")]
        public string SearchGroup { get; set; }
        public PluginListModel()
        {
            AvailableLoadModes = new List<SelectListItem>();
            AvailableGroups = new List<SelectListItem>();
        }
    }
}