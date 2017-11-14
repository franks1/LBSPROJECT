using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class CostSheetItemsConfiguration:EntityTypeConfiguration<CostSheetItems>
    {
        public CostSheetItemsConfiguration()
        {
            Property(x => x.CostSheetId).IsRequired();
            Property(x => x.GroupName).IsOptional().HasMaxLength(20);
            Property(x => x.Gang).IsOptional().HasMaxLength(20);
            Property(x => x.FullName).IsOptional().HasMaxLength(150);
            Property(x => x.StaffCode).IsOptional().HasMaxLength(20);
            Property(x => x.HourseWorked).IsOptional();
            HasRequired(x => x.CostSheetEntry).WithMany(x => x.SheetItems)
                .HasForeignKey(x => x.CostSheetId).WillCascadeOnDelete(false);
        }
    }
}