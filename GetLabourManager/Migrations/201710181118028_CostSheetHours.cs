namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheets", "HoursWorked", c => c.Double(nullable: false));
            AddColumn("dbo.CostSheets", "OvertimeHours", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostSheets", "OvertimeHours");
            DropColumn("dbo.CostSheets", "HoursWorked");
        }
    }
}
