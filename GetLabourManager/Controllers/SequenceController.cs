using GetLabourManager.ActionFilters;
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
    public class SequenceController : Controller
    {
        // GET: Sequence
        RBACDbContext db = new RBACDbContext();

        public ActionResult Index()
        {
            ViewBag.SequenceType = SequenceTypes();
            return View(new MasterSequence());
        }

        public List<SelectListItem> SequenceTypes()
        {
            List<SelectListItem> sequence_types = new List<SelectListItem>()
            {
                new SelectListItem() { Text="EMPLOYEE",Value="EMPLOYEE" },
                new SelectListItem() { Text="GANG ADVICE",Value="GANG ADVICE" },
                 new SelectListItem() { Text="GANG NUMBER",Value="GANG NUMBER" },
                 new SelectListItem() { Text="INVOICING",Value="INVOICING" },
                new SelectListItem() { Text="COST SHEET",Value="COST SHEET" },
                new SelectListItem() { Text="TRANSACTION",Value="TRANSACTION" },
                 new SelectListItem() { Text="TIME-AUDIT",Value="TIME-AUDIT" },
                new SelectListItem() { Text="OTHER",Value="OTHER" }
            };
            return sequence_types;
        }
        public PartialViewResult AddSequence()
        {
            ViewBag.SequenceType = SequenceTypes();
            return PartialView("_AddSequence", new MasterSequence());
        }

        public PartialViewResult ManageSequence()
        {
            return PartialView("_ManageSequence", new MasterSequence());
        }

        public JsonResult SaveSequence(MasterSequence model)
        {
            if (string.IsNullOrEmpty(model.SequenceName))
            {
                return Json(new { message = "PLEASE SPECIFY SEQUENCE NAME" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.SequenceType))
            {
                return Json(new { message = "PLEASE SPECIFY SEQUENCE TYPE" }, JsonRequestBehavior.AllowGet);
            }

            if ((model.SequenceNumber) < 0)
            {
                return Json(new { message = "SEQUENCE NUMBER MUST BE GREATER THAN ZERO" }, JsonRequestBehavior.AllowGet);
            }

            if ((model.SequenceLength) <= 0)
            {
                return Json(new { message = "SEQUENCE LENGTH MUST BE GREATER THAN ZERO" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.SequencePrefix))
            {
                model.SequencePrefix = "";
            }

            if (string.IsNullOrEmpty(model.SequenceSuffix))
            {
                model.SequenceSuffix = "";
            }

            model.SequenceName = model.SequenceName.ToUpper();
            model.SequencePrefix = model.SequencePrefix.ToUpper();
            model.SequenceSuffix = model.SequenceSuffix.ToUpper();
            using (RBACDbContext db = new RBACDbContext())
            {
                db.Sequence.Add(model);
                db.SaveChanges();
            }
            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SequenceList()
        {
            RBACDbContext db_ = new RBACDbContext();
            var records = db_.Sequence.Select(c => new
            {
                ID = c.Id,
                Name = c.SequenceName,
                Type = c.SequenceType,
                Prefix = c.SequencePrefix,
                Suffix = c.SequenceSuffix,
                Length = c.SequenceLength
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SequenceManageList()
        {
            RBACDbContext db = new RBACDbContext();
            var records = db.Sequence.Select(c => new
            {
                ID = c.Id,
                Name = c.SequenceName,
                Type = c.SequenceType,
                Prefix = c.SequencePrefix,
                Suffix = c.SequenceSuffix,
                Length = c.SequenceLength
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetDelete(int Id)
        {
            var sequence = db.Sequence.FirstOrDefault(c => c.Id == Id);
            return PartialView("_DeleteSequence", sequence);
        }

        public PartialViewResult GetEdit(int Id)
        {
            var sequence = db.Sequence.FirstOrDefault(c => c.Id == Id);
            ViewBag.SequenceTypes = SequenceTypes();
            return PartialView("_EditSequence", sequence);
        }

        public JsonResult DeleteSequence(int Id)
        {
            using (RBACDbContext db = new RBACDbContext())
            {
                try
                {
                    var sequence = db.Sequence.FirstOrDefault(c => c.Id == Id);
                    db.Sequence.Remove(sequence); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult EditSavedSequence(MasterSequence model)
        {
            using (RBACDbContext db = new RBACDbContext())
            {
                try
                {
                    if (model != null)
                    {
                        var sequence = db.Sequence.FirstOrDefault(c => c.Id == model.Id);
                        sequence.SequenceType = model.SequenceType;
                        sequence.SequenceName = model.SequenceName;
                        sequence.SequenceLength = model.SequenceLength;
                        sequence.SequencePrefix = model.SequencePrefix;
                        sequence.SequenceSuffix = model.SequenceSuffix;
                        db.Entry<MasterSequence>(sequence).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { message = "SEQUENCE NOT FOUND !" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #region REGIONAL CODE
        //public PartialViewResult AddRegional()
        //{
        //    ViewBag.SequenceType = SequenceTypes();
        //    return PartialView("_AddRegionalCode", new RegionalCode());
        //}
        //public JsonResult RegionalCodeList()
        //{
        //    db = new RBACDbContext();
        //    var records = db.RegionalCode.Select(c => new
        //    {
        //        ID = c.Id,
        //        Name = c.Name,
        //        Code = c.Code
        //    }).ToList();
        //    return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<JsonResult> SaveRegional(RegionalCode model)
        //{
        //    var codes = db.RegionalCode.Select(x => x).ToList();

        //    if (string.IsNullOrEmpty(model.Name))
        //    {
        //        return Json(new { message = "PLEASE SPECIFY REGIONAL NAME" }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (string.IsNullOrEmpty(model.Code))
        //    {
        //        return Json(new { message = "PLEASE SPECIFY REGIONAL CODE" }, JsonRequestBehavior.AllowGet);
        //    }

        //    if (codes.Exists(a => a.Code == model.Code))
        //    {
        //        return Json(new { message = "REGIONAL CODE ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
        //    }

        //    using (RBACDbContext db_ = new RBACDbContext())
        //    {
        //        db_.RegionalCode.Add(model);
        //        await db_.SaveChangesAsync();
        //    }
        //    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

    }
}