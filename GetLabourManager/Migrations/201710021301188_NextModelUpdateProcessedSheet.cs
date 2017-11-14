namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NextModelUpdateProcessedSheet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessedSheetCasuals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CostSheet = c.Int(nullable: false),
                        InvoiceCode = c.String(nullable: false, maxLength: 35),
                        CasualCode = c.String(nullable: false, maxLength: 30),
                        Group = c.String(nullable: false, maxLength: 50),
                        GangType = c.String(nullable: false, maxLength: 50),
                        Basic = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Overtime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Premium = c.Double(nullable: false),
                        NightAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VAT = c.Double(nullable: false),
                        Transportation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreparedBy = c.Int(nullable: false),
                        TPointer = c.String(),
                    }).Index(x=>x.Id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessedSheetCasuals");
        }
    }
}
