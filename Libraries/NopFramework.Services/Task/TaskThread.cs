﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NopFramework.Services.Task
{
    /// <summary>
    /// 任务线程管理类
    /// 主要负责判断任务的执行状态，线程执行间隔时间及调用任务执行的主方法Execute，通过Timer定时器实现定时自动运行。
    /// </summary>
    public partial class TaskThread
    {
        #region 属性
        public int Seconds { get; set; }
        public DateTime StartedUtc { get; private set; }
        public bool IsRunning { get; private set; }
        public IList<Task> Tasks
        {
            get
            {
                var list = new List<Task>();
                foreach (var task in this._tasks.Values)
                {
                    list.Add(task);
                }
                return new ReadOnlyCollection<Task>(list);
            }
        }
        public int Interval
        {
            get
            {
                return this.Seconds * 1000;
            }
        }

        /// <summary>
        /// 线程是否只运行一次（每个应用程序启动）
        /// </summary>
        public bool RunOnlyOnce { get; set; }
        #endregion
        private Timer _timer;
        private bool _disposed;
        private readonly Dictionary<string, Task> _tasks;
        internal TaskThread()
        {
            this._tasks = new Dictionary<string, Task>();
            this.Seconds = 10 * 60;
        }
        private void Run()
        {
            if (Seconds < 0)
                return;
            this.StartedUtc = DateTime.UtcNow;
            this.IsRunning = true;
            foreach (Task task in this._tasks.Values)
            {
                task.Execute();
            }
            this.IsRunning = false;
        }
        private void TimerHandler(object state)
        {
            this._timer.Change(-1,-1);
            this.Run();
            if (this.RunOnlyOnce)
            {
                this.Dispose();
            }
            else
            {
                this._timer.Change(this.Interval,this.Interval);
            }
        }
        public void Dispose()
        {
            if ((this._timer != null) && !this._disposed)
            {
                lock (this)
                {
                    this._timer.Dispose();
                    this._timer = null;
                    this._disposed = true;
                }
            }
        }
        /// <summary>
        /// 初始化定时器，创建了Timer对象，在定时的时间内会执行TimerHandler()方法体中内容
        /// </summary>
        public void InitTimer()
        {
            if (this._timer == null)
            {
                this._timer = new Timer(new TimerCallback(this.TimerHandler), null, this.Interval, this.Interval);
            }
        }
        /// <summary>
        /// 添加任务到线程
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            if (!this._tasks.ContainsKey(task.Name))
            {
                this._tasks.Add(task.Name, task);
            }
        }
    }
}
