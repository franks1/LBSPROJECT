using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class GangSheetHeader
    {
        public int Id { get; set; }
        public string RequestCode { get; set; }
        public DateTime DateIssued { get; set; }
        public string Narration { get; set; }
        public string Status { get; set; }
        public int PreparedBy { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovalNote { get; set; }
        public string WorkShift { get; set; }
        public string WorkWeek { get; set; }
        public string GangCode { get; set; }
        public int GangType { get; set; }
        public int FieldClient { get; set; }
        public int FieldLocation { get; set; }
        public List<GangSheetItems> SheetItems { get; set; }
    }
}