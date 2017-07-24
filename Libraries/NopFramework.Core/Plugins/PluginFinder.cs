using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件查找类
    /// </summary>
    public class PluginFinder : IPluginFinder
    {
        #region 字段
        private IList<PluginDescriptor> _plugins;
        private bool _arePluginsLoaded;
        #endregion
        #region 方法
        public PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
        {
            return GetPluginDescriptors(loadMode)
                .SingleOrDefault(p=>p.SystemName.Equals(systemName,StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// 获取插件描述descriptors--泛型方法
        /// </summary>
        /// <param name="loadMode">加载方式</param>
        /// <param name="group">分组 NULL加载所有记录</param>
        /// <returns></returns>
        public IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            string group = null)
        {
            //确保插件已被加载
            EnsurePluginsAreLoaded();
            return _plugins.Where(p => CheckLoadMode(p, loadMode) && CheckGroup(p, group));
        }
        /// <summary>
        /// 获取插件分组信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPluginGroups()
        {
            return GetPluginDescriptors(LoadPluginsMode.All).Select(x => x.Group).Distinct().OrderBy(x=>x);
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public virtual void ReloadPlugins()
        {
            _arePluginsLoaded = false;
            EnsurePluginsAreLoaded();
        }

        /// <summary>
        /// 通过插件的系统名字获取插件--泛型方法
        /// </summary>
        /// <typeparam name="T">类型.</typeparam>
        /// <param name="systemName">插件的系统名字</param>
        /// <param name="loadMode">加载插件模式</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
            where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(loadMode)
                .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode, string group=null)
            where T:class,IPlugin
        {
            return GetPluginDescriptors(loadMode, group)
                .Where(p => typeof(T).IsAssignableFrom(p.PluginType));
        }
        /// <summary>
        /// 获取指定类型的插件集合
        /// </summary>
        /// <typeparam name="T">插件类型.</typeparam>
        /// <param name="loadMode">加载插件模式</param>
        /// <param name="group">分组 NULL加载所有记录</param>
        /// <returns>Plugins</returns>
        public virtual IEnumerable<T> GetPlugins<T>(LoadPluginsMode loadMode, string group)
            where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(loadMode, group).Select(p => p.Instance<T>());
        }
        #endregion
        #region 辅助方法
        /// <summary>
        /// 插件加载
        /// </summary>
        protected virtual void EnsurePluginsAreLoaded()
        {
            if (!_arePluginsLoaded)
            {
                var foundPlugins = PluginManager.ReferencedPlugins.ToList();
                foundPlugins.Sort();
                _plugins = foundPlugins.ToList();
                _arePluginsLoaded = true;
            }
        }
        protected virtual bool CheckLoadMode(PluginDescriptor pluginDescriptor, LoadPluginsMode loadMode)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");
            switch (loadMode)
            {
                case LoadPluginsMode.All:
                    return true;
                case LoadPluginsMode.InstalledOnly:
                    return pluginDescriptor.Installed;
                case LoadPluginsMode.NotInstalledOnly:
                    return !pluginDescriptor.Installed;
                default:
                    throw new Exception("Not supported LoadPluginsMode");
            }
        }
        /// <summary>
        /// 插件是否存在某组中
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        /// <param name="group">分组</param>
        /// <returns>true - available; false - no</returns>
        protected virtual bool CheckGroup(PluginDescriptor pluginDescriptor, string group)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            if (String.IsNullOrEmpty(group))
                return true;

            return group.Equals(pluginDescriptor.Group, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion
    }
}
