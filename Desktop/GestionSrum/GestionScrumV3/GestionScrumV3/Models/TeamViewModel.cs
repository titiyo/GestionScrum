using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Models
{
    public class TeamViewModel
    {
        [Display(Name = "Team Id")]
        public Guid TeamId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}