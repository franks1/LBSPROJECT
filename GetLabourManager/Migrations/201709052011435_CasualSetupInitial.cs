namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasualSetupInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasualPaymentSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Basic = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NightAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportationAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Overtime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SSF = c.Double(nullable: false),
                        UnionDues = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PF = c.Double(nullable: false),
                        Welfare = c.Decimal(nullable: false, precision: 18, scale: 2),
                    }).Index(x=>x.Id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CasualPaymentSetups");
        }
    }
}
