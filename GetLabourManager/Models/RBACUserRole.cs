using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class RBACUserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("UserId")]
        public RBACUser User { get; set; }
        [ForeignKey("RoleId")]
        public RBACRole Role { get; set; }

    }
}