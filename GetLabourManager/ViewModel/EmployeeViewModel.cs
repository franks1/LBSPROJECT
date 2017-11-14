using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Dob { get; set; }
        public DateTime DateJoined { get; set; }
        public string Gender { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
        public int Category { get; set; }
        public int BranchId { get; set; }
        public byte[] ImagePix { get; set; }
        public string SSN { get; set; }

        public string GuarantorName { get; set; }
        public string GuarantorPhone { get; set; }
        public string GuarantorRelation { get; set; }
        public string GuarantorAddress { get; set; }
        public string NextofKinName { get; set; }
        public string NextofKinPhone { get; set; }
        public string NextofKinRelation { get; set; }
        public string NextofKinAddress { get; set; }
        public bool SSF { get; set; }
        public bool Welfare { get; set; }
        public bool UnionDues { get; set; }
    }
}