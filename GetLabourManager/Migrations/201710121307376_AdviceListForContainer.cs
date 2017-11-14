namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdviceListForContainer : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdviceListContainer", x =>
                new
                {
                    term = x.String(20)
                },
                @"select vc.Continer as Container,ac.ContainerNumber,eg.Category from AllocationContainers ac
                inner join VesselContainers vc on  ac.ContainerId=vc.Id
                inner join EmployeeCategories eg on ac.CategoryId=eg.Id
                where RequestCode=@term");
        }
        
        public override void Down()
        {
            DropStoredProcedure("AdviceListContainer");
        }
    }
}
