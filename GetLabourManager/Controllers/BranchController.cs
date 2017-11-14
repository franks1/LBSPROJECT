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
    public class BranchController : Controller
    {
        // GET: Branch
        RBACDbContext db;
        public BranchController()
        {
            db = new RBACDbContext();
        }
        public ActionResult Index()
        {
            return View(new Branch());
        }

        public JsonResult BranchList()
        {
            var records = db.Branch.AsEnumerable().Select(x =>
            new
            {
                Id = x.Id,
                Name = x.Name,
                IsHeadBranch = x.IsHead == true ? "YES" : "NO"
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetEdit(int Id)
        {
            var entity = db.Branch.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditBranch", entity);
        }


        public JsonResult DeleteBranch(int Id)
        {
            try
            {
                var entity = db.Branch.FirstOrDefault(x => x.Id == Id);
                db.Branch.Remove(entity);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        //SaveBranch
        public JsonResult SaveBranch(Branch model)
        {
            try
            {
                var branches = db.Branch.Select(x => x).ToList();
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return Json(new { message = "BRANCH NAME NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                if (branches.Exists(c => c.Name.ToLower().Equals(model.Name.ToLower())))
                {
                    return Json(new { message = "BRANCH NAME ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
                }

                db.Branch.Add(model);
                db.SaveChanges();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditBranch(Branch model)
        {
            try
            {
                var branches = db.Branch.FirstOrDefault(x => x.Id==model.Id);
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return Json(new { message = "BRANCH NAME NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                //if (branches.Exists(c => c.Name.ToLower().Equals(model.Name.ToLower())))
                //{
                //    return Json(new { message = "BRANCH NAME ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
                //}
                branches.Name = model.Name;
                branches.IsHead = model.IsHead;
                db.Entry<Branch>(branches).State= System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}