using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class FieldClients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public string FieldClientType { get; set; }
        public double Premium { get; set; }
    }
}