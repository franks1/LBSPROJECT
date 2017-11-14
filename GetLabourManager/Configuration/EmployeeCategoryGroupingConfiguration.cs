using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class EmployeeCategoryGroupingConfiguration:EntityTypeConfiguration<EmployeeCategoryGrouping>
    {
        public EmployeeCategoryGroupingConfiguration()
        {
            Property(x => x.Category).IsRequired();
            Property(x => x.Group).IsRequired();
        }
    }
}