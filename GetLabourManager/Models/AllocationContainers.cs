using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class AllocationContainers
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }
        public string ContainerNumber { get; set; }
        public int CategoryId { get; set; }
        public int GroupId  { get; set; }
        public string RequestCode { get; set; }
        public int AllocationId { get; set; }
        [ForeignKey("AllocationId")]
        public virtual GangAllocation GangAllocated { get; set; }
    }
}