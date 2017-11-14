using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class FieldClientsConfiguration:EntityTypeConfiguration<FieldClients>
    {
        public FieldClientsConfiguration()
        {
            Property(x => x.Name).IsRequired().HasMaxLength(100);
            Property(x => x.Address).IsOptional().HasMaxLength(50);
            Property(x => x.Telephone1).IsOptional().HasMaxLength(20);
            Property(x => x.Telephone2).IsOptional().HasMaxLength(20);
            Property(x => x.EmailAddress).IsOptional().HasMaxLength(30);
            Property(x => x.FieldClientType).IsOptional().HasMaxLength(20);
            Property(x => x.Premium).IsOptional();
        }
    }
}