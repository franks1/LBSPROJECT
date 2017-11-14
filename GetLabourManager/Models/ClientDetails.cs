using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetLabourManager.Models
{
    public class ClientDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddress2 { get; set; }
        public int Branch { get; set; }
        public string LicenseKey { get; set; }
        public string LicenseStatus { get; set; }
        public byte[] ImagePix { get; set; }
        public bool IsHeadOffice { get; set; }
    }
}