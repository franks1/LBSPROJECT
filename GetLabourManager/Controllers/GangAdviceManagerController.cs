using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
using GetLabourManager.Models;
using GetLabourManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class GangAdviceManagerController : Controller
    {
        RBACDbContext db;
        GangHelper helper;
        public GangAdviceManagerController()
        {
            this.db = new RBACDbContext();
            helper = new GangHelper(db);
        }
        // GET: GangAdviceManager
        public ActionResult Index(string code)
        {
            return View();
        }

        public ActionResult GangAdviceDetails(string code)
        {
            ViewBag.VGangs = getGangs();
            ViewBag.VClients = getClients();
            ViewBag.VCategory = getCategory();
            var gang_sheet_header = db.GangSheetHeader.FirstOrDefault(x => x.RequestCode == code);
            return View(gang_sheet_header);
        }
        public async Task<JsonResult> getData()
        {
           
            var records = await helper.getGangAdviceList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        //
        public async Task<JsonResult> getAdviceItems(int Id)
        {
            var records = await helper.getAdviceItems(Id);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GangRequest()
        {
            ViewBag.VGangs = getGangs();
            ViewBag.VClients = getClients();
            ViewBag.VCategory = getCategory();
            ViewBag.VShift = getShift();
            ViewBag.VWeekScheme = getWorkingSchedule();
            ViewBag.VLocations = getlocation();
            ViewBag.VRequestCode = SequenceHelper.getSequence(db, SequenceHelper.NType.GANG_ADVICE);
            return View();
        }

        public JsonResult RaiseGangRequest(GangAdviceViewModel model)
        {
            var transaction_track = db.Database.BeginTransaction();

            try
            {


                var CLIENT_PAYMENT_PARAMETERS = db.PaymentSetup
                    .Where(x => x.Client == model.FieldClient).Select(a => a).ToList();

                var CASUAL_PAYMENT_PARAMETERS = db.CasualPaymentSetup
                   .Select(a => a).ToList();


                if (!CLIENT_PAYMENT_PARAMETERS.Exists(x=>x.WorkShift==model.Shift && x.WorkWeek == model.Week))
                {
                    string work_schedule = model.Shift + "-" + model.Week;
                    return Json(new { message = work_schedule+" PAYMENT PARAMETER HAS NOT BEEN SET FOR CLIENT" }, JsonRequestBehavior.AllowGet);
                }

                bool flag = false;
                string group = "";
                foreach (var item in model.Entries)
                {
                    if (!CLIENT_PAYMENT_PARAMETERS.
                        Exists(x => x.WorkShift == model.Shift && x.WorkWeek == model.Week && x.Group==item.Group))
                    {
                        group = item.GroupName;
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    string work_schedule = model.Shift + "-" + model.Week +"-"+ group;
                    return Json(new { message = work_schedule + " PAYMENT PARAMETER HAS NOT BEEN SET FOR CLIENT" }, JsonRequestBehavior.AllowGet);
                }

                
                foreach (var item in model.Entries)
                {
                    if(!CASUAL_PAYMENT_PARAMETERS
                      .Exists(x => x.WorkShift == model.Shift && x.WorkWeek == model.Week && x.Group == item.Group))
                    {
                        group = item.GroupName;
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    string work_schedule = model.Shift + "-" + model.Week + "-" + group;
                    return Json(new { message = work_schedule + " PAYMENT PARAMETER HAS NOT BEEN SET FOR CASUALS" }, JsonRequestBehavior.AllowGet);
                }



                ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);
                GangSheetHeader header = new GangSheetHeader();
                var entry_item=model.Entries.Take(1).FirstOrDefault();
                header.Status = "PENDING";
                header.PreparedBy = user.Id;
                header.GangCode = model.Gang;
                header.FieldClient = model.FieldClient;
                header.WorkShift = model.Shift;
                header.WorkWeek = model.Week;
                header.DateIssued = model.DateIssued;
                header.GangType = entry_item.Category;
                header.RequestCode = model.RequestCode;
                header.ApprovalNote = "N/A";
                header.FieldLocation = model.FieldLocation;
                header.Narration = model.DateIssued.ToShortDateString() + " GANG REQUEST-" + model.RequestCode;
                db.GangSheetHeader.Add(header);
                db.SaveChanges();

                List<GangSheetItems> entries = new List<GangSheetItems>();
                foreach (var item in model.Entries)
                {
                    GangSheetItems items = new GangSheetItems()
                    {
                        StaffCode = item.StaffCode,
                        Category = item.Category,
                        Group = item.Group
                    };
                    entries.Add(items);
                }
                entries.ForEach(x => x.Header = header.Id);
                db.GangSheetItems.AddRange(entries); db.SaveChanges();
                SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.GANG_ADVICE);
                transaction_track.Commit();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                transaction_track.Rollback();
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult getCasuals()
        {
            return PartialView("_EmployeeSearch");
        }

        //
        public JsonResult DeleteGangItems(int Id, string[] Items)
        {
            var transaction_track = db.Database.BeginTransaction();

            try
            {

                var entries = db.GangSheetItems.Where(x => x.Id == Id);
                foreach (var item in Items)
                {
                    var entity = db.GangSheetItems.FirstOrDefault(x => x.Header == Id && x.StaffCode == item);
                    if (entity != null)
                    {
                        db.GangSheetItems.Remove(entity); db.SaveChanges();
                    }
                }
                transaction_track.Commit();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                transaction_track.Rollback();
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult getAdviceCode()
        {
            var code = SequenceHelper.getSequence(db, SequenceHelper.NType.GANG_ADVICE);
            return Json(new { data = code }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateGangEntry(List<NewGangEntry> model)
        {
            bool isValid = true;
            string note = "";
            var entryId = model.Take(1).FirstOrDefault();
            var sheetHeader = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.Id == entryId.HeaderId);

            foreach (var item in sheetHeader.SheetItems)
            {
                foreach (var item_ in model)
                {
                    if (item.StaffCode == item_.StaffCode)
                    {
                        isValid = false;
                        note = item.StaffCode + " PERSONNEL ALREADY EXIST";
                        break;
                    }
                }
                if (isValid == true)
                    continue;
                else
                    break;
            }

            if (isValid == true)
            {
                List<GangSheetItems> entries = new List<GangSheetItems>();
                foreach (var item in model)
                {
                    GangSheetItems items = new GangSheetItems()
                    {
                        StaffCode = item.StaffCode,
                        Category = item.Category,
                        Group = item.Group
                    };
                    entries.Add(items);
                }
                entries.ForEach(x => x.Header = entryId.HeaderId);
                db.GangSheetItems.AddRange(entries); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = note }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<SelectListItem> getGangs()
        {
            List<SelectListItem> branches = new List<SelectListItem>() { };
            var gangs = db.Gang.Select(x => x).ToList();
            foreach (var item in gangs)
            {
                branches.Add(new SelectListItem() { Text = item.Description, Value = item.Code.ToString() });
            }

            return branches;
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

        public List<SelectListItem> getCategory()
        {
            List<SelectListItem> item_group = new List<SelectListItem>();
            var records = db.EmpCategory.Select(x => x).ToList();
            foreach (var item in records)
            {
                item_group.Add(new SelectListItem() { Text = item.Category, Value = item.Id.ToString() });
            }
            return item_group;
        }

        public List<SelectListItem> getlocation()
        {
            List<SelectListItem> item_group = new List<SelectListItem>();
            var records = db.LocationArea.Select(x => x).ToList();
            foreach (var item in records)
            {
                item_group.Add(new SelectListItem() { Text = item.Location, Value = item.Id.ToString() });
            }
            return item_group;
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


        public JsonResult getAssignedGroups(int category)
        {
            var records = (from a in db.EmpCategory
                           join b in db.EmployeeGrouping on a.Id equals b.Category
                           join c in db.EmployeeGroup on b.Group equals c.Id
                           where b.Category == category
                           select new
                           {
                               Id = c.Id,
                               GroupName = c.GroupName
                           }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAssignedClients(int category)
        {
            var records = (from a in db.EmpCategory
                           join b in db.GangClientGrouping on a.Id equals b.Category
                           join c in db.FieldClient on b.Client equals c.Id
                           where b.Category == category
                           select new
                           {
                               Id = c.Id,
                               Name = c.Name
                           }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        #region GANG ALLOCATION
        //Get Categories for Request Code
        public ActionResult getRequestCategories(string code)
        {
            var records = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.RequestCode == code);
            List<EmployeeCategory> category_list = new List<EmployeeCategory>();
            if (records.SheetItems.Count > 0)
            {
                var categories_id = records.SheetItems.Select(a => a.Category).ToList().Distinct();
                foreach (var item in categories_id)
                {
                    var entity = db.EmpCategory.FirstOrDefault(x => x.Id == item);
                    if (entity != null)
                        category_list.Add(entity);
                }
            }
            return Json(new { data = category_list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRequestGroup(string code, int category)
        {
            var records = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.RequestCode == code);
            List<EmployeeGroup> group_list = new List<EmployeeGroup>();
            if (records.SheetItems.Count > 0)
            {
                var group_id = records.SheetItems.Where(x => x.Category == category).Select(a => a.Group).ToList().Distinct();
                var allocation = db.GangAllocation.Include("Containers")
                    .FirstOrDefault(x => x.RequestCode == code);
                foreach (var item in group_id)
                {
                    var entity = db.EmployeeGroup.FirstOrDefault(x => x.Id == item);
                    if (entity != null)
                    {
                        if (allocation != null)
                        {
                            if (!allocation.Containers.Exists(x => x.CategoryId == category && x.GroupId == entity.Id))
                            {
                                group_list.Add(entity);
                            }
                        }
                        else
                        {
                            group_list.Add(entity);
                        }
                    }
                }
            }
            return Json(new { data = group_list }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}