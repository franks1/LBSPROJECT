namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GangSheetHEader_And_Items : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GangSheetHeaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestCode = c.String(nullable: false, maxLength: 20),
                        DateIssued = c.DateTime(nullable: false),
                        Narration = c.String(nullable: false, maxLength: 25),
                        Status = c.String(),
                        PreparedBy = c.Int(nullable: false),
                        ApprovedBy = c.Int(nullable: false),
                        GangCode = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GangSheetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffCode = c.String(nullable: false, maxLength: 20),
                        Header = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GangSheetHeaders", t => t.Header)
                .Index(t => t.Header);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GangSheetItems", "Header", "dbo.GangSheetHeaders");
            DropIndex("dbo.GangSheetItems", new[] { "Header" });
            DropTable("dbo.GangSheetItems");
            DropTable("dbo.GangSheetHeaders");
        }
    }
}
