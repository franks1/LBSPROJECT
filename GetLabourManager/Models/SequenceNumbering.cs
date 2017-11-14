using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class SequenceNumbering
    {

        public int Id { get; set; }
        [Display(Name = "Name")]
        public string SequenceName { get; set; }
        [Display(Name = "Start Number")]
        public int SequenceNumber { get; set; }
        [Display(Name = "Length")]
        public int SequenceLength { get; set; }
        [Display(Name = "Prefix")]
        public string SequencePrefix { get; set; }
        [Display(Name = "Suffix")]
        public string SequenceSuffix { get; set; }
        [Display(Name = "Type")]
        public string SequenceType { get; set; }
        // public virtual List<DrugCategory> DrugTypes { get; set; }
    }
}