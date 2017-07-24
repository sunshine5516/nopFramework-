using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件基类
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        /// 设置插件的属性信息
        /// </summary>
        public virtual PluginDescriptor PluginDescriptor{get;set;}
        /// <summary>
        /// 安装插件接口
        /// </summary>
        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(this.PluginDescriptor.SystemName);
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 卸载插件接口
        /// </summary>
        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(this.PluginDescriptor.SystemName);
            //throw new NotImplementedException();
        }
    }
}
