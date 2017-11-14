namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessedInvoiceModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessedInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProccessdOn = c.DateTime(nullable: false),
                        Client = c.Int(nullable: false),
                        Invoice = c.String(nullable: false, maxLength: 25),
                        ProcessedBy = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessedInvoices");
        }
    }
}
