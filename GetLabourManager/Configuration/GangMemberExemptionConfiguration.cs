using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class GangMemberExemptionConfiguration:EntityTypeConfiguration<GangMemberExemption>
    {
        public GangMemberExemptionConfiguration()
        {
            Property(X => X.RequestCode).IsRequired().HasMaxLength(30);
            Property(X => X.CasualCode).IsRequired().HasMaxLength(25);
        }
    }
}