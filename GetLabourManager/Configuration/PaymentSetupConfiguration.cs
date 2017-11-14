using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class PaymentSetupConfiguration:EntityTypeConfiguration<PaymentSetup>
    {
        public PaymentSetupConfiguration()
        {
            Property(x => x.Client).IsRequired();
            Property(x => x.WorkShift).HasMaxLength(10).IsOptional();
            Property(x => x.WorkWeek).HasMaxLength(15).IsOptional();
          
        }
    }
}