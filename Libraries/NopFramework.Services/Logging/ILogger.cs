using NopFramework.Core.Domains.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;

namespace NopFramework.Services.Logging
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public partial interface ILogger
    {
        bool IsEnabled(LogLevel level);
        void DeleteLog(Log log);
        void DeleteLogs(IList<Log> logs);
        void ClearLog();
        IPagedList<Log> GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
        Log GetLogById(int logId);
        IList<Log> GetLogByIds(int[] logIds);
        Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "");
    }
}
