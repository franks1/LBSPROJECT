using RBACv3;
using RBACv3.Models;
using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using GetLabourManager;

public static class RBAC_ExtendedMethods_4_Principal
{
    public static int GetUserId(this IIdentity _identity)
    {
        int _retVal = 0;
        try
        {
            if (_identity != null && _identity.IsAuthenticated)
            {
                var ci = _identity as ClaimsIdentity;
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                if (!string.IsNullOrEmpty(_userId))
                {
                    _retVal = int.Parse(_userId);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static bool HasPermission(this IPrincipal _principal, string _requiredPermission)
    {
        bool _retVal = false;
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(int.Parse(_userId));
                    _retVal = _authenticatedUser.IsPermissionInUserRoles(_requiredPermission);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    //public static string LicenseCheckSum(this IPrincipal _principal)
    //{
    //    try
    //    {
    //        RBACDbContext db = new RBACDbContext();
    //        var records = db.LicenseStore.Where(c => c.Id == db.LicenseStore.Max(x => x.Id)).FirstOrDefault();
    //        int subscribed_days = records.ExpiredOn.Subtract(DateTime.Now.Date).Days;
    //        if (records.ExpiredOn.Subtract(DateTime.Now.Date).Days <= 0)
    //        {
    //            records.LicenseStatus = "EXPIRED";
    //            db.Entry<LicenseStore>(records).State = System.Data.Entity.EntityState.Modified;
    //            db.SaveChanges();
    //            return "EXPIRED";
    //        }
    //        if (subscribed_days >= 14 & subscribed_days <= 180)
    //        {
    //            return "YOU HAVE " + subscribed_days + " DAYS TO RENEW YOU YOUR LICENSE";
    //        }
    //        else
    //        {
    //            string describe = subscribed_days > 1 ? "DAYS" : "DAY";
    //            return "YOU HAVE " + subscribed_days + " " + describe + " TO RENEW YOUR LICENSE";
    //        }
    //    }
    //    catch (Exception err)
    //    {
    //        return "";
    //    }
    //}

    //public static string LicenseCheckSumDays(this IPrincipal _principal)
    //{
    //    try
    //    {
    //        RBACDbContext db = new RBACDbContext();
    //        var records = db.LicenseStore.Where(c => c.Id == db.LicenseStore.Max(x => x.Id)).FirstOrDefault();
    //        int subscribed_days = records.ExpiredOn.Subtract(DateTime.Now.Date).Days;
    //        return subscribed_days.ToString();
    //    }
    //    catch (Exception err)
    //    {
    //        return "0";
    //    }
    //}

    public static string HasPermissionAdmin(this IPrincipal _principal, string Module)
    {
        if (_principal.IsSysAdmin()) return "normal";

        string _retVal = "hidden";
        string[] _Customer = new string[] { "Customer-Index", "Customer-Create" };

        string[] _Account = new string[] { "CustomerAccount-Index",
            "CustomerAccount-AccountTransaction" , "CustomerAccount-AccountManager"};

        string[] _Loans = new string[] { "Loan-Index", "Loan-Create", "Loan-ManageLoans", "Loan-LoanPayment" };


        string[] _MasterSetup = new string[] { "Sequence-Index",
            "SmsApi-Index" , "ClientDetails-Index","MasterSetup-AccountSetupIndex","MasterSetup-AccountTypeIndex",
            "MasterSetup-NominalCodeIndex","MasterSetup-LoanChartIndex",  "MasterSetup-GroupIndex","CashbookManager-Index" };

        string[] _Finance = new string[] { "CashbookManager-CashbookTransferIndex", "Finance-PaymentReversal" };
        string[] _BackOffice = new string[] { "ChequeManager-Index" };
        string[] _UserManager = new string[] { "Admin-Index", "Admin-RoleIndex", "Admin-PermissionIndex" };
        string[] _Report = new string[] { "" };


        //string[] Master = new string[] { "DataMigration-Index",
        //    "Sequence-Index","ClientSetup-Index"};


        if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
        {
            var ci = _principal.Identity as ClaimsIdentity;
            string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

            if (!string.IsNullOrEmpty(_userId))
            {
                ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(int.Parse(_userId));
                switch (Module)
                {
                    case "CUSTOMER":
                        for (int i = 0; i < _Customer.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_Customer[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "ACCOUNT":
                        for (int i = 0; i < _Account.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_Account[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "LOANS":
                        for (int i = 0; i < _Loans.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_Loans[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "USER MANAGEMENT":
                        for (int i = 0; i < _UserManager.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_UserManager[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "FINANCE":
                        for (int i = 0; i < _Finance.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_Finance[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "MASTER SETUP":
                        for (int i = 0; i < _MasterSetup.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_MasterSetup[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                    case "BACK OFFICE":
                        for (int i = 0; i < _BackOffice.Length; i++)
                        {
                            _retVal = _authenticatedUser.IsPermissionInUserRoles(_BackOffice[i]) == true ? "normal" : "none";
                            if (_retVal == "normal") break;
                        }
                        break;
                        //case "ParentReport":
                        //    for (int i = 0; i < ParentReport.Length; i++)
                        //    {
                        //        _retVal = _authenticatedUser.IsPermissionInUserRoles(ParentReport[i]) == true ? "normal" : "none";
                        //        if (_retVal == "normal") break;
                        //    }
                        //    break;
                        //case "MASTER SETUP":
                        //    for (int i = 0; i < Master.Length; i++)
                        //    {
                        //        _retVal = _authenticatedUser.IsPermissionInUserRoles(Master[i]) == true ? "normal" : "none";
                        //        if (_retVal == "normal") break;
                        //    }
                        //    break;
                        //
                        //case "Event":
                        //    for (int i = 0; i < Event.Length; i++)
                        //    {
                        //        _retVal = _authenticatedUser.IsPermissionInUserRoles(Event[i]) == true ? "normal" : "none";
                        //        if (_retVal == "normal") break;
                        //    }
                        //    break;
                        ////
                        //case "Messaging":
                        //    for (int i = 0; i < Messaging.Length; i++)
                        //    {
                        //        _retVal = _authenticatedUser.IsPermissionInUserRoles(Messaging[i]) == true ? "normal" : "none";
                        //        if (_retVal == "normal") break;
                        //    }
                        //    break;
                        //case "User":
                        //    for (int i = 0; i < UserManagement.Length; i++)
                        //    {
                        //        _retVal = _authenticatedUser.IsPermissionInUserRoles(UserManagement[i]) == true ? "normal" : "none";
                        //        if (_retVal == "normal") break;
                        //    }
                        //    break;


                }
            }
        }

        return _retVal;
    }



    public static string HasPermissionView(this IPrincipal _principal, string _requiredPermission)
    {
        string _retVal = "hidden";
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                if (_principal.IsSysAdmin())
                {
                    _retVal = "normal";
                    return _retVal;
                }
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(int.Parse(_userId));
                    _retVal = _authenticatedUser.IsPermissionInUserRoles(_requiredPermission) == true ? "normal" : "none";
                }

            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }



    public static bool IsSysAdmin(this IPrincipal _principal)
    {
        bool _retVal = false;
        try
        {
            if (_principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated)
            {
                var ci = _principal.Identity as ClaimsIdentity;
                string _userId = ci != null ? ci.FindFirstValue(ClaimTypes.NameIdentifier) : null;

                if (!string.IsNullOrEmpty(_userId))
                {
                    ApplicationUser _authenticatedUser = ApplicationUserManager.GetUser(int.Parse(_userId));
                    _retVal = _authenticatedUser.IsSysAdmin();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }

    public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        string _retVal = string.Empty;
        try
        {
            if (identity != null)
            {
                var claim = identity.FindFirst(claimType);
                _retVal = claim != null ? claim.Value : null;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return _retVal;
    }
}