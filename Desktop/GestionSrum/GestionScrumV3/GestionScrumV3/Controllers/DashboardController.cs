using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.DAL;
using GestionScrumV3.Entity;
using GestionScrumV3.Models;
using WebMatrix.WebData;

namespace GestionScrumV3.Controllers
{
    public class DashboardController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();

        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            if (HttpContext.Application.Count > 0 
                && HttpContext.Application.AllKeys.Contains("currentProject"))
            {
                Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;
                Project project = _context.Project.Include("Team.Users").Include("UserStories")
                    .Include("ActionLogs.LogType").Where(x => x.ProjectId == projectId).FirstOrDefault();

                model.Users = project.Team.Users.ToList();
                model.TeamName = project.Team.Name;
                model.Project = project;
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                if (project.UserStories != null && project.UserStories.Count() > 0)
                {
                    model.UserStories = project.UserStories.ToList();
                }
            }
        
            return View(model);
        }

    }
}
