using Autofac;
using NopFramework.Core.Configuration;
using NopFramework.Core.Demains;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Task
{
    /// <summary>
    /// Task处理任务的执行过程以及执行过程的结果处理
    /// </summary>
    public partial class Task
    {
        #region 属性

        /// <summary>
        /// 最后一个开始的日期
        /// </summary>
        public DateTime? LastStartUtc { get; private set; }

        /// <summary>
        /// 最后一个的日期
        /// </summary>
        public DateTime? LastEndUtc { get; private set; }

        /// <summary>
        /// 最后成功的日期
        /// </summary>
        public DateTime? LastSuccessUtc { get; private set; }

        /// <summary>
        /// task类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 是否错误停止
        /// </summary>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }

        #endregion
        #region 构造函数
        private Task()
        {
            this.Enabled = true;
        }
        public Task(ScheduleTask task)
        {
            this.Type = task.Type;
            this.Enabled = task.Enable;
            this.StopOnError = task.StopOnError;
            this.Name = task.Name;
        }
        #endregion
        #region Utilities
        private ITask CreateTask(ILifetimeScope scope)
        {
            ITask task = null;
            if (this.Enabled)
            {
                ///获取类型
                var type2 = System.Type.GetType(this.Type);
                if (type2 != null)
                {
                    object instance;
                    if (!EngineContext.Current.ContainerManager.TryResolve(type2, scope, out instance))
                    {
                        instance = EngineContext.Current.ContainerManager.ResolveUnregistered(type2, scope);
                    }
                    task = instance as ITask;
                }
            }
            return task;
        }
        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            var scope = EngineContext.Current.ContainerManager.Scope();
            var scheduleTaskService = EngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>("", scope);
            var scheduleTask = scheduleTaskService.GetTaskByType(this.Type);
            try
            {
                var nopConfig = EngineContext.Current.ContainerManager.Resolve<NopConfig>("",scope);
                if (nopConfig.MultipleInstancesEnabled)
                {

                }
                var task = this.CreateTask(scope);
                if (task != null)
                {
                    this.LastEndUtc = DateTime.UtcNow;
                    if (scheduleTask != null)
                    {
                        scheduleTask.LastStartUtc = this.LastStartUtc;
                        scheduleTaskService.UpdateTask(scheduleTask);
                    }
                    task.Execute();
                }
            }
            catch(Exception exc)
            {
                this.Enabled = !this.StopOnError;
                this.LastEndUtc = DateTime.UtcNow;

                //var logger = EngineContext.Current.ContainerManager.Resolve<ILogger>("", scope);
                //logger.Error(string.Format("Error while running the '{0}' schedule task. {1}", this.Name, exc.Message), exc);
                //if (throwException)
                //    throw;
            }
            if (scheduleTask != null)
            {
                scheduleTask.LastEndUtc = this.LastEndUtc;
                scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                scheduleTaskService.UpdateTask(scheduleTask);
            }
            //if (dispose)
            //{
            //    scope.Dispose();
            //}
        }
        #endregion
    }
}
