using System;
using System.Collections.Generic;

using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GetLabourManager.Models;
namespace GetLabourManager.Configuration
{
    public class ContainerConfiguration:EntityTypeConfiguration<VesselContainer>
    {
        public ContainerConfiguration()
        {
            Property(x => x.Continer).HasMaxLength(80).IsRequired();
        }
    }
}