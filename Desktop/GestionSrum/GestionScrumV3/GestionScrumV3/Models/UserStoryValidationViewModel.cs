using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Models
{
    public class UserStoryValidationViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "User Story Validation Id")]
        public Guid UserStoryValidationId { get; set; }
    }
}