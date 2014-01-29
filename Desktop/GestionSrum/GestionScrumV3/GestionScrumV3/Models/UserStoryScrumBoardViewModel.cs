using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionScrumV3.Models
{
    public class UserStoryScrumBoardViewModel
    {
        public List<SelectListItem> Users { get; set; }
        [Display(Name="Assigned to")]
        public int UserId { get; set; }

        [Display(Name = "Progess in %")]
        public int Progress { get; set; }

        public Guid UserStoryId { get; set; }
    }
}