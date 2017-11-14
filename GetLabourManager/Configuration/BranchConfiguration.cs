﻿using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GetLabourManager.Configuration
{
    public class BranchConfiguration:EntityTypeConfiguration<Branch>
    {
        public BranchConfiguration()
        {
            Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}