using NopFramework.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Configuration
{
    public partial class SettingMap: SunEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("Setting");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2000);
        }
    }
}
