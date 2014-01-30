using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionScrumV3.Entity;
using System.ComponentModel.DataAnnotations;

namespace GestionScrumV3.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Users")]
        public List<User> Users { get; set; }
        [Display(Name = "User Stories")]
        public List<UserStory> UserStories { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        [Display(Name = "Project")]
        public Project Project { get; set; }
        [Display(Name = "Current User")]
        public User CurrentUser { get; set; }
    }
}