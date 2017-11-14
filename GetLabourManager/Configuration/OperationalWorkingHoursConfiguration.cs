using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class OperationalWorkingHoursConfiguration:EntityTypeConfiguration<OperationalWorkingHours>
    {
        public OperationalWorkingHoursConfiguration()
        {
            Property(x => x.WorkingHours).IsRequired();
        }
    }
}