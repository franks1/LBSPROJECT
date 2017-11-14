using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeContributionsConfiguration:EntityTypeConfiguration<EmployeeContributions>
    {
        public EmployeeContributionsConfiguration()
        {
            Property(x => x.StaffId).IsRequired();
            Property(x => x.SSN).IsOptional().HasMaxLength(30);
        }
        }
}