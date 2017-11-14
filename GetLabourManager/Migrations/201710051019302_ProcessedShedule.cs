namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessedShedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcessedSheetCasuals", "SheetKind", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProcessedSheetCasuals", "TaxOnTandT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProcessedSheetCasuals", "SSF", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProcessedSheetCasuals", "TaxOnBasic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProcessedSheetCasuals", "TaxOnOverTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProcessedSheetCasuals", "UnionDues", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProcessedSheetCasuals", "Welfare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProcessedSheetCasuals", "Welfare");
            DropColumn("dbo.ProcessedSheetCasuals", "UnionDues");
            DropColumn("dbo.ProcessedSheetCasuals", "TaxOnOverTime");
            DropColumn("dbo.ProcessedSheetCasuals", "TaxOnBasic");
            DropColumn("dbo.ProcessedSheetCasuals", "SSF");
            DropColumn("dbo.ProcessedSheetCasuals", "TaxOnTandT");
            DropColumn("dbo.ProcessedSheetCasuals", "SheetKind");
        }
    }
}
