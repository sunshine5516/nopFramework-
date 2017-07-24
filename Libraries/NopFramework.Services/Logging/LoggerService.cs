using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Logging;
using NopFramework.Core.Data;
using NopFramework.Data;
using NopFramework.Core.Domains.Common;

namespace NopFramework.Services.Logging
{
    /// <summary>
    /// 日志服务类
    /// </summary>
    public partial class LoggerService : ILogger
    {
        #region Fields
        private readonly IRepository<Log> _logRepository;
        //private readonly IWebHelper _webHelper;
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        private readonly CommonSettings _commonSettings;
        #endregion
        #region 构造函数

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logRepository">Log repository</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="dbContext">DB context</param>
        /// <param name="dataProvider">WeData provider</param>
        /// <param name="commonSettings">Common settings</param>
        public LoggerService(IRepository<Log> logRepository,
            //IWebHelper webHelper,
            IDbContext dbContext,
            IDataProvider dataProvider,
            CommonSettings commonSettings)
        {
            this._logRepository = logRepository;
            //this._webHelper = webHelper;
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
            this._commonSettings = commonSettings;
        }

        #endregion
        #region 方法
        /// <summary>
        /// 确定是否启用日志级别
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="log"></param>
        public void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            _logRepository.Delete(log);
        }
        /// <summary>
        /// 批量删除日志
        /// </summary>
        /// <param name="logs"></param>
        public void DeleteLogs(IList<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException("logs");
            _logRepository.Delete(logs);
        }
        /// <summary>
        /// 清除日志
        /// </summary>
        public void ClearLog()
        {
            ///存储过程
            if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {

            }
            else
            {
                var log = _logRepository.Table.ToList();
                foreach (var logItem in log)
                    _logRepository.Delete(logItem);
            }
        }

        /// <summary>
        /// 获取所有日志记录
        /// </summary>
        /// <param name="fromUtc">开始时间，null加载所有记录</param>
        /// <param name="toUtc">结束时间，null加载所有记录</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level;null加载所有记录</param>
        /// <param name="pageIndex">index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Log item items</returns>
        public IPagedList<Log> GetAllLogs(DateTime? fromUtc = default(DateTime?), 
            DateTime? toUtc = default(DateTime?), string message = "", LogLevel? logLevel = default(LogLevel?)
            , int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _logRepository.Table;
            if (fromUtc.HasValue)
                query = query.Where(l => fromUtc.Value <= l.CreatedOnUtc);
            if (toUtc.HasValue)
                query = query.Where(l => toUtc.Value >= l.CreatedOnUtc);
            if (logLevel.HasValue)
            {
                var logLevelId = (int)logLevel.Value;
                query = query.Where(l => logLevelId == l.LogLevelId);
            }
            if (!String.IsNullOrEmpty(message))
                query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
            query = query.OrderByDescending(l=>l.CreatedOnUtc);
            var log = new PagedList<Log>(query, pageIndex, pageSize);
            return log;
        }
        /// <summary>
        /// 根据Id获取log
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;
            return _logRepository.GetById(logId);
            //throw new NotImplementedException();
        }

        public IList<Log> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();
            var query = from l in _logRepository.Table
                        where logIds.Contains(l.Id)
                        select l;
            //var q = _logRepository.Table.Contains(l=>l.Id);
            var logItems = query.ToList();
            var sortedLogItems = new List<Log>();
            foreach (var id in logIds)
            {
                var log = logItems.Find(x => x.Id == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
        }
        /// <summary>
        /// 插入日志项
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="shortMessage">信息</param>
        /// <param name="fullMessage">完整的消息</param>
        /// <param name="customer">客户关联日志记录</param>
        /// <returns>A log item</returns>
        public Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            //check ignore word/phrase list?
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;
            var log = new Log
            {
                LogLevel=logLevel,
                ShortMessage=shortMessage,
                FullMessage=fullMessage,
                //IpAddress=_webHelper.GetCurrentIpAddress(),
                //PageUrl=_webHelper.GetThisPageUrl(true),
                //ReferrerUrl=_webHelper.GetUrlReferrer(),
                IpAddress ="",
                PageUrl ="",
                ReferrerUrl ="",
                CreatedOnUtc =DateTime.UtcNow
            };
            _logRepository.Insert(log);

            return log;
        }

        #endregion
        #region 辅助方法
        protected virtual bool IgnoreLog(string message)
        {
            if (!_commonSettings.IgnoreLogWordlist.Any())
                return false;
            if (String.IsNullOrWhiteSpace(message))
                return false;
            return _commonSettings.IgnoreLogWordlist
                .Any(x=>message.IndexOf(x, StringComparison.InvariantCultureIgnoreCase)>=0);
        }
        #endregion
    }
}
