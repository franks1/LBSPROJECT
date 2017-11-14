using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
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
    public class ForemenController : Controller
    {
        RBACDbContext db;
        public ForemenController()
        {
            db = new RBACDbContext();
        }

        [HttpGet]
        // GET: Foremen
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getData()
        {
            var records = db.Foremen.AsEnumerable().Select(X => new
            {
                Id = X.Id,
                Code = X.Code,
                Name = X.FullName,
                Branch = X.Branch,
                Status = X.Status,
                DateJoined = string.Format("{0:dd/MM/yyyy}", X.DateJoined)
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.FieldClient = getClients();
            ViewBag.VBranch = getBranches();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Foremen model)
        {
            try
            {
                var code = SequenceHelper.getSequence(db, SequenceHelper.NType.EMPLOYEE);
                var client = db.ClientSetup.FirstOrDefault(x => x.Id > 0);
                model.Code = code;
                model.Status = "ACTIVE";
                if (model.IsClientForeman == false)
                {
                    model.ClientId = client.Id;
                }
                db.Foremen.Add(model); db.SaveChanges();
                SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.EMPLOYEE);
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message.ToUpper());
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            try
            {
                ViewBag.FieldClient = getClients();
                ViewBag.VBranch = getBranches();
                var entity = db.Foremen.FirstOrDefault(x => x.Id == Id);
                return View(entity);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Foremen model)
        {
            try
            {
                var entity = db.Foremen.FirstOrDefault(x => x.Id == model.Id);
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.MiddleName = model.MiddleName;

                entity.DateJoined = model.DateJoined;
                entity.DateOfBirth = model.DateOfBirth;
                if (!string.IsNullOrEmpty(entity.ClientId.ToString()))
                {
                    entity.ClientId = model.ClientId;
                    entity.Branch = model.Branch ?? "";
                    entity.IsClientForeman = model.IsClientForeman;
                }
                db.Entry<Foremen>(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message.ToUpper());
                return View(model);
            }
        }


        public List<SelectListItem> getBranches()
        {
            List<SelectListItem> field_client = new List<SelectListItem>();
            var client = db.Branch.Select(x => x).ToList();
            foreach (var item in client)
            {
                field_client.Add(new SelectListItem() { Text = item.Name, Value = item.Name.ToString() });
            }
            return field_client;
        }

        public List<SelectListItem> getClients()
        {
            List<SelectListItem> field_client = new List<SelectListItem>();
            var client = db.FieldClient.Select(x => x).ToList();
            foreach (var item in client)
            {
                field_client.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return field_client;
        }

        public JsonResult DisableForeman(int Id)
        {
            var entity = db.Foremen.FirstOrDefault(x => x.Id == Id);
            if (entity != null)
            {
                entity.Status = entity.Status == "DISABLED" ? "ACTIVE" : "DISABLED";
                db.Entry<Foremen>(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "FOREMAN NOT FOUND" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}