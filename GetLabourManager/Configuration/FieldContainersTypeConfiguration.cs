using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class FieldContainersTypeConfiguration:EntityTypeConfiguration<FieldContainersType>
    {
        public FieldContainersTypeConfiguration()
        {
            Property(x => x.ContainerType).HasMaxLength(30).IsRequired();
        }
    }
}