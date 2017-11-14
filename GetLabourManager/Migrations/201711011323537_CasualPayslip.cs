namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasualPayslip : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("CasualPaySlipProc", @"
                    Declare @vat_rate float
                    set @vat_rate=0.175;
                    select u.UserName as PreparedBy, g_sh.DateIssued,f_c.Name, g_sh.RequestCode,g_sh.WorkShift,g_sh.WorkWeek,e_g.Category as GangType,
                    g.[Description] as Gang, p_sc.Id, p_sc.InvoiceCode,  p_sc.CasualCode,
                    (e.FirstName+' '+e.MiddleName+' '+e.LastName) as FullName,p_sc.[Group],p_sc.[Basic],
                    p_sc.Overtime,p_sc.NightAllowance,p_sc.Transportation
                    ,(p_sc.[Basic]+
                    p_sc.Overtime+p_sc.NightAllowance+p_sc.Transportation) as Gross,
    	                            p_sc.Welfare,p_sc.PF,p_sc.UnionDues,p_sc.TaxOnBasic,p_sc.SSF, p_sc.TaxOnOverTime,p_sc.TaxOnTandT,
    	                            (p_sc.Welfare+p_sc.PF+p_sc.UnionDues+p_sc.SSF+p_sc.TaxOnBasic+p_sc.TaxOnOverTime+p_sc.TaxOnTandT) AS Deduction,
    	                            (p_sc.[Basic]+
                    p_sc.Overtime+p_sc.NightAllowance+p_sc.Transportation)-(p_sc.Welfare+p_sc.PF+p_sc.UnionDues+p_sc.TaxOnBasic+p_sc.SSF+p_sc.TaxOnOverTime+p_sc.TaxOnTandT) as NetAmount,
    	                            c_s.HoursWorked,c_s.OvertimeHours 
    
                    from ProcessedSheetCasuals p_sc  
                    inner join CostSheets c_s on p_sc.CostSheet = c_s.Id
                    inner join GangSheetHeaders g_sh on c_s.RequestHeader=g_sh.Id
                    inner join Employees e on p_sc.CasualCode=e.Code
                    inner join FieldClients f_c on c_s.Client=f_c.Id
                    inner join Gangs g on g_sh.GangCode=g.Code
                    inner join EmployeeCategories e_g on g_sh.GangType=e_g.Id
    	                            inner join USERS u on c_s.PreparedBy = u.UserId
                    where (p_sc.SheetKind=1)
                    order by p_sc.Id asc"
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("CasualPaySlipProc");
        }
    }
}