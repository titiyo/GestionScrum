using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class DashboardViewModel
    {
        public List<User> Users { get; set; }
        public List<UserStory> UserStories { get; set; }
        public string TeamName { get; set; }
        public Project Project { get; set; }
        public User CurrentUser { get; set; }
    }
}