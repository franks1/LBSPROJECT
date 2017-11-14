using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            Property(x => x.Code).HasMaxLength(30).IsRequired();
            Property(x => x.FirstName).HasMaxLength(70).IsRequired();
            Property(x => x.LastName).HasMaxLength(80).IsRequired();
            Property(x => x.MiddleName).HasMaxLength(90).IsRequired();
            Property(x => x.Region).HasMaxLength(30).IsRequired();
            Property(x => x.Status).HasMaxLength(12).IsOptional();
            Property(x => x.Telephone1).HasMaxLength(18).IsRequired();
            Property(x => x.Telephone2).HasMaxLength(18).IsOptional();
            Property(x => x.EmailAddress).HasMaxLength(50).IsOptional();
            Property(x => x.Gender).HasMaxLength(10).IsOptional();
            Property(x => x.Address).HasMaxLength(80).IsOptional();

        }
    }
}