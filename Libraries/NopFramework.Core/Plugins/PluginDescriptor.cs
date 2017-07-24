using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件描述类，包含了插件的版本、描述、类型、文件名称、作者等
    /// </summary>
    public class PluginDescriptor : IComparable<PluginDescriptor>
    {
        #region 构造函数
        public PluginDescriptor()
        {
            this.SupportedVersions = new List<string>();

        }
        public PluginDescriptor(Assembly referencedAssembly, FileInfo originalAssemblyFile,
            Type pluginType)
            : this()
        {
            this.ReferencedAssembly = referencedAssembly;
            this.OriginalAssemblyFile = originalAssemblyFile;
            this.PluginType = pluginType;
        }
        #endregion
        #region 属性
        /// <summary>
        /// 插件名称
        /// </summary>
        public virtual string PluginFileName { get; set; }
        /// <summary>
        /// 插件类型
        /// </summary>
        public virtual Type PluginType { get; set; }
        public virtual Assembly ReferencedAssembly { get; internal set; }
        public virtual FileInfo OriginalAssemblyFile { get; internal set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string FriendlyName { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public virtual string SystemName { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public virtual string Version { get; set; }
        /// <summary>
        /// 支持的版本
        /// </summary>
        public virtual IList<string> SupportedVersions { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public virtual string Author { get; set; }
        /// <summary>
        /// 是否加载
        /// </summary>
        public virtual bool Installed { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        /// <summary>
        /// 所属分组
        /// </summary>
        public virtual string Group { get; set; }
        #endregion

        //public virtual IList<int> LimitToStores { get; set; }
        /// <summary>
        /// 获取插件实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Instance<T>() where T : class, IPlugin
        {
            object instance;
            //通过IoC容器获取插件类型的实例对象
            if (!EngineContext.Current.ContainerManager.TryResolve(PluginType, null, out instance))
            {
                instance = EngineContext.Current.ContainerManager.ResolveUnregistered(PluginType);
            }
            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;
            return typedInstance;
        }
        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }
        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);
            return FriendlyName.CompareTo(other.FriendlyName);
        }
        public override string ToString()
        {
            return FriendlyName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PluginDescriptor;
            return other != null &&
                SystemName != null &&
                SystemName.Equals(other.SystemName);
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }
    }
}
