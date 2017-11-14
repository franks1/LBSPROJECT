using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class ClientDetailsConfiguration:EntityTypeConfiguration<ClientDetails>
    {
        public ClientDetailsConfiguration()
        {
            Property(x => x.Name).HasMaxLength(70).IsRequired();
            Property(x => x.Address).HasMaxLength(80).IsOptional();
            Property(x => x.Phone1).HasMaxLength(20).IsOptional();
            Property(x => x.Phone2).HasMaxLength(20).IsOptional();
            Property(x => x.EmailAddress).HasMaxLength(70).IsOptional();
            Property(x => x.EmailAddress2).HasMaxLength(70).IsOptional();
            Property(x => x.LicenseKey).HasMaxLength(100).IsOptional();
            Property(x => x.LicenseStatus).HasMaxLength(10).IsOptional();
        }
    }
}