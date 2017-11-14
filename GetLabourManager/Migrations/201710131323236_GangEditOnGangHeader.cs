namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GangEditOnGangHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetHeaders", "GangType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetHeaders", "GangType");
        }
    }
}
