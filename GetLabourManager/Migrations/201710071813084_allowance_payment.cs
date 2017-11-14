namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allowance_payment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasualPaymentSetups", "Allowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AddColumn("dbo.PaymentSetups", "Allowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentSetups", "Allowance");
            DropColumn("dbo.CasualPaymentSetups", "Allowance");
        }
    }
}
