namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editGangHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetHeaders", "FieldClient", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetHeaders", "FieldClient");
        }
    }
}
