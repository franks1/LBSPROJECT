using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.ViewModel
{
    public class GangAdviceViewModel
    {
        public string RequestCode { get; set; }
        public DateTime DateIssued { get; set; }
        public int FieldClient { get; set; }
        public string Shift { get; set; }
        public string Week { get; set; }
        public string Gang { get; set; }
        public int FieldLocation { get; set; }
        public List<GangItems> Entries { get; set; }
    }

    public class GangItems
    {
        public string RequestCode { get; set; }
        public string StaffCode { get; set; }
        public string Name { get; set; }
        public string Gang{ get; set; }
        public string GangCode { get; set; }
        public int Group { get; set; }
        public string GroupName { get; set; }
        public int Category { get; set; }

    }

}