namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayType = c.Boolean(nullable: false),
                        Client = c.Int(nullable: false),
                        Group = c.Int(nullable: false),
                        Basic = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NightAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportationAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Overtime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Premuim = c.Double(nullable: false),
                        VatRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    }).Index(x=>x.Id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentSetups");
        }
    }
}
