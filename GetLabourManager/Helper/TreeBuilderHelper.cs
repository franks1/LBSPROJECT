using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Helper
{
    public class TreeBuilderHelper
    {
        RBACDbContext db;
        public TreeBuilderHelper(RBACDbContext _db)
        {
            this.db = _db;
        }

        public List<TreeParent> getGangAdvice()
        {
            var tree_result = (from a in db.GangSheetHeader
                               join b in db.FieldClient on a.FieldClient equals b.Id
                               where a.Status == "PENDING"

                               select new { a, b }).ToList();
            TreeParent builder = new TreeParent() { id = 1, expanded = true, text = "PENDING REQUEST" };
            List<TreeParent> tb = new List<TreeParent>();
            //    builder.state = new TreeState() { opened = true };
            builder.items = new List<TreeNodes>();
            foreach (var item in tree_result)
            {
                TreeNodes node = new TreeNodes()
                {
                    id = builder.id.ToString() + "_" + item.a.Id.ToString(),
                    text = item.a.RequestCode + "-" + item.b.Name ,
                    tag = item.a.RequestCode,
                };
                builder.items.Add(node);
            }
            tb.Add(builder);

            return tb;
        }

        //public List<TreeBuilder> getGangAdvice()
        //{
        //    var tree_result = (from a in db.GangSheetHeader
        //                       where a.Status == "PENDING"
        //                       select a).ToList();
        //    TreeBuilder builder = new TreeBuilder() { id = 1, text = "PENDING REQUEST" };
        //    List<TreeBuilder> tb = new List<TreeBuilder>();
        //    builder.state = new TreeState() { opened = true };
        //    builder.children = new List<TreeNode>();
        //    foreach (var item in tree_result)
        //    {
        //        TreeNode node = new TreeNode()
        //        {
        //            id = item.Id,
        //            text = item.RequestCode
        //        };
        //        builder.children.Add(node);
        //    }
        //    tb.Add(builder);

        //    return tb;
        //}

        public List<TreeParent> getAppliedCostSheet()
        {
            var tree_result = (from a in db.CostSheet
                               where a.Status == "GENERATED"
                               join b in db.FieldClient on a.Client equals b.Id
                               select new { a, b }).ToList();
           // TreeBuilder builder = new TreeBuilder() { id = 0, text = "APPLIED COST SHEET" };
            TreeParent builder = new TreeParent() { id = 1, expanded = true, text = "APPLIED COST SHEET" };
            List<TreeParent> tb = new List<TreeParent>();
           // builder.state = new TreeState() { opened = true };
            builder.items = new List<TreeNodes>();
            foreach (var item in tree_result)
            {
                TreeNodes node = new TreeNodes()
                {
                    id = builder.id.ToString() + "_" + item.a.Id.ToString(),
                    text = item.a.CostSheetNumber+"-"+item.b.Name,
                    tag=item.a.CostSheetNumber
                };
                builder.items.Add(node);
            }
            tb.Add(builder);

            return tb;
        }

        public CostSheetDetails getCostSheet(string code)
        {
            var cost_sheet = (from a in db.CostSheet.AsEnumerable()
                              join b in db.FieldClient.AsEnumerable() on a.Client equals b.Id
                              where a.CostSheetNumber == code
                              select new CostSheetDetails
                              {
                                  Client = b.Name,
                                  CostSheetNumber = a.CostSheetNumber,
                                  DatePrepared = string.Format("{0:dd/MM/yyyy}", a.DatePrepared)
                              }).FirstOrDefault();
            return cost_sheet;
        }


        public RequestDetails getRequestDetails(string request_code)
        {
            var records = (from a in db.GangSheetHeader.Include("SheetItems").AsEnumerable()
                           join b in db.Gang.AsEnumerable() on a.GangCode equals b.Code
                           join c in db.FieldClient.AsEnumerable() on a.FieldClient equals c.Id
                           where a.RequestCode == request_code
                           select new RequestDetails
                           {
                               Id = a.Id,
                               Casuals = a.SheetItems.Count,
                               RequestDate = string.Format("{0:dd/MM/yyyy}", a.DateIssued),
                               RequestedClient = c.Name,
                               RequestedGang = b.Description,
                               Shift = a.WorkShift ?? "",
                               Week = a.WorkWeek ?? ""
                           }).FirstOrDefault();

            var sheet_item = db.GangSheetItems.FirstOrDefault(x => x.Header == records.Id);
            if (sheet_item != null)
            {
                var gang_type = db.EmpCategory.FirstOrDefault(x => x.Id == sheet_item.Category);
                records.GangType = gang_type.Category;
            }

            return records;
        }


        public class RequestDetails
        {

            public int Id { get; set; }
            public string RequestDate { get; set; }
            public string RequestedClient { get; set; }
            public string RequestedGang { get; set; }
            public int Casuals { get; set; }
            public string Shift { get; set; }
            public string Week { get; set; }
            public string GangType { get; set; }
        }

        public class CostSheetDetails
        {

            public string Client { get; set; }
            public string CostSheetNumber { get; set; }
            public string DatePrepared { get; set; }
        }

    }
}