using GetLabourManager.ActionFilters;
using GetLabourManager.Helper;
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
    public class HomeController : Controller
    {
        RBACDbContext db;
        DashBoardHelper helper;
        public HomeController()
        {
            db = new RBACDbContext();
            helper = new DashBoardHelper(db);
        }
        
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult HeaderDetails()
        {
            var record = helper.getHeaderSummary();
            return Json(new { data = record }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> getMonthlyGangRequest()
        {
            var record =await helper.GangAdviceSummary();
            return Json(new { data = record }, JsonRequestBehavior.AllowGet);
        }

    }
}