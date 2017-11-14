using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeCategoryConfiguration:EntityTypeConfiguration<EmployeeCategory>
    {
        public EmployeeCategoryConfiguration()
        {
            Property(x => x.Category).HasMaxLength(30).IsRequired();
        }
    }
}