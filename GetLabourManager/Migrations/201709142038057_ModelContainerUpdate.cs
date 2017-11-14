namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelContainerUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AllocationContainers", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.AllocationContainers", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AllocationContainers", "GroupId");
            DropColumn("dbo.AllocationContainers", "CategoryId");
        }
    }
}
