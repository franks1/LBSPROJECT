using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class CostSheetItems
    {
        public int Id { get; set; }
        public DateTime RaisedOn { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string Gang { get; set; }
        public string GroupName { get; set; }
        public int Container { get; set; }
        public double HourseWorked { get; set; }
        public double OvertimeHrs { get; set; }
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public CostSheet CostSheetEntry { get; set; }
    }
}