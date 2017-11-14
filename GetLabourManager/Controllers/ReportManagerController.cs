using DevExpress.XtraReports.UI;
using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
using GetLabourManager.Models;
using GetLabourManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPARA = DevExpress.XtraReports.Parameters;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class ReportManagerController : Controller
    {
        // GET: ReportManager
        RBACDbContext db;
        EmployeeHelper employee_;
        public ReportManagerController()
        {
            this.db = new RBACDbContext();
            this.employee_ = new EmployeeHelper(db);
        }
        public ActionResult Index()
        {
            ViewBag.VROption = getViewReportOptions();
            return View();
        }

        //getReportOptions
        public JsonResult getReportOptions()
        {
            var report_options = new string[] { "Casual Report".ToUpper(), "Processed Cost Sheet Report".ToUpper(),
                "Client Invoice Summary Report".ToUpper(),"CASUAL CONTRIBUTION REPORT","CASUAL PAYSLIP REPORT",
                "CONTRIBUTION SUMMARY REPORT","PAYROLL JOURNAL REPORT",
                "PAYE DEDUCTION REPORT","PAYMENT CHECKSHEET REPORT","TIER 1 REPORT","TIER 2 REPORT" };
            return Json(new { data = report_options }, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> getClients()
        {
            List<SelectListItem> clients = new List<SelectListItem>() { };
            var field_clients = db.FieldClient.Select(x => x).ToList();
            foreach (var item in field_clients)
            {
                clients.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return clients;
        }

        public List<SelectListItem> getGangType()
        {
            List<SelectListItem> clients = new List<SelectListItem>() { };
            var field_clients = db.EmpCategory.Select(x => x).ToList();
            foreach (var item in field_clients)
            {
                clients.Add(new SelectListItem() { Text = item.Category, Value = item.Category.ToString() });
            }

            return clients;
        }


        public List<SelectListItem> getViewReportOptions()
        {
            List<SelectListItem> report_options = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value= "CASUAL REPORT", Text="CASUAL REPORT"
                },
                new SelectListItem()
                {
                    Value= "PROCESSED COST SHEET REPORT", Text="PROCESSED COST SHEET REPORT"
                },
                new SelectListItem()
                {
                    Value= "CLIENT INVOICE SUMMARY REPORT", Text="CLIENT INVOICE SUMMARY REPORT"
                },
                new SelectListItem()
                {
                    Value= "CASUAL CONTRIBUTION REPORT", Text="CASUAL CONTRIBUTION REPORT"
                },
                new SelectListItem()
                {
                    Value= "CONTRIBUTION SUMMARY REPORT", Text="CONTRIBUTION SUMMARY REPORT"
                },
                new SelectListItem()
                {
                    Value= "PAYROLL JOURNAL REPORT", Text="PAYROLL JOURNAL REPORT"
                }
                , new SelectListItem()
                {
                    Value= "CASUAL PAYSLIP REPORT", Text="CASUAL PAYSLIP REPORT"
                },
                 new SelectListItem()
                {
                    Value= "PAYMENT CHECKSHEET REPORT", Text="PAYMENT CHECKSHEET REPORT"
                },
                  new SelectListItem()
                {
                    Value= "TIER 1 REPORT", Text="TIER 1 REPORT"
                },
                   new SelectListItem()
                {
                    Value= "TIER 2 REPORT", Text="TIER 2 REPORT"
                }
            };


            return report_options;
        }



        public List<SelectListItem> getCategories()
        {
            List<SelectListItem> clients = new List<SelectListItem>() { };
            var category = db.EmpCategory.Select(x => x).ToList();
            foreach (var item in category)
            {
                clients.Add(new SelectListItem() { Text = item.Category, Value = item.Category });
            }

            return clients;
        }
        public JsonResult getCasuals()
        {
            var employee = employee_.getEmployeeList();
            return Json(new { data = employee }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ReportOption(string option)
        {
            switch (option.ToUpper())
            {
                case "CASUAL REPORT":
                    ViewBag.VCategories = getCategories();
                    return PartialView("_CasualListFilter");
                case "PROCESSED COST SHEET REPORT":
                    ViewBag.VClients = getClients();
                    return PartialView("_ProcessedCostSheetFilter");
                case "CLIENT INVOICE SUMMARY REPORT":
                    ViewBag.VClients = getClients();
                    return PartialView("_ClientInvoiceFilter");
                case "CASUAL CONTRIBUTION REPORT":
                    return PartialView("_CasualContributionFilter");
                case "CONTRIBUTION SUMMARY REPORT":
                    return PartialView("_CasualContributionFilter");
                case "PAYROLL JOURNAL REPORT":
                    return PartialView("_PayrollJournalFilter");
                case "CASUAL PAYSLIP REPORT":
                    ViewBag.VClients = getClients();
                    return PartialView("_CasualPaySlipFilter");
                case "PAYE DEDUCTION REPORT":
                    return PartialView("_PayeDeductionFilter");
                case "PAYMENT CHECKSHEET REPORT":
                    ViewBag.VGangType = getGangType();
                    return PartialView("_PaymentCheckSheet");
                case "TIER 1 REPORT":
                    return PartialView("_TiersFilter");
                case "TIER 2 REPORT":
                    return PartialView("_TiersFilter");
                default:
                    ViewBag.VClients = getClients();
                    return PartialView("_ClientInvoiceFilter");
            }
        }

        public ActionResult ReportPreview(ReportFilters model)
        {
            if (model.ReportOption == "CASUAL REPORT")
            {
                rptCasualReportA report = new rptCasualReportA();

                var catelist_list = "";
                model.Invoice = model.Invoice.TrimEnd(',');
                model.Categories = model.Invoice.Split(',');
                List<XPARA.Parameter> param_list = new List<XPARA.Parameter>();

                int index = 0;
                foreach (var item in model.Categories)
                {
                    XPARA.Parameter param = new XPARA.Parameter()
                    {
                        Name = "param" + index,
                        Type = typeof(string),
                        Visible = false,
                        Value = item
                    };
                    //  catelist_list += item + ",";
                    catelist_list += string.Format("?{0},", param.Name);
                    param_list.Add(param);
                    index++;
                }
                catelist_list = catelist_list.TrimEnd(',');

                if (!string.IsNullOrEmpty(catelist_list))
                {

                    report.Parameters.Clear();
                    report.Parameters.AddRange(param_list.ToArray());
                    report.FilterString = "[Category] in (" + catelist_list + ")";
                }

                return View(report);
            }
            else if (model.ReportOption == "PROCESSED COST SHEET REPORT")
            {
                rptProcessedCostSheet report_cost_sheet = new rptProcessedCostSheet();

                XPARA.Parameter param1 = new XPARA.Parameter();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();
                if (!string.IsNullOrEmpty(model.ClientName) && model.IsDateRangeActive == false)
                {
                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    report_cost_sheet.Parameters.Add(param1);
                    report_cost_sheet.FilterString = "[Name]=?clientName";
                }

                if (model.IsDateRangeActive == true && !string.IsNullOrEmpty(model.ClientName))
                {
                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report_cost_sheet.Parameters.Add(param1);
                    report_cost_sheet.Parameters.Add(param2);
                    report_cost_sheet.Parameters.Add(param3);
                    report_cost_sheet.FilterString
                        = "[Name]=?clientName And [DateIssued] Between(?fromDate,?toDate)";
                    report_cost_sheet.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }
                return View(report_cost_sheet);
            }
            else if (model.ReportOption == "CLIENT INVOICE SUMMARY REPORT")
            {
                rptProcessedClientInvoice report_client_invoice = new rptProcessedClientInvoice();

                XPARA.Parameter param1 = new XPARA.Parameter();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();

                if (!string.IsNullOrEmpty(model.ClientName) && string.IsNullOrEmpty(model.Invoice))
                {
                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;


                    report_client_invoice.Parameters.Add(param1);


                    report_client_invoice.FilterString = "[Name]=?clientName";
                }

                if (!string.IsNullOrEmpty(model.ClientName) && !string.IsNullOrEmpty(model.Invoice))
                {
                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    param2.Type = typeof(string);
                    param2.Name = "invoiceClient";
                    param2.Value = model.Invoice;
                    param2.Visible = false;

                    report_client_invoice.Parameters.Add(param1);
                    report_client_invoice.Parameters.Add(param2);

                    report_client_invoice.FilterString = "[Name]=?clientName and [InvoiceCode]=?invoiceClient";
                }

                if (model.IsDateRangeActive == true && !string.IsNullOrEmpty(model.ClientName))
                {
                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report_client_invoice.Parameters.Add(param1);
                    report_client_invoice.Parameters.Add(param2);
                    report_client_invoice.Parameters.Add(param3);
                    report_client_invoice.FilterString
                        = "[Name]=?clientName And [ProccessdOn] Between(?fromDate,?toDate)";
                    report_client_invoice.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }

                return View(report_client_invoice);
            }
            else if (model.ReportOption == "CASUAL CONTRIBUTION REPORT")
            {
                rptCasualConstribution report = new rptCasualConstribution();
                if (model.IsCasual == true && model.IsDateRangeActive == false)
                {
                    XPARA.Parameter param1 = new XPARA.Parameter()
                    {
                        Name = "paramCode",
                        Type = typeof(string),
                        Value = model.Casual,
                        Visible = false
                    };
                    report.Parameters.Add(param1);
                    report.FilterString = "[CasualCode]=?paramCode";
                }
                else if (model.IsCasual == true && model.IsDateRangeActive == true)
                {
                    XPARA.Parameter param1 = new XPARA.Parameter();
                    XPARA.Parameter param2 = new XPARA.Parameter();
                    XPARA.Parameter param3 = new XPARA.Parameter();

                    param1.Type = typeof(string);
                    param1.Name = "paramCode";
                    param1.Value = model.Casual;
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param1);
                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[CasualCode]=?paramCode And [ProccessdOn] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);

                }
                else if (model.IsCasual == false && model.IsDateRangeActive == true)
                {
                    XPARA.Parameter param2 = new XPARA.Parameter();
                    XPARA.Parameter param3 = new XPARA.Parameter();

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString = "[ProccessdOn] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                    model.FromDate, model.ToDate);

                }
                return View(report);
            }
            else if (model.ReportOption == "CONTRIBUTION SUMMARY REPORT")
            {
                rptContributionSummary report = new rptContributionSummary();
                if (model.IsCasual == true && model.IsDateRangeActive == false)
                {
                    XPARA.Parameter param1 = new XPARA.Parameter()
                    {
                        Name = "paramCode",
                        Type = typeof(string),
                        Value = model.Casual,
                        Visible = false
                    };
                    report.Parameters.Add(param1);
                    report.FilterString = "[CasualCode]=?paramCode";
                }
                else if (model.IsCasual == true && model.IsDateRangeActive == true)
                {
                    XPARA.Parameter param1 = new XPARA.Parameter();
                    XPARA.Parameter param2 = new XPARA.Parameter();
                    XPARA.Parameter param3 = new XPARA.Parameter();

                    param1.Type = typeof(string);
                    param1.Name = "paramCode";
                    param1.Value = model.Casual;
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param1);
                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[CasualCode]=?paramCode And [ProccessdOn] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);

                }
                else if (model.IsCasual == false && model.IsDateRangeActive == true)
                {
                    XPARA.Parameter param2 = new XPARA.Parameter();
                    XPARA.Parameter param3 = new XPARA.Parameter();

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString = "[ProccessdOn] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                    model.FromDate, model.ToDate);

                }
                return View(report);
            }
            else if (model.ReportOption == "PAYROLL JOURNAL REPORT")
            {
                rptPayRollJournal report = new rptPayRollJournal();
                if (model.IsDateRangeActive == true)
                {
                    XPARA.Parameter param2 = new XPARA.Parameter();
                    XPARA.Parameter param3 = new XPARA.Parameter();

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString = "[DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                    model.FromDate, model.ToDate);
                }
                return View(report);

            }
            else if (model.ReportOption == "CASUAL PAYSLIP REPORT")
            {
                rptCasualPayslip report = new rptCasualPayslip();
                XPARA.Parameter param1 = new XPARA.Parameter();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();

                if (!string.IsNullOrEmpty(model.ClientName) && !string.IsNullOrEmpty(model.Invoice) && model.IsDateRangeActive == false)
                {

                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    param2.Type = typeof(string);
                    param2.Name = "invoiceClient";
                    param2.Value = model.Invoice;
                    param2.Visible = false;

                    report.Parameters.Add(param1);
                    report.Parameters.Add(param2);

                    report.FilterString = "[Name]=?clientName and [InvoiceCode]=?invoiceClient";

                }
                else if (!string.IsNullOrEmpty(model.ClientName) && string.IsNullOrEmpty(model.Invoice) && model.IsDateRangeActive == false)
                {

                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;
                    report.Parameters.Add(param1);
                    report.FilterString = "[Name]=?clientName";

                }
                else if (!string.IsNullOrEmpty(model.ClientName) && string.IsNullOrEmpty(model.Invoice)
                    && model.IsDateRangeActive == true)
                {

                    param1.Type = typeof(string);
                    param1.Name = "clientName";
                    param1.Value = model.ClientName;
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Add(param1);
                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[Name]=?clientName And [DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);

                }


                return View(report);
            }
            else if (model.ReportOption == "PAYE DEDUCTION REPORT")
            {
                rptPayeDeduction report = new rptPayeDeduction();
                XPARA.Parameter param1 = new XPARA.Parameter();
                XPARA.Parameter param2 = new XPARA.Parameter();
                if (model.IsDateRangeActive == true)
                {
                    param1.Type = typeof(DateTime);
                    param1.Name = "fromDate";
                    param1.Value = DateTime.Parse(model.FromDate);
                    param1.Visible = false;

                    param2.Type = typeof(DateTime);
                    param2.Name = "toDate";
                    param2.Value = DateTime.Parse(model.ToDate);
                    param2.Visible = false;

                    report.Parameters.Add(param1);
                    report.Parameters.Add(param2);

                    report.FilterString
                        = "[DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }
                return View(report);
            }
            else if (model.ReportOption == "PAYMENT CHECKSHEET REPORT")
            {
                rptPaymentCheckSheet report = new rptPaymentCheckSheet();
                XPARA.Parameter param1 = new XPARA.Parameter();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();
                //
                var gang_list = "";
                model.GangType = model.GangType.TrimEnd(',');
                var gangs = model.GangType.Split(',');
                List<XPARA.Parameter> param_list = new List<XPARA.Parameter>();

                int index = 0;
                foreach (var item in gangs)
                {
                    XPARA.Parameter param = new XPARA.Parameter()
                    {
                        Name = "param" + index,
                        Type = typeof(string),
                        Visible = false,
                        Value = item
                    };
                    //  catelist_list += item + ",";
                    gang_list += string.Format("?{0},", param.Name);
                    param_list.Add(param);
                    index++;
                }
                gang_list = gang_list.TrimEnd(',');

                if (model.IsDateRangeActive == false && !string.IsNullOrEmpty(model.GangType))
                {
                    report.Parameters.AddRange(param_list.ToArray());
                    report.FilterString = "[GangType] IN (" + gang_list + ")";
                }
                else if (model.IsDateRangeActive == true && string.IsNullOrEmpty(model.GangType))
                {

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;

                    report.Parameters.Clear();
                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);

                }
                else if (model.IsDateRangeActive == true && !string.IsNullOrEmpty(model.GangType))
                {

                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;


                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.Parameters.AddRange(param_list.ToArray());
                    report.FilterString
                        = "[GangType] IN (" + gang_list + ") And [DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }
                return View(report);
            }
            else if(model.ReportOption== "TIER 1 REPORT")
            {
                rptTier1 report = new rptTier1();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();
                if (model.IsDateRangeActive == true)
                {
                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;


                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }
                return View(report);
            }
            else if (model.ReportOption == "TIER 2 REPORT")
            {
                rptTier2 report = new rptTier2();
                XPARA.Parameter param2 = new XPARA.Parameter();
                XPARA.Parameter param3 = new XPARA.Parameter();
                if (model.IsDateRangeActive == true)
                {
                    param2.Type = typeof(DateTime);
                    param2.Name = "fromDate";
                    param2.Value = DateTime.Parse(model.FromDate);
                    param2.Visible = false;

                    param3.Type = typeof(DateTime);
                    param3.Name = "toDate";
                    param3.Value = DateTime.Parse(model.ToDate);
                    param3.Visible = false;


                    report.Parameters.Add(param2);
                    report.Parameters.Add(param3);
                    report.FilterString
                        = "[DateIssued] Between(?fromDate,?toDate)";
                    report.txtDateRange.Text = string.Format("FROM: {0} TO {1}",
                        model.FromDate, model.ToDate);
                }
                return View(report);
            }
            else
                return RedirectToAction("Index");
        }
    }
}
