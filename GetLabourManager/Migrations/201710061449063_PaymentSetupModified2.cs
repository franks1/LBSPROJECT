namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentSetupModified2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasualPaymentSetups", "TaxOnBasic", c => c.Double());
            AddColumn("dbo.CasualPaymentSetups", "TaxOnOvertime", c => c.Double());
            AddColumn("dbo.CasualPaymentSetups", "TaxOnTransport", c => c.Double());
            AddColumn("dbo.PaymentSetups", "TaxOnOvertime", c => c.Double());
            AddColumn("dbo.PaymentSetups", "TaxOnTransport", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentSetups", "TaxOnTransport");
            DropColumn("dbo.PaymentSetups", "TaxOnOvertime");
            DropColumn("dbo.CasualPaymentSetups", "TaxOnTransport");
            DropColumn("dbo.CasualPaymentSetups", "TaxOnOvertime");
            DropColumn("dbo.CasualPaymentSetups", "TaxOnBasic");
        }
    }
}
