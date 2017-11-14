using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class RBACRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsSystemAdmin { get; set; }
        public virtual List<RBACUserRole> UserRole { get; set; }
        public virtual List<RBACRolePermissions> RolePermissions { get; set; }

    }
}