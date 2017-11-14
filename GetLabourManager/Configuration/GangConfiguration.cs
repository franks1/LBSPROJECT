using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class GangConfiguration:EntityTypeConfiguration<Gang>
    {
        public GangConfiguration()
        {
            Property(x => x.Code).IsRequired().HasMaxLength(25);
            Property(x => x.Description).IsRequired().HasMaxLength(30);
            Property(x => x.Status).IsOptional().HasMaxLength(15);
        }
    }
}