namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasualDetailContribution : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CasualContributionProc",
                @"select I.ProccessdOn,  (E.FirstName+' '+Isnull(e.MiddleName,'')+' '+e.LastName) as FullName,P.CasualCode,p.[Basic],
                p.Welfare,p.SSF,p.UnionDues,p.PF   from ProcessedSheetCasuals p
                inner join ProcessedInvoices I on p.InvoiceCode=I.Invoice
                inner join Employees E on E.Code=p.CasualCode
                where sheetkind=1"
                    );
        }
        
        public override void Down()
        {
            DropStoredProcedure("CasualContributionProc");
        }
    }
}
