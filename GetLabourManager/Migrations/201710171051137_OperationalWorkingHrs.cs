namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperationalWorkingHrs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperationalWorkingHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkingHours = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OperationalWorkingHours");
        }
    }
}
