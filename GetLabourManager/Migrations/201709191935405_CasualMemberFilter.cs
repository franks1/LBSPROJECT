namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CasualMemberFilter : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CasualMembersFilter",
                x => new
                {
                    term = x.String(maxLength: 15)
                },
                     @" SELECT  distinct t1.RequestCode,t1.StaffCode,t1.FullName,t1.Gang,t1.GroupName,
                 dbo.[CostSheetTotalContainers](t1.GroupName,t1.RequestCode) as Containers
                 FROM (
                select distinct gs.RequestCode, si.staffcode,(emp.FirstName+' '+emp.MiddleName+' '+emp.LastName) as FullName
                ,eg.GroupName,g.[Description] as Gang
                 from GangSheetItems si
                left join Employees emp on emp.Code= si.StaffCode
                join GangSheetHeaders gs on gs.Id=SI.Header
                join Gangs g on g.Code=gs.GangCode
                join EmployeeGroups eg on eg.Id= si.[Group])t1
                where t1.RequestCode=@term"
                );

        }

        public override void Down()
        {
            DropStoredProcedure("CasualMembersFilter");
        }
    }
}
