using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class ProcessedInvoice
    {
        public int Id { get; set; }
        public DateTime ProccessdOn { get; set; }
        public int Client { get; set; }
        public string Invoice { get; set; }
        public string ProcessedBy { get; set; }
        public string Status { get; set; }
    }
}