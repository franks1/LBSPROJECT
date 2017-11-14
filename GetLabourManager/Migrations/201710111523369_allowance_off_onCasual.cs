namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allowance_off_onCasual : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CasualPaymentSetups", "Allowance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CasualPaymentSetups", "Allowance", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
    }
}
