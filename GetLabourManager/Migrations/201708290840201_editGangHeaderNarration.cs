namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editGangHeaderNarration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GangSheetHeaders", "Narration", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GangSheetHeaders", "Narration", c => c.String(nullable: false, maxLength: 25));
        }
    }
}