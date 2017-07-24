using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 设置插件的属性信息
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }
        /// <summary>
        /// 安装插件接口
        /// </summary>
        void Install();
        /// <summary>
        /// 卸载插件接口
        /// </summary>
        void Uninstall();
    }
}
