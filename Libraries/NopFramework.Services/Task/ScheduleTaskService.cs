using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Demains;
using NopFramework.Core.Data;

namespace NopFramework.Services.Task
{
    /// <summary>
    /// IScheduleTaskService的实现类，完成对数据库的相关操作。
    /// </summary>
    public partial class ScheduleTaskService : IScheduleTaskService
    {
        private readonly IRepository<ScheduleTask> _taskRepository;
        public ScheduleTaskService(IRepository<ScheduleTask> taskRepository)
        {
            this._taskRepository = taskRepository;
        }
        #region 方法
        public void DeleteTask(ScheduleTask tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException("task");
            _taskRepository.Delete(tasks);
        }

        public IList<ScheduleTask> GetAllTasks(bool showHidden = false)
        {
            var query = _taskRepository.Table;
            if (!showHidden)
            {
                query = query.Where(t=>t.Enable);
            }
            query = query.OrderByDescending(t=>t.Seconds);
            var tasks = query.ToList();
            return tasks;
        }

        public ScheduleTask GetTaskById(int taskId)
        {
            if (taskId == 0)
                return null;
            return _taskRepository.GetById(taskId);
        }

        public ScheduleTask GetTaskByType(string type)
        {
            if (String.IsNullOrEmpty(type))
                return null;
            var query = _taskRepository.Table;
            query = query.Where(t=>t.Type==type);
            query = query.OrderByDescending(t=>t.Seconds);
            return query.FirstOrDefault();
        }

        public void InsertTask(ScheduleTask task)
        {
            if(task==null)
                throw new ArgumentNullException("task");
            this._taskRepository.Insert(task);
            
        }

        public void UpdateTask(ScheduleTask task)
        {
            if (task == null)
                throw new ArgumentNullException("task");
            this._taskRepository.Update(task);
        }
        #endregion

    }
}
