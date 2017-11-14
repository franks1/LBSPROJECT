using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
        public int Category { get; set; }
        public int BranchId { get; set; }
        public byte[] ImagePix { get; set; }
        public DateTime DateJoined { get; set; }
        public string Status { get; set; }
        public string FullName
        {
            get { return  FirstName + " " + MiddleName + " " + LastName; }
        }
    }
}