using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GetLabourManager.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using GetLabourManager.Helper;

namespace GetLabourManager.InvoiceHub
{
    public class InvoiceHub : Hub
    {
        RBACDbContext db;
        //  SequenceHelper seqeuenceManager;
        public InvoiceHub()
        {
            db = new RBACDbContext();
            // seqeuenceManager = new SequenceHelper(db);
        }

        public void ProcessInvoice(string[] code, int userId)
        {

            Task.Factory.StartNew(() =>
            {
                dynamic flag_job = false;
                dynamic finished = false;
                var transaction = db.Database.BeginTransaction();

                try
                {
                    var Processing_user = ApplicationUserManager.GetUser((userId));
                    DateTime dt = DateTime.Now;

                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    dynamic d_val = DateTime.Now.ToLongTimeString() + "     :" + dt.Date.ToShortDateString();
                    Clients.All.newMessages(d_val);
                    Clients.All.IsWorking(flag_job);
                    Clients.All.IsComplete(finished);
                    var sequence = SequenceHelper.getSequence(db, SequenceHelper.NType.INVOICING);
                    foreach (var cost_sheet_code in code)
                    {
                        var cost_sheet = (from a in db.CostSheet
                                          join b in db.FieldClient on a.Client equals b.Id
                                          where a.CostSheetNumber == cost_sheet_code
                                          select b).FirstOrDefault();
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " PREPARING CLIENT INVOICE FOR: "
                        + cost_sheet.Name + " [" + cost_sheet_code + "] ";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);

                        var records = db.CostSheet.Include("SheetItems").FirstOrDefault(x => x.CostSheetNumber ==
                        cost_sheet_code & x.Status == "GENERATED");
                        var casuals_workers = db.Employee.Select(x => x).ToList();
                        var header = records;
                        var casuals = records.SheetItems;
                        var requisition = db.GangSheetHeader.FirstOrDefault(x => x.Id == header.RequestHeader);

                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " CASUALS: " + casuals.Count();
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " PREPARING PAYMENT SETUP";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);
                        var payment_setup_client = db.PaymentSetup.Where(x => x.Client == header.Client
                        && (x.WorkShift == requisition.WorkShift && x.WorkWeek == requisition.WorkWeek)).Select(x => x).ToList();
                        var FIELD_CLIENT_ = db.FieldClient.FirstOrDefault(x => x.Id == header.Client);

                        var casual_payment_setup = db.CasualPaymentSetup
                       .Where(x => x.WorkShift == requisition.WorkShift && x.WorkWeek == requisition.WorkWeek)
                        .Select(x => x).ToList();

                        var tax_setup = db.TaxSetup.Where(x => x.Id == db.TaxSetup.Max(a => a.Id)).FirstOrDefault();

                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);

                        var field_client = db.FieldClient.FirstOrDefault(x => x.Id == header.Client);
                        d_val = DateTime.Now.ToLongTimeString() + " PROCESSING INVOICE FOR CLIENT: " + field_client.Name;
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);


                        List<ProcessedSheetCasual> casual_list = new List<ProcessedSheetCasual>();

                        //foreach (var item in casual_payment_setup)
                        //{   }

                        //CASUALS TASK
                        foreach (var casual in casuals)
                        {
                            var group_name = db.EmployeeGroup.FirstOrDefault(x => x.GroupName == casual.GroupName);
                            d_val = DateTime.Now.ToLongTimeString() + "   GROUP :               " + group_name.GroupName;
                            Clients.All.newMessages(d_val);
                            d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                            Clients.All.newMessages(d_val);
                            d_val = DateTime.Now.ToLongTimeString() + "   " + casual.FullName + "   " + casual.Gang;
                            Clients.All.newMessages(d_val);
                            //payment_setup_client = db.PaymentSetup.Where(x => x.Client == header.Client
                            //&& (x.WorkShift == requisition.WorkShift && x.WorkWeek == requisition.WorkWeek)).Select(x => x).ToList();
                            var item = casual_payment_setup.FirstOrDefault(x => x.Group == group_name.Id
                              && x.WorkShift == requisition.WorkShift && x.WorkWeek == requisition.WorkWeek);

                            if (casual.GroupName == group_name.GroupName)
                            {
                                if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "WEEKDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                   .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "WEEKDAY"
                                   && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString());
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;

                                    //var tax_on_allowance = decimal.Parse((item.TaxOnAllowance / 100).ToString()) * item.NightAllowance;
                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;
                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;
                                    
                                    var GrossAmt = basic_amt + overtime_charge ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund;//+ tax_on_allowance;
                                    // var premium_rate = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GrossAmt;

                                    // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt - Deductions) + transportation;
                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = 0m,//WILL CONSIDER TAX ON ALLOWANCE LATER(T AND T)
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);
                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "WEEKDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                    .FirstOrDefault(x => x.WorkShift == "NIGHT" && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString()));
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;


                                    var tax_on_allowance = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;



                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;

                                    var GrossAmt = basic_amt + overtime_charge + item.NightAllowance ;// + ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund + tax_on_allowance;
                                //    var premium_rate = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GrossAmt;

                                   // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt - Deductions)+ transportation;
                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = item.NightAllowance,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = tax_on_allowance,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);
                                }
                                else if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "WEEKEND")
                                {
                                    var client_setup_premium = payment_setup_client
                                   .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "WEEKEND"
                                   && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;



                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;
                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;

                                    var tax_on_allowance = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;

                                    var GrossAmt = basic_amt + overtime_charge ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund + tax_on_allowance;
                                    //  var premium_rate = decimal.Parse((client_setup_premium.Premuim / 100).ToString()) * GrossAmt;

                                    // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt) - (Deductions) + transportation;


                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = tax_on_allowance,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);


                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "WEEKEND")
                                {
                                    var client_setup_premium = payment_setup_client
                                  .FirstOrDefault(x => x.WorkShift == "NIGHT" && x.WorkWeek == "WEEKEND"
                                  && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;

                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;
                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;

                                    var tax_on_allowance = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;

                                    var GrossAmt = basic_amt + overtime_charge + item.NightAllowance ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund + tax_on_allowance;
                                    //  var premium_rate = decimal.Parse((client_setup_premium.Premuim / 100).ToString()) * GrossAmt;

                                    // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt - Deductions) + transportation;


                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = item.NightAllowance,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = tax_on_allowance,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);
                                }
                                else if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "HOLIDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                  .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "HOLIDAY"
                                  && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;

                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;
                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;

                                    var tax_on_allowance = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;

                                    var GrossAmt = basic_amt + overtime_charge ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund + tax_on_allowance;
                                    //  var premium_rate = decimal.Parse((client_setup_premium.Premuim / 100).ToString()) * GrossAmt;

                                    // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt - Deductions) + transportation;


                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = tax_on_allowance,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);
                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "HOLIDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "HOLIDAY"
                                && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var transportation = item.TransportationAllowance;
                                    var basic_amt = item.Basic;
                                    var ssf_ = contribution.SSF == true ? decimal.Parse((item.SSF / 100).ToString()) * item.Basic : 0m;

                                    var tax_on_basic = decimal.Parse((item.TaxOnBasic / 100).ToString()) * item.Basic;
                                    var tax_on_overtime = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;
                                    var tax_on_transportation = decimal.Parse((item.TaxOnTransport / 100).ToString()) *
                                    item.TransportationAllowance;
                                    var pf_fund = decimal.Parse((item.PF / 100).ToString()) * item.Basic;
                                    var union_dues = contribution.UnionDues == true ? item.UnionDues : 0m;
                                    var welfare = contribution.Welfare == true ? item.Welfare : 0m;

                                    var tax_on_allowance = decimal.Parse((item.TaxOnOvertime / 100).ToString()) * overtime_charge;

                                    var GrossAmt = basic_amt + overtime_charge + item.NightAllowance ;
                                    var Deductions = ssf_ + tax_on_basic + tax_on_overtime + tax_on_transportation
                                    + union_dues + welfare + pf_fund + tax_on_allowance;
                                    //  var premium_rate = decimal.Parse((client_setup_premium.Premuim / 100).ToString()) * GrossAmt;

                                    // var vat_rate = decimal.Parse((client_setup_premium.VatRate / 100).ToString()) * premium_rate;
                                    var NetAmount = (GrossAmt  - Deductions)+ transportation;
                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = item.NightAllowance,
                                        Overtime = overtime_charge,
                                        Premium = 0,
                                        TPointer = sequence,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = tax_on_allowance,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = true,
                                        SSF = ssf_,
                                        PF = pf_fund,
                                        TaxOnBasic = tax_on_basic,
                                        TaxOnOverTime = tax_on_overtime,
                                        TaxOnTandT = tax_on_transportation,
                                        UnionDues = union_dues,
                                        Welfare = welfare,
                                        GrossAmount = GrossAmt,
                                        NetAmount = NetAmount
                                    };
                                    casual_list.Add(model);
                                }

                            }
                        }
                        d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                        Clients.All.newMessages(d_val);

                        if (casual_list.Count > 0)
                        {
                            db.ProcessedSheetCasual.AddRange(casual_list);
                            db.SaveChanges();
                            // SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.INVOICING);
                        }

                        List<ProcessedSheetCasual> client_list = new List<ProcessedSheetCasual>();
                        //foreach (var item in payment_setup_client)
                        //{ }

                        foreach (var casual in casuals)
                        {
                            var group_name = db.EmployeeGroup.FirstOrDefault(x => x.GroupName == casual.GroupName);
                            d_val = DateTime.Now.ToLongTimeString() + "   GROUP :               " + group_name.GroupName;
                            Clients.All.newMessages(d_val);
                            d_val = DateTime.Now.ToLongTimeString() + " ======================================";
                            Clients.All.newMessages(d_val);
                            d_val = DateTime.Now.ToLongTimeString() + "   " + casual.FullName + "   " + casual.Gang;
                            Clients.All.newMessages(d_val);
                            var item = payment_setup_client.FirstOrDefault(x => x.Group == group_name.Id
                          && x.WorkShift == requisition.WorkShift && x.WorkWeek == requisition.WorkWeek);

                            if (casual.GroupName == group_name.GroupName)
                            {
                                if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "WEEKDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                          .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "WEEKDAY"
                                          && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString());
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * item.Basic;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance;
                                    
                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "WEEKDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                         .FirstOrDefault(x => x.WorkShift == "NIGHT" && x.WorkWeek == "WEEKDAY"
                                         && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString());
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * item.Basic;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance + item.NightAllowance;

                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                                else if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "WEEKEND")
                                {
                                    var client_setup_premium = payment_setup_client
                                        .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "WEEKEND"
                                        && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString());
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance;


                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "WEEKEND")
                                {
                                    var client_setup_premium = payment_setup_client
                                       .FirstOrDefault(x => x.WorkShift == "NIGHT" && x.WorkWeek == "WEEKEND"
                                       && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = item.Overtime * decimal.Parse(casual.OvertimeHrs.ToString());
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance + item.NightAllowance;

                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = item.NightAllowance,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                                else if (requisition.WorkShift == "DAY" && requisition.WorkWeek == "HOLIDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                       .FirstOrDefault(x => x.WorkShift == "DAY" && x.WorkWeek == "HOLIDAY"
                                       && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime *
                                    decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance;

                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = 0m,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                                else if (requisition.WorkShift == "NIGHT" && requisition.WorkWeek == "HOLIDAY")
                                {
                                    var client_setup_premium = payment_setup_client
                                       .FirstOrDefault(x => x.WorkShift == "NIGHT" && x.WorkWeek == "HOLIDAY"
                                       && x.Group == group_name.Id);

                                    var staff = casuals_workers.FirstOrDefault(x => x.Code == casual.StaffCode);
                                    var contribution = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == staff.Id);

                                    var overtime_charge = (item.Overtime *
                                    decimal.Parse(casual.OvertimeHrs.ToString())) - item.NightAllowance;
                                    var GROSS = item.Basic + overtime_charge;

                                    decimal premium_amt = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;
                                    decimal tax_charge = item.VatRate / 100 * premium_amt;
                                    decimal premium_charge = decimal.Parse((FIELD_CLIENT_.Premium / 100).ToString()) * GROSS;

                                    var NET_AMOUNT = GROSS + premium_charge + item.TransportationAllowance + item.NightAllowance;

                                    ProcessedSheetCasual model = new ProcessedSheetCasual()
                                    {
                                        TaxOnBasic = tax_charge,
                                        Transportation = item.TransportationAllowance,
                                        TaxOnAllowance = decimal.Parse(premium_amt.ToString()),
                                        Premium = double.Parse(premium_charge.ToString()),
                                        NetAmount = NET_AMOUNT,
                                        GrossAmount = GROSS,
                                        Basic = item.Basic,
                                        CasualCode = casual.StaffCode,
                                        CostSheet = casual.CostSheetId,
                                        GangType = casual.Gang,
                                        Group = group_name.GroupName,
                                        InvoiceCode = sequence,
                                        NightAllowance = item.NightAllowance,
                                        Overtime = overtime_charge,
                                        TPointer = sequence,
                                        PreparedBy = Processing_user.Id,
                                        SheetKind = false,
                                        SSF = 0,
                                        PF = 0m,
                                        TaxOnOverTime = 0,
                                        TaxOnTandT = 0,
                                        UnionDues = 0,
                                        Welfare = 0
                                    };
                                    client_list.Add(model);
                                }
                            }
                        }
                        
                        if (client_list.Count > 0)
                        {
                            db.ProcessedSheetCasual.AddRange(client_list);
                            db.SaveChanges();

                        }
                        var entity = db.CostSheet.FirstOrDefault(x => x.CostSheetNumber == cost_sheet_code);
                        entity.Status = "PROCESSED";
                        db.Entry<CostSheet>(entity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        flag_job = true;
                        Clients.All.IsWorking(flag_job);
                        watch.Stop();
                        string duration = watch.Elapsed.ToString(@"hh\:mm\:ss");
                        finished = true;
                        Clients.All.IsComplete(finished);
                        Clients.All.newMessages("ELAPSED TIME: " + duration);
                    }
                    var RESULT = code[0];
                    var entity_RESULT = db.CostSheet.FirstOrDefault(x => x.CostSheetNumber == RESULT);

                    ProcessedInvoice invoice = new ProcessedInvoice()
                    {
                        Client = entity_RESULT.Client,
                        Invoice = sequence,
                        ProccessdOn = DateTime.Now.Date,
                        ProcessedBy = Processing_user.UserName,
                        Status = "PROCESSED"
                    };
                    db.ProcessedInvoice.Add(invoice);db.SaveChanges();

                    SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.INVOICING);
                    transaction.Commit();
                }
                catch (Exception err)
                {
                    string d_val = DateTime.Now.ToLongTimeString() + "    AN ERROR OCCURED";
                    Clients.All.newMessages(d_val);
                     d_val = DateTime.Now.ToLongTimeString() + " =========================== ";
                    Clients.All.newMessages(d_val);
                    d_val = DateTime.Now.ToLongTimeString() + "    ROLLING BACK TRANSACTION";
                    Clients.All.newMessages(d_val);
                    transaction.Rollback();
                    d_val = DateTime.Now.ToLongTimeString() + " =========================== ";
                    Clients.All.newMessages(d_val);

                    Clients.All.IsWorking(false);
                    Clients.All.IsComplete(true);
                    Clients.All.newMessages("    ERROR: " + err.Message.ToString());
                }
            });
        }
    }
}