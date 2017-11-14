using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class GangAllocation
    {
        public int Id { get; set; }
        public string RequestCode { get; set; }
        public DateTime Allocated { get; set; }
        public int AllocatedBy { get; set; }
        public DateTime AllocatedOn { get; set; }
        public virtual List<AllocationContainers> Containers { get; set; }
    }
}