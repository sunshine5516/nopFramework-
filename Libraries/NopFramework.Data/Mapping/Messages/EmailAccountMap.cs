using NopFramework.Core.Domains.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Data.Mapping.Messages
{
    public partial class EmailAccountMap : SunEntityTypeConfiguration<EmailAccount>
    {
        public EmailAccountMap()
        {
            this.ToTable("EmailAccount");
            this.HasKey(ea => ea.Id);
            this.Property(ea => ea.Email).IsRequired().HasMaxLength(255);
            this.Property(ea => ea.DisplayName).HasMaxLength(255);
            this.Property(ea => ea.Host).IsRequired().HasMaxLength(255);
            this.Property(ea => ea.UserName).IsRequired().HasMaxLength(255);
            this.Property(ea => ea.Password).IsRequired().HasMaxLength(255);
            this.Ignore(ea => ea.FriendlyName);
        }
    }
}
