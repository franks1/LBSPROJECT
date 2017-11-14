using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeGroupConfiguration:EntityTypeConfiguration<EmployeeGroup>
    {
        public EmployeeGroupConfiguration()
        {
            Property(x => x.GroupName).HasMaxLength(50).IsRequired();
    }
    }
}