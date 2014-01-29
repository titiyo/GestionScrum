using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<User> Users { get; set; }
    }
}