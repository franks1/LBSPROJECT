namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldLocationmigra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetHeaders", "FieldLocation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GangSheetHeaders", "FieldLocation");
        }
    }
}
