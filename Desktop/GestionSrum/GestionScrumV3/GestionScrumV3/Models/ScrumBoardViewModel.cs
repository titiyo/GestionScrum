using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class ScrumBoardViewModel
    {
        [Display(Name = "User Stories")]
        public List<UserStory> UserStories { get; set; }
        [Display(Name="Sprint")]
        public Guid SprintId { get; set; }
        [Display(Name = "Sprints")]
        public List<SelectListItem> Sprints { get; set; }
        [Display(Name = "Sprint Start Date")]
        public DateTime SprintStartDate { get; set; }
        [Display(Name = "Sprint End Date")]
        public DateTime SprintEndDate { get; set; }
        [Display(Name = "Project")]
        public Project Project { get; set; }
        [Display(Name = "Current User")]
        public User CurrentUser { get; set; }

        [Display(Name = "To Do")]
        public List<UserStory> ToDo { get; set; }
        [Display(Name = "In Progress")]
        public List<UserStory> InProgress { get; set; }
        [Display(Name = "To Verify")]
        public List<UserStory> ToVerify { get; set; }
        [Display(Name = "Done")]
        public List<UserStory> Done { get; set; }
    }
}