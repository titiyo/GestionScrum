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
        public List<UserStory> UserStories { get; set; }
        [Display(Name="Sprint")]
        public Guid SprintId { get; set; }
        public List<SelectListItem> Sprints { get; set; }
        public DateTime SprintStartDate { get; set; }
        public DateTime SprintEndDate { get; set; }
        public Project Project { get; set; }
        public User CurrentUser { get; set; }

        public List<UserStory> ToDo { get; set; }
        public List<UserStory> InProgress { get; set; }
        public List<UserStory> ToVerify { get; set; }
        public List<UserStory> Done { get; set; }
    }
}