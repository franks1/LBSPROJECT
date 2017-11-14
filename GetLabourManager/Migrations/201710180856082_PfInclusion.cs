namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PfInclusion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcessedSheetCasuals", "PF", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProcessedSheetCasuals", "PF");
        }
    }
}
