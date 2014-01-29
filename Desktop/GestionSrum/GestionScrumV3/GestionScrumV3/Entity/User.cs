using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Guid FunctionId { get; set; }
        [ForeignKey("FunctionId")]
        public Function Function { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<ActionLog> ActionLogs { get; set; }
        public ICollection<UserStory> UserStories { get; set; }

        public override string ToString()
        {
            if (Function != null)
            {
                return FirstName + " " + LastName + " (" + Function.Name + ")";
            }
            else
            {
                return FirstName + " " + LastName;
            }
        }
    }
}