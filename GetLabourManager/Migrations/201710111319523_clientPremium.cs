namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientPremium : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldClients", "Premium", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FieldClients", "Premium");
        }
    }
}
