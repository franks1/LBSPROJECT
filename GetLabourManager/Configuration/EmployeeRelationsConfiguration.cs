using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeRelationsConfiguration : EntityTypeConfiguration<EmployeeRelations>
    {
        public EmployeeRelationsConfiguration()
        {
            Property(x => x.StaffId).IsRequired();
            Property(x => x.GuarantorName).HasMaxLength(80).IsRequired();
            Property(x => x.GuarantorPhone).HasMaxLength(18).IsRequired();
            Property(x => x.GuarantorAddress).HasMaxLength(70).IsRequired();
            Property(x => x.GuarantorRelation).HasMaxLength(20).IsOptional();

            Property(x => x.NextofKinName).HasMaxLength(80).IsRequired();
            Property(x => x.NextofKinPhone).HasMaxLength(18).IsRequired();
            Property(x => x.NextofKinAddress).HasMaxLength(70).IsRequired();
            Property(x => x.NextofKinRelation).HasMaxLength(20).IsOptional();
        }
    }
}