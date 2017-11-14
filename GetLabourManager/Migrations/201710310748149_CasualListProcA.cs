namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasualListProcA : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CasualListProcA", 
                @"select Code,(e.FirstName+' '+Isnull(e.MiddleName,' ')+' '+e.LastName) as FullName
                ,e.Telephone1,e.DateJoined, ec.Category from Employees e
                inner join EmployeeCategories ec on e.Category = ec.Id"
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("CasualListProcA");
        }
    }
}
