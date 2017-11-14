namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayrollJournal : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("PayRollJournalProc",
                @"
                select  gsh.DateIssued,P.CasualCode, DateName(month,gsh.DateIssued)+'-'+Convert(varchar(5),Datepart(year,gsh.DateIssued)) as MonthAndYear,
                (E.FirstName+' '+Isnull(E.MiddleName,'')+' '+e.LastName) as FullName,  Count(P.CasualCode) as JobCount , sum(P.Basic) as TotalBasic
                ,Sum(p.Overtime) as TotalOverTime,sum(p.NightAllowance) as TotalNightAllowance,sum(p.Transportation)
                 as TotalTransport,Sum(p.ssf) as TotalSSF,Sum(p.pf) as TotalPf
                 from ProcessedSheetCasuals P
                inner join Employees E on E.Code=P.CasualCode
                inner join CostSheets c on P.CostSheet=c.Id
                inner join GangSheetHeaders gsh on c.RequestHeader=gsh.Id
                where p.SheetKind=1
                group by gsh.DateIssued,P.CasualCode,DateName(month,gsh.DateIssued)+'-'+Convert(varchar(5),Datepart(year,gsh.DateIssued)),
                (E.FirstName+' '+Isnull(E.MiddleName,'')+' '+e.LastName)"
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("PayRollJournalProc");
        }
    }
}
