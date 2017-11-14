using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GetLabourManager.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using GetLabourManager.Helper;

namespace GetLabourManager.InvoiceHub
{
    public class TimeAuditHub : Hub
    {
        RBACDbContext db;
        public TimeAuditHub()
        {
            db = new RBACDbContext();
        }

        public void ApplyTimeSlots(string[] requestCode, int userId, double OverTime)
        {
            //Task.Factory.StartNew(async () =>
            //{
            dynamic flag_job = false;
            dynamic finished = false;
            Clients.All.IsWorking(flag_job);
            Clients.All.IsComplete(finished);
            dynamic d_val = "";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //PROCESSING 

            ApplicationUser user = ApplicationUserManager.GetUser(userId);
            var operational_hours = db.OperationalWorkHours.Where(x => x.Id ==
            db.OperationalWorkHours.Max(a => a.Id)).FirstOrDefault();


            var casuals = db.Employee.Select(x => x).ToList();
            var gangs = db.Gang.Select(x => x).ToList();
            var group = db.EmployeeGroup.Select(x => x).ToList();
            var transaction_track = db.Database.BeginTransaction();
            try
            {
                foreach (var codeitem in requestCode)
                {
                    var sequence = SequenceHelper.getSequence(db, SequenceHelper.NType.COSTSHEET);

                    d_val = DateTime.Now.ToLongTimeString() + "==============================================";
                    Clients.All.newMessages(d_val);
                    d_val = DateTime.Now.ToLongTimeString() + " APPLYING TIME SLOT [ "+ operational_hours.WorkingHours.ToString()+","+OverTime +" ] FOR "+
                         codeitem;
                    Clients.All.newMessages(d_val);
                    d_val = DateTime.Now.ToLongTimeString() + "===============================================";
                    var gang_request = db.GangSheetHeader.FirstOrDefault(x => x.RequestCode == codeitem);
                    var client = db.FieldClient.FirstOrDefault(a => a.Id == gang_request.FieldClient);
                    string gang_name = gangs.FirstOrDefault(x => x.Code == gang_request.GangCode).Code;
                    d_val = DateTime.Now.ToLongTimeString() + "         CREATING COST SHEET      ";
                    Clients.All.newMessages(d_val);

                    if (gang_request != null)
                    {
                        CostSheet sheet = new CostSheet()
                        {
                            Client = client.Id,
                            CostSheetNumber = sequence,
                            Note = "",
                            PreparedBy = user.Id,
                            RequestHeader = gang_request.Id,
                            Status = "GENERATED",
                            DatePrepared = DateTime.Now.Date,
                            HoursWorked= operational_hours.WorkingHours,
                            OvertimeHours = OverTime
                        };
                        db.CostSheet.Add(sheet);
                        db.SaveChanges();
                       
                        d_val = DateTime.Now.ToLongTimeString() + "============================================";
                        Clients.All.newMessages(d_val);
                        d_val = DateTime.Now.ToLongTimeString() + "     PREPARING SHEET ITEMS WITH TIME SLOTS";
                        Clients.All.newMessages(d_val);

                        List<CostSheetItems> sheet_items = new List<CostSheetItems>();
                        var members = db.GangSheetItems.Where(x => x.Header == gang_request.Id).Select(x => x).ToList();

                        var vessels = db.AllocationContainers.Where(x => x.RequestCode == gang_request.RequestCode)
                            .Select(x => x).ToList().Count;
                        var allocation = db.GangAllocation.FirstOrDefault(x => x.RequestCode == codeitem);

                        foreach (var item in members)
                        {
                            var FullName = casuals.FirstOrDefault(x => x.Code == item.StaffCode).FullName;
                            string group_name = group.FirstOrDefault(x => x.Id == item.Group).GroupName;

                            CostSheetItems sheet_item = new CostSheetItems()
                            {
                                Container = vessels,
                                FullName = FullName,
                                CostSheetId = 0,
                                Gang = gang_name,
                                GroupName = group_name,
                                HourseWorked = operational_hours.WorkingHours,
                                RaisedOn = gang_request.DateIssued,
                                StaffCode = item.StaffCode,
                                OvertimeHrs = OverTime
                            };
                            sheet_items.Add(sheet_item);
                        }

                        sheet_items.ForEach(x => x.CostSheetId = sheet.Id);
                        db.CostSheetItems.AddRange(sheet_items); db.SaveChanges();
                        gang_request.Status = "APPLIED";
                        db.Entry<GangSheetHeader>(gang_request).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        d_val = DateTime.Now.ToLongTimeString() + "     UPDATING GANG REQUEST";
                        Clients.All.newMessages(d_val);
                        SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.COSTSHEET);
                        d_val = DateTime.Now.ToLongTimeString() + "============================================";
                        Clients.All.newMessages(d_val);
                    }
                }
                d_val = DateTime.Now.ToLongTimeString() + "     SAVING BATCH ITEMS";
                Clients.All.newMessages(d_val);
                transaction_track.Commit();
                d_val = DateTime.Now.ToLongTimeString() + "=============================================";
                Clients.All.newMessages(d_val);
                d_val = DateTime.Now.ToLongTimeString() + "     TASK COMPLETED";
                Clients.All.newMessages(d_val);
                d_val = DateTime.Now.ToLongTimeString() + "=============================================";
                Clients.All.newMessages(d_val);

                flag_job = true;
                Clients.All.IsWorking(flag_job);
                watch.Stop();
                string duration = watch.Elapsed.ToString(@"hh\:mm\:ss");
                finished = true;
                Clients.All.IsComplete(finished);
                Clients.All.newMessages("ELAPSED TIME: " + duration);
            }
            catch (Exception err)
            {
                 d_val = DateTime.Now.ToLongTimeString() + "    AN ERROR OCCURED";
                Clients.All.newMessages(d_val);
                Clients.All.IsWorking(false);
                Clients.All.IsComplete(true);
                Clients.All.newMessages("    ERROR: " + err.InnerException.ToString());
            }
        }
    }
}