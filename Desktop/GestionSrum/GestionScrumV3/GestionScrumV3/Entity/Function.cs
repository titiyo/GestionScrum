using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class Function
    {
        public Guid FunctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

        public ICollection<User> Users { get; set; }
    }
}