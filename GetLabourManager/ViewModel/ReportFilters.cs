using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.ViewModel
{
    public class ReportFilters
    {
        public string ReportOption { get; set; }
        public int ClientId { get; set; }
        public string GangType { get; set; }
        public string ClientName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string Casual { get; set; }
        public bool IsCasual { get; set; }
        public string RequestCode { get; set; }
        public string Invoice { get; set; }
        public string[] Categories { get; set; }
        public bool IsDateRangeActive { get; set; }
    }
}