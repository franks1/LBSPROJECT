using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class ForemenConfiguration : EntityTypeConfiguration<Foremen>
    {
        public ForemenConfiguration()
        {
            Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            Property(x => x.LastName).HasMaxLength(60).IsRequired();
            Property(x => x.MiddleName).HasMaxLength(70).IsOptional();
            Property(x => x.Code).HasMaxLength(30).IsRequired();

        }

    }
}