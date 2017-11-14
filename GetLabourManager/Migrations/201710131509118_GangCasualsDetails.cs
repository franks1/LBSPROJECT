namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GangCasualsDetails : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GangCasualsDetailsProc", x => new { term = x.String(25) },
                @"select h.DateIssued, i.StaffCode,(e.FirstName+' '+Isnull(e.MiddleName,'')+e.LastName) as Name,
                eg.GroupName,ec.Category
                    from GangSheetHeaders h
                inner join GangSheetItems i on h.Id=i.Header
                inner join Employees e on i.StaffCode=e.Code
                inner join EmployeeCategories ec on  h.GangType=ec.Id
                inner join EmployeeGroups eg on eg.Id=i.[Group]
                where h.RequestCode=@term"
        );
        }

        public override void Down()
        {
            DropStoredProcedure("GangCasualsDetailsProc");
        }
    }
}
