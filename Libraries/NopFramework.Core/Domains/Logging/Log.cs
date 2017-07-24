using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Logging
{
   public partial class Log : BaseEntity
    {
        public int LogLevelId { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public int? CustomerId { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)this.LogLevelId;
            }
            set
            {
                this.LogLevelId = (int)value;
            }
        }
        //public virtual Customer Customer { get; set; }
    }
}
