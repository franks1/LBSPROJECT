namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class request_code_container : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AllocationContainers", "RequestCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AllocationContainers", "RequestCode");
        }
    }
}
