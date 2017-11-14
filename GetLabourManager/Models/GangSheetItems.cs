using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class GangSheetItems
    {
        public int Id { get; set; }
        public string StaffCode { get; set; }
        public int Group { get; set; }
        public int Category { get; set; }
        public int Header { get; set; }

        [ForeignKey("Header")]
        public GangSheetHeader SheetHeader { get; set; }
        public int AllocationId { get; set; }

    }
}