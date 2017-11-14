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
    public class ManageFieldClientController : Controller
    {
        RBACDbContext db;
        public ManageFieldClientController()
        {
            this.db = new RBACDbContext();
        }

        // GET: ManageFieldClient
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getFieldClient()
        {
            var records = db.FieldClient.Select(x => x).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View(new FieldClients());
        }

        public PartialViewResult GetEdit(int Id)
        {
            var entity = db.FieldClient.FirstOrDefault(X => X.Id == Id);
            return PartialView("_EditClient", entity);
        }

        public JsonResult Save(FieldClients model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace((model.Name)))
                {
                    return Json(new { message = "NAME HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.Address)))
                {
                    return Json(new { message = "ADDRESS HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.EmailAddress)))
                {
                    return Json(new { message = "EMAIL ADDRESS HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.Telephone1)))
                {
                    return Json(new { message = "PRIMARY TELEPHONE NUMBER HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }


                if (model.Premium <= 0)
                {
                    return Json(new { message = "PREMIUM CHARGE MUST BE >0.00" }, JsonRequestBehavior.AllowGet);
                }
                FieldClients entity = new FieldClients()
                {
                    Name = model.Name.ToUpper(),
                    Address = model.Address,
                    Telephone1 = model.Telephone1,
                    Telephone2 = model.Telephone2 ?? "",
                    EmailAddress = model.EmailAddress ?? "",
                    FieldClientType = model.FieldClientType ?? "",
                    Status = model.Status ?? ""
                    ,
                    Premium = model.Premium
                };
                db.FieldClient.Add(entity);
                db.SaveChanges();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        //

        public JsonResult EditClient(FieldClients model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace((model.Name)))
                {
                    return Json(new { message = "NAME HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.Address)))
                {
                    return Json(new { message = "ADDRESS HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.EmailAddress)))
                {
                    return Json(new { message = "EMAIL ADDRESS HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace((model.Telephone1)))
                {
                    return Json(new { message = "PRIMARY TELEPHONE NUMBER HAS NOT BEEN SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }

                if (model.Premium <= 0)
                {
                    return Json(new { message = "PREMIUM CHARGE MUST BE >0.00" }, JsonRequestBehavior.AllowGet);
                }

                FieldClients entity = db.FieldClient.FirstOrDefault(x => x.Id == model.Id);
                entity.Name = model.Name.ToUpper();
                entity.Address = model.Address.ToUpper();
                entity.Telephone1 = model.Telephone1;
                entity.Telephone2 = model.Telephone2 ?? "";
                entity.EmailAddress = model.EmailAddress ?? "";
                entity.Status = model.Status ?? "";
                entity.FieldClientType = model.FieldClientType ?? "";
                entity.Premium = model.Premium;
                db.Entry<FieldClients>(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteClient(int Id)
        {
            try
            {

                FieldClients entity = db.FieldClient.FirstOrDefault(x => x.Id == Id);

                db.FieldClient.Remove(entity);
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