using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class Foremen
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public string Branch { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsClientForeman { get; set; }
        public string Status { get; set; }

        public string FullName { get { return FirstName + " " + MiddleName + " " + LastName; } }
    }
}