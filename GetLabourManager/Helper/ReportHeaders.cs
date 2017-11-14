using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Headers
{
    public class ReportHeaders
    {
        private static RBACDbContext db = new RBACDbContext();
        public static string ClientName { get; set; }
        public static string Telephone { get; set; }
        public static string Branch { get; set; }
        public static byte[] ClientPix { get; set; }
        public static string Address { get; set; }

        public static void GetReportHeader()
        {
            var client = db.ClientSetup.Where(c => c.Id == db.ClientSetup.Max(x => x.Id)).FirstOrDefault();
            if (client != null)
            {
                var branch = db.Branch.FirstOrDefault(x => x.Id == client.Id);
                ClientName = client.Name;
                string tel1 = client.Phone1 ?? "";
                string tel2 = client.Phone2 ?? "";
                Telephone = tel1 + "-" + tel2;
                Branch = branch == null ? "" : branch.Name.ToString();
                Address = client.Address;
                if (client.ImagePix != null)
                {
                    if (client.ImagePix.Length > 10)
                        ClientPix = client.ImagePix.ToArray();
                }
                else
                    ClientPix = null;

            }
        }

        //public static decimal getChangeDue(string receipt)
        //{
        //    var records = db.DrugSales.Where(C => C.ReceiptNumber.ToLower().Equals(receipt.ToLower()))
        //        .Sum(x => x.Quantity * x.SellingPrice) ;
        //    var paid = db.Receipt.Where(q => q.ReceiptNumber.ToLower().Equals(receipt)).Sum(a => a.AmountPaid);

        //    return paid - records;
        //}

    }
}