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
        public Guid UserStoryId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Started { get; set; }
        public int Complexity { get; set; }
        [Required]
        public int NumberOfHours { get; set; }
        public string Priority { get; set; }
        public int PercentageOfProgress { get; set; }

        [Display(Name = "En tant que <qui>")]
        [Required]
        public string AsARole { get; set; }
        [Display(Name = "Je veux <quoi>")]
        [Required]
        public string IWantGoalOrDesire { get; set; }
        [Display(Name = "Afin de <pourquoi>")]
        [Required]
        public string SoThatBenefit { get; set; }

        public int AssignedId { get; set; }

        public List<UserStoryValidationViewModel> Validations { get; set; }

        public ICollection<UserStoryValidation> Users { get; set; }

        public List<SelectListItem> Priorities { get; set; }

        public List<SelectListItem> Complexities { get; set; }

        public List<SelectListItem> Hours { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserStoryStatusTypeId { get; set; }
    }

    public enum Fibo { Un = 1, Deux = 2, Trois = 3, Cinq = 5, Huit = 8, Treize = 13, Vingt = 20, Quarante = 40, Cent = 100 };
    public enum Priority { Low = 1, Medium = 2, High = 3 };

    public class ProductBacklogViewModel
    {
        public List<User> Users { get; set; }
        public List<UserStory> UserStories { get; set; }
        public string TeamName { get; set; }
        public Project Project { get; set; }
        public User CurrentUser { get; set; }
    }
}