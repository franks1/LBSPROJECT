namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FieldLocationAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(nullable: false, maxLength: 50),
                        LocationLat = c.String(nullable: false, maxLength: 50),
                        LocationLong = c.String(nullable: false, maxLength: 50),
                    }).Index(x=>x.Id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FieldLocationAreas");
        }
    }
}
