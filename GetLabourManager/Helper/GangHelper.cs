using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GetLabourManager.Helper
{
    public class GangHelper
    {
        private RBACDbContext db;
        public GangHelper(RBACDbContext _db)
        {
            this.db = _db;
        }

        public List<GangList> getGangs()
        {

            var records = (from a in db.Gang
                           join b in db.Branch on a.Branch equals b.Id
                           select new GangList
                           {
                               Id = a.Id,
                               Branch = b.Name,
                               Code = a.Code,
                               Description = a.Description,
                               Status = a.Status
                           }).ToList();

            return records;
        }

        public int SaveGang(Gang model)
        {
            int state = 0;
            var code = SequenceHelper.getSequence(db, SequenceHelper.NType.GANG_NUMBER);
            var transaction_track = db.Database.BeginTransaction();
            try
            {
                model.Code = code;
                db.Gang.Add(model);
                state = db.SaveChanges();
                transaction_track.Commit();
                SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.GANG_NUMBER);
                return state;
            }
            catch (Exception)
            {
                transaction_track.Rollback();
                state = 0;
                return state;
            }
        }

        public int EditGang(Gang model)
        {
            int state = 0;
            var transaction_track = db.Database.BeginTransaction();
            var entity = db.Gang.FirstOrDefault(x => x.Id == model.Id);
            try
            {
                entity.Description = model.Description;
                entity.Branch = model.Branch;
                db.Entry<Gang>(entity).State = System.Data.Entity.EntityState.Modified;
                state = db.SaveChanges();
                transaction_track.Commit();
                return state;
            }
            catch (Exception)
            {
                transaction_track.Rollback();
                state = 0;
                return state;
            }
        }

        public int DeleteGang(int Id)
        {
            int state = 0;
            var transaction_track = db.Database.BeginTransaction();
            try
            {
                var gang = db.Gang.FirstOrDefault(x => x.Id == Id);
                if (gang != null)
                {
                    db.Gang.Remove(gang);
                    state = db.SaveChanges();
                    transaction_track.Commit();
                }
                return state;
            }
            catch (Exception)
            {
                transaction_track.Rollback();
                state = 0;
                return state;
            }
        }

        public Task<List<GangAdviceListProc>> getGangAdviceList()
        {
            TaskCompletionSource<List<GangAdviceListProc>> source = new TaskCompletionSource<List<GangAdviceListProc>>();
            var result = this.db.Database.SqlQuery<GangAdviceList>("GangAdviceListProc").Where(x=>x.Status=="PENDING").ToList();
            List<GangAdviceListProc> entries = new List<GangAdviceListProc>();
            foreach (var item in result)
            {
                GangAdviceListProc items_value = new GangAdviceListProc()
                {
                    DateEntered = string.Format("{0:dd/MM/yyyy}", item.DateIssued),
                    Gang = item.Gang,
                    PreparedFor = item.PreparedFor,
                    RequestCode = item.RequestCode,
                    UserName = item.UserName,
                    Status = item.Status,
                    TotalCasuals = item.TotalCasuals
                };
                entries.Add(items_value);
            }

            source.SetResult(entries);
            return source.Task;
        }

        public Task<List<GangAdviceListProc>> getCostSheetList()
        {
            TaskCompletionSource<List<GangAdviceListProc>> source = new TaskCompletionSource<List<GangAdviceListProc>>();
            var result = this.db.Database.SqlQuery<GangAdviceList>("GangAdviceListProc")
                .Where(x => x.Status == "APPROVED" || x.Status=="APPLIED").ToList();
            List<GangAdviceListProc> entries = new List<GangAdviceListProc>();
            foreach (var item in result)
            {
                GangAdviceListProc items_value = new GangAdviceListProc()
                {
                    DateEntered = string.Format("{0:dd/MM/yyyy}", item.DateIssued),
                    Gang = item.Gang,
                    PreparedFor = item.PreparedFor,
                    RequestCode = item.RequestCode,
                    UserName = item.UserName,
                    Status = item.Status,
                    TotalCasuals = item.TotalCasuals
                };
                entries.Add(items_value);
            }

            source.SetResult(entries);
            return source.Task;
        }


        public Task<List<GangSummary>> getAdviceItems(int Id)
        {
            var entity = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.Id == Id);
            TaskCompletionSource<List<GangSummary>> source = new TaskCompletionSource<List<GangSummary>>();
            List<GangSummary> gang_summary = new List<GangSummary>();
            if (entity != null)
            {
                //gang_summary = (from x in db.Employee.AsEnumerable()
                //               join y in db.GangSheetItems on x.Code equals y.StaffCode
                //               join z in db.EmployeeGroup on y.Group equals z.Id
                //               join z2 in db.EmpCategory on y.Category equals z2.Id
                //               where y.Header== Id
                //                select new GangSummary
                //               {
                //                   state = false,
                //                   Id = y.Id,
                //                   Gang = entity.GangCode,
                //                   GroupName = z.GroupName,
                //                   Name = x.FullName,
                //                   StaffCode = y.StaffCode,
                //                   Category = z2.Category
                //               }).ToList();


                foreach (var item in entity.SheetItems)
                {
                    var staff = db.Employee.AsEnumerable().FirstOrDefault(x => x.Code == item.StaffCode);
                    var group = db.EmployeeGroup.FirstOrDefault(x => x.Id == item.Group);
                    var category = db.EmpCategory.FirstOrDefault(x => x.Id == item.Category);


                    GangSummary summary = new GangSummary()
                    {
                        state = false,
                        Id = item.Id,
                        Gang = entity.GangCode,
                        GroupName = group == null ? "" : group.GroupName,
                        Category = category == null ? "" : category.Category,
                        Name = staff.FullName,
                        StaffCode = item.StaffCode
                    };
                    gang_summary.Add(summary);
                }
            }
            source.SetResult(gang_summary);
            return source.Task;
        }

        public class GangList
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string Branch { get; set; }
            public string Status { get; set; }

        }

        public class GangAdviceList
        {
            public DateTime DateIssued { get; set; }
            public string RequestCode { get; set; }
            public string Gang { get; set; }
            public string UserName { get; set; }
            public string PreparedFor { get; set; }
            public int TotalCasuals { get; set; }
            public string Status { get; set; }
        }

        public class GangAdviceListProc : GangAdviceList
        {
            public string DateEntered { get; set; }

        }

        public class GangSummary
        {
            public int Id { get; set; }
            public bool state { get; set; }
            public string StaffCode { get; set; }
            public string Name { get; set; }
            public string GroupName { get; set; }
            public string Category { get; set; }
            public string Gang { get; set; }
        }
    }
}