using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class GangSheetHeaderConfiguration:EntityTypeConfiguration<GangSheetHeader>
    {
        public GangSheetHeaderConfiguration()
        {
            Property(x => x.GangCode).IsRequired().HasMaxLength(20);
            Property(x => x.RequestCode).IsRequired().HasMaxLength(20);
            Property(x => x.Narration).IsRequired().HasMaxLength(50);
            Property(x => x.ApprovalNote).IsRequired().HasMaxLength(100);
            Property(x => x.PreparedBy).IsRequired();
            Property(x => x.WorkShift).HasMaxLength(10).IsOptional();
            Property(x => x.WorkWeek).HasMaxLength(15).IsOptional();
            //
            Property(x => x.GangType).IsOptional();

        }
    }
}