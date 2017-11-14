namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContributionSummary : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ConstributionSummaryProc",
                @"SELECT datepart(year,I.ProccessdOn) as Years,i.Invoice,
                 I.ProccessdOn,DATENAME(month,I.ProccessdOn)+'-'+Convert(varchar(5),datepart(year,I.ProccessdOn)) 
                  as MonthAndYear, (E.FirstName+' '+Isnull(e.MiddleName,'')+' '+e.LastName) as FullName,P.CasualCode,sum(p.[Basic]) as [Basic],
                    sum(p.Welfare) as Welfare ,sum(p.SSF) as SSF,sum(p.UnionDues) AS UnionDues,sum(p.PF) AS PF
                  FROM ProcessedSheetCasuals p
                INNER JOIN ProcessedInvoices I
                ON P.InvoiceCode=I.Invoice
                 inner join Employees E on E.Code=p.CasualCode
                 where p.SheetKind=1
                group by datepart(year,I.ProccessdOn),i.Invoice,
                 I.ProccessdOn,DATENAME(month,I.ProccessdOn)+'-'+Convert(varchar(5),datepart(year,I.ProccessdOn)) 
                 , (E.FirstName+' '+Isnull(e.MiddleName,'')+' '+e.LastName),P.CasualCode
                order by Years asc"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("ConstributionSummaryProc");
        }
    }
}
