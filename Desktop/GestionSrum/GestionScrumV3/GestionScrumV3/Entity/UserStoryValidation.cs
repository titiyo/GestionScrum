using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GestionScrumV3.Entity
{
    public class UserStoryValidation
    {
        public Guid UserStoryValidationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid UserStoryId { get; set; }
        [ForeignKey("UserStoryId")]
        public UserStory UserStory { get; set; }
    }
}
