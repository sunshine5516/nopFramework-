﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 获取插件的信息接口
    /// </summary>
    public interface IPluginFinder
    {
        /// <summary>
        /// 检测某个插件在特定的应用是否可用，暂未实现
        /// </summary>
        /// <param name="pluginDescriptor">插件</param>
        /// <param name="storeId">商店</param>
        /// <returns>true - available; false - no</returns>
        //bool AuthenticateStore(PluginDescriptor pluginDescriptor, int storeId);
        /// <summary>
        /// 获取插件分组
        /// </summary>
        /// <returns>Plugins groups</returns>
        IEnumerable<string> GetPluginGroups();
        /// <summary>
        /// 获取插件
        /// </summary>
        /// <typeparam name="T">插件类型</typeparam>
        /// <param name="loadMode">插件加载模式</param>
        /// <param name="group">group; 0加载所有</param>
        /// <returns></returns>
        IEnumerable<T> GetPlugins<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly
            , string group = null) where T : class, IPlugin;
        /// <summary>
        /// 获取插件描述文件信息
        /// </summary>
        /// <param name="loadMode"></param>
        /// <param name="storeId"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
          string group = null);
        /// <summary>
        /// 获取插件描述文件
        /// </summary>
        /// <typeparam name="T">插件类型.</typeparam>
        /// <param name="loadMode">插件加载模式</param>
        /// <param name="storeId">storeId;0加载所有</param>
        /// <param name="group">group; 0加载所有</param>
        /// <returns>Plugin descriptors</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly,
            string group = null) where T : class, IPlugin;
        /// <summary>
        /// 根据sysName获取插件描述信息
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="loadMode"></param>
        /// <returns></returns>
        PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly);
        /// <summary>
        /// 根据sysName获取插件描述信息
        /// </summary>
        /// <typeparam name="T">插件类型</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="loadMode">加载模式</param>
        /// <returns>>Plugin descriptor</returns>
        PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
            where T : class, IPlugin;
        /// <summary>
        /// 重新加载插件
        /// </summary>
        void ReloadPlugins();
    }
}
