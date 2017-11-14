namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TierTwoContribution : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("TierTwoProc",
                @"
                    declare @charge as float;
                    set @charge=0.05
                    SELECT  gsh.DateIssued, p.CasualCode,e.FirstName,e.MiddleName,e.LastName,Isnull(c.SSN,'N/A') as SSN ,
                    sum(P.[BASIC]) as BasicAmount,(sum(p.[Basic]))*@charge as Tier2Amount
                    FROM EMPLOYEES E
                    INNER JOIN EmployeeContributions C on e.Id=c.StaffId
                    INNER join ProcessedSheetCasuals P on p.casualCode=e.code
                    inner join  CostSheets sc on sc.Id=p.CostSheet
                    inner join GangSheetHeaders gsh on gsh.id=sc.RequestHeader
                    group by gsh.DateIssued, p.CasualCode,e.FirstName,e.MiddleName,e.LastName ,Isnull(c.SSN,'N/A')
                 "
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("TierTwoProc");
        }
    }
}