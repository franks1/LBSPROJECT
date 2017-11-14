using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class CasualPaymentSetupConfiguration:EntityTypeConfiguration<CasualPaymentSetup>
    {
        public CasualPaymentSetupConfiguration()
        {
            Property(x => x.Basic).IsRequired();
            Property(x => x.NightAllowance).IsRequired();
            Property(x => x.TransportationAllowance).IsRequired();
            Property(x => x.WorkShift).HasMaxLength(10).IsOptional();
            Property(x => x.WorkWeek).HasMaxLength(15).IsOptional();
            Property(x => x.TaxOnBasic).IsOptional();
            Property(x => x.TaxOnOvertime).IsOptional();
            Property(x => x.TaxOnTransport).IsOptional();
            Property(x => x.TaxOnAllowance).IsOptional();

        }
    }
}