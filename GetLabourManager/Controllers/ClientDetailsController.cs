using GetLabourManager.ActionFilters;
using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GetLabourManager.Controllers
{
    [Authorize]
    [RBAC]
    public class ClientDetailsController : Controller
    {
        // GET: ClientDetails
        RBACDbContext db;
        public ClientDetailsController()
        {
            db = new RBACDbContext();
        }
        // GET: ClientInfo
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getData()
        {
            var records = (from x in db.ClientSetup
                           join y in db.Branch on x.Branch equals y.Id
                           select new
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Branch = y.Name,
                               Phone = x.Phone1,
                               Address = x.Address,
                               HeadOffice = x.IsHeadOffice == true ? "YES" : "NO"
                           }).ToList();

            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        byte[] ConvertTotByte(HttpPostedFileBase img)
        {
            byte[] image = null;
            BinaryReader reader = new BinaryReader(img.InputStream);
            image = reader.ReadBytes((int)img.ContentLength);
            return image;
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.VBranch = getBranches();
            return View(new ClientDetails());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientDetails model)
        {
            try
            {
                lock (this)
                {
                    ViewBag.VBranch = getBranches();

                    if (string.IsNullOrEmpty(model.Branch.ToString()))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH NAME");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.Name))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH NAME");
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.Address))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH ADDRESS");
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.EmailAddress))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH PRIMARY EMAIL ADDRESS");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.Phone1))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH PRIMARY PHONE NUMBER");
                        return View(model);
                    }


                    var records = db.ClientSetup.Where(x => x.IsHeadOffice == true).Count();
                    if (records > 0 & model.IsHeadOffice == true)
                    {
                        ModelState.AddModelError("", "THERE ALREADY EXIST A BRANCH AS HEAD OFFICE");
                        return View(model);
                    }

                    HttpPostedFileBase post_result_pix = Request.Files["ImageData"];
                    if (post_result_pix != null)
                    {
                        if (post_result_pix.ContentLength > 10)
                        {
                            byte[] img_ = ConvertTotByte(post_result_pix);
                            model.ImagePix = img_;
                        }
                    }
                    db.ClientSetup.Add(model);
                    db.SaveChanges(); return Redirect("Index");
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int Id)
        {
            var model = await db.ClientSetup.FindAsync(Id);

            lock (this)
            {
                if (model != null)
                {
                    ViewBag.VBranch = getBranches();
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientDetails model)
        {
            ViewBag.VBranch = getBranches();
            try
            {
                lock (this)
                {
                    if (string.IsNullOrEmpty(model.Branch.ToString()))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH NAME");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.Name))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH NAME");
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.Address))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH ADDRESS");
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.EmailAddress))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH PRIMARY EMAIL ADDRESS");
                        return View(model);
                    }

                    if (string.IsNullOrEmpty(model.Phone1))
                    {
                        ModelState.AddModelError("", "PLEASE SPECIFY BRANCH PRIMARY PHONE NUMBER");
                        return View(model);
                    }


                    var records = db.ClientSetup.Where(x => x.IsHeadOffice == true).Count();
                    if (records > 1 & model.IsHeadOffice == true)
                    {
                        ModelState.AddModelError("", "THERE ALREADY EXIST A BRANCH AS HEAD OFFICE");
                        return View(model);
                    }


                    var entity = db.ClientSetup.FirstOrDefault(x => x.Id == model.Id);
                    if (entity != null)
                    {
                        HttpPostedFileBase post_result_pix = Request.Files["ImageData"];
                        if (post_result_pix != null)
                        {
                            if (post_result_pix.ContentLength > 10)
                            {
                                byte[] img_ = ConvertTotByte(post_result_pix);
                                entity.ImagePix = img_;
                            }
                            else
                            {
                                entity.ImagePix = null;
                            }
                        }
                        else
                        {
                            entity.ImagePix = null;
                        }
                    }
                    entity.Name = model.Name; entity.Address = model.Address; entity.Branch = model.Branch;
                    entity.EmailAddress = model.EmailAddress; entity.EmailAddress2 = model.EmailAddress2 ?? "";
                    entity.IsHeadOffice = model.IsHeadOffice; entity.Phone1 = model.Phone1; entity.Phone2 = model.Phone2;
                    db.Entry<ClientDetails>(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> GetImage(int Id)
        {
            if (Id == 0)
            {
                return null;
            }

            byte[] img = null;
            var entity = await db.ClientSetup.FindAsync(Id);
            if (entity != null)
            {
                img = entity.ImagePix ?? null;

                if (img != null)
                {
                    return File(img, "image/jpg");
                }
                else { return null; }
            }

            else { return null; }
        }

        public async Task<JsonResult> DeleteInfo(int Id)
        {
            var entity = await db.ClientSetup.FindAsync(Id);
            lock (entity)
            {
                try
                {
                    db.ClientSetup.Remove(entity);
                    db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception err)
                {
                    return Json(new { message = err.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }



        public List<SelectListItem> getBranches()
        {
            var branches = db.Branch.Select(x => x).ToList();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var item in branches)
            {
                options.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return options;
        }
    }
}