using NopFramework.Core.Configuration;
using NopFramework.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Infrastructure
{
    /// <summary>
    /// 实现此接口的类可以充当组成nop引擎的各种服务的门户，编辑功能，模块和实现通过此接口访问大多数Nop功能
    /// </summary>
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        void Initialize(NopConfig config);
        /// <summary>
        /// 解析依赖
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  解析依赖
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 解析所有的依赖关系
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
