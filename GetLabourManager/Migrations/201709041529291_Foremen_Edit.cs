namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foremen_Edit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foremen", "ClientId", c => c.Int(nullable: false));
            AddColumn("dbo.Foremen", "Branch", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Foremen", "Branch");
            DropColumn("dbo.Foremen", "ClientId");
        }
    }
}
