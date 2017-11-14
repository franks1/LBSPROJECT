using GetLabourManager.ActionFilters;
using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class PaymentSetupController : Controller
    {

        RBACDbContext db;
        // GET: PaymentSetup
        public PaymentSetupController()
        {
            db = new RBACDbContext();
        }
        #region CLIENT
        public ActionResult Index()
        {
            ViewBag.VFields = getFieldClient();
            ViewBag.VGroup = getGroups();
            ViewBag.VShift = getShift();
            ViewBag.VWeekScheme = getWorkingSchedule();
            return View();
        }
        public List<SelectListItem> getShift()
        {
            List<SelectListItem> item_group = new List<SelectListItem>();
            item_group.Add(new SelectListItem() { Text = "DAY", Value = "DAY" });
            item_group.Add(new SelectListItem() { Text = "NIGHT", Value = "NIGHT" });
            return item_group;
        }
        public List<SelectListItem> getWorkingSchedule()
        {
            List<SelectListItem> item_group = new List<SelectListItem>();
            item_group.Add(new SelectListItem() { Text = "WEEKDAY", Value = "WEEKDAY" });
            item_group.Add(new SelectListItem() { Text = "WEEKEND", Value = "WEEKEND" });
            return item_group;
        }
        public JsonResult getData()
        {
            var records = (from a in db.PaymentSetup.AsEnumerable()
                           join b in db.FieldClient.AsEnumerable() on a.Client equals b.Id
                           join c in db.EmployeeGroup.AsEnumerable() on a.Group equals c.Id
                           select new
                           {
                               Id = a.Id,
                               Name = b.Name,
                               Shift=a.WorkShift,
                               Week=a.WorkWeek,
                               GroupName = c.GroupName,
                               Basic = string.Format("{0:N2}", a.Basic),
                               NightAllowance = string.Format("{0:N2}", a.NightAllowance),
                               TransportationAllowance = string.Format("{0:N2}", a.TransportationAllowance),
                               OverTime = string.Format("{0:N2}", a.Overtime),
                               Tax = string.Format("{0:N2}", a.VatRate),
                           }).OrderBy(x=>x.Name).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        public List<SelectListItem> getFieldClient()
        {
            List<SelectListItem> items_ = new List<SelectListItem>();
            var records = db.FieldClient.Select(x => x).ToList();
            foreach (var item in records)
            {
                items_.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return items_;
        }

        public JsonResult SaveClientPaymentSetup(PaymentSetup model)
        {

            if (string.IsNullOrEmpty(model.Client.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY CLIENT" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Group.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY GROUP" }, JsonRequestBehavior.AllowGet);
            }

            if (model.Basic <= 0m)
            {
                return Json(new { message = "PLEASE SPECIFY BASIC AMOUNT" }, JsonRequestBehavior.AllowGet);
            }


            if (model.VatRate < 0m)
            {
                return Json(new { message = "PLEASE SPECIFY VAT RATE" }, JsonRequestBehavior.AllowGet);
            }


            try
            {
                var records = db.PaymentSetup.Select(x => x).ToList();
                if (records.Exists(x => x.Client == model.Client && x.Group == model.Group 
                && x.WorkShift== model.WorkShift && x.WorkWeek==model.WorkWeek))
                {
                    return Json(new { message = "GROUP SPECIFIED FOR CLIENT HAS ALREADY BEEN ASSIGNED TO THIS CLIENT" }, JsonRequestBehavior.AllowGet);
                }
                db.PaymentSetup.Add(model); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteClientPaySetup(int Id)
        {
            var entity = db.PaymentSetup.FirstOrDefault(x => x.Id == Id);
            if (entity != null)
            {
                db.PaymentSetup.Remove(entity); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "SETUP NOT FOUND" }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GetClientEdit(int Id)
        {
            ViewBag.VFields = getFieldClient();
            ViewBag.VGroup = getGroups();
            ViewBag.VShift = getShift();
            ViewBag.VWeekScheme = getWorkingSchedule();
            var entity = db.PaymentSetup.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditClientPaymentSetup", entity);
        }

        //
        public JsonResult SaveEditClientPaymentSetup(PaymentSetup model)
        {
            if (string.IsNullOrEmpty(model.Client.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY CLIENT" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Group.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY GROUP" }, JsonRequestBehavior.AllowGet);
            }

            if (model.Basic <= 0m)
            {
                return Json(new { message = "PLEASE SPECIFY BASIC AMOUNT" }, JsonRequestBehavior.AllowGet);
            }

            if (model.VatRate <= 0m)
            {
                return Json(new { message = "PLEASE SPECIFY VAT RATE" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var entity = db.PaymentSetup.FirstOrDefault(x => x.Id == model.Id);
                entity.Group = model.Group;
                entity.Client = model.Client;
                entity.Basic = model.Basic;
                entity.TransportationAllowance = model.TransportationAllowance;
                entity.NightAllowance = model.NightAllowance;
                entity.WorkShift = model.WorkShift;
                entity.WorkWeek = model.WorkWeek;
                entity.Overtime = model.Overtime;
                entity.VatRate = model.VatRate;
                db.Entry<PaymentSetup>(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }

        }


        public List<SelectListItem> getGroups()
        {
            List<SelectListItem> items_ = new List<SelectListItem>();
            var records = db.EmployeeGroup.Select(x => x).ToList();
            foreach (var item in records)
            {
                items_.Add(new SelectListItem() { Text = item.GroupName, Value = item.Id.ToString() });
            }
            return items_;
        }
        #endregion


        #region CASUAL


        public PartialViewResult getEditCasualPay(int Id)
        {
            ViewBag.VGroup = getGroups();
            ViewBag.VShift = getShift();
            ViewBag.VWeekScheme = getWorkingSchedule();
            var entity = db.CasualPaymentSetup.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditCasualPaymentSetup", entity);
        }

        //
        public JsonResult getWorkingHours()
        {
            var records = db.OperationalWorkHours.AsEnumerable().Select(a =>
            new
            {
                Id = a.Id,
                Hours = string.Format("{0:N2}", a.WorkingHours)
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCasualSetup()
        {
            var records_ = (from a in db.CasualPaymentSetup.AsEnumerable()
                            join b in db.EmployeeGroup.AsEnumerable() on a.Group equals b.Id
                            select new
                            {
                                Id = a.Id,
                                Basic = string.Format("{0:N2}", a.Basic),
                                Group=b.GroupName,
                                Shift=a.WorkShift,
                                Week=a.WorkWeek,
                                NightAllowance = string.Format("{0:N2}", a.NightAllowance),
                                TransportationAllowance = string.Format("{0:N2}", a.TransportationAllowance),
                                OverTime = string.Format("{0:N2}", a.Overtime),
                                SSF = string.Format("{0:N2}", a.SSF),
                                Union = string.Format("{0:N2}", a.UnionDues),
                                Welfare = string.Format("{0:N2}", a.Welfare),
                                PF = string.Format("{0:N2}", a.PF),
                            }).ToList();


            //var records = db.CasualPaymentSetup.AsEnumerable().Select(a =>
            //new
            //{
                
            //}).ToList();
            return Json(new { data = records_ }, JsonRequestBehavior.AllowGet);
        }

        //SaveCasualPaymentSetup
        public JsonResult SaveCasualPaymentSetup(CasualPaymentSetup model)
        {
            if (model.Basic <= 0m)
            {
                return Json(new { message = "PLEASE SPECIFY BASIC AMOUNT" }, JsonRequestBehavior.AllowGet);
            }

            if (model.NightAllowance < 0m)
            {
                return Json(new { message = "PLEASE CHECK NIGHT ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TransportationAllowance < 0m)
            {
                return Json(new { message = "PLEASE CHECK TRANSPORTATION ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.SSF < 0d)
            {
                return Json(new { message = "PLEASE CHECK SSF RATE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.PF < 0d)
            {
                return Json(new { message = "PLEASE CHECK PROVIDENT FUND" }, JsonRequestBehavior.AllowGet);
            }

            if (model.UnionDues < 0m)
            {
                return Json(new { message = "PLEASE CHECK UNION DUES" }, JsonRequestBehavior.AllowGet);
            }

            if (model.Overtime < 0m)
            {
                return Json(new { message = "PLEASE CHECK OVERTIME" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnOvertime < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON OVERTIME" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnTransport < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON TRANSPORT" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnAllowance < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }


            if (model.TaxOnBasic < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON BASIC" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.CasualPaymentSetup.Add(model); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        public JsonResult EditCasualPaymentSetup(CasualPaymentSetup model)
        {
            if (model.Basic <= 0m)
            {
                return Json(new { message = "PLEASE SPECIFY BASIC AMOUNT" }, JsonRequestBehavior.AllowGet);
            }

            if (model.NightAllowance < 0m)
            {
                return Json(new { message = "PLEASE CHECK NIGHT ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TransportationAllowance < 0m)
            {
                return Json(new { message = "PLEASE CHECK TRANSPORTATION ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.SSF < 0d)
            {
                return Json(new { message = "PLEASE CHECK SSF RATE" }, JsonRequestBehavior.AllowGet);
            }

            if (model.PF < 0d)
            {
                return Json(new { message = "PLEASE CHECK PROVIDENT FUND" }, JsonRequestBehavior.AllowGet);
            }

            if (model.UnionDues < 0m)
            {
                return Json(new { message = "PLEASE CHECK UNION DUES" }, JsonRequestBehavior.AllowGet);
            }

            if (model.Overtime < 0m)
            {
                return Json(new { message = "PLEASE CHECK OVERTIME" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnOvertime < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON OVERTIME" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnTransport < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON TRANSPORT" }, JsonRequestBehavior.AllowGet);
            }

            if (model.TaxOnAllowance < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON ALLOWANCE" }, JsonRequestBehavior.AllowGet);
            }

       
            if (model.TaxOnBasic < 0)
            {
                return Json(new { message = "PLEASE SPECIFY TAX ON BASIC" }, JsonRequestBehavior.AllowGet);
            }

            var entity = db.CasualPaymentSetup.FirstOrDefault(x => x.Id == model.Id);
            if (entity != null)
            {
                entity.Basic = model.Basic;
                entity.Group = model.Group;
                entity.WorkShift = model.WorkShift;
                entity.WorkWeek = model.WorkWeek;
                entity.NightAllowance = model.NightAllowance;
                entity.TransportationAllowance = model.TransportationAllowance;
                entity.UnionDues = model.UnionDues;
                entity.Welfare = model.Welfare;
                entity.PF = model.PF;
                entity.SSF = model.SSF;
                entity.Overtime = model.Overtime;
                entity.TaxOnBasic = model.TaxOnBasic;
                entity.TaxOnAllowance = model.TaxOnAllowance;
                entity.TaxOnOvertime = model.TaxOnOvertime;
                entity.TaxOnTransport = model.TaxOnTransport;

                db.Entry<CasualPaymentSetup>(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "PAYMENT SETUP NOT FOUND" }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region TAX SETUP

        //getEditTax

        public PartialViewResult getEditWorkingHours(int Id)
        {
            var entity = db.OperationalWorkHours.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditWorkingHours", entity);
        }
        public JsonResult DeleteTaxSetup(int Id)
        {
            var entity = db.OperationalWorkHours.FirstOrDefault(x => x.Id == Id);
            if (entity != null)
            {
                db.OperationalWorkHours.Remove(entity); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "WORKING HOURS NOT FOUND" }, JsonRequestBehavior.AllowGet);
        }

        //
        public JsonResult editHoursSetup(OperationalWorkingHours model)
        {
            if (model.WorkingHours <= 0d)
            {
                return Json(new { message = "HOURS MUST BE GREATER THAN ZERO" }, JsonRequestBehavior.AllowGet);
            }

            if (model.WorkingHours > 24)
            {
                return Json(new { message = "HOURS MUST BE BETWEEN 1-24 " }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var entity = db.OperationalWorkHours.FirstOrDefault(x => x.Id == model.Id);
                entity.WorkingHours = model.WorkingHours;
             
                db.Entry<OperationalWorkingHours>(entity).State= System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult saveHoursSetup(OperationalWorkingHours model)
        {
            var work_hours = db.OperationalWorkHours.Select(x => x).ToList();
            if (work_hours.Count > 0)
            {
                return Json(new { message = "OPERATIONAL WORKING HOURS HAS ALREADY BEEN SET" }, JsonRequestBehavior.AllowGet);
            }


            if (model.WorkingHours <= 0d)
            {
                return Json(new { message = "HOURS MUST BE GREATER THAN ZERO" }, JsonRequestBehavior.AllowGet);
            }

            if (model.WorkingHours >24)
            {
                return Json(new { message = "HOURS MUST BE BETWEEN 1-24 " }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                db.OperationalWorkHours.Add(model); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}