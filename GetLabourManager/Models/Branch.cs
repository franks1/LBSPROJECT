﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsHead { get; set; }
    }
}