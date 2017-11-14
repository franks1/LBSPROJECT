using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class ProcessedInvoiceConfiguration:EntityTypeConfiguration<ProcessedInvoice>
    {
        public ProcessedInvoiceConfiguration()
        {
            Property(x => x.Invoice).IsRequired().HasMaxLength(25);
        }
    }
}