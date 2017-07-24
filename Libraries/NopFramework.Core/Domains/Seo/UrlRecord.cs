using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Seo
{
    /// <summary>
    /// URL记录
    /// </summary>
    public partial class UrlRecord:BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// 实体名
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }

        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
    }
}
