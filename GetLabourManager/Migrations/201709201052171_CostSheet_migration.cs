namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheet_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePrepared = c.DateTime(nullable: false),
                        CostSheetNumber = c.String(nullable: false, maxLength: 25),
                        RequestHeader = c.Int(nullable: false),
                        PreparedBy = c.Int(nullable: false),
                        Note = c.String(),
                        Status = c.String(maxLength: 15),
                        Client = c.Int(nullable: false),
                    }).Index(x=>x.Id)
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CostSheetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RaisedOn = c.DateTime(nullable: false),
                        StaffCode = c.String(maxLength: 20),
                        FullName = c.String(maxLength: 150),
                        Gang = c.String(maxLength: 20),
                        GroupName = c.String(maxLength: 20),
                        Container = c.Int(nullable: false),
                        HourseWorked = c.Double(),
                        CostSheetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId)
                .Index(t => t.CostSheetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CostSheetItems", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.CostSheetItems", new[] { "CostSheetId" });
            DropTable("dbo.CostSheetItems");
            DropTable("dbo.CostSheets");
        }
    }
}
