using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static GetLabourManager.Helper.TreeBuilderHelper;

namespace GetLabourManager.Helper
{
    public class GangAllocationHelper
    {
        RBACDbContext db;
        public GangAllocationHelper(RBACDbContext _db)
        {
            this.db = _db;
        }

        //IsGangAllocated
        public bool IsGangAllocated(string code)
        {
            var records = db.GangSheetHeader.Include("SheetItems").FirstOrDefault(x => x.RequestCode == code);
            List<EmployeeGroup> group_list = new List<EmployeeGroup>();
            bool isAssigned = false;
            if (records.SheetItems.Count > 0)
            {
                var categories_id = records.SheetItems.Select(a => a.Category).ToList().Distinct();
                foreach (var item_id in categories_id)
                {
                    var group_id = records.SheetItems.Where(x => x.Category == item_id).Select(a => a.Group).ToList().Distinct();

                    var allocation = db.GangAllocation.Include("Containers")
                        .FirstOrDefault(x => x.RequestCode == code);
                    foreach (var item in group_id)
                    {
                        var entity = db.EmployeeGroup.FirstOrDefault(x => x.Id == item);
                        if (entity != null)
                        {
                            if (allocation != null)
                            {
                                if (!allocation.Containers.Exists(x => x.GroupId == entity.Id))
                                {
                                    group_list.Add(entity);
                                }
                            }
                            else
                            {
                                group_list.Add(entity);
                            }
                        }
                    }
                }
            }

            isAssigned = group_list.Count > 0 ? false : true;

            return isAssigned;
        }

        public Task<searchHeader> allocatedGangDetails(string term)
        {
            TaskCompletionSource<searchHeader> source = new TaskCompletionSource<searchHeader>();

            var records = (from a in db.GangSheetHeader.AsEnumerable()
                           join b in db.FieldClient on a.FieldClient equals b.Id
                           where (a.RequestCode.Equals(term) & a.Status == "ALLOCATED")
                           select new searchHeader
                           {
                               Code = a.RequestCode,
                               Issued = string.Format("{0:dd/MM/yyyy}", a.DateIssued),
                               Client = b.Name
                           }).ToList().FirstOrDefault();
            source.SetResult(records);
            return source.Task;
        }

        public RequestDetails getRequestDetails(string request_code)
        {
            var records = (from a in db.GangSheetHeader.Include("SheetItems").AsEnumerable()
                           join b in db.Gang.AsEnumerable() on a.GangCode equals b.Code
                           join c in db.FieldClient.AsEnumerable() on a.FieldClient equals c.Id
                           where (a.RequestCode == request_code & a.Status == "APPROVED")
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

        public Task<List<GangSearch>> SearchAllocatedGangs(string term)
        {
            TaskCompletionSource<List<GangSearch>> source = new TaskCompletionSource<List<GangSearch>>();

            var records = (from a in db.GangSheetHeader.AsEnumerable()

                           where (a.RequestCode.Contains(term) & a.Status == "ALLOCATED")
                           select new GangSearch
                           {
                               title = a.RequestCode,
                               description = "GANG: " + a.GangCode
                           }).ToList();
            source.SetResult(records);


            return source.Task;
        }

        public Task<List<GangAllocatedMembers>> getAllocatedGangs(string term)
        {
            TaskCompletionSource<List<GangAllocatedMembers>> source = new TaskCompletionSource<List<GangAllocatedMembers>>();
            SqlParameter param = new SqlParameter("@term", term);
            var result = this.db.Database.SqlQuery<GangAllocatedMembers>("CasualMembersFilter @term", param).ToList();
            source.SetResult(result.Distinct().ToList());
            return source.Task;
        }

        public Task<List<GangRequests>> getGangsRequests()
        {
            TaskCompletionSource<List<GangRequests>> source = new TaskCompletionSource<List<GangRequests>>();
            var result = this.db.Database.SqlQuery<GangRequests>("GangRequestList").ToList();
            foreach (var item in result)
            {
                item.RequestDate = string.Format("{0:dd/MM/yyyy}", item.DateIssued);
            }


            source.SetResult(result.ToList());
            return source.Task;
        }

        public GangDetails getGangDetails(string requestcode)
        {
            var entity = (from a in db.GangSheetHeader.Include("SheetItems").AsEnumerable()
                          where a.RequestCode == requestcode
                          join b in db.Gang.AsEnumerable() on a.GangCode equals b.Code
                          join c in db.Users.AsEnumerable() on a.PreparedBy equals c.Id
                          join d in db.EmpCategory on a.SheetItems.FirstOrDefault().Category equals d.Id

                          select new GangDetails
                          {
                              Gang = b.Description,
                              GangType = d.Category,
                              PreparedBy = c.UserName,
                              Shift = a.WorkShift + "-" + a.WorkWeek
                          }).FirstOrDefault();

            return entity;
        }

        public Task<List<GangAllocatedMembers>> getGangMembers(string term)
        {
            TaskCompletionSource<List<GangAllocatedMembers>> source = new TaskCompletionSource<List<GangAllocatedMembers>>();
            SqlParameter param = new SqlParameter("@term", term);
            var result = this.db.Database.SqlQuery<GangAllocatedMembers>("AdviceLisApprovalProc @term", param).ToList();
            source.SetResult(result.OrderBy(x => x.StaffCode).ThenBy(x => x.GroupName).ToList());
            return source.Task;
        }
        public Task<List<ContainerDetails>> getGangContainers(string term)
        {
            TaskCompletionSource<List<ContainerDetails>> source = new TaskCompletionSource<List<ContainerDetails>>();
            SqlParameter param = new SqlParameter("@term", term);
            var result = this.db.Database.SqlQuery<ContainerDetails>("AdviceListContainer @term", param).ToList();
            source.SetResult(result.ToList());
            return source.Task;
        }

        public Task<CostSheetDetails> getCostSheetRequest(string term)
        {
            TaskCompletionSource<CostSheetDetails> source = new TaskCompletionSource<CostSheetDetails>();
            SqlParameter param = new SqlParameter("@term", term);
            var result = this.db.Database.SqlQuery<CostSheetDetails>("CostSheetRquestDetailsProc @term", param).FirstOrDefault();
            source.SetResult(result);
            return source.Task;
        }
        //
        public Task<GangMemberListGrid> getGangCasualMembers(string term)
        {
            TaskCompletionSource<GangMemberListGrid> source = new TaskCompletionSource<GangMemberListGrid>();
            SqlParameter param = new SqlParameter("@term", term);
            var result = this.db.Database.SqlQuery<GangRequestMembers>("GangCasualsDetailsProc @term", param).ToList();

            foreach (var item in result)
            {
                item.Issued = string.Format("{0:dd/MM/yyyy}", item.DateIssued);
            }
            GangMemberListGrid members = new GangMemberListGrid()
            {
                items = new List<GangRequestMembers>(),
                totalCount = 0

            };
            members.items = result; members.totalCount = result.Count;
            source.SetResult(members);
            return source.Task;
        }

        public Task<ContainerListGrid> getGangContainerList(string term)
        {
            TaskCompletionSource<ContainerListGrid> source = new TaskCompletionSource<ContainerListGrid>();
            var records = (from a in db.AllocationContainers
                           where a.RequestCode == term
                           join b in db.EmpCategory on a.CategoryId equals b.Id
                           join c in db.VesselContainer on a.ContainerId equals c.Id
                           select new ContainerDetails
                           {
                               Category = b.Category,
                               Container = c.Continer,
                               ContainerNumber = a.ContainerNumber
                           }).ToList();
            ContainerListGrid list_grid = new ContainerListGrid() { items = new List<ContainerDetails>(), totalCount = 0 };
            list_grid.items = records;list_grid.totalCount = records.Count();

            source.SetResult(list_grid);
            return source.Task;
        }


        public class searchHeader
        {
            public string Code { get; set; }
            public string Issued { get; set; }
            public string Client { get; set; }

        }

        public class GangSearch
        {
            public string title { get; set; }
            public string description { get; set; }
        }

        public class GangAllocatedMembers
        {
            public string RequestCode { get; set; }
            public string StaffCode { get; set; }
            public string FullName { get; set; }
            public string Gang { get; set; }
            public string GroupName { get; set; }
            public string Description { get; set; }
            public int Containers { get; set; }
        }

        public class GangAllocatedMembersList : GangAllocatedMembers
        {

            public double HoursWorked { get; set; }
            public double OvertimeHrs { get; set; }
        }

        public class GangRequests
        {
            public int Id { get; set; }
            public string RequestCode { get; set; }
            public string Name { get; set; }
            public DateTime DateIssued { get; set; }
            public string Status { get; set; }
            public string RequestDate { get; set; }

        }

        public class GangDetails
        {
            public string GangType { get; set; }
            public string Gang { get; set; }
            public string Shift { get; set; }
            public string PreparedBy { get; set; }


        }

        public class ContainerDetails
        {
            public string Container { get; set; }
            public string ContainerNumber { get; set; }
            public string Category { get; set; }

        }

        public class CostSheetDetails
        {
            public string RequestCode { get; set; }
            public string Name { get; set; }
            public string Gang { get; set; }
            public string Category { get; set; }
            public int Casuals { get; set; }
            public int Vessels { get; set; }
        }

        public class GangRequestMembers
        {
            public DateTime DateIssued { get; set; }
            public string Issued { get; set; }
            public string StaffCode { get; set; }
            public string Name { get; set; }
            public string GroupName { get; set; }
            public string Category { get; set; }
        }

        public class GangMemberListGrid
        {
            public int totalCount { get; set; }
            public List<GangRequestMembers> items { get; set; }
        }

        public class ContainerListGrid
        {
            public int totalCount { get; set; }
            public List<ContainerDetails> items { get; set; }

        }


    }
}