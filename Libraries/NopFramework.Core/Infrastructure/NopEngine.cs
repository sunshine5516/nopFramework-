using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Infrastructure.DependencyManagement;
using NopFramework.Core.Configuration;
using Autofac;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace NopFramework.Core.Infrastructure
{
    public class NopEngine : IEngine
    {
        #region 字段
        /// <summary>
        /// 容器管理
        /// </summary>
        private ContainerManager _containerManager;

        #endregion
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        public void Initialize(NopConfig config)
        {
            //注册依赖
            RegisterDependencies(config);
            //启动task线程
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }
        /// <summary>
        /// 注册依赖，找到项目中所有的依赖项并注册
        /// </summary>
        /// <param name="config"></param>
        protected virtual void RegisterDependencies(NopConfig config)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            this._containerManager = new ContainerManager(container);
            //依赖
            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<NopConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            builder = new ContainerBuilder();
            //查找所有实现了接口IDependencyRegistrar的类
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
            {
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));//通过反射，获取实例
            }
            ////排序
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();            
            //依次调用实现IDependencyRegistrar接口的类的方法Register
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder, config); //按顺序注册依赖
            builder.Update(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected virtual void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //排序
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }
    }
}
