using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Cms
{
    public class WidgetSettings: ISettings
    {
        public WidgetSettings()
        {
            ActiveWidgetSystemNames = new List<string>();
        }
        public List<string> ActiveWidgetSystemNames { get; set; }
    }
}
