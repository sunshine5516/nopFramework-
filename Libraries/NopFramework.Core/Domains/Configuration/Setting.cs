using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains
{
    /// <summary>
    /// 设置
    /// </summary>
    public partial class Setting:BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
