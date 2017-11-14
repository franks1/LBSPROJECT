namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasualPaymentModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasualPaymentSetups", "Group", c => c.Int(nullable: false));
            AddColumn("dbo.CasualPaymentSetups", "WorkShift", c => c.String(maxLength: 10));
            AddColumn("dbo.CasualPaymentSetups", "WorkWeek", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CasualPaymentSetups", "WorkWeek");
            DropColumn("dbo.CasualPaymentSetups", "WorkShift");
            DropColumn("dbo.CasualPaymentSetups", "Group");
        }
    }
}
