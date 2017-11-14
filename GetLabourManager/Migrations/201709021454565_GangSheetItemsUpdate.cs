namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GangSheetItemsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetItems", "Group", c => c.Int(nullable: false));
            AddColumn("dbo.GangSheetItems", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetItems", "Category");
            DropColumn("dbo.GangSheetItems", "Group");
        }
    }
}
