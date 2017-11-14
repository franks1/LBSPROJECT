using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
using GetLabourManager.Models;
using GetLabourManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class ForemanOperationController : Controller
    {
        RBACDbContext db;
        TreeBuilderHelper helper;
        GangAllocationHelper gang_allocation_helper;
        public ForemanOperationController()
        {
            db = new RBACDbContext();
            helper = new TreeBuilderHelper(db);
            gang_allocation_helper = new GangAllocationHelper(db);
        }

        // GET: ForemanOperation
        public ActionResult Index()
        {
            ViewBag.VContainer = getContainers();
            return View();
        }

        public JsonResult getPendingRequest()
        {
            var result = helper.getGangAdvice();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRequestDetails(string request_code)
        {

            try
            {
                var result = helper.getRequestDetails(request_code);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new TreeBuilderHelper.RequestDetails(), JsonRequestBehavior.AllowGet);
            }
        }
        public List<SelectListItem> getContainers()
        {
            var records = db.VesselContainer.Select(x => x).ToList();
            List<SelectListItem> _items = new List<SelectListItem>();
            foreach (var item in records)
            {
                _items.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Continer });
            }
            return _items;
        }

        public JsonResult ConainerList()
        {
            var records = db.VesselContainer.Select(x => new
            {
                Id = x.Id,
                Container = x.Continer,
                Number = ""
            }).ToList();

            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllocateGang(string RequestCode, List<AssignedContainers> containers)
        {
            ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);
            var transaction = db.Database.BeginTransaction();

            try
            {
                if (containers.Count == 0)
                {
                    return Json(new { message = "CONTAINERS NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }

                var valid_entry = containers.AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(x.Number)).Select(x => x).ToList();

                if (valid_entry.Count == 0)
                {
                    return Json(new { message = "PLEASE SPECIFY CONTAINER NUMBER FOR GANGS" }, JsonRequestBehavior.AllowGet);
                }

                var gang_request = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.RequestCode == RequestCode & x.Status == "PENDING");

                if (gang_request == null)
                {
                    return Json(new { message = "GANG ADVICE NOT FOUND !" }, JsonRequestBehavior.AllowGet);
                }

                var gangs_allocated = db.GangAllocation.FirstOrDefault(x => x.RequestCode == RequestCode);
                if (gangs_allocated == null)
                {
                    GangAllocation allocation = new GangAllocation()
                    {
                        RequestCode = gang_request.RequestCode,
                        Allocated = gang_request.DateIssued,
                        AllocatedBy = user.Id,
                        AllocatedOn = DateTime.Now.Date,
                    };
                    db.GangAllocation.Add(allocation);
                    db.SaveChanges();
                    var casuals = gang_request.SheetItems.Take(1).FirstOrDefault();

                    List<AllocationContainers> containers_items = new List<AllocationContainers>() { };
                    
                    foreach (var item in containers)
                    {
                            if (!string.IsNullOrWhiteSpace(item.Number))
                            {
                                var entity = db.VesselContainer.FirstOrDefault(x => x.Continer == item.Container);
                                var container = new AllocationContainers()
                                {
                                    ContainerNumber = item.Number,
                                    ContainerId = entity.Id,
                                    GroupId = 0,
                                    CategoryId = casuals.Category,// category,
                                    RequestCode = gang_request.RequestCode
                                };
                                containers_items.Add(container);
                            }
                    }
                    if (containers_items.Count > 0)
                    {
                        containers_items.ForEach(x => x.AllocationId = allocation.Id);
                        db.AllocationContainers.AddRange(containers_items); db.SaveChanges();
                    }

                    gang_request.SheetItems.ForEach(x => x.AllocationId = allocation.Id);
                    db.SaveChanges();

                    gang_request.Status = "ALLOCATED";
                    db.Entry<GangSheetHeader>(gang_request).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);

                    //var allocated = gang_allocation_helper.IsGangAllocated(gang_request.RequestCode);
                }
                return Json(new { message = "REQUEST ALREADY ALLOCATED" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                transaction.Rollback();
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}