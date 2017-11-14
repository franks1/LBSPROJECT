namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInvoiceSummaryProc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("ClientInvoiceSummaryProc",
                    @"
                    Declare @vat_rate float
                    set @vat_rate=0.175;
                    SELECT t1.ProccessdOn, t1.PM,t1.VATR, t1.Name, t1.InvoiceCode,t1.RequestCode,t1.DateIssued,t1.GangType,t1.[Group], count(t1.Id)  as Casuals,sum(t1.GrossAmount) as Gross,
                    sum(t1.Transportation) as Transport,sum(t1.NightAllowance) as NightAllowance,sum(t1.GrossAmount+t1.Transportation+t1.NightAllowance) as NetAmount
                    FROM (
                    select f_c.Premium as PM, @vat_rate as VATR, p_i.ProccessdOn, u.UserName as PreparedBy, g_sh.DateIssued,f_c.Name, g_sh.RequestCode,g_sh.WorkShift,g_sh.WorkWeek,e_g.Category as GangType,
                    g.[Description] as Gang, p_sc.Id, p_sc.InvoiceCode,  p_sc.CasualCode,
                    (e.FirstName+' '+e.MiddleName+' '+e.LastName) as FullName,p_sc.[Group],p_sc.[Basic],
                    p_sc.Overtime,p_sc.GrossAmount,((f_c.Premium/100)*p_sc.[Basic]) as Premium,p_sc.NightAllowance
                    ,(((f_c.Premium/100)*p_sc.[Basic])*@vat_rate) as VAT,p_sc.Transportation,
                    (p_sc.[Basic]+p_sc.Overtime + ((f_c.Premium/100)*p_sc.[Basic]) + p_sc.NightAllowance
                    + p_sc.Transportation) as Net,c_s.HoursWorked,c_s.OvertimeHours 
                    from ProcessedSheetCasuals p_sc  
                    inner join CostSheets c_s on p_sc.CostSheet = c_s.Id
                    inner join GangSheetHeaders g_sh on c_s.RequestHeader=g_sh.Id
                    inner join Employees e on p_sc.CasualCode=e.Code
                    inner join FieldClients f_c on c_s.Client=f_c.Id
                    inner join Gangs g on g_sh.GangCode=g.Code
                    inner join EmployeeCategories e_g on g_sh.GangType=e_g.Id
	                inner join USERS u on c_s.PreparedBy = u.UserId
	                inner join ProcessedInvoices p_i on p_sc.InvoiceCode=p_i.Invoice
                    where p_sc.SheetKind=0 )t1
	                group by t1.ProccessdOn, t1.PM,t1.VATR, t1.Name, t1.InvoiceCode, 
                    t1.RequestCode,t1.DateIssued,t1.GangType,t1.Gang,t1.[Group]"
                    );

        }
        
        public override void Down()
        {
            DropStoredProcedure("ClientInvoiceSummaryProc");
        }
    }
}
