namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AdviceListForApproval : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AdviceLisApprovalProc", x =>
            new
            {
                term = x.String(20)
            },
                @"SELECT i.StaffCode, (e.FirstName+' '+e.MiddleName+' '+e.LastName) as FullName,
                g.[Description],eg.GroupName from GangSheetHeaders h
                inner join GangSheetItems i on h.Id=i.Header
                inner join Employees e on i.StaffCode = e.Code
                inner join Gangs g on h.GangCode=g.Code
                inner join EmployeeGroups eg on i.[Group]=eg.Id
                where h.RequestCode=@term");
        }

        public override void Down()
        {

            DropStoredProcedure("AdviceLisApprovalProc");
        }
    }
}
