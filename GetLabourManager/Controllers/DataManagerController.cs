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
    public class DataManagerController : Controller
    {
        RBACDbContext db;
        DataProcessingHelper helper;
        public DataManagerController()
        {
            db = new RBACDbContext();
            helper = new DataProcessingHelper(db);
        }
        // GET: DataManager
        public ActionResult Index()
        {
            if (TempData.Count > 0)
            {
                var obj1 = TempData.FirstOrDefault(x => x.Key == "0");
                var obj2 = TempData.FirstOrDefault(x => x.Key == "1");
                var obj3 = TempData.FirstOrDefault(x => x.Key == "2");
                var obj4 = TempData.FirstOrDefault(x => x.Key == "3");
                var obj5 = TempData.FirstOrDefault(x => x.Key == "4");
                var obj6 = TempData.FirstOrDefault(x => x.Key == "5");
                var obj7 = TempData.FirstOrDefault(x => x.Key == "6");


                if (!string.IsNullOrEmpty(obj1.Key))
                {
                    ViewBag.ResponseMessage = TempData.FirstOrDefault(x => x.Key == "0").Value.ToString();
                }
                if (!string.IsNullOrEmpty(obj2.Key))
                {
                    ViewBag.ResponseMessage2 = TempData.FirstOrDefault(x => x.Key == "1").Value.ToString();
                }
                if (!string.IsNullOrEmpty(obj3.Key))
                {
                    ViewBag.ResponseMessage3 = TempData.FirstOrDefault(x => x.Key == "2").Value.ToString();
                }
                if (!string.IsNullOrEmpty(obj4.Key))
                {
                    ViewBag.ResponseMessage4 = TempData.FirstOrDefault(x => x.Key == "3").Value.ToString();
                }
                if (!string.IsNullOrEmpty(obj5.Key))
                {
                    ViewBag.ResponseMessage4 = TempData.FirstOrDefault(x => x.Key == "4").Value.ToString();
                }

                if (!string.IsNullOrEmpty(obj6.Key))
                {
                    ViewBag.ResponseMessage6 = TempData.FirstOrDefault(x => x.Key == "5").Value.ToString();
                }

                if (!string.IsNullOrEmpty(obj7.Key))
                {
                    ViewBag.ResponseMessage7 = TempData.FirstOrDefault(x => x.Key == "6").Value.ToString();
                }
            }
            else
            {
                ViewBag.ResponseMessage = "";
                ViewBag.ResponseMessage2 = "";
                ViewBag.ResponseMessage3 = "";
                ViewBag.ResponseMessage4 = "";
                ViewBag.ResponseMessage5 = "";
                ViewBag.ResponseMessage6 = "";
                ViewBag.ResponseMessage7 = "";
            }

            return View();
        }

        [HttpPost]
        public ActionResult ImportCasuals(HttpPostedFileBase posted_data)
        {

            //  HttpPostedFileBase post_result_document = Request.Files["posted_data"];
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("0", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();

                TempData.Add("0", "No Data Found !");

                return RedirectToAction("Index");
            }


            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            posted_data.SaveAs(path);
            var result = helper.ExtractCasualsFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("0", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("0", "File import Failed");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult ImportGuarantor(HttpPostedFileBase posted_data)
        {

            //  HttpPostedFileBase post_result_document = Request.Files["posted_data"];
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("1", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("1", "No Data Found !");
                return RedirectToAction("Index");
            }


            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            posted_data.SaveAs(path);
            var result = helper.ExtractGuarantorFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("1", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("1", "File import Failed");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult ImportNextOfKin(HttpPostedFileBase posted_data)
        {

            //  HttpPostedFileBase post_result_document = Request.Files["posted_data"];
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("2", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("2", "No Data Found !");
                return RedirectToAction("Index");
            }


            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            posted_data.SaveAs(path);
            var result = helper.ExtractNextOfKinFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("2", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("2", "File import Failed");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult ImportContribution(HttpPostedFileBase posted_data)
        {

            //  HttpPostedFileBase post_result_document = Request.Files["posted_data"];
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("2", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("3", "No Data Found !");
                return RedirectToAction("Index");
            }


            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            posted_data.SaveAs(path);
            var result = helper.ExtractContributionFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("3", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("3", "File import Failed");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult ImportFieldClient(HttpPostedFileBase posted_data)
        {
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("4", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("4", "No Data Found !");
                return RedirectToAction("Index");
            }

            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            posted_data.SaveAs(path);
            var result = helper.ExtractFieldClientFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("4", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("4", "File import Failed");
                return RedirectToAction("Index");
            }

        }

        //
        [HttpPost]
        public ActionResult ImportGangType(HttpPostedFileBase posted_data)
        {
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("5", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("5", "No Data Found !");
                return RedirectToAction("Index");
            }

            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            posted_data.SaveAs(path);
            var result = helper.ExtractGangTypeFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("5", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("5", "File import Failed");
                return RedirectToAction("Index");
            }
        }

        public ActionResult ImportGangs(HttpPostedFileBase posted_data)
        {
            if (posted_data == null)
            {
                TempData.Clear();
                TempData.Add("6", "No Data Found !");
                return RedirectToAction("Index");
            }

            if (posted_data.ContentLength == 0)
            {
                TempData.Clear();
                TempData.Add("6", "No Data Found !");
                return RedirectToAction("Index");
            }

            var path = Server.MapPath("~/Doc/Migration/" + posted_data.FileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            posted_data.SaveAs(path);
            var result = helper.ExtractGangsFromFile(path);
            if (result == true)
            {
                TempData.Clear();
                TempData.Add("6", "File Successfully imported");
                return RedirectToAction("Index");
            }
            else
            {
                TempData.Clear();
                TempData.Add("6", "File import Failed");
                return RedirectToAction("Index");
            }
        }
    }
}