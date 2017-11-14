namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessedSheduleModelEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcessedSheetCasuals", "GrossAmount", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AddColumn("dbo.ProcessedSheetCasuals", "NetAmount", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "Basic", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "Overtime", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnTandT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "SSF", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnBasic", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnOverTime", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "UnionDues", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "Welfare", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "NightAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.ProcessedSheetCasuals", "Transportation", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProcessedSheetCasuals", "Transportation", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "NightAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "Welfare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "UnionDues", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnOverTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnBasic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "SSF", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "TaxOnTandT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "Overtime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProcessedSheetCasuals", "Basic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ProcessedSheetCasuals", "NetAmount");
            DropColumn("dbo.ProcessedSheetCasuals", "GrossAmount");
        }
    }
}
