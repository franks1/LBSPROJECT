using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
using GetLabourManager.Models;
using GetLabourManager.ViewModel;
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
    public class EmployeeController : Controller
    {
        RBACDbContext db;
        EmployeeHelper employee_helper;
        public EmployeeController()
        {
            db = new RBACDbContext();
            this.employee_helper = new EmployeeHelper(db);
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getData()
        {
            var records = this.employee_helper.getEmployeeList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.VGender = getGender();
            ViewBag.VCategory = getCategory();
            ViewBag.VRelation = getRelation();
            ViewBag.VBranch = getBranches();
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel model)
        {
            ViewBag.VGender = getGender();
            ViewBag.VCategory = getCategory();
            ViewBag.VRelation = getRelation();
            ViewBag.VBranch = getBranches();
            try
            {

                var result_message = employee_helper.ValidateModel(model);
                if (result_message.ContainsValue(false))
                {
                    ModelState.AddModelError("", result_message.FirstOrDefault().Key);
                    return View(model);
                }

                //VALIDATE ENTRY
                HttpPostedFileBase post_result_pix = Request.Files["ImageData"];

                if (post_result_pix.ContentLength > 10)
                {
                    byte[] img_ = ConvertTotByte(post_result_pix);
                    model.ImagePix = img_;
                }

                var result = employee_helper.SaveEmployee(model);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "AN ERROR OCCURED WHEN CREATING EMPLOYEE");
                    return View(model);
                }
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message.ToUpper());
                return View(model);
            }
        }

        [HttpGet]
        public async Task< ActionResult> Edit(int Id)
        {
            var record = await this.employee_helper.FindEmployee(Id);
            ViewBag.VGender = getGender();
            ViewBag.VCategory = getCategory();
            ViewBag.VRelation = getRelation();
            ViewBag.VBranch = getBranches();
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeViewModel model)
        {
            var record = await this.employee_helper.FindEmployee(model.Id);
            ViewBag.VGender = getGender();
            ViewBag.VCategory = getCategory();
            ViewBag.VRelation = getRelation();
            ViewBag.VBranch = getBranches();

            HttpPostedFileBase post_result_pix = Request.Files["ImageData"];

            if (post_result_pix.ContentLength > 10)
            {
                byte[] img_ = ConvertTotByte(post_result_pix);
                model.ImagePix = img_;
            }
            var state = this.employee_helper.EditEmployee(model);
            if (state > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "AN ERROR OCCURED WHEN PERFORMING OPERATION");
                return View(record);
            }

        }


        public List<SelectListItem> getGender()
        {
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="MALE", Value="MALE"},
                new SelectListItem(){ Text="FEMALE", Value="FEMALE"}
            };
            return gender;
        }

        public List<SelectListItem> getRelation()
        {
            List<SelectListItem> relation = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="BROTHER", Value="BROTHER"},
                new SelectListItem(){ Text="SISTER", Value="SISTER"},
                new SelectListItem(){ Text="AUNTY", Value="AUNTY"},
                new SelectListItem(){ Text="NEPHEW", Value="NEPHEW"},
                new SelectListItem(){ Text="NIECE", Value="NIECE"},
                new SelectListItem(){ Text="FATHER", Value="FATHER"},
                new SelectListItem(){ Text="MOTHER", Value="MOTHER"},
                new SelectListItem(){ Text="UNCLE", Value="UNCLE"}
            };
            return relation;
        }

        public List<SelectListItem> getCategory()
        {
            List<SelectListItem> category = new List<SelectListItem>() { };
            var categories = db.EmpCategory.Select(x => x).ToList();
            foreach (var item in categories)
            {
                category.Add(new SelectListItem() { Text = item.Category, Value = item.Id.ToString() });
            }

            return category;
        }

        public List<SelectListItem> getBranches()
        {
            List<SelectListItem> branches = new List<SelectListItem>() { };
            var branch = db.Branch.Select(x => x).ToList();
            foreach (var item in branch)
            {
                branches.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return branches;
        }

        public async Task<ActionResult> GetImage(int Id)
        {
            if (Id == 0)
            {
                return null;
            }

            byte[] img = null;
            var entity = await db.Employee.FindAsync(Id);
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

        byte[] ConvertTotByte(HttpPostedFileBase img)
        {
            byte[] image = null;
            BinaryReader reader = new BinaryReader(img.InputStream);
            image = reader.ReadBytes((int)img.ContentLength);
            return image;
        }
    }
}