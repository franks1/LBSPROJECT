namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taxes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasualPaymentSetups", "TaxOnAllowance", c => c.Double());
            AddColumn("dbo.PaymentSetups", "TaxOnAllowance", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentSetups", "TaxOnAllowance");
            DropColumn("dbo.CasualPaymentSetups", "TaxOnAllowance");
        }
    }
}
