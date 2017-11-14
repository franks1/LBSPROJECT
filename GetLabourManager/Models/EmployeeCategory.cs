using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class EmployeeCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int GroupId { get; set; }
    }
}