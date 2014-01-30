using GestionScrumV3.DAL;
using GestionScrumV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Data;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Controllers
{
    public class ProjectController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();

        static List<int> listSprintDuration = new List<int> { 5, 10, 15, 20 };
        //
        // GET: /Project/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ProjectViewModel projectModel = new ProjectViewModel();
            projectModel.CreatorId = WebSecurity.CurrentUserId;
            projectModel.TeamId = new Guid();
            projectModel.Teams = new List<SelectListItem>();
            projectModel.Teams.Add(new SelectListItem()
            {
                Selected = true,
                Text = "Not Selected",
                Value = new Guid().ToString()
            });

            projectModel.SprintDuration = listSprintDuration.Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();

            if(_context.Team.Count() > 0)
            {
                foreach (var item in _context.Team)
                {
                    projectModel.Teams.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.TeamId.ToString()
                    });
                }
            }

            return View(projectModel);
        }

        [HttpPost]
        public ActionResult Create(ProjectViewModel o)
        {
            if (ModelState.IsValid)
            {
                Team team = null;
                if (!string.IsNullOrEmpty(o.TeamName))
                {
                    List<User> users = new List<Entity.User>();
                    users.Add(_context.User.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault());

                    team = new Team()
                    {
                        Name = o.TeamName,
                        Users = users,
                        TeamId = Guid.NewGuid()
                    };
                }
                else
                {
                    team = _context.Team.Include("Users").Where(x => x.TeamId == o.TeamId).FirstOrDefault();
                    team.Users.Add(_context.User.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault());
                }

                Project project = new Project()
                {
                    Name = o.Name,
                    Description = o.Description,
                    SprintDurationInDays = o.SprintDurationInDays,
                    UserId = o.CreatorId,
                    Team = team,
                    CreateDate = DateTime.Now,
                    ProjectId = Guid.NewGuid()
                };

                _context.Project.Add(project);

                _context.ActionLog.Add(new ActionLog()
                {
                    CreateDate = DateTime.Now,
                    LogType = _context.LogType.Where(x => x.Name == "Create Project").FirstOrDefault(),
                    UserId = WebSecurity.CurrentUserId,
                    ProjectId = project.ProjectId,
                    ActionLogId = Guid.NewGuid()
                });

                _context.SaveChanges();

                HttpContext.Application.Set("currentProject", project);
                HttpContext.Application.Set("projects", _context.Project.Where(x => x.Team.Users.Where(y => y.UserId == WebSecurity.CurrentUserId).Count() > 0).ToList());

                return RedirectToAction("Select", new { @projectId = project.ProjectId });
            }

            o.Teams = new List<SelectListItem>();
            o.Teams.Add(new SelectListItem()
            {
                Selected = true,
                Text = "Not Selected",
                Value = "0"
            });

            return  View(o);
        }

        public ActionResult Select(Guid projectId)
        {
            HttpContext.Application.Set("currentProject", _context.Project.Where(x => x.ProjectId == projectId).FirstOrDefault());
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Edit()
        {
            Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;
            Project project = _context.Project.Where(x=>x.ProjectId == projectId).FirstOrDefault(); 
            return View(new ProjectViewModel()
            {
                CreatorId = project.UserId,
                Description = project.Description,
                Name = project.Name,
                ProjectId = project.ProjectId,
                SprintDurationInDays = project.SprintDurationInDays,
                SprintDuration = listSprintDuration.Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                }).ToList(),
                TeamId = project.TeamId
            });
        }

        [HttpPost]
        public ActionResult Edit(ProjectViewModel o)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project()
                {
                    ProjectId = o.ProjectId,
                    Name = o.Name,
                    Description = o.Description,
                    SprintDurationInDays = o.SprintDurationInDays,
                    UserId = o.CreatorId,
                    TeamId = o.TeamId,
                    CreateDate = DateTime.Now
                };

                _context.ActionLog.Add(new ActionLog()
                {
                    CreateDate = DateTime.Now,
                    LogType = _context.LogType.Where(x => x.Name == "Edit Project").FirstOrDefault(),
                    UserId = WebSecurity.CurrentUserId,
                    ProjectId = project.ProjectId,
                    ActionLogId = Guid.NewGuid()
                });

                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();

                HttpContext.Application.Set("currentProject", _context.Project.Where(x => x.ProjectId == o.ProjectId).FirstOrDefault());
                HttpContext.Application.Set("projects", _context.Project.Where(x => x.Team.Users.Where(y => y.UserId == WebSecurity.CurrentUserId).Count() > 0).ToList());

                return RedirectToAction("Index", "Dashboard");
            }
            return View(o);
        }
    }
}
