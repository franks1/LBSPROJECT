namespace RBACv3.DatabaseInitializer
{
    using Microsoft.AspNet.Identity;
    using RBACv3.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using GetLabourManager.Models;
    using GetLabourManager;

    public class RBACDatabaseInitializer : CreateDatabaseIfNotExists<RBACDbContext>
    {
        private readonly string c_SysAdmin = "Administrator";
        private readonly string c_DefaultUser = "Default User";

        protected override void Seed(RBACDbContext context)
        {
            //Create Default Roles...
            IList<ApplicationRole> defaultRoles = new List<ApplicationRole>();
            defaultRoles.Add(new ApplicationRole { Name = c_SysAdmin, RoleDescription = "Allows system administration of Users/Roles/Permissions", LastModified = DateTime.Now, IsSysAdmin = true });
            defaultRoles.Add(new ApplicationRole { Name = c_DefaultUser, RoleDescription = "Default role with limited permissions", LastModified = DateTime.Now, IsSysAdmin = false });

            ApplicationRoleManager RoleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
            foreach (ApplicationRole role in defaultRoles)
            {

                RoleManager.Create(role);
            }

            //Create User...
            var user = new ApplicationUser { UserName = "Admin", Email = "fkbani@gmail.com", Firstname = "System", Lastname = "Administrator", LastModified = DateTime.Now, Inactive = false, EmailConfirmed = true };

            ApplicationUserManager UserManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var result = UserManager.Create(user, "@12345");

            if (result.Succeeded)
            {
                //Add User to Admin Role...
                UserManager.AddToRole(user.Id, c_SysAdmin);
            }


            //Create Default User...
            user = new ApplicationUser { UserName = "DefaultUser", Email = "defaultuser@somedomain.com", Firstname = "Default", Lastname = "User", LastModified = DateTime.Now, Inactive = false, EmailConfirmed = true };
            result = UserManager.Create(user, "password@1");

            if (result.Succeeded)
            {
                //Add User to Admin Role...
                UserManager.AddToRole(user.Id, c_DefaultUser);
            }

            //Create User with NO Roles...
            user = new ApplicationUser { UserName = "Guest", Email = "guest@somedomain.com", Firstname = "Guest", Lastname = "User", LastModified = DateTime.Now, Inactive = false, EmailConfirmed = true };
            result = UserManager.Create(user, "password@2");

            base.Seed(context);

            //Create a permission...
            //PERMISSION _permission = new PERMISSION();
            //_permission.PermissionDescription = "DashBoard-Index";
            //// _permission.PermissionGroup = "Student";
            //ApplicationRoleManager.AddPermission(_permission);

            //Add Permission to DefaultUser Role...
            ApplicationRoleManager.AddPermission2Role(context.Roles.Where(p => p.Name == c_DefaultUser).First().Id, context.PERMISSIONS.First().PermissionId);

            //SEQUENCE
            List<MasterSequence> sequence = new List<MasterSequence>()
            {
                new  MasterSequence() { SequenceLength =4, SequenceNumber=5, SequenceName="CUSTOMER",
                    SequencePrefix ="CS",  SequenceSuffix="T", SequenceType="CUSTOMER" },
                new  MasterSequence() { SequenceLength =4, SequenceNumber=5, SequenceName="TRANSACTION",
                    SequencePrefix ="T", SequenceSuffix="R", SequenceType="TRANSACTION" },
                 new  MasterSequence() { SequenceLength =4, SequenceNumber=5, SequenceName="ITEM",
                    SequencePrefix ="I", SequenceSuffix="G", SequenceType="ITEM CATEGORY" }
            };
            context.Sequence.AddRange(sequence); context.SaveChanges();
        }
    }
}
