using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class ProcessedSheetCasualConfiguration:EntityTypeConfiguration<ProcessedSheetCasual>
    {
        public ProcessedSheetCasualConfiguration()
        {
            Property(x => x.InvoiceCode).HasMaxLength(35).IsRequired();
            Property(x => x.CasualCode).HasMaxLength(30).IsRequired();
            Property(x => x.PF).HasPrecision(18, 4);
            Property(x => x.Group).HasMaxLength(50).IsRequired();
            Property(x => x.GangType).HasMaxLength(50).IsRequired();
            Property(x => x.Basic).HasPrecision(18,4);
            Property(x => x.NightAllowance).HasPrecision(18, 4);
            Property(x => x.Overtime).HasPrecision(18, 4);
            Property(x => x.SSF).HasPrecision(18, 4);
            Property(x => x.TaxOnBasic).HasPrecision(18, 4);
            Property(x => x.TaxOnOverTime).HasPrecision(18, 4);
            Property(x => x.TaxOnTandT).HasPrecision(18, 4);
            Property(x => x.Transportation).HasPrecision(18, 4);
            Property(x => x.UnionDues).HasPrecision(18, 4);
            Property(x => x.Welfare).HasPrecision(18, 4);

            Property(x => x.GrossAmount).HasPrecision(18, 4);
            Property(x => x.NetAmount).HasPrecision(18, 4);

        }
    }
}