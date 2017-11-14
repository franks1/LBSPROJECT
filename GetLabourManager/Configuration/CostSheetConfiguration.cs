using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{

    public class CostSheetConfiguration:EntityTypeConfiguration<CostSheet>
    {
        public CostSheetConfiguration()
        {
            Property(x => x.CostSheetNumber).IsRequired().HasMaxLength(25);
            Property(x => x.Status).IsOptional().HasMaxLength(15);
        }
    }
}