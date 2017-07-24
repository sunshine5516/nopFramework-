using NopFramework.Core.Domains.Cms;
using NopFramework.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Cms
{
    public partial class WidgetService : IWidgetService
    {
        #region 声明实例
        private readonly IPluginFinder _pluginFinder;
        private readonly WidgetSettings _widgetSettings;
        #endregion
        #region 构造函数

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="widgetSettings">Widget settings</param>
        public WidgetService(IPluginFinder pluginFinder,
            WidgetSettings widgetSettings)
        {
            this._pluginFinder = pluginFinder;
            this._widgetSettings = widgetSettings;
        }

        #endregion
        #region 方法

        /// <summary>
        /// 加载活动小部件
        /// </summary>
        /// <returns></returns>
        public IList<IWidgetPlugin> LoadActiveWidgets()
        {
            return LoadAllWidgets()
                .Where(x => _widgetSettings.ActiveWidgetSystemNames.Contains(x.PluginDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }
        /// <summary>
        /// 加载活动小部件
        /// </summary>
        /// <param name="widgetZone"></param>
        /// <returns></returns>
        public IList<IWidgetPlugin> LoadActiveWidgetsByWidgetZone(string widgetZone)
        {
            if (String.IsNullOrWhiteSpace(widgetZone))
                return new List<IWidgetPlugin>();
            //通过GetWidgetZones方法返回的集合与widgetZone做比较
            var model = LoadActiveWidgets()
                .Where(x => x.GetWidgetZones().Contains(widgetZone, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
            var model2 = LoadActiveWidgets().ToList();
            return LoadActiveWidgets()
                .Where(x => x.GetWidgetZones().Contains(widgetZone, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
        }
        /// <summary>
        /// 加载所有小部件
        /// </summary>
        /// <returns></returns>
        public IList<IWidgetPlugin> LoadAllWidgets()
        {
            return _pluginFinder.GetPlugins<IWidgetPlugin>().ToList();
        }
        /// <summary>
        /// 按系统名称加载小部件
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public IWidgetPlugin LoadWidgetBySystemName(string systemName)
        {
            var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IWidgetPlugin>(systemName);
            if (descriptor != null)
                return descriptor.Instance<IWidgetPlugin>();

            return null;
        }
        #endregion
    }
}
