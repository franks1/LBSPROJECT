using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class GangSheetItemSConfiguration:EntityTypeConfiguration<GangSheetItems>
    {
        public GangSheetItemSConfiguration()
        {
            Property(x => x.StaffCode).IsRequired().HasMaxLength(20);
            Property(x => x.Header).IsRequired();

            HasRequired(x => x.SheetHeader).WithMany(x => x.SheetItems)
                .HasForeignKey(x => x.Header).WillCascadeOnDelete(false);
        }
    }
}