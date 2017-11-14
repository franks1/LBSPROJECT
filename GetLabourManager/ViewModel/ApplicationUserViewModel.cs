using GetLabourManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetLabourManager.ViewModel
{
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }
        public DateTime LastModified { get; set; }
        public string UserName { get; set; }
        public bool Inactive { get; set; }

        // [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public List<ApplicationUserRole> Roles { get; set; }
        public string Lastname { get; set; }
        [Required]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {6} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string UserStatus { get; set; }

        //[StringLength(100, ErrorMessage = "The {0} must be at least {6} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public ApplicationUserViewModel()
        {
            LastModified = DateTime.Now;
            Inactive = false;
        }
    }
}