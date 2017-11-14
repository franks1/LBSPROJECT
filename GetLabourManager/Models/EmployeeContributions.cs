using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class EmployeeContributions
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string SSN { get; set; }
        public bool SSF { get; set; }
        public bool Welfare { get; set; }
        public bool UnionDues { get; set; }

    }
}