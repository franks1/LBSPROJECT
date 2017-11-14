namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GangAdviceProc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GangAdviceListProc",
                @"select DateIssued,RequestCode,gor.Description as Gang, u.UserName,fc.Name as PreparedFor,
                COUNT(gs.StaffCode) as  TotalCasuals,g.[Status] from GangSheetHeaders g
                inner join USERS u on g.PreparedBy=u.UserId
                inner join FieldClients fc on g.FieldClient = fc.Id
                inner join GangSheetItems gs on g.Id=gs.Header
                inner join Gangs gor on g.GangCode = gor.Code
                group by DateIssued,RequestCode,gor.Description, u.UserName,fc.Name,g.[Status]");
        }

        public override void Down()
        {
            DropStoredProcedure("GangAdviceListProc");
        }
    }
}
