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
    public class MasterSetupController : Controller
    {
        RBACDbContext db;
        public MasterSetupController()
        {
            db = new RBACDbContext();
        }

        // GET: MasterSetup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContainerIndex()
        {
            return View();
        }

        public ActionResult ContainerList()
        {
            var records = db.VesselContainer.Select(x =>
             new
             {
                 Id = x.Id,
                 Container = x.Continer
             }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddContiner(VesselContainer model)
        {
            var _list = db.VesselContainer.Select(x => x).ToList();
            if (_list.Exists(x => x.Continer.ToLower().Equals(model.Continer.ToLower())))
            {
                return Json(new { message = "THE SPECIFIED CONTAINER ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var container = new VesselContainer() { Continer = model.Continer.ToUpper() };
                db.VesselContainer.Add(container);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteContainer(int Id)
        {
            var record = db.VesselContainer.FirstOrDefault(x => x.Id == Id);
            if (record != null)
            {
                db.VesselContainer.Remove(record);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { message = "CONTAINER NOT FOUND" }, JsonRequestBehavior.AllowGet);

            }
        }

        //GetEditContainer
        public PartialViewResult GetEditContainer(int Id)
        {
            var records = db.VesselContainer.Find(Id);
            return PartialView("_EditContainer", records);
        }
        //
        public ActionResult EditContiner(VesselContainer model)
        {

            try
            {
                var entity = db.VesselContainer.FirstOrDefault(x => x.Id == model.Id);

                if (entity != null)
                {
                    entity.Continer = model.Continer.ToUpper();
                    db.Entry<VesselContainer>(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { message = "CONTAINER NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }


        #region  FIELD LOCATION AREA

        public JsonResult getFieldLocation()
        {
            var records = db.LocationArea.Select(x => new
            {
                Id = x.Id,
                Location = x.Location,
                Lat = x.LocationLat,
                Long = x.LocationLong
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        //FIELD LOCATION INDEX
        public ActionResult FieldLocation()
        {
            return View();
        }

        public JsonResult SaveLocation(FieldLocationArea model)
        {
            try
            {
                var locations = db.LocationArea.Select(x => x).ToList();
                if (locations.Exists(a => a.Location.ToLower().Equals(model.Location.ToLower())))
                {
                    return Json(new { message = "THE SPECIFIED LOCATION DESCRIPTION ALREADY EXIST" }, JsonRequestBehavior.AllowGet);

                }


                db.LocationArea.Add(model); db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult DeleteLocation(int Id)
        {
            try
            {
                //CHECK IF LOCATION EXIST IN A GANG REQUEST
                var locations = db.LocationArea.Find(Id);

                if (locations != null)
                {
                    db.LocationArea.Remove(locations); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { message = "LOCATION DETAILS NOT FOUND" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);

            }
        }

        public  PartialViewResult LocationEditView(int Id)
        {
            var entity = db.LocationArea.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditFieldLocation", entity);
        }

        public JsonResult EditLocation(FieldLocationArea model)
        {
            try
            {
                var locations = db.LocationArea.FirstOrDefault(x => x.Id==model.Id);
                locations.Location = model.Location;
                locations.LocationLat = model.LocationLat;
                locations.LocationLong = model.LocationLong;
                db.Entry<FieldLocationArea>(locations).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
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