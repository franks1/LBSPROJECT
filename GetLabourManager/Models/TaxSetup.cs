using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class TaxSetup
    {
        public int Id { get; set; }
        public double Basic { get; set; }
        public double OverTime { get; set; }
        public double TT { get; set; }
    }
}