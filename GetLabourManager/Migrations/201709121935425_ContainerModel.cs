namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContainerModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VesselContainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Continer = c.String(nullable: false, maxLength: 80),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VesselContainers");
        }
    }
}
