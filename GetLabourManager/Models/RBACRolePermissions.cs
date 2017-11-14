using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class RBACRolePermissions
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        [ForeignKey("RoleId")]
        public RBACRole Role { get; set; }
        [ForeignKey("PermissionId")]
        public RBACPermission Permissions { get; set; }
    }
}