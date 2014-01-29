using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class UserStory
    {
        public Guid UserStoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Started { get; set; }
        public int Complexity { get; set; }
        public int NumberOfHours { get; set; }
        public string Priority { get; set; }
        public string AsARole { get; set; }
        public string IWantGoalOrDesire { get; set; }
        public string SoThatBenefit { get; set; }
        public int PercentageOfProgress { get; set; }

        public ICollection<UserStoryValidation> UserStoryValidations { get; set; }

        public int? AssignedId { get; set; }
        [ForeignKey("AssignedId")]
        public User Assigned { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public Guid? SprintId { get; set; }
        [ForeignKey("SprintId")]
        public Sprint Sprint { get; set; }

        public Guid UserStoryStatusTypeId { get; set; }
        [ForeignKey("UserStoryStatusTypeId")]
        public UserStoryStatusType UserStoryStatusType { get; set; }
    }

    
}