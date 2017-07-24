using NopFramework.Services.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Logging
{
    /// <summary>
    /// 清除日志Task
    /// </summary>
    public partial class ClearLogTask : ITask
    {
        private readonly ILogger _logger;
        public ClearLogTask(ILogger logger)
        {
            this._logger = logger;
        }
        public void Execute()
        {
            _logger.ClearLog();
        }
    }
}
