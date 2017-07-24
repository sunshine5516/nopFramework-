using NopFramework.Core.Demains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Task
{
    /// <summary>
    /// 获取数据库里的任务信息
    /// </summary>
    public partial interface IScheduleTaskService
    {
        void DeleteTask(ScheduleTask tasks);
        ScheduleTask GetTaskById(int taskId);
        ScheduleTask GetTaskByType(string type);
        IList<ScheduleTask> GetAllTasks(bool showHidden = false);
        void InsertTask(ScheduleTask task);
        void UpdateTask(ScheduleTask task);
    }
}
