using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Models
{
    public class UserStoryValidationViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserStoryValidationId { get; set; }
    }
}