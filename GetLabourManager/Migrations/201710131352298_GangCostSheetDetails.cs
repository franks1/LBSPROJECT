namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GangCostSheetDetails : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CostSheetRquestDetailsProc", x => new
            {
                term = x.String(25)
            },
            @"select h.RequestCode,fc.Name,g.[Description] as Gang,ec.Category,COUNT(distinct gi.StaffCode) as Casuals,
            COUNT(distinct ac.ContainerId) as Vessels
            from GangSheetHeaders h
            inner join FieldClients fc on h.FieldClient=fc.id
            inner join Gangs g on h.GangCode=g.Code
            inner join EmployeeCategories ec on h.gangtype = ec.Id
            inner join GangSheetItems gi on h.Id=gi.Header
            inner join AllocationContainers ac on h.RequestCode=ac.RequestCode
            where h.RequestCode=@term
            group by h.RequestCode,fc.Name,g.[Description],ec.Category");
        }

        public override void Down()
        {
            DropStoredProcedure("CostSheetRquestDetailsProc");
        }
    }
}
