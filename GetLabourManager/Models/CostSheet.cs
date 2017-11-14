using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class CostSheet
    {
        public int Id { get; set; }
        public DateTime DatePrepared { get; set; }
        public string CostSheetNumber { get; set; }
        public int RequestHeader { get; set; }
        public int PreparedBy { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public int Client { get; set; }
        public double HoursWorked { get; set; }
        public double OvertimeHours { get; set; }
        public List<CostSheetItems> SheetItems { get; set; }
    }
}