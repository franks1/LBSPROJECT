using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ExcelDataReader;
using System.Data;
using ExcelDataReader.Log;

namespace GetLabourManager.Helper
{
    public class DataProcessingHelper
    {
        RBACDbContext db;

        public string MainFolder { get; set; }

        public DataProcessingHelper(RBACDbContext _db)
        {
            this.db = _db;
            MainFolder = AppDomain.CurrentDomain.BaseDirectory + @"/Doc/Migration";
            if (!Directory.Exists(this.MainFolder))
            {
                Directory.CreateDirectory(this.MainFolder);
            }
        }

        public bool ExtractCasualsFromFile(string file_path)
        {
            bool complete = false;
            List<Employee> employees = new List<Employee>();
            var branch_entity = db.ClientSetup.FirstOrDefault(x => x.Id > 0);
            var category = db.EmpCategory.FirstOrDefault(x => x.Id > 0);

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            Employee employee = new Employee();
                            employee.Code = reader.GetValue(0).ToString();
                            employee.FirstName = reader.GetValue(1).ToString();
                            employee.MiddleName = reader.GetValue(2).ToString();
                            employee.LastName = reader.GetValue(3).ToString();
                            employee.Gender = reader.GetValue(4).ToString() == "M" ? "MALE" : "FEMALE";
                            employee.EmailAddress = reader.GetValue(5).ToString();
                            employee.Dob = DateTime.Parse(reader.GetValue(6).ToString());
                            employee.DateJoined = DateTime.Parse(reader.GetValue(7).ToString());
                            employee.Telephone1 = reader.GetValue(8).ToString();
                            employee.Address = reader.GetValue(9).ToString();
                            employee.BranchId = branch_entity.Id;
                            employee.Category = category.Id;
                            employee.Region = "GREATER ACCRA";
                            employee.Status = "ACTIVE";
                            employees.Add(employee);
                        }
                        index++;
                    }
                    reader.Close();
                }
                if (employees.Count > 0)
                {
                    db.Employee.AddRange(employees);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception err)
            {
                return complete;
                // throw;
            }

            return complete;

        }

        //
        public bool ExtractGuarantorFromFile(string file_path)
        {
            bool complete = false;
            List<EmployeeRelations> employees = new List<EmployeeRelations>();
            var all_casuals = db.Employee.Select(x => x).ToList();
            var branch_entity = db.ClientSetup.FirstOrDefault(x => x.Id > 0);
            var category = db.EmpCategory.FirstOrDefault(x => x.Id > 0);

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            var code = reader.GetValue(0).ToString();
                            var entity = all_casuals.FirstOrDefault(x => x.Code == code);
                            EmployeeRelations employee = new EmployeeRelations();
                            employee.GuarantorName = reader.GetValue(1).ToString() + " " + reader.GetValue(2).ToString();
                            employee.GuarantorPhone = reader.GetValue(3).ToString();
                            employee.GuarantorAddress = "";
                            employee.GuarantorRelation = "";

                            employee.NextofKinName = "";
                            employee.NextofKinPhone = "";
                            employee.NextofKinAddress = "";
                            employee.NextofKinRelation = "";
                            employee.StaffId = entity.Id;
                            employees.Add(employee);
                        }
                        index++;
                    }
                    reader.Close();
                }
                if (employees.Count > 0)
                {
                    db.EmployeeRelation.AddRange(employees);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception)
            {
                return complete;
            }
            return complete;

        }

        public bool ExtractNextOfKinFromFile(string file_path)
        {
            bool complete = false;
            List<EmployeeRelations> employees = new List<EmployeeRelations>();
            var all_casuals = db.Employee.Select(x => x).ToList();
            var relations = db.EmployeeRelation.Select(x => x).ToList();

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            var code = reader.GetValue(0).ToString();
                            var entity = all_casuals.FirstOrDefault(x => x.Code == code);
                            var employee = relations.FirstOrDefault(x => x.StaffId == entity.Id);
                            if (employee != null)
                            {
                                employee.NextofKinName = reader.GetValue(1).ToString() + " " + reader.GetValue(2).ToString();
                                employee.NextofKinPhone = reader.GetValue(3).ToString();
                                employee.NextofKinAddress = "";
                                employee.NextofKinRelation = "";
                                employee.StaffId = entity.Id;
                                db.Entry<EmployeeRelations>(employee).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                employees.Add(employee);
                            }

                        }
                        index++;
                    }
                    reader.Close();
                }
                if (employees.Count > 0)
                {
                    //db.EmployeeRelation.AddRange(employees);
                    //db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception)
            {
                return complete;
            }
            return complete;

        }

        public bool ExtractContributionFromFile(string file_path)
        {
            bool complete = false;
            List<EmployeeContributions> contributions = new List<EmployeeContributions>();
            var all_casuals = db.Employee.Select(x => x).ToList();
            var relations = db.EmployeeRelation.Select(x => x).ToList();

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            EmployeeContributions contribution = new EmployeeContributions();

                            var code = reader.GetValue(0).ToString();
                            var entity = all_casuals.FirstOrDefault(x => x.Code == code);

                            if (entity != null)
                            {
                                contribution.StaffId = entity.Id;
                                contribution.SSN = "";
                                if (string.IsNullOrWhiteSpace(reader.GetValue(1).ToString()))
                                {
                                    contribution.SSF = false;
                                }
                                else
                                {
                                    contribution.SSF = reader.GetValue(1).ToString() == "Y" ? true : false;
                                }

                                if (string.IsNullOrEmpty(reader.GetValue(2).ToString()))
                                {
                                    contribution.Welfare = false;
                                }
                                else if ((reader.GetValue(2).ToString()) == "0" || (reader.GetValue(2).ToString()) == "N")
                                {
                                    contribution.Welfare = false;
                                }
                                else if ((reader.GetValue(2).ToString()) == "1" || (reader.GetValue(2).ToString()) == "Y")
                                {
                                    contribution.Welfare = true;
                                }

                                if (string.IsNullOrEmpty(reader.GetValue(3).ToString()))
                                {
                                    contribution.UnionDues = false;
                                }
                                else if ((reader.GetValue(3).ToString()) == "0" || (reader.GetValue(3).ToString()) == "N")
                                {
                                    contribution.UnionDues = false;
                                }
                                else if ((reader.GetValue(3).ToString()) == "1" || (reader.GetValue(3).ToString()) == "Y")
                                {
                                    contribution.UnionDues = true;
                                }

                                if (string.IsNullOrEmpty(reader.GetValue(4).ToString()))
                                {
                                    contribution.SSN = "";
                                }
                                else if ((reader.GetValue(4).ToString()) == "N/A" || (reader.GetValue(4).ToString()) == "RETIRED")
                                {
                                    contribution.SSN = "";
                                }
                                else
                                {
                                    contribution.SSN = reader.GetValue(4).ToString();
                                }


                                contributions.Add(contribution);
                            }

                        }
                        index++;
                    }
                    reader.Close();
                }
                if (contributions.Count > 0)
                {
                    db.EmployeeContribution.AddRange(contributions);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception err)
            {
                return complete;
            }
            return complete;

        }

        public bool ExtractFieldClientFromFile(string file_path)
        {
            bool complete = false;
            List<FieldClients> clients = new List<FieldClients>();
            var all_casuals = db.Employee.Select(x => x).ToList();
            var relations = db.EmployeeRelation.Select(x => x).ToList();

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            FieldClients client = new FieldClients();

                            var name = reader.GetValue(0).ToString();
                            var telephone = reader.GetValue(1).ToString();
                            var address = reader.GetValue(2).ToString();
                            var premium = reader.GetValue(3).ToString();
                            var email = reader.GetValue(4).ToString();

                            client.Name = name;
                            client.Telephone1 = string.IsNullOrEmpty(telephone) ? "0000000000" : telephone;
                            client.Address = string.IsNullOrEmpty(address) ? "N/A" : address;
                            client.Premium = string.IsNullOrEmpty(premium) ? 0d : double.Parse(premium);
                            client.EmailAddress = email;
                            clients.Add(client);
                        }
                        index++;
                    }
                    reader.Close();
                }
                if (clients.Count > 0)
                {
                    db.FieldClient.AddRange(clients);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception err)
            {
                return complete;
            }
            return complete;

        }


        public bool ExtractGangTypeFromFile(string file_path)
        {
            bool complete = false;
            List<EmployeeCategory> gangtypes = new List<EmployeeCategory>();
            var all_casuals = db.EmpCategory.Select(x => x).ToList();


            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            EmployeeCategory gang = new EmployeeCategory();

                            var name = reader.GetValue(0).ToString();
                            if (!all_casuals.Exists(x => x.Category.ToLower().Equals(name.ToLower())))
                            {
                                if (!string.IsNullOrEmpty(name))
                                {
                                    gang.Category = name;
                                    gang.GroupId = 0;
                                    gangtypes.Add(gang);
                                }

                            }


                        }
                        index++;
                    }
                    reader.Close();
                }
                if (gangtypes.Count > 0)
                {
                    db.EmpCategory.AddRange(gangtypes);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception err)
            {
                return complete;
            }
            return complete;

        }
        //
        public bool ExtractGangsFromFile(string file_path)
        {
            bool complete = false;
            List<Gang> gangs = new List<Gang>();
            var all_casuals = db.Gang.Select(x => x).ToList();
            var branch_entity = db.ClientSetup.FirstOrDefault(x => x.Id > 0);

            FileStream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        if (index > 0)
                        {
                            Gang gang = new Gang();

                            var name = reader.GetValue(0).ToString();
                            var code = SequenceHelper.getSequence(db, SequenceHelper.NType.GANG_NUMBER);
                            if (!all_casuals.Exists(x => x.Description.ToLower().Equals(name.ToLower())))
                            {

                                if (!string.IsNullOrEmpty(name))
                                {

                                    gang.Description = name;
                                    gang.Branch = branch_entity.Id;
                                    gang.Status = "ACTIVE";
                                    gang.Code = code;
                                }

                            }
                            gangs.Add(gang);
                            SequenceHelper.IncreaseSequence(db, SequenceHelper.NType.GANG_NUMBER);
                        }
                        index++;
                    }
                    reader.Close();
                }
                if (gangs.Count > 0)
                {
                    db.Gang.AddRange(gangs);
                    db.SaveChanges();
                    System.IO.File.Delete(file_path);
                    complete = true;
                }
            }
            catch (Exception)
            {
                return complete;
            }
            return complete;

        }


    }
}