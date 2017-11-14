namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsHead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70),
                        Address = c.String(maxLength: 80),
                        Phone1 = c.String(maxLength: 20),
                        Phone2 = c.String(maxLength: 20),
                        EmailAddress = c.String(maxLength: 70),
                        EmailAddress2 = c.String(maxLength: 70),
                        Branch = c.Int(nullable: false),
                        LicenseKey = c.String(maxLength: 100),
                        LicenseStatus = c.String(maxLength: 10),
                        ImagePix = c.Binary(),
                        IsHeadOffice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClientDetails");
            DropTable("dbo.Branches");
        }
    }
}
