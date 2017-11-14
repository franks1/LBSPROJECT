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
using static GetLabourManager.Helper.GangAllocationHelper;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class OperationController : Controller
    {
        RBACDbContext db;
        GangAllocationHelper gang_helper;
        TreeBuilderHelper helper;
        GangHelper casual_helper;
        public OperationController()
        {
            db = new RBACDbContext();
            gang_helper = new GangAllocationHelper(db);
            helper = new TreeBuilderHelper(db);
            casual_helper = new GangHelper(db);
        }

        // GET: Operation
        public ActionResult Index()
        {
          
            return View();
        }
        public async Task<JsonResult> getCostSheetData()
        {
            //
            var records = await casual_helper.getCostSheetList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
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

        public ActionResult CostSheetIndex()
        {
            return View();
        }

        public ActionResult CostSheetDetails(string code)
        {
            var gang_request = db.GangSheetHeader.FirstOrDefault(x => x.RequestCode == code);
            ViewBag.IsFound = false;

            if (gang_request != null)
            {
                ViewBag.IsFound = true;
                ViewBag.NumberCode = gang_request.RequestCode;
                ViewBag.VStatus = gang_request.Status;
            }
            //ViewBag.VRequest = getAllocatedRequest();
            return View();
        }


        public async Task<JsonResult> searchGangs(string q)
        {
            var records = await gang_helper.SearchAllocatedGangs(q);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllocatedGangs(string term)
        {
            var records =  gang_helper.getRequestDetails(term);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        //getAllocatedGangs

        public async Task<JsonResult> getAllocatedGangsMembers(string term)
        {
            var records = await gang_helper.getAllocatedGangs(term);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GangContainers(string code, string GroupName)
        {
            var container = db.EmployeeGroup.FirstOrDefault(x => x.GroupName == GroupName);
            var group_id = container == null ? 0 : container.Id;

            var records = (from a in db.AllocationContainers
                           join b in db.VesselContainer on a.ContainerId equals b.Id
                           where (a.RequestCode == code && a.GroupId == group_id)
                           select new VesselContainerViewModel
                           {
                               Id = b.Id,
                               Container = b.Continer,
                               Number = a.ContainerNumber

                           }).ToList();
            ViewBag.VList = records;
            return PartialView("_GangContainers");
        }

        public List<SelectListItem> getAllocatedRequest()
        {
            var records = db.GangSheetHeader.Where(x => x.Status == "APPROVED").Select(x => x).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in records)
            {
                items.Add(new SelectListItem() { Text = item.RequestCode, Value = item.Id.ToString() });
            }
            return items;
        }

        //

        #region MANAGE COST SHEET

        public JsonResult getAppliedCostSheet()
        {
            var result = helper.getAppliedCostSheet();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAppliedCostSheetItems()
        {
            var result = helper.getAppliedCostSheet();
            return Json(new {data=result.FirstOrDefault().items }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> getCostSheetRequest(string term)
        {
            var result =await gang_helper.getCostSheetRequest(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        public JsonResult getCostSheet(string code)
        {
            var result = helper.getCostSheet(code);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CasualMembers(string term)
        {
            ViewBag.Term = term;
            return PartialView("_GangCasuals");
        }

        public PartialViewResult ListCasualContainers(string term)
        {
            ViewBag.Term = term;
            return PartialView("_GangListContainers");
        }

        public async Task<JsonResult> getCasualMembers(string term)
        {
            var result = await gang_helper.getGangCasualMembers(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //
        public async Task<JsonResult> getGangContainers(string term)
        {
            var result = await gang_helper.getGangContainerList(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddCostSheet(List<GangAllocatedMembers> members, CostSheetHeader header,
            double HoursWorked,double OverTime)
        {
            try
            {
                ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);
                var transaction_track = db.Database.BeginTransaction();
                var gang_request = db.GangSheetHeader.FirstOrDefault(x => x.RequestCode == header.RequestCode);
                var client = db.FieldClient.FirstOrDefault(a => a.Name == header.FieldClient);
                var sequence = SequenceHelper.getSequence(db, SequenceHelper.NType.COSTSHEET);

                if (string.IsNullOrEmpty(sequence))
                {
                    return Json(new { message = "COST SHEET SEQUENCE NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }

                if (gang_request != null)
                {
                    CostSheet sheet = new CostSheet()
                    {
                        Client = client.Id,
                        CostSheetNumber = sequence,
                        Note = "",
                        PreparedBy = user.Id,
                        RequestHeader = gang_request.Id,
                        Status = "GENERATED",
                        DatePrepared = header.DateApplied,
                    };
                    db.CostSheet.Add(sheet);
                    db.SaveChanges();

                    List<CostSheetItems> sheet_items = new List<CostSheetItems>();
                    foreach (var item in members)
                    {
                            CostSheetItems sheet_item = new CostSheetItems()
                            {
                                Container = item.Containers,
                                FullName = item.FullName,
                                CostSheetId = 0,
                                Gang = item.Gang,
                                GroupName = item.GroupName,
                                HourseWorked = HoursWorked,
                                RaisedOn = header.DateApplied,
                                StaffCode = item.StaffCode,
                                OvertimeHrs=OverTime
                            };
                            sheet_items.Add(sheet_item);
                    }

                    sheet_items.ForEach(x => x.CostSheetId = sheet.Id);
                    db.CostSheetItems.AddRange(sheet_items);db.SaveChanges();
                    gang_request.Status = "APPLIED";
                    db.Entry<GangSheetHeader>(gang_request).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.COSTSHEET);
                    transaction_track.Commit();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { message = "GANG REQUEST NOT FOUND" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
       
        public ActionResult ProcessCostSheet()
        {
            ViewBag.UserId = ApplicationUserManager.FindUserByName(db, User.Identity.Name).Id;
            ViewBag.VClients = getClients();
            return View();
        }
        
        #endregion
    }
}