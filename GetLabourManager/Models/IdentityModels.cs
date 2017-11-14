//using LManager.Configuration;
using GetLabourManager.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RBACv3.DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetLabourManager.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUserLogin : IdentityUserLogin<int> { }

    public class ApplicationUserClaim : IdentityUserClaim<int> { }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUserRole() : base()
        {
        }

        public ApplicationRole Role { get; set; }

        public bool IsPermissionInRole(string _permission)
        {
            bool _retVal = false;
            try
            {
                _retVal = this.Role.IsPermissionInRole(_permission);
            }
            catch (Exception)
            {
            }
            return _retVal;
        }

        public bool IsSysAdmin { get { return this.Role.IsSysAdmin; } }
    }

    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DateTime LastModified { get; set; }
        public bool Inactive { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserKey { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        public ApplicationUser()
        {
            LastModified = DateTime.Now;
            Inactive = false;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        //public virtual List<ApplicationRole> UserRoles { get; set; }

        public bool IsPermissionInUserRoles(string _permission)
        {
            bool _retVal = false;
            try
            {
                foreach (ApplicationUserRole _role in this.Roles)
                {
                    if (_role.IsPermissionInRole(_permission))
                    {
                        _retVal = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return _retVal;
        }

        public bool IsSysAdmin()
        {
            bool _retVal = false;
            try
            {
                foreach (ApplicationUserRole _role in this.Roles)
                {
                    if (_role.IsSysAdmin)
                    {
                        _retVal = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return _retVal;
        }
    }
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            //this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.RoleDescription = description;
        }

        public DateTime LastModified { get; set; }
        public bool IsSysAdmin { get; set; }
        public string RoleDescription { get; set; }

        public virtual ICollection<PERMISSION> PERMISSIONS { get; set; }

        public bool IsPermissionInRole(string _permission)
        {
            bool _retVal = false;
            try
            {
                foreach (PERMISSION _perm in this.PERMISSIONS)
                {
                    if (_perm.PermissionDescription == _permission)
                    {
                        _retVal = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return _retVal;
        }
    }

    public class RBACDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DbSet<PERMISSION> PERMISSIONS { get; set; }
        public DbSet<MasterSequence> Sequence { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<ClientDetails> ClientSetup { get; set; }
        public DbSet<EmployeeCategory> EmpCategory { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeContributions> EmployeeContribution { get; set; }
        public DbSet<EmployeeRelations> EmployeeRelation { get; set; }
        public DbSet<Gang> Gang { get; set; }
        public DbSet<GangSheetHeader> GangSheetHeader { get; set; }
        public DbSet<GangSheetItems> GangSheetItems { get; set; }
        public DbSet<FieldClients> FieldClient { get; set; }
        public DbSet<EmployeeGroup> EmployeeGroup { get; set; }
        public DbSet<EmployeeCategoryGrouping> EmployeeGrouping { get; set; }
        public DbSet<Foremen> Foremen { get; set; }
        public DbSet<FieldContainersType> FieldContainersType { get; set; }
        public DbSet<PaymentSetup> PaymentSetup { get; set; }
        public DbSet<CasualPaymentSetup> CasualPaymentSetup{ get; set; }
        public DbSet<TaxSetup> TaxSetup { get; set; }
        public DbSet<VesselContainer> VesselContainer { get; set; }
        public DbSet<CostSheet> CostSheet { get; set; }
        public DbSet<CostSheetItems> CostSheetItems { get; set; }
        public DbSet<GangMemberExemption> ExemptedCasuals { get; set; }
        public DbSet<ProcessedSheetCasual> ProcessedSheetCasual { get; set; }
        public DbSet<EmployeeClientGroup> GangClientGrouping { get; set; }
        public DbSet<OperationalWorkingHours> OperationalWorkHours { get; set; }
        public DbSet<ProcessedInvoice> ProcessedInvoice { get; set; }
        public DbSet<FieldLocationArea> LocationArea { get; set; }
        //
        //
        public DbSet<GangAllocation> GangAllocation { get; set; }
        public DbSet<AllocationContainers> AllocationContainers { get; set; }
        //
        public RBACDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<RBACDbContext>(new RBACDatabaseInitializer());
        }

        public static RBACDbContext Create()
        {
            //   var db=new RBACDbContext();
            //  db.Database.Initialize(true);
            return new RBACDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new MasterSequenceConfiguration());
            modelBuilder.Configurations.Add(new BranchConfiguration());
            modelBuilder.Configurations.Add(new ClientDetailsConfiguration());
            modelBuilder.Configurations.Add(new EmployeeCategoryConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new EmployeeContributionsConfiguration());
            modelBuilder.Configurations.Add(new EmployeeRelationsConfiguration());
            modelBuilder.Configurations.Add(new GangConfiguration());

            modelBuilder.Configurations.Add(new GangSheetHeaderConfiguration());
            modelBuilder.Configurations.Add(new GangSheetItemSConfiguration());

            modelBuilder.Configurations.Add(new FieldClientsConfiguration());

            modelBuilder.Configurations.Add(new EmployeeGroupConfiguration());
            modelBuilder.Configurations.Add(new EmployeeCategoryGroupingConfiguration());
            modelBuilder.Configurations.Add(new ForemenConfiguration());
            modelBuilder.Configurations.Add(new FieldContainersTypeConfiguration());
            modelBuilder.Configurations.Add(new PaymentSetupConfiguration());

            modelBuilder.Configurations.Add(new CasualPaymentSetupConfiguration());
            modelBuilder.Configurations.Add(new TaxSetupConfiguration());
            modelBuilder.Configurations.Add(new ContainerConfiguration());

            modelBuilder.Configurations.Add(new GangAllocationConfiguration());
            modelBuilder.Configurations.Add(new AllocationContainersConfiguration());

            modelBuilder.Configurations.Add(new CostSheetConfiguration());
            modelBuilder.Configurations.Add(new CostSheetItemsConfiguration());
            modelBuilder.Configurations.Add(new GangMemberExemptionConfiguration());
            //
            modelBuilder.Configurations.Add(new EmployeeClientGroupConfiguration());
            modelBuilder.Configurations.Add(new OperationalWorkingHoursConfiguration());

            modelBuilder.Configurations.Add(new ProcessedSheetCasualConfiguration());
            modelBuilder.Configurations.Add(new ProcessedInvoiceConfiguration());
            modelBuilder.Configurations.Add(new FieldLocationAreaConfiguration());

            //
            //modelBuilder.Configurations.Add(new LicenseStoreConfiguration());

            modelBuilder.Entity<ApplicationUser>().ToTable("USERS").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationRole>().ToTable("ROLES").Property(p => p.Id).HasColumnName("RoleId");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("LNK_USER_ROLE");
            modelBuilder.Entity<ApplicationRole>().
            HasMany(c => c.PERMISSIONS).
            WithMany(p => p.ROLES).
            Map(
                m =>
                {
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("PermissionId");
                    m.ToTable("LNK_ROLE_PERMISSION");
                });
        }
    }
}