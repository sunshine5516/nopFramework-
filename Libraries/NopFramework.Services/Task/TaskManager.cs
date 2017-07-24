using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Task
{
    public partial class TaskManager
    {
        private static readonly TaskManager _taskManager = new TaskManager();
        private readonly List<TaskThread> _taskThreads = new List<TaskThread>();
        private const int _notRunTasksInterval = 60 * 30;
        private TaskManager()
        { }
        /// <summary>
        /// 初始化线程管理
        /// </summary>
        public void Initialize()
        {
            this._taskThreads.Clear();
            var taskService = EngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>();
            var scheduleTasks = taskService
                .GetAllTasks()
                .OrderBy(x => x.Seconds).ToList();
            foreach (var scheduleTaskGrouped in scheduleTasks.GroupBy(x => x.Seconds))
            {
                var taskThread = new TaskThread
                {
                    Seconds=scheduleTaskGrouped.Key
                };
                foreach (var scheduleTask in scheduleTaskGrouped)
                {
                    var task = new Task(scheduleTask);
                    taskThread.AddTask(task);
                }
                this._taskThreads.Add(taskThread);
            }

            var notRunTasks = scheduleTasks.Where(x => x.Seconds >= _notRunTasksInterval)
                .Where(x => !x.LastStartUtc.HasValue || x.LastStartUtc.Value.AddSeconds(x.Seconds) < DateTime.UtcNow)
                .ToList();
            //为没有运行很长时间的任务创建一个线程
            if (notRunTasks.Any())
            {
                var taskThread = new TaskThread
                {
                    RunOnlyOnce = true,
                    Seconds = 60 * 5
                };
                foreach (var scheduleTask in notRunTasks)
                {
                    var task = new Task(scheduleTask);
                    taskThread.AddTask(task);
                }
                this._taskThreads.Add(taskThread);
            }
        }
        /// <summary>
        /// 启动任务管理器
        /// </summary>
        public void Start()
        {
            foreach (var taskThread in this._taskThreads)
            {
                taskThread.InitTimer();
            }
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
            foreach (var taskThread in this._taskThreads)
            {
                taskThread.Dispose();
            }
        }
        /// <summary>
        /// 获得一个任务管理的实例
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                return _taskManager;
            }
        }
        /// <summary>
        /// 获取此任务管理器的任务线程的列表
        /// </summary>
        public IList<TaskThread> TaskThreads
        {
            get
            {
                return new ReadOnlyCollection<TaskThread>(this._taskThreads);
            }
        }
        //private 
    }
}
