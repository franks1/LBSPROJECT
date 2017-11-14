namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayeDeduction : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("PayeDedcutionProc",
                @"
                   declare @withholdingTax as float
                    set @withholdingTax=0.05
                    select
                            DATENAME(MONTH,gsh.DateIssued)+'-'+convert(varchar(5),datepart(year,gsh.DateIssued)) as MonthYear,
                     gsh.DateIssued, P.CasualCode,(E.FirstName+' '+ISNULL(E.MiddleName,'')+' '+e.LastName) as FullName
                    ,sum(p.[Basic]) as TotalBasic,sum(p.Overtime) as TotalOverTime,sum(p.Transportation) as TotalTransportation,sum(p.SSF) as SSF,
                    sum(p.[Basic]+p.Overtime+p.Transportation) as TaxableAmount,(sum(p.[Basic]+p.Overtime+p.Transportation) * @withholdingTax) as TaxAmount
                     from ProcessedSheetCasuals P
                    INNER JOIN Employees E ON P.CasualCode=E.Code
                    inner join CostSheets C on c.Id=p.CostSheet
                    inner join GangSheetHeaders gsh on gsh.Id=c.RequestHeader
                    where p.SheetKind=1
                    group by  DATENAME(MONTH,gsh.DateIssued)+'-'+convert(varchar(5),datepart(year,gsh.DateIssued)), gsh.DateIssued, p.CasualCode,(E.FirstName+' '+ISNULL(E.MiddleName,'')+' '+e.LastName)
                    ");

        }
        
        public override void Down()
        {
            DropStoredProcedure("PayeDedcutionProc");
        }
    }
}
