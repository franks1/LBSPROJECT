using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class GangMemberExemption
    {
        public int Id { get; set; }
        public string CasualCode { get; set; }
        public string RequestCode { get; set; }
        public DateTime AddedOn { get; set; }
        public int PerformedBy { get; set; }
        public int RequestId { get; set; }
    }
}