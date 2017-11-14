using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class FieldLocationAreaConfiguration:EntityTypeConfiguration<FieldLocationArea>
    {
        public FieldLocationAreaConfiguration()
        {
            Property(x => x.Location).HasMaxLength(50).IsRequired();
            Property(x => x.LocationLat).HasMaxLength(50).IsRequired();
            Property(x => x.LocationLong).HasMaxLength(50).IsRequired();

        }
    }
}