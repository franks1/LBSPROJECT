namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp_update : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentSetups", "Allowance");
            DropColumn("dbo.PaymentSetups", "Premuim");
            DropColumn("dbo.PaymentSetups", "TaxOnOvertime");
            DropColumn("dbo.PaymentSetups", "TaxOnTransport");
            DropColumn("dbo.PaymentSetups", "TaxOnAllowance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentSetups", "TaxOnAllowance", c => c.Double());
            AddColumn("dbo.PaymentSetups", "TaxOnTransport", c => c.Double());
            AddColumn("dbo.PaymentSetups", "TaxOnOvertime", c => c.Double());
            AddColumn("dbo.PaymentSetups", "Premuim", c => c.Double(nullable: false));
            AddColumn("dbo.PaymentSetups", "Allowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
    }
}
