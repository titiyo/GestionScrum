using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class ProjectViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Creator Id")]
        public int CreatorId { get; set; }

        [Required]
        [Display(Name = "Duration of a sprint in working days (Fixed for the entire project)")]
        public int SprintDurationInDays { get; set; }
        [Display(Name = "Sprint Duration")]
        public List<SelectListItem> SprintDuration { get; set; }

        [Display(Name = "Project Id")]
        public Guid ProjectId { get; set; }

        [Display(Name = "Team Id")]
        public Guid TeamId { get; set; }
        [Display(Name = "Teams")]
        public List<SelectListItem> Teams { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
    }
}