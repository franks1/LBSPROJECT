using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeClientGroupConfiguration:EntityTypeConfiguration<EmployeeClientGroup>
    {
        public EmployeeClientGroupConfiguration()
        {
            Property(x => x.Category).IsRequired();
            Property(x => x.Client).IsRequired();
        }
    }
}