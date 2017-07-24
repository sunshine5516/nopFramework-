using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件加载模式
    /// </summary>
    public enum LoadPluginsMode
    {
        /// <summary>
        /// 加载所有
        /// </summary>
        All = 0,
        /// <summary>
        /// 只加载安装的
        /// </summary>
        InstalledOnly = 10,
        /// <summary>
        /// 只加载未安装的
        /// </summary>
        NotInstalledOnly = 20
    }
}
