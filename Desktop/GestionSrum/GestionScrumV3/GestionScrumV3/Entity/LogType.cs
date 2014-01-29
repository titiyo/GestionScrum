using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionScrumV3.Entity
{
    public class LogType
    {
        public Guid LogTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ActionLog> Users { get; set; }
    }
}
