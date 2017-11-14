using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;



namespace GetLabourManager.Helper
{
    public class DashBoardHelper
    {
        RBACDbContext db;
        public DashBoardHelper(RBACDbContext _db)
        {
            this.db = _db;
        }

        public HeaderValueSummary getHeaderSummary()
        {
            HeaderValueSummary summary = new HeaderValueSummary();
            //casuals
            var casuals = db.Employee.Where(x => x.Status == "ACTIVE").Select(x => x).ToList().Count;
            var gangs = db.Gang.Select(x => x.Id).ToList().Count;
            var users = db.Users.Select(x => x).ToList().Count;
            var foremen = db.Foremen.Where(x => x.Status == "ACTIVE").Select(x => x).ToList().Count;
            summary.Casuals = casuals;
            summary.Gangs = gangs;
            summary.Users = users;
            summary.Foremen = foremen;
            return summary;
        }

        public Task<List<MonthlyStat>> GangAdviceSummary()
        {
            var records = db.GangSheetHeader.Include("SheetItems").Select(x => x).ToList();
            List<AnnualGangRequest> request = new List<AnnualGangRequest>();
            foreach (var item in records)
            {
                AnnualGangRequest r = new AnnualGangRequest()
                {
                    MonthName = item.DateIssued.ToString("MMM") + "-" + item.DateIssued.Year,
                    Total = item.SheetItems.Count
                };
                request.Add(r);
            }

            var grouped_records = (from a in request.AsEnumerable()
                                   group a by a.MonthName into gryp
                                   select new MonthlyStat
                                   {
                                       MonthName = gryp.Key,
                                       value = gryp.Sum(c => c.Total)
                                   }).ToList();

            return Task.FromResult<List<MonthlyStat>>(grouped_records);
        }


        public class HeaderValueSummary
        {
            public int Casuals { get; set; }
            public int Gangs { get; set; }
            public int Users { get; set; }
            public int Foremen { get; set; }
        }

        public class AnnualGangRequest
        {
            public string MonthName { get; set; }
            public int Total { get; set; }
        }
        public class MonthlyStat
        {
            public string MonthName { get; set; }
            public int value { get; set; }

        }


    }
}