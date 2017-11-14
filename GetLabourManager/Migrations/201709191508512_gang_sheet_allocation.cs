namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gang_sheet_allocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetItems", "AllocationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetItems", "AllocationId");
        }
    }
}
