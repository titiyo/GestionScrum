using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [Remote("LoginAvailable", "Home", ErrorMessage = "Login already exists")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Password Confirmed")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string PasswordConfirmed { get; set; }

        [Required]
        [Display(Name = "Function")]
        public Guid FunctionId { get; set; }
        public List<SelectListItem> Functions { get; set; }

    }
}