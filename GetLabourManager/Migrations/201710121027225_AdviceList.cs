namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdviceList : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GangRequestList",
                @"select G.Id, RequestCode,f.Name,g.DateIssued,g.Status
                 from GangSheetHeaders g
                inner join FieldClients f on g.FieldClient=f.Id
                where g.[Status] not in('APPLIED','CANCELLED')
                ORDER BY Id");

        }
        
        public override void Down()
        {
            DropStoredProcedure("GangRequestList");
        }
    }
}
