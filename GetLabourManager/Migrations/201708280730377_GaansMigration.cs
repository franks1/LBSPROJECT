namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GaansMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gangs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 25),
                        Description = c.String(nullable: false, maxLength: 30),
                        Branch = c.Int(nullable: false),
                        Status = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Gangs");
        }
    }
}
