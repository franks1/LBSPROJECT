using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class AllocationContainersConfiguration:EntityTypeConfiguration<AllocationContainers>
    {
        public AllocationContainersConfiguration()
        {
            Property(x => x.ContainerId).IsRequired();
            Property(x => x.ContainerNumber).IsOptional().HasMaxLength(25);
            Property(x => x.AllocationId).IsRequired();

            HasRequired(x => x.GangAllocated).WithMany(x => x.Containers)
                .HasForeignKey(x => x.AllocationId).WillCascadeOnDelete(false);

        }
    }
}