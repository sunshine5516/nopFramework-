using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Demains
{
    /// <summary>
    /// 任务类实体
    /// </summary>
    public partial class ScheduleTask:BaseEntity
    {
        //public string Tage { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int Seconds { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }
        public bool StopOnError { get; set; }
        public string LeasedByMachineName { get; set; }
        public DateTime? LeasedUntilUtc { get; set; }
        public DateTime? LastStartUtc { get; set; }
        public DateTime? LastEndUtc { get; set; }
        public DateTime? LastSuccessUtc { get; set; }
    }
}
