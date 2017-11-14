namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldAgent_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FieldClients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 50),
                        Telephone1 = c.String(maxLength: 20),
                        Telephone2 = c.String(maxLength: 20),
                        EmailAddress = c.String(maxLength: 30),
                        Status = c.String(),
                        FieldClientType = c.String(maxLength: 20),
                    }).Index(x=>x.Id).PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FieldClients");
        }
    }
}
