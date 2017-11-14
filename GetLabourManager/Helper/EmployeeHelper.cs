using GetLabourManager.Models;
using GetLabourManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GetLabourManager.Helper
{
    public class EmployeeHelper
    {
        private RBACDbContext db;
        public EmployeeHelper(RBACDbContext _db)
        {
            this.db = _db;
        }

        public Dictionary<string, bool> ValidateModel(EmployeeViewModel model)
        {
            Dictionary<string, bool> model_note = new Dictionary<string, bool>();
            model_note.Add("passed", true);
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                model_note.Add("First Name has not been  specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                model_note.Add("Last Name has not been  specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.Telephone1))
            {
                model_note.Add("Primary Phone Number has not been  specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.Category.ToString()))
            {
                model_note.Add("Category has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.BranchId.ToString()))
            {
                model_note.Add("Branch has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.Address))
            {
                model_note.Add("Address has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.GuarantorName))
            {
                model_note.Add("Guarantors Name has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.GuarantorPhone))
            {
                model_note.Add("Guarantors Phone Number has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.GuarantorAddress))
            {
                model_note.Add("Guarantors Address has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.NextofKinName))
            {
                model_note.Add("Next of Kin's Name has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.NextofKinPhone))
            {
                model_note.Add("Next of Kin's Phone Number has not been specified", false);
                return model_note;
            }

            if (string.IsNullOrWhiteSpace(model.NextofKinAddress))
            {
                model_note.Add("Next of Kin's Address has not been specified", false);
                return model_note;
            }
            return model_note;
        }

        public List<EmployeeList> getEmployeeList()
        {
            var records = (from a in db.Employee.AsEnumerable()
                           join b in db.EmpCategory.AsEnumerable() on a.Category equals b.Id
                           join c in db.Branch.AsEnumerable() on a.BranchId equals c.Id
                           select new EmployeeList
                           {
                               Id=a.Id,
                               Code = a.Code,
                               Branch = c.Name,
                               Category = b.Category,
                               Name = a.FullName,
                               DateJoined = string.Format("{0:dd/MM/yyyy}", a.DateJoined),
                               Status = a.Status
                           }).ToList();
            return records;
        }
        
        public int SaveEmployee(EmployeeViewModel model)
        {
            var transaction_track = db.Database.BeginTransaction();
            string code = SequenceHelper.getSequence(db, SequenceHelper.NType.EMPLOYEE);
            int key = 0;
            try
            {
                Employee employee = new Employee()
                {
                    Code = code,
                    Address = model.Address,
                    BranchId = model.BranchId,
                    Category = model.Category,
                    Dob = model.Dob,
                    DateJoined = model.DateJoined,
                    EmailAddress = model.EmailAddress ?? "",
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Gender = model.Gender,
                    ImagePix = model.ImagePix,
                    Region = model.Region ?? "",
                    Telephone1 = model.Telephone1,
                    Telephone2 = model.Telephone2,
                    Status = "ACTIVE"
                };
                this.db.Employee.Add(employee);
                key = db.SaveChanges();

                EmployeeContributions contributions = new EmployeeContributions()
                {
                    StaffId = employee.Id,
                    SSN = model.SSN,
                    SSF = model.SSF,
                    UnionDues = model.UnionDues,
                    Welfare = model.Welfare
                };
                this.db.EmployeeContribution.Add(contributions); db.SaveChanges();

                EmployeeRelations relations = new EmployeeRelations()
                {
                    StaffId = employee.Id,
                    GuarantorName = model.GuarantorName,
                    GuarantorAddress = model.GuarantorAddress,
                    GuarantorPhone = model.GuarantorPhone,
                    GuarantorRelation = model.GuarantorRelation,
                    NextofKinName = model.NextofKinName,
                    NextofKinAddress = model.NextofKinAddress,
                    NextofKinPhone = model.NextofKinPhone,
                    NextofKinRelation = model.NextofKinRelation
                };
                this.db.EmployeeRelation.Add(relations); db.SaveChanges();
                SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.EMPLOYEE);
                transaction_track.Commit();
                return key;
            }
            catch (Exception)
            {
                transaction_track.Rollback();
                key = 0;
                return key;
            }
        }

        public int EditEmployee(EmployeeViewModel model)
        {
            var transaction_track = db.Database.BeginTransaction();
            int key = 0;

            try
            {
               var view_model=  FindEmployee(model.Id);
                var employee = db.Employee.FirstOrDefault(x => x.Id == model.Id);
                if (employee != null)
                {
                    employee.LastName = model.LastName;
                    employee.MiddleName = model.MiddleName;
                    employee.FirstName = model.FirstName;
                    employee.Dob = model.Dob;
                    employee.DateJoined = model.DateJoined;
                    employee.Gender = model.Gender;
                    employee.EmailAddress = model.EmailAddress ?? "";
                    employee.Address = model.Address ?? "";
                    employee.BranchId = model.BranchId;
                    employee.Category = model.Category;
                    if (employee.Status == "" || employee.Status == null)
                    {
                        employee.Status = "ACTIVE";
                    }
                    if (model.ImagePix != null)
                    {
                        if (model.ImagePix.Length > 10)
                            employee.ImagePix = model.ImagePix;
                    }
                    employee.Region = model.Region ?? "";
                    employee.Telephone1 = model.Telephone1 ?? "";
                    employee.Telephone2 = model.Telephone2 ?? "";
                    db.Entry<Employee>(employee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                var relation = db.EmployeeRelation.FirstOrDefault(x => x.StaffId == model.Id);
                if (relation != null)
                {
                    relation.GuarantorAddress = model.GuarantorAddress;
                    relation.GuarantorName = model.GuarantorName;
                    relation.GuarantorPhone = model.GuarantorPhone;
                    relation.GuarantorRelation = model.GuarantorRelation ?? "";

                    relation.NextofKinAddress = model.NextofKinAddress;
                    relation.NextofKinName = model.NextofKinName;
                    relation.NextofKinPhone = model.NextofKinPhone;
                    relation.NextofKinRelation = model.NextofKinRelation;

                    db.Entry<EmployeeRelations>(relation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }


                var contributions = db.EmployeeContribution.FirstOrDefault(x => x.StaffId == model.Id);
                if (contributions != null)
                {
                    contributions.SSF = model.SSF;
                    contributions.SSN = model.SSN;
                    contributions.UnionDues = model.UnionDues;
                    contributions.Welfare = model.Welfare;
                    db.Entry<EmployeeContributions>(contributions).State = System.Data.Entity.EntityState.Modified;
                    key= db.SaveChanges();
                }

                transaction_track.Commit();
                return key;
            }
            catch(Exception)
            {
                transaction_track.Rollback();
                key = 0; return key;
                   
            }
        }

        public Task<EmployeeViewModel> FindEmployee(int Id)
        {
            TaskCompletionSource<EmployeeViewModel> source = new TaskCompletionSource<EmployeeViewModel>();
            var record = (from a in db.Employee.AsEnumerable()
                          join b in db.EmployeeRelation.AsEnumerable() on a.Id equals b.StaffId
                          join c in db.EmployeeContribution.AsEnumerable() on a.Id equals c.StaffId
                          where a.Id==Id
                          select new EmployeeViewModel
                          {
                              Id = a.Id,
                              Address = string.IsNullOrEmpty(a.Address)?"":a.Address,
                              BranchId = a.BranchId,
                              Category = a.Category,
                              Code = a.Code,
                              DateJoined = a.DateJoined,
                              Dob = a.Dob,
                              EmailAddress = a.EmailAddress ?? "",
                              FirstName = a.FirstName,
                              LastName = a.LastName,
                              MiddleName = a.MiddleName,
                              Gender = a.Gender,
                              GuarantorAddress = b.GuarantorAddress,
                              GuarantorName = b.GuarantorName,
                              GuarantorPhone = b.GuarantorPhone ?? "",
                              GuarantorRelation = b.GuarantorRelation ?? "",
                              ImagePix = a.ImagePix,
                              NextofKinAddress = b.NextofKinAddress,
                              NextofKinName = b.NextofKinName,
                              NextofKinPhone = b.NextofKinPhone,
                              NextofKinRelation = b.NextofKinRelation,
                              Region = a.Region,
                              SSF = c.SSF,
                              SSN = c.SSN,
                              Telephone1 = a.Telephone1,
                              Telephone2 = a.Telephone2,
                              UnionDues = c.UnionDues,
                              Welfare = c.Welfare
                          }).FirstOrDefault();

            source.SetResult(record);
            return source.Task;
        }

        public class EmployeeList
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public string Branch { get; set; }
            public string Status { get; set; }
            public string DateJoined { get; set; }
        }
    }
}