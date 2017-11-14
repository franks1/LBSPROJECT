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
    public class EmployeeCategoryController : Controller
    {
        RBACDbContext db;
        public EmployeeCategoryController()
        {
            db = new RBACDbContext();
        }
        // GET: EmployeeCategory
        public ActionResult Index()
        {
            ViewBag.VCategory = getCategory();
            return View(new EmployeeCategory());
        }
        //
        public ActionResult IndexGroup()
        {
            return View(new EmployeeGroup());
        }
        public JsonResult CategoryList()
        {
            var records = db.EmpCategory.AsEnumerable().Select(x =>
            new
            {
                Id = x.Id,
                Name = x.Category,
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GroupList()
        {
            var records = db.EmployeeGroup.AsEnumerable().Select(x =>
            new
            {
                Id = x.Id,
                Name = x.GroupName,
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClientList()
        {
            var records = db.FieldClient.AsEnumerable().Select(x =>
            new
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult GetEdit(int Id)
        {
            var entity = db.EmpCategory.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditCategory", entity);
        }

        public PartialViewResult GetGroupEdit(int Id)
        {
            var entity = db.EmployeeGroup.FirstOrDefault(x => x.Id == Id);
            return PartialView("_EditGroup", entity);
        }

        public JsonResult DeleteCategory(int Id)
        {
            try
            {
                var entity = db.EmpCategory.FirstOrDefault(x => x.Id == Id);
                db.EmpCategory.Remove(entity);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        //
        public JsonResult AssignGroup(List<EmployeeGroup> Items,int category)
        {
            try
            {
                bool valid_flag = true;
                string name = "";
                var category_group = db.EmployeeGrouping.Where(x=>x.Category==category)
                    .Select(x => x).ToList();
                if (category_group.Count > 0)
                {
                    foreach(var item in Items)
                    {
                        if(category_group.Exists(x => x.Group == item.Id))
                        {
                            valid_flag = false;
                            name = item.GroupName;
                            break;
                        }
                    }
                }

                if (valid_flag == false)
                {
                    return Json(new { message = "THERE ALREADY EXIST "+ name+" IN THIS CATEGORY" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<EmployeeCategoryGrouping> group = new List<EmployeeCategoryGrouping>();
                    foreach (var item in Items)
                    {
                        EmployeeCategoryGrouping emp_group = new EmployeeCategoryGrouping()
                        {
                            Category = category,
                            Group = item.Id
                        };
                        group.Add(emp_group);
                    }

                    db.EmployeeGrouping.AddRange(group); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }


               
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AssignClients(List<JoinedClient> Items, int category)
        {
            try
            {
                bool valid_flag = true;
                string name = "";
                var category_group = db.GangClientGrouping.Where(x => x.Category == category)
                    .Select(x => x).ToList();
                if (category_group.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        if (category_group.Exists(x => x.Client == item.Id))
                        {
                            valid_flag = false;
                            name = item.Name;
                            break;
                        }
                    }
                }

                if (valid_flag == false)
                {
                    return Json(new { message = "THERE ALREADY EXIST " + name + " IN THIS CATEGORY" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<EmployeeClientGroup> group = new List<EmployeeClientGroup>();
                    foreach (var item in Items)
                    {
                        EmployeeClientGroup emp_group = new EmployeeClientGroup()
                        {
                            Category = category,
                            Client = item.Id
                        };
                        group.Add(emp_group);
                    }

                    db.GangClientGrouping.AddRange(group); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteGroup(int Id)
        {
            try
            {
                var entity = db.EmployeeGroup.FirstOrDefault(x => x.Id == Id);
                db.EmployeeGroup.Remove(entity);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        //SaveBranch
        public JsonResult SaveCategory(EmployeeCategory model)
        {
            try
            {
                var category = db.EmpCategory.Select(x => x).ToList();
                if (string.IsNullOrWhiteSpace(model.Category))
                {
                    return Json(new { message = "CATEGORY NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                if (category.Exists(c => c.Category.ToLower().Equals(model.Category.ToLower())))
                {
                    return Json(new { message = "CATEGORY ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
                }

                db.EmpCategory.Add(model);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveGroup(EmployeeGroup model)
        {
            try
            {
                var category = db.EmployeeGroup.Select(x => x).ToList();
                if (string.IsNullOrWhiteSpace(model.GroupName))
                {
                    return Json(new { message = "GROUP DESCRIPTION NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                if (category.Exists(c => c.GroupName.ToLower().Equals(model.GroupName.ToLower())))
                {
                    return Json(new { message = "GROUP NAME ALREADY EXIST" }, JsonRequestBehavior.AllowGet);
                }

                db.EmployeeGroup.Add(model);
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditGroup(EmployeeGroup model)
        {
            try
            {
                var branches = db.EmployeeGroup.FirstOrDefault(x => x.Id == model.Id);
                if (string.IsNullOrWhiteSpace(model.GroupName))
                {
                    return Json(new { message = "GROUP NAME NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                branches.GroupName = model.GroupName;
                db.Entry<EmployeeGroup>(branches).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditCategory(EmployeeCategory model)
        {
            try
            {
                var branches = db.EmpCategory.FirstOrDefault(x => x.Id == model.Id);
                if (string.IsNullOrWhiteSpace(model.Category))
                {
                    return Json(new { message = "CATEGORY NAME NOT SPECIFIED" }, JsonRequestBehavior.AllowGet);
                }
                branches.Category = model.Category;
                db.Entry<EmployeeCategory>(branches).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public List<SelectListItem> getCategory()
        {
            List<SelectListItem> item_group = new List<SelectListItem>();
            var records = db.EmpCategory.Select(x => x).ToList();
            foreach (var item in records)
            {
                item_group.Add(new SelectListItem() { Text = item.Category, Value = item.Id.ToString() });
            }
            return item_group;
        }



        public JsonResult getAssignedGroups(int category)
        {
            var records = (from a in db.EmpCategory
                          join b in db.EmployeeGrouping on a.Id equals b.Category
                          join c in db.EmployeeGroup on b.Group equals c.Id
                          where b.Category==category
                          select new
                          {
                              Id = b.Id,
                              Name = c.GroupName
                          }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAssignedClients(int category)
        {
            var records = (from a in db.EmpCategory
                           join b in db.GangClientGrouping on a.Id equals b.Category
                           join c in db.FieldClient on b.Client equals c.Id
                           where b.Category == category
                           select new
                           {
                               Id = b.Id,
                               Name = c.Name
                           }).ToList();
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteJoinedGroup(int Id)
        {
            var record = db.EmployeeGrouping.FirstOrDefault(x => x.Id == Id);
            try
            {
                if (record != null)
                {
                    db.EmployeeGrouping.Remove(record); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "GROUP NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult DeleteJoinedClient(int Id)
        {
            var record = db.GangClientGrouping.FirstOrDefault(x => x.Id == Id);
            try
            {
                if (record != null)
                {
                    db.GangClientGrouping.Remove(record); db.SaveChanges();
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "CLIENT NOT FOUND" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message.ToUpper() }, JsonRequestBehavior.AllowGet);

            }
        }

        public class JoinedClient
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}