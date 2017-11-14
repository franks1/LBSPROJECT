namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class category_grouping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeCategoryGroupings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Group = c.Int(nullable: false),
                    })
                    .Index(x=>x.Category)
                    .Index(x=>x.Group)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeCategoryGroupings");
        }
    }
}
