using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class ActionLog
    {
        public Guid ActionLogId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid LogTypeId { get; set; }
        [ForeignKey("LogTypeId")]
        public LogType LogType { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}