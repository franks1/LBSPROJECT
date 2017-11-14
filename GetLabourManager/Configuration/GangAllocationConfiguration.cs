using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class GangAllocationConfiguration:EntityTypeConfiguration<GangAllocation>
    {
        public GangAllocationConfiguration()
        {
            Property(x => x.RequestCode).IsRequired().HasMaxLength(30);
        }
    }
}