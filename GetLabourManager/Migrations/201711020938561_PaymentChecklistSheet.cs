namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentChecklistSheet : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("PaymentCheckSheetProc",
                        @"
                           select g_sh.DateIssued,f_c.Name, g_sh.RequestCode,e_g.Category as GangType,g.[Description] as Gang,count(p_sc.casualCode) as Casuals,
                           sum(  (p_sc.[Basic]+p_sc.Overtime+p_sc.NightAllowance+p_sc.Transportation)
                           -(p_sc.Welfare+p_sc.PF+p_sc.UnionDues+p_sc.TaxOnBasic+p_sc.SSF+p_sc.TaxOnOverTime+p_sc.TaxOnTandT)) as NetAmount
                            from ProcessedSheetCasuals p_sc  
                            inner join CostSheets c_s on p_sc.CostSheet = c_s.Id
                            inner join GangSheetHeaders g_sh on c_s.RequestHeader=g_sh.Id
                            inner join Employees e on p_sc.CasualCode=e.Code
                            inner join FieldClients f_c on c_s.Client=f_c.Id
                            inner join Gangs g on g_sh.GangCode=g.Code
                            inner join EmployeeCategories e_g on g_sh.GangType=e_g.Id
    	                                    inner join USERS u on c_s.PreparedBy = u.UserId
                            where (p_sc.SheetKind=1)
                            group by  g_sh.DateIssued,f_c.Name, g_sh.RequestCode,e_g.Category,  g.[Description]
	                        order by g_sh.DateIssued asc");
        }
        
        public override void Down()
        {
            DropStoredProcedure("PaymentCheckSheetProc");
        }
    }
}
