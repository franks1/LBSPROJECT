using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class RBACPermission
    {
        public int Id { get; set; }
        public string PermissionGroup { get; set; }
        public string PermissionDescription { get; set; }
        public virtual List<RBACRolePermissions> RolePermissions { get; set; }
    }
}