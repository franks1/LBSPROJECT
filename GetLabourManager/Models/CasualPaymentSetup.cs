using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class CasualPaymentSetup
    {
        public int Id { get; set; }
        public decimal Basic { get; set; }
        public int Group { get; set; }
        public string WorkShift { get; set; }
        public string WorkWeek { get; set; }
        public decimal NightAllowance { get; set; }
        public decimal TransportationAllowance { get; set; }
        public decimal Overtime { get; set; }
        public double SSF { get; set; }
        public decimal UnionDues { get; set; }
        public double PF { get; set; }
        public decimal Welfare { get; set; }
        public double TaxOnBasic { get; set; }
        public double TaxOnAllowance { get; set; }
        public double TaxOnOvertime { get; set; }
        public double TaxOnTransport { get; set; }
    }
}