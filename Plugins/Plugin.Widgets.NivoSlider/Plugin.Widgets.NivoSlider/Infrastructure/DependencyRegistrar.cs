using NopFramework.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NopFramework.Core.Configuration;
using NopFramework.Core.Infrastructure;
using Plugin.Widgets.NivoSlider.Controllers;
using NopFramework.Core.Caching;
using Autofac.Core;

namespace Plugin.Widgets.NivoSlider.Infrastructure
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务接口
        /// </summary>
        /// <param name="builder">容器</param>
        /// <param name="typeFinder">类型</param>
        /// <param name="config">配置文件</param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //throw new NotImplementedException();
            builder.RegisterType<WidgetsNivoSliderController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
        }
        /// <summary>
        /// 实现顺序
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
