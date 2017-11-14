namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class approvalNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetHeaders", "ApprovalNote", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetHeaders", "ApprovalNote");
        }
    }
}
