namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentWorkScheme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GangSheetHeaders", "WorkShift", c => c.String(maxLength: 10));
            AddColumn("dbo.GangSheetHeaders", "WorkWeek", c => c.String(maxLength: 15));
            AddColumn("dbo.PaymentSetups", "WorkShift", c => c.String(maxLength: 10));
            AddColumn("dbo.PaymentSetups", "WorkWeek", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentSetups", "WorkWeek");
            DropColumn("dbo.PaymentSetups", "WorkShift");
            DropColumn("dbo.GangSheetHeaders", "WorkWeek");
            DropColumn("dbo.GangSheetHeaders", "WorkShift");
        }
    }
}
