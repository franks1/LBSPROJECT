using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class Gang
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Branch { get; set; }
        public string Status { get; set; }
    }
}