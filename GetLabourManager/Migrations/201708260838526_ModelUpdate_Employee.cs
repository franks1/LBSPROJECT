namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate_Employee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "DateJoined", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "Status", c => c.String(maxLength: 12));
            AddColumn("dbo.EmployeeContributions", "SSN", c => c.String(maxLength: 30));
            DropColumn("dbo.Employees", "SSN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "SSN", c => c.String(maxLength: 25));
            DropColumn("dbo.EmployeeContributions", "SSN");
            DropColumn("dbo.Employees", "Status");
            DropColumn("dbo.Employees", "DateJoined");
        }
    }
}
