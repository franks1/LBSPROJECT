namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Allocation_Containers_Gang : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AllocationContainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerId = c.Int(nullable: false),
                        ContainerNumber = c.String(maxLength: 25),
                        AllocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GangAllocations", t => t.AllocationId)
                .Index(t => t.AllocationId);
            
            CreateTable(
                "dbo.GangAllocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestCode = c.String(nullable: false, maxLength: 30),
                        Allocated = c.DateTime(nullable: false),
                        AllocatedBy = c.Int(nullable: false),
                        AllocatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AllocationContainers", "AllocationId", "dbo.GangAllocations");
            DropIndex("dbo.AllocationContainers", new[] { "AllocationId" });
            DropTable("dbo.GangAllocations");
            DropTable("dbo.AllocationContainers");
        }
    }
}
