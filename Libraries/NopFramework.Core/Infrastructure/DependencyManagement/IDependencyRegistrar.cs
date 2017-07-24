using Autofac;
using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注入接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务和接口
        /// </summary>
        /// <param name="builder"></param>
        void Register(ContainerBuilder builder,ITypeFinder typeFinder, NopConfig config);
        int Order { get;}
    }
}
