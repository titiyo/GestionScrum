using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class UserViewModel
    {
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public Guid TeamId { get; set; }
        public Guid ProjectId { get; set; }

        [Display(Name = "Function")]
        public Function Function { get; set; }
    }

    public class AddUserInTeamViewModel
    {
        [Display(Name="User")]
        public string User { get; set; }
        public Guid TeamId { get; set; }
        public Guid ProjectId { get; set; }
    }
}