using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class PaymentSetup
    {
        public int Id { get; set; }
        public bool PayType { get; set; }
        public int Client { get; set; }
        public int Group { get; set; }
        public decimal Basic { get; set; }
        public decimal NightAllowance { get; set; }
        public string WorkShift { get; set; }
        public string WorkWeek { get; set; }
        public decimal TransportationAllowance { get; set; }
        public decimal Overtime { get; set; }
        public decimal VatRate { get; set; }
        //public decimal Allowance { get; set; }
        //public double Premuim { get; set; }
     
        //public double TaxOnOvertime { get; set; }
        //public double TaxOnTransport { get; set; }
        //public double TaxOnAllowance { get; set; }


    }
}