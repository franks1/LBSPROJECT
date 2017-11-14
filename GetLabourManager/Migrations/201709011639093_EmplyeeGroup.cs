namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmplyeeGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmployeeCategories", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeCategories", "GroupId");
            DropTable("dbo.EmployeeGroups");
        }
    }
}
