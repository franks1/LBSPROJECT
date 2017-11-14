using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class ProcessedSheetCasual
    {
        public int Id { get; set; }
        public int CostSheet { get; set; }
        public string InvoiceCode { get; set; }
        public string CasualCode { get; set; }
        public string  Group { get; set; }
        public string GangType { get; set; }
        public decimal Basic { get; set; }
        public decimal Overtime { get; set; }
        public double Premium { get; set; }
        public bool SheetKind { get; set; }
        public decimal TaxOnTandT { get; set; }
        public decimal SSF { get; set; }
        public decimal TaxOnBasic { get; set; }
        public decimal TaxOnOverTime { get; set; }
        public decimal UnionDues { get; set; }
        public decimal Welfare { get; set; }
        public decimal NightAllowance { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal PF { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxOnAllowance { get; set; }
        public decimal Transportation { get; set; }
        public int PreparedBy { get; set; }
        
        public string TPointer { get; set; }
    }
}