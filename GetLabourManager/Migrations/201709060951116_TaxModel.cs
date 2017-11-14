namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaxSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Basic = c.Double(nullable: false),
                        OverTime = c.Double(nullable: false),
                        TT = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TaxSetups");
        }
    }
}
