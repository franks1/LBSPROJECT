using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GetLabourManager.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
//using LManager.ViewModels;
using GetLabourManager.ActionFilters;
using Microsoft.AspNet.Identity;
using System.Web.Routing;
using System.Reflection;
using GetLabourManager.ViewModel;
using GridMvc;
using GetLabourManager;

namespace LManager.Controllers
{
    [Authorize]
    [RBAC]
    public class AdminController : Controller
    {
        RBACDbContext database;
        ApplicationRoleManager RoleManager;
        //string CURRENTPASSWORD = "";
        public AdminController()
        {
            database = new RBACDbContext();
            RoleManager = new ApplicationRoleManager
                (
                new ApplicationRoleStore(database)
                );
        }
        // GET: Admin

        public ActionResult Index()
        {
            var records = ApplicationUserManager.GetUsers();
            List<ApplicationUserViewModel> model = new List<ApplicationUserViewModel>();

            foreach (var item in records)
            {
                var role_ = item.Roles.FirstOrDefault();
                if (role_ != null)
                {
                    ApplicationUserViewModel m = new ApplicationUserViewModel();
                    var MainRole = ApplicationRoleManager.GetRole(role_.RoleId);

                    m.UserName = item.UserName;
                    m.Email = item.Email;
                    m.Firstname = item.Firstname;
                    m.Lastname = item.Lastname;
                    m.Id = item.Id;
                    m.RoleName = MainRole.Name;
                    m.Role = role_.RoleId.ToString();
                    model.Add(m);
                }
                else
                {
                    ApplicationUserViewModel m = new ApplicationUserViewModel();
                    m.UserName = item.UserName;
                    m.Email = item.Email;
                    m.Firstname = item.Firstname;
                    m.Lastname = item.Lastname;
                    m.Id = item.Id;
                    m.RoleName = "N/A";
                    m.Role = "";
                    model.Add(m);
                }
            }

            //  List<ApplicationUserViewModel> models = model.ToList();
            return View(model.ToList());
        }

        public List<SelectListItem> GetUserRole()
        {
            var roles = ApplicationRoleManager.GetRoles4SelectList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in roles)
            {
                SelectListItem combo = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };
                items.Add(combo);
            }
            return items;
        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Name = GetUserRole();
            return View(new ApplicationUserViewModel());
        }

        [HttpPost]

        public ActionResult CreateUser(ApplicationUserViewModel user)
        {
            ViewBag.Name = GetUserRole();
            if (user.UserName == "" || user.UserName == null)
            {
                ModelState.AddModelError("Error", "Username cannot be blank");
                return View(user);
            }

            if (user.Firstname == "" || user.Firstname == null)
            {
                ModelState.AddModelError("Error", "FirstName cannot be blank");
                return View(user);
            }

            if (user.Lastname == "" || user.Lastname == null)
            {
                ModelState.AddModelError("Error", "LastName cannot be blank");
                return View(user);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //List<string> results = database.Database.SqlQuery<String>(string.Format("SELECT Username FROM USERS WHERE Username = '{0}'", user.UserName)).ToList();
                    var result = ApplicationUserManager.GetUsers4SelectList().
                        Where(c => c.UserName.Equals(user.UserName)).ToList();
                    bool _userExistsInTable = (result.Count > 0);

                    int roleId = int.Parse(user.Role);


                    if (_userExistsInTable == true)
                    {
                        // ViewBag.Name = GetUserRole();
                        ModelState.AddModelError("Error", "User name already exist");
                        return View(user);
                    }
                    else
                    {
                        ApplicationUserManager UserManager =
                        new ApplicationUserManager(new ApplicationUserStore(database));

                        //   var user_model = user;
                        ApplicationUser user_model = new ApplicationUser()
                        {
                            UserName = user.UserName,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Email = "",
                            LastModified = DateTime.Now,
                            Inactive = true,
                            EmailConfirmed = false
                        };
                        UserManager.Create(user_model, user.Password);

                        var prior_user = ApplicationUserManager.GetUsers().FirstOrDefault(c => c.UserName.Equals(user.UserName));
                        int userId = prior_user.Id;
                        ApplicationUserManager.AddUser2Role(userId, roleId);

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Error", error.Message);

                return View(user);
            }
            return View(user);
        }
        [HttpGet]

        public ActionResult EditUser(int Id)
        {
            ApplicationUser user = ApplicationUserManager.GetUser(Id);
            ApplicationUserViewModel model = new ApplicationUserViewModel()
            {
                Id = user.Id,
                Password = "",
                Email = user.Email,
                Firstname = user.Firstname ?? "",
                Lastname = user.Lastname ?? "",
                Inactive = user.Inactive,
                UserStatus = user.Inactive == true ? "ACTIVE" : "INACTIVE",
                Role = "",
                Roles = user.Roles.ToList(),
                UserName = user.UserName
            };
            ViewBag.UStatus = getStatus();
            SetViewBagData(Id);
            return View(model);
        }

        public ActionResult EditUser(ApplicationUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id.ToString()))
            {
                ModelState.AddModelError(string.Empty, "Invalid User");
            }

            ApplicationUser user = ApplicationUserManager.GetUser(model.Id);
            if (user != null)
            {
                try
                {
                    var home = ApplicationUserManager.UpdateUser(model);
                    if (home == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "An error occured when Editing user's details");
                    return RedirectToAction("EditUser", new RouteValueDictionary(new { id = model.Id }));

                }
                catch (Exception err)
                {
                    ModelState.AddModelError(string.Empty, err.Message);
                    ViewBag.UStatus = getStatus();
                    SetViewBagData(model.Id);
                    return View(model);
                }
            }
            ViewBag.UStatus = getStatus();
            SetViewBagData(model.Id);
            return View(model);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public PartialViewResult DeleteUserRoleReturnPartialView(int id, int userId)
        //{
        public PartialViewResult DeleteUserRole(int id, int userId)
        {
            ApplicationUserManager.RemoveUser4Role(userId, id);
            return GetRolesForUser(userId);
        }

        [HttpGet]
        public ActionResult DeleteUser(string Id, string RoleId)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("Index");
            }

            var user = ApplicationUserManager.GetUser(int.Parse(Id));
            var role_ = user.Roles.FirstOrDefault();
            ApplicationUserViewModel model = new ApplicationUserViewModel();

            if (role_ != null)
            {
                var MainRole = ApplicationRoleManager.GetRole(role_.RoleId);

                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Firstname = user.Firstname;
                model.Lastname = user.Lastname;
                model.Id = user.Id;
                model.RoleName = MainRole.Name;
                model.Role = role_.RoleId.ToString();
            }
            else
            {
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Firstname = user.Firstname;
                model.Lastname = user.Lastname;
                model.Id = user.Id;
                model.RoleName = "";
                model.Role = "";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(ApplicationUserViewModel user)
        {
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Role))
                {
                    ApplicationUserManager.RemoveUser4Role(user.Id, int.Parse(user.Role));
                }
                ApplicationUserManager.DeleteUser(user.Id);
            }
            return RedirectToAction("Index");
        }


        public PartialViewResult GetRolesForUser(int userId)
        {
            ApplicationUser user = ApplicationUserManager.GetUser(userId);
            ApplicationUserViewModel model = new ApplicationUserViewModel()
            {
                Id = user.Id,
                Password = "",
                Email = user.Email,
                Firstname = user.Firstname ?? "",
                Lastname = user.Lastname ?? "",
                Inactive = user.Inactive,
                UserStatus = user.Inactive == true ? "ACTIVE" : "INACTIVE",
                Role = "",
                Roles = user.Roles.ToList(),
                UserName = user.UserName
            };
            return PartialView("_ListUserRoleTable", model);
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserRoleReturnPartialView(int id, int userId)
        {
            ApplicationUserManager.AddUser2Role(userId, id);
            SetViewBagData(userId);
            // return PartialView("_ListUserRoleTable", ApplicationUserManager.GetUser(userId));
            return GetRolesForUser(userId);
        }
        private void SetViewBagData(int _userId)
        {
            ViewBag.UserId = _userId;
            ViewBag.RoleId = new SelectList(database.Roles.OrderBy(p => p.Name), "Id", "Name");
        }

        [HttpPost]
        public ActionResult UserDetails(ApplicationUser user)
        {
            if (user.UserName == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid USER Name");
            }

            if (ModelState.IsValid)
            {
                database.Entry(user).Entity.Inactive = user.Inactive;
                database.Entry(user).Entity.LastModified = System.DateTime.Now;
                database.Entry(user).State = EntityState.Modified;
                database.SaveChanges();
            }
            return View(user);
        }

        public List<SelectListItem> getStatus()
        {
            List<SelectListItem> select_item = new List<SelectListItem>()
            {
            new SelectListItem() { Text = "ACTIVE", Value = "ACTIVE" },
            new SelectListItem() { Text = "INACTIVE", Value = "INACTIVE" }
        };
            return select_item;
        }

        public ActionResult RoleIndex()
        {
            var roles = ApplicationRoleManager.GetRoles();
            return View(roles.Select(c => c).ToList());
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            ApplicationRole role = new ApplicationRole();
            // role.LastModified = DateTime.Now;
            return View(role);
        }

        [HttpPost]

        public ActionResult AddRole(ApplicationRole role)
        {
            var roles = ApplicationRoleManager.GetRoles4SelectList();

            if (roles.Exists(c => c.Name.ToLower() == role.Name.ToLower()))
            {
                ModelState.AddModelError(string.Empty, "The Specified Role Name  Already Exist !");
                return View(role);
            }

            if (ModelState.IsValid)
            {
                role.LastModified = DateTime.Now;
                RoleManager.Create(role);
                return RedirectToAction("RoleIndex");
            }
            return View(role);
        }

        public List<PERMISSION> ValidPermissions(ApplicationRole _role, int RoleId)
        {
            var records = database.PERMISSIONS.Select(c => c).ToList();
            List<PERMISSION> permisisons = new List<PERMISSION>();
            foreach (var item in _role.PERMISSIONS)
            {
                if (records.Exists(a => a.PermissionId == item.PermissionId))
                {
                    var p = records.FirstOrDefault(x => x.PermissionId == item.PermissionId);
                    records.Remove(p);
                }
            }
            return records;
        }

        public JsonResult showRolePermissions(int RoleId)
        {
            var records = RolePermissionsList(RoleId);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        public List<PermissionViewModel> RolePermissionsList(int RoleId)
        {
            ApplicationRole _role = ApplicationRoleManager.GetRole(RoleId);

            var records = database.PERMISSIONS.Select(c => c).ToList();
            List<PermissionViewModel> permissions = new List<PermissionViewModel>();
            foreach (var item in _role.PERMISSIONS)
            {
                if (records.Exists(a => a.PermissionId == item.PermissionId))
                {
                    PermissionViewModel permission = new PermissionViewModel()
                    {
                        PermissionId = item.PermissionId,
                        PermissionDescription = item.PermissionDescription
                    };
                    permissions.Add(permission);
                }

            }
            return permissions;
        }


        public List<PermissionViewModel> ValidPermissions(int RoleId)
        {
            ApplicationRole _role = ApplicationRoleManager.GetRole(RoleId);

            var records = database.PERMISSIONS.Select(c => c).ToList();
            List<PermissionViewModel> permissions = new List<PermissionViewModel>();
            foreach (var item in records)
            {
                PermissionViewModel permission = new PermissionViewModel()
                {
                    PermissionId = item.PermissionId,
                    PermissionDescription = item.PermissionDescription
                };
                permissions.Add(permission);
            }

            return permissions;
        }

        //  [HttpGet]
        //  [ActionName("EditRole")]

        //  [RBAC]
        public ActionResult ManageRole(int id)
        {
            ApplicationRole _role = ApplicationRoleManager.GetRole(id);
            ViewBag.Permissions = ValidPermissions(_role, id);

            return View(_role);
        }

        [HttpPost]
        // [ActionName("EditRole")]
        public ActionResult ManageRole(ApplicationRole _role)
        {

            if (string.IsNullOrEmpty(_role.RoleDescription))
            {
                ModelState.AddModelError("", "Role Description must be entered"); return View(_role);
            }

            if (string.IsNullOrEmpty(_role.Name))
            {
                ModelState.AddModelError("", "Role must be entered"); return View(_role);
            }

            if (ModelState.IsValid)
            {
                RoleViewModel model = new RoleViewModel();
                model.Id = _role.Id;
                model.IsSysAdmin = _role.IsSysAdmin;
                model.Name = _role.Name;
                model.RoleDescription = _role.RoleDescription;

                if (ApplicationRoleManager.UpdateRole(model))
                    //return RedirectToAction("RoleDetails", new RouteValueDictionary(new { id = _role.Id }));
                    return RedirectToAction("RoleIndex");
            }
            ViewBag.Permissions = ValidPermissions(_role, _role.Id);
            return View(_role);

        }

        public JsonResult getPermissionsList()
        {
            var _permissions = database.PERMISSIONS.Select(x => new
            {
                Id = x.PermissionId,
                Permission = x.PermissionDescription
            }).OrderBy(x=>x.Permission).ToList();
            return Json(new { data = _permissions }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult PermissionIndex(string sort, string searchValue, int? page)
        {
            // ViewBag.NumberSort = string.IsNullOrEmpty(sort) ? "number_desc" : string.Empty;
            ViewBag.PermissionSort = string.IsNullOrEmpty(sort) ? "permission_desc" : string.Empty;
            //PermissionDescription  ViewBag.LastNameSort = sort == "lastname" ? "lastname_desc" : "lastname";

            ViewBag.CurrentSort = "";
            ViewBag.SearchValue = searchValue;


            IQueryable<PERMISSION> _permissions = database.PERMISSIONS;
            //.Include(a => a.ROLES).ToList();


            if (!string.IsNullOrEmpty(searchValue))
            {
                _permissions = _permissions.
                    Where(c => c.PermissionDescription.ToLower()
                    .StartsWith(searchValue.ToLower()));
            }

            switch (sort)
            {
                case "permission_desc":
                    _permissions = _permissions.OrderByDescending(c => c.PermissionDescription);
                    break;
                default:
                    _permissions = _permissions.OrderBy(c => c.PermissionDescription);
                    break;
            }
            int pageSize = 20;
            int pageNumber = page ?? 1;

            return View(_permissions.ToPagedList(pageNumber, pageSize));

            //return View(_permissions);
        }
        [HttpGet]

        public ActionResult PermissionCreate()
        {
            return View(new PERMISSION());
        }
        [HttpPost]

        public ActionResult PermissionCreate(PERMISSION permission)
        {
            if (permission.PermissionDescription == null)
            {
                ModelState.AddModelError(string.Empty, "Please specify Permission Description.");
                return View(permission);
            }
            database.PERMISSIONS.Add(permission);
            database.SaveChanges();
            return RedirectToAction("PermissionIndex");
        }
        [HttpGet]

        public ActionResult PermissionEdit(int Id)
        {
            PERMISSION permission = database.PERMISSIONS.Find(Id);
            //database.Roles.Select(x=>x.n)
            ViewBag.RoleId = new SelectList(database.Roles.OrderBy(p => p.RoleDescription), "Id", "Name");
            ViewBag.Name = GetUserRole();
            return View(permission);
        }
        [HttpPost]

        public ActionResult PermissionEdit(PERMISSION permission)
        {
            if (ModelState.IsValid)
            {
                database.Entry(permission).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("PermissionDetails",
                new RouteValueDictionary(new { id = permission.PermissionId }));
            }
            ViewBag.RoleId = new SelectList(database.Roles.OrderBy(p => p.RoleDescription), "Id", "Name");
            ViewBag.Name = GetUserRole();
            return View(permission);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]

        public ActionResult PermissionDelete(int id)
        {
            PERMISSION permission = database.PERMISSIONS.Find(id);
            if (permission.ROLES.Count > 0)
                permission.ROLES.Clear();

            database.Entry(permission).State = EntityState.Deleted;
            database.SaveChanges();
            return RedirectToAction("PermissionIndex");
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermission(int id, int permissionId)
        {
            ApplicationRole role = database.Roles.Find(id);
            PERMISSION permission = database.PERMISSIONS.Find(permissionId);

            if (role.PERMISSIONS.Contains(permission))
            {
                role.PERMISSIONS.Remove(permission);
                database.SaveChanges();
            }
            return PartialView("_ListRolesTableForPermission", permission);
        }
        public PartialViewResult ResetView(int permissionId)
        {
            PERMISSION permission = database.PERMISSIONS.Find(permissionId);
            return PartialView("_ListRolesTableForPermission", permission);
        }

        public PartialViewResult GetRolesAfterDelete(int permissionId)
        {
            PERMISSION permission = database.PERMISSIONS.Find(permissionId);
            if (permission != null)
            {
                return PartialView("_ListRolesTableForPermission", permission);
            }
            else
                return PartialView("_ListRolesTableForPermission", new PERMISSION());
        }

        public JsonResult AssignRoleFromView(int roleId, int permissionId)
        {
            ApplicationRole role = database.Roles.Find(roleId);
            PERMISSION _permission = database.PERMISSIONS.Find(permissionId);
            if (role.PERMISSIONS == null)
            {
                role.PERMISSIONS.Add(_permission);
                database.SaveChanges();
            }
            else if (!role.PERMISSIONS.Contains(_permission))
            {
                role.PERMISSIONS.Add(_permission);
                database.SaveChanges();
            }
            else
            {
                return Json(new { message = "Permission Has Already Been Assigned To This Role !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRolesForSelectedUser(int Id)
        {
            var result = (from a in ApplicationUserManager.GetUser(Id).Roles
                          join b in database.Roles on a.RoleId equals b.Id
                          select new
                          {
                              Name = b.Name,
                              RoleId = b.Id
                          }).ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        // 


        public JsonResult RemoveRoleForUser(int UserId, int RoleId)
        {
            var result = ApplicationUserManager.RemoveUser4Role(UserId, RoleId);
            return Json(new { message = result == true ? "success" : "failed" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddRoleForUser(int UserId, int RoleId)
        {
            var result = ApplicationUserManager.AddUser2Role(UserId, RoleId);
            return Json(new { message = result == true ? "success" : "failed" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeletePermissionFromRole(int roleId, int permissionId)
        {
            ApplicationRoleManager.RemovePermission4Role(roleId, permissionId);
            return Json(new { message = "Saved" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetails(PermissionDeleteModel data)
        {
            PermissionDeleteModel model = new PermissionDeleteModel()
            {
                PermissionId = data.PermissionId,
                RoleId = data.RoleId,
                Role = ""
            };
            return PartialView("_DeletePermissionRole", model);
        }
        public ActionResult GetDetailsRole(UserRoleDeleteModel data)
        {
            UserRoleDeleteModel model = new UserRoleDeleteModel()
            {
                UserId = data.UserId,
                RoleId = data.RoleId
            };
            return PartialView("_DeleteUserRole", model);
        }
        public JsonResult GetDeleteView(int permissionId)
        {
            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PermissionsImport()
        {
            try
            {
                var _controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .Where(t => t != null
                                    && t.IsPublic
                                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                                    && !t.IsAbstract
                                    && typeof(IController).IsAssignableFrom(t));

                var _controllerMethods = _controllerTypes.ToDictionary(controllerType => controllerType,
                        controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));

                foreach (var _controller in _controllerMethods)
                {
                    string _controllerName = _controller.Key.Name;
                    foreach (var _controllerAction in _controller.Value)
                    {
                        if (_controllerAction.ReturnType.Name != "JsonResult")
                        {
                            string _controllerActionName = _controllerAction.Name;
                            if (_controllerName.EndsWith("Controller"))
                            {
                                _controllerName = _controllerName.Substring(0, _controllerName.LastIndexOf("Controller"));
                            }

                            string _permissionDescription = string.Format("{0}-{1}", _controllerName, _controllerActionName);
                            PERMISSION _permission = database.PERMISSIONS.Where(p => p.PermissionDescription == _permissionDescription).FirstOrDefault();
                            if (_permission == null)
                            {
                                if (ModelState.IsValid)
                                {
                                    PERMISSION _perm = new PERMISSION();
                                    _perm.PermissionDescription = _permissionDescription;

                                    database.PERMISSIONS.Add(_perm);
                                    database.SaveChanges();

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("PermissionIndex");
            }
            return RedirectToAction("PermissionIndex");
        }
        public ViewResult PermissionDetails(int id)
        {
            PERMISSION _permission = database.PERMISSIONS.Find(id);
            return View(_permission);
        }

        //public PartialViewResult GetAllPermissions(int RoleId)
        //{
        //    var records = ValidPermissions(RoleId);
        //    return PartialView("_AddPermissionToRole", records);
        //}

        public JsonResult getListPermissions(int RoleId)
        {
            var records = ValidPermissions(RoleId);
            return Json(new { data = records }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult AppliedRolePermissions(int RoleId)
        {
            ApplicationRole role = ApplicationRoleManager.GetRole(RoleId);
            return PartialView("_ListPermissionsForRole", role);
        }

        public ActionResult ChangePassword()
        {
            ApplicationUser user_model = ApplicationUserManager.GetUsers().FirstOrDefault
                (c => c.UserName.ToLower().Equals(User.Identity.Name.ToLower()));
            ApplicationUserViewModel model = new ApplicationUserViewModel()
            {
                UserName = user_model.UserName,
                CurrentPassword = ""
            };
            ViewBag.SetView = "none";
            ViewBag.SetMain = "normal";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ChangePassword(ApplicationUserViewModel user_model)
        {
            if (string.IsNullOrEmpty(user_model.Password))
            {
                ModelState.AddModelError("", "PLEASE SPECIFY PASSWORD.");
                ViewBag.SetView = "normal";
                ViewBag.SetMain = "none";
                user_model.UserName = user_model.UserName;
                ViewBag.Current = user_model.CurrentPassword;
                return View(user_model);
            }

            if (string.IsNullOrEmpty(user_model.ConfirmPassword))
            {
                ModelState.AddModelError("", "PLEASE SPECIFY CONFIRMATION PASSWORD.");
                ViewBag.SetView = "normal";
                ViewBag.SetMain = "none";
                user_model.UserName = user_model.UserName;
                ViewBag.Current = user_model.CurrentPassword;
                return View(user_model);
            }

            if ((user_model.ConfirmPassword != user_model.Password))
            {
                ModelState.AddModelError("", "PASSWORD MISMATCH !");
                ViewBag.SetView = "normal";
                ViewBag.SetMain = "none";
                user_model.UserName = user_model.UserName;
                ViewBag.Current = user_model.CurrentPassword;
                return View(user_model);
            }


            try
            {
                ApplicationUserManager manager = new ApplicationUserManager(new ApplicationUserStore(database));
                ApplicationUser user_edit = ApplicationUserManager.GetUsers().FirstOrDefault
                   (c => c.UserName.ToLower().Equals(user_model.UserName.ToLower()));

                manager.ChangePassword(user_edit.Id, user_model.CurrentPassword, user_model.Password);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Cache.SetNoStore();

                //  AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                //return RedirectToAction("Login", "Account");
                return Redirect("/Account/Login");
            }
            catch (Exception err)
            {
                ModelState.AddModelError("", err.Message.ToUpper());
                ViewBag.SetView = "normal";
                ViewBag.SetMain = "none";
                user_model.UserName = user_model.UserName;
                ViewBag.Current = user_model.CurrentPassword;
                return View(user_model);
            }
        }

        public JsonResult VerifyPassword(string userName, string currentPassword)
        {
            ApplicationUser user = ApplicationUserManager.GetUsers().FirstOrDefault
                  (c => c.UserName.ToLower().Equals(userName.ToLower()));

            ApplicationUserManager manager = new ApplicationUserManager(new ApplicationUserStore(database));

            var isValid = manager.CheckPassword(user, currentPassword);

            return Json(new { message = isValid }, JsonRequestBehavior.AllowGet);
        }
    }
}