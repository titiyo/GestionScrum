using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class SprintViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }
        [Display(Name = "Current User")]
        public User CurrentUser { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage="Title field is mandatory !")]
        public string Title { get; set; }
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date field is mandatory !")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Sprint Duration In Working Days")]
        public int SprintDurationInDays { get; set; }
        [Display(Name = "Id")]
        public Guid Id { get; set; }
        [Display(Name = "Error")]
        public string Error { get; set; }

        [Display(Name = "Sprint Planning Time")]
        public int SprintPlanningTime { get; set; }
        [Display(Name = "Daily Scrum Time")]
        public int DailyScrumTime { get; set; }
        [Display(Name = "Sprint Review Time")]
        public int SprintReviewTime { get; set; }
        [Display(Name = "Sprint Retrospective Time")]
        public int SprintRetrospectiveTime { get; set; }
    }
}