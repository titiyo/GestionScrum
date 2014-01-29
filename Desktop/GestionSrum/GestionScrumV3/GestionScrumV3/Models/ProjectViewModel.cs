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
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int CreatorId { get; set; }

        [Required]
        [Display(Name="Durée d'un sprint en jours (Fixe pour tout le projet)")]
        public int SprintDurationInDays { get; set; }
        public List<SelectListItem> SprintDuration { get; set; }

        public Guid ProjectId { get; set; }

        public Guid TeamId { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public string TeamName { get; set; }
    }
}