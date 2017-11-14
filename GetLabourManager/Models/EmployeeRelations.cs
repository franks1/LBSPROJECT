using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class EmployeeRelations
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string GuarantorName { get; set; }
        public string GuarantorPhone { get; set; }
        public string GuarantorRelation { get; set; }
        public string GuarantorAddress { get; set; }
        public string NextofKinName { get; set; }
        public string NextofKinPhone { get; set; }
        public string NextofKinRelation { get; set; }
        public string NextofKinAddress { get; set; }
    }
}