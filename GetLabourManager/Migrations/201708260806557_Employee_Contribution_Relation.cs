namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Employee_Contribution_Relation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeContributions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.Int(nullable: false),
                        SSF = c.Boolean(nullable: false),
                        Welfare = c.Boolean(nullable: false),
                        UnionDues = c.Boolean(nullable: false),
                    }).Index(x=>x.StaffId)
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeRelations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.Int(nullable: false),
                        GuarantorName = c.String(nullable: false, maxLength: 80),
                        GuarantorPhone = c.String(nullable: false, maxLength: 18),
                        GuarantorRelation = c.String(maxLength: 20),
                        GuarantorAddress = c.String(nullable: false, maxLength: 70),
                        NextofKinName = c.String(nullable: false, maxLength: 80),
                        NextofKinPhone = c.String(nullable: false, maxLength: 18),
                        NextofKinRelation = c.String(maxLength: 20),
                        NextofKinAddress = c.String(nullable: false, maxLength: 70),
                    }).Index(x => x.StaffId)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeRelations");
            DropTable("dbo.EmployeeContributions");
        }
    }
}
