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
        public Project Project { get; set; }
        public User CurrentUser { get; set; }

        [Required(ErrorMessage="Title field is mandatory !")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Start Date field is mandatory !")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SprintDurationInDays { get; set; }
        public Guid Id { get; set; }
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