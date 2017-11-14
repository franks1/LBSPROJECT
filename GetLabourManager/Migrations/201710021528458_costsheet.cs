namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class costsheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheetItems", "OvertimeHrs", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostSheetItems", "OvertimeHrs");
        }
    }
}
