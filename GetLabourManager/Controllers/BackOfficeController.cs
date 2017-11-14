using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
using GetLabourManager.Models;
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
    public class BackOfficeController : Controller
    {
        RBACDbContext db;
        GangAllocationHelper helper;
        public BackOfficeController()
        {
            db = new RBACDbContext();
            helper = new GangAllocationHelper(db);
        }

        // GET: BackOffice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TimeAudit()
        {
            ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);
            ViewBag.UserId = user.Id;
            ViewBag.VClients = getClients();
            return View();
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

        #region APPROVAL AND EXEMPTION
        public async Task<JsonResult> getGangRequest()
        {
            var records = await helper.getGangsRequests();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getClientInvoices(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                //
                return Json(new { data ="" }, JsonRequestBehavior.AllowGet);

            }
            var entity = db.FieldClient.FirstOrDefault(x => x.Name.Equals(name));
            var invoices = db.ProcessedInvoice.AsEnumerable().Where(x => x.Client == entity.Id)
                .Select(a => new
                {
                    Id = a.Id,
                    Date =string.Format("{0:dd/MM/yyyy}",a.ProccessdOn),
                    Invoice = a.Invoice,
                    PreparedBy=a.ProcessedBy,
                    Status=a.Status
                }).OrderByDescending(a=>a.Id).ToList();
            return Json(new { data = invoices }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> getGangMembers(string code)
        {
            var records = await helper.getGangMembers(code);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        //getGangContainers
        public async Task<JsonResult> getGangContainers(string code)
        {
            var records = await helper.getGangContainers(code);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getGangDetails(string code)
        {
            var records = helper.getGangDetails(code);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveGangAdvice(string code, string reason)
        {
            ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);
            //ADD USER EXCLUSION HERE

            if (string.IsNullOrWhiteSpace(reason))
            {
                return Json(new { message = "PLEASE SPECIFY REASON" }, JsonRequestBehavior.AllowGet);
            }
            //MEANWHILE
            var gang_request = db.GangSheetHeader.FirstOrDefault(x => x.RequestCode == code);

            if (gang_request.Status == "APPROVED")
            {
                return Json(new { message = "REQUEST HAS ALREADY BEEN APPROVED" }, JsonRequestBehavior.AllowGet);
            }
            if (gang_request.Status == "PENDING")
            {
                return Json(new { message = "GANG HAS NOT BEEN ALLOCATED" }, JsonRequestBehavior.AllowGet);
            }

            if (gang_request != null)
            {
                gang_request.ApprovalNote = reason;
                gang_request.ApprovedBy = user.Id;
                gang_request.Status = "APPROVED";
                db.Entry<GangSheetHeader>(gang_request).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "GANG ADVICE NOT FOUND" }, JsonRequestBehavior.AllowGet);
            }
        }

        //ExemptCasuals
        public JsonResult ExemptCasuals(string[] staff, string request_code, string reason)
        {
            ApplicationUser user = ApplicationUserManager.FindUserByName(db, User.Identity.Name);

            try
            {
                var gang_request = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.RequestCode == request_code);


                if (gang_request.Status == "APPROVED")
                {
                    return Json(new { message = "REQUEST HAS ALREADY BEEN APPROVED" }, JsonRequestBehavior.AllowGet);
                }
                if (gang_request.Status == "CANCELLED")
                {
                    return Json(new { message = "REQUEST HAS ALREADY BEEN CANCELLED" }, JsonRequestBehavior.AllowGet);
                }

                if (gang_request.SheetItems.Count == staff.Length)
                {
                    return Json(new { message = "CAN'T EXEMPT ALL CASUALS" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<GangSheetItems> exempted_casuals = new List<GangSheetItems>();
                    List<GangMemberExemption> casuals_removed = new List<GangMemberExemption>();
                    foreach (var item_code in staff)
                    {
                        foreach (var casual_worker in gang_request.SheetItems)
                        {
                            if (casual_worker.StaffCode == item_code)
                            {
                                exempted_casuals.Add(casual_worker);
                                GangMemberExemption model = new GangMemberExemption()
                                {
                                    AddedOn = DateTime.Now,
                                    CasualCode = casual_worker.StaffCode,
                                    PerformedBy = user.Id,
                                    RequestCode = request_code,
                                    RequestId = gang_request.Id,

                                };
                                casuals_removed.Add(model);
                            }
                        }
                    }
                    if (exempted_casuals.Count > 0)
                    {
                        db.GangSheetItems.RemoveRange(exempted_casuals);
                        db.SaveChanges();

                        db.ExemptedCasuals.AddRange(casuals_removed); db.SaveChanges();
                        return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { message = "SELECTED CASUAL WORKER(S) NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ERR)
            {
                return Json(new { message = ERR.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        #region  TIME AUDIT

        public JsonResult getFieldClients()
        {
            var records = db.FieldClient.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRequestByClient(int Id)
        {
            var records = (from a in db.GangSheetHeader
                           where (a.Status.Equals("APPROVED") && a.FieldClient == Id)
                           join b in db.EmpCategory on a.GangType equals b.Id
                           select new
                           {
                               Selected = false,
                               Code = a.RequestCode,
                               Reference = a.RequestCode + "-" + b.Category
                           }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getCostSheetByClient(int Id)
        {
            var records = (from a in db.CostSheet.Include("SheetItems")
                           where (a.Status.Equals("GENERATED") && a.Client == Id)
                           join b in db.GangSheetHeader on a.RequestHeader equals b.Id
                           select new
                           {
                               Selected = false,
                               RCode = b.RequestCode,
                               CCode = a.CostSheetNumber,
                               Casuals = a.SheetItems.Count
                           }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}