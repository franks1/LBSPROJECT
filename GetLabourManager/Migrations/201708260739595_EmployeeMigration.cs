namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        FirstName = c.String(nullable: false, maxLength: 70),
                        LastName = c.String(nullable: false, maxLength: 80),
                        MiddleName = c.String(nullable: false, maxLength: 90),
                        Dob = c.DateTime(nullable: false),
                        Gender = c.String(maxLength: 10),
                        Telephone1 = c.String(nullable: false, maxLength: 18),
                        Telephone2 = c.String(maxLength: 18),
                        EmailAddress = c.String(maxLength: 50),
                        Address = c.String(maxLength: 80),
                        Region = c.String(nullable: false, maxLength: 30),
                        Category = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        ImagePix = c.Binary(),
                        SSN = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id).Index(x=>x.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
