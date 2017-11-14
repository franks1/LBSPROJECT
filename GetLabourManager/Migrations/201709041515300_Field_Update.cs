namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Field_Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FieldContainersTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerType = c.String(nullable: false, maxLength: 30),
                    }).Index(a => a.Id)
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Foremen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 70),
                        LastName = c.String(nullable: false, maxLength: 60),
                        DateOfBirth = c.DateTime(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        IsClientForeman = c.Boolean(nullable: false),
                        Status = c.String(),
                    }).Index(a=>a.Id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Foremen");
            DropTable("dbo.FieldContainersTypes");
        }
    }
}
