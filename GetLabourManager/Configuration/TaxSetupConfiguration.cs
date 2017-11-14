using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class TaxSetupConfiguration:EntityTypeConfiguration<TaxSetup>
    {
        public TaxSetupConfiguration()
        {
            Property(x => x.Basic).IsRequired();
        }
    }
}