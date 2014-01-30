using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Models
{
    public class UserStoryViewModel
    {
        [Display(Name = "User Story Id")]
        public Guid UserStoryId { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Started")]
        public bool Started { get; set; }
        [Display(Name = "Complexity")]
        public int Complexity { get; set; }
        [Required]
        [Display(Name = "Number Of Hours")]
        public int NumberOfHours { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Display(Name = "Percentage Of Progress")]
        public int PercentageOfProgress { get; set; }

        [Display(Name = "As a <role>")]
        [Required]
        public string AsARole { get; set; }
        [Display(Name = "I want <goal/desire>")]
        [Required]
        public string IWantGoalOrDesire { get; set; }
        [Display(Name = "So that <benefit>")]
        [Required]
        public string SoThatBenefit { get; set; }

        [Display(Name = "Assigned Id")]
        public int AssignedId { get; set; }

        [Display(Name = "Validations")]
        public List<UserStoryValidationViewModel> Validations { get; set; }

        [Display(Name = "Users")]
        public ICollection<UserStoryValidation> Users { get; set; }

        [Display(Name = "Priorities")]
        public List<SelectListItem> Priorities { get; set; }

        [Display(Name = "Complexities")]
        public List<SelectListItem> Complexities { get; set; }

        [Display(Name = "Hours")]
        public List<SelectListItem> Hours { get; set; }

        [Display(Name = "Project Id")]
        public Guid ProjectId { get; set; }

        [Display(Name = "User Story Status Type Id")]
        public Guid UserStoryStatusTypeId { get; set; }
    }

    public enum Fibo { Un = 1, Deux = 2, Trois = 3, Cinq = 5, Huit = 8, Treize = 13, Vingt = 20, Quarante = 40, Cent = 100 };
    public enum Priority { Low = 1, Medium = 2, High = 3 };

    public class ProductBacklogViewModel
    {
        [Display(Name = "Users")]
        public List<User> Users { get; set; }
        [Display(Name = "user Stories")]
        public List<UserStory> UserStories { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        [Display(Name = "Project")]
        public Project Project { get; set; }
        [Display(Name = "Current User")]
        public User CurrentUser { get; set; }
    }
}