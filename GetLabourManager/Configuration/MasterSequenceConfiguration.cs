using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class MasterSequenceConfiguration : EntityTypeConfiguration<MasterSequence>
    {
        public MasterSequenceConfiguration()
        {
            Property(p => p.SequenceName).HasMaxLength(70).IsRequired();
            Property(p => p.SequenceType).HasMaxLength(70).IsRequired();
            Property(p => p.SequenceNumber).IsRequired();
            Property(p => p.SequencePrefix).HasMaxLength(10).IsOptional();
            Property(p => p.SequenceSuffix).HasMaxLength(10).IsOptional();

        }

    }
}