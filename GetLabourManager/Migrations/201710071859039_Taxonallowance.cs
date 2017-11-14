namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Taxonallowance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcessedSheetCasuals", "TaxOnAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            DropColumn("dbo.ProcessedSheetCasuals", "VAT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProcessedSheetCasuals", "VAT", c => c.Double(nullable: false));
            DropColumn("dbo.ProcessedSheetCasuals", "TaxOnAllowance");
        }
    }
}
