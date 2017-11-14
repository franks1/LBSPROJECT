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
    public class ManageGangController : Controller
    {
        RBACDbContext db;
        GangHelper helper;
        public ManageGangController()
        {
            this.db = new RBACDbContext();
            helper = new GangHelper(db);
        }
        // GET: ManageGang

        public ActionResult Index()
        {
            ViewBag.VBranch = getBranches();
            return View(new Gang());
        }

        public JsonResult getGangs()
        {
            var records=helper.getGangs();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(Gang model)
        {
            var gangs = db.Gang.Select(x => x).ToList();
            if (gangs.Exists(a => a.Description.ToLower().Equals(model.Description.ToLower())))
            {
                return Json(new { message = "GANG ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return Json(new { message = "PLEASE SPECIFY GANG NAME" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.Branch.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY BRANCH" }, JsonRequestBehavior.AllowGet);
            }

            int result=helper.SaveGang(model);
            if (result > 0)
            {
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "AN ERROR OCCURED" }, JsonRequestBehavior.AllowGet);
            }
        }
        //
        public JsonResult DeleteGang(int Id)
        {
            int result = helper.DeleteGang(Id);
            if (result > 0)
            {
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "AN ERROR OCCURED" }, JsonRequestBehavior.AllowGet);
            }
        }
        public List<SelectListItem> getBranches()
        {
            List<SelectListItem> branches = new List<SelectListItem>() { };
            var branch = db.Branch.Select(x => x).ToList();
            foreach(var item in branch)
            {
                branches.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return branches;
        }


        public PartialViewResult getEditGang(int Id)
        {
            ViewBag.VBranch = getBranches();
            var entity = db.Gang.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditGang", entity);
        }

        //
        public JsonResult EditGang(Gang model)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return Json(new { message = "PLEASE SPECIFY GANG NAME" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.Branch.ToString()))
            {
                return Json(new { message = "PLEASE SPECIFY BRANCH" }, JsonRequestBehavior.AllowGet);
            }

            int result = helper.EditGang(model);
            if (result > 0)
            {
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "AN ERROR OCCURED" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}