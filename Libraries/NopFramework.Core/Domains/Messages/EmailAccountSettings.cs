using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Messages
{
    public class EmailAccountSettings:ISettings
    {
        /// <summary>
        /// 默认电子邮件账号Id
        /// </summary>
        public int DefaultEmailAccountId { get; set; }
    }
}
