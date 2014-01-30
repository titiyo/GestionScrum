using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.DAL;
using GestionScrumV3.Entity;
using GestionScrumV3.Models;
using WebMatrix.WebData;

namespace GestionScrumV3.Controllers
{
    public class ScrumBoardController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();
        //
        // GET: /ScrumBoard/

        public ActionResult Index(Guid? sprintId)
        {
            ScrumBoardViewModel model = new ScrumBoardViewModel();
            if (((Project)HttpContext.Application.Get("currentProject")) != null)
            {
                model.Project = ((Project)HttpContext.Application.Get("currentProject"));
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
            }

            List<Sprint> SprintsProject = _context.Sprint.Where(x => x.ProjectId == model.Project.ProjectId).ToList();
            model.Sprints = new List<SelectListItem>();
            model.Sprints = SprintsProject.Select(v => new SelectListItem
            {
                Text = v.Title,
                Value = v.Id.ToString()
            }).ToList();

            if (sprintId != null)
            {
                model.SprintId = (Guid)sprintId;
                model.SprintStartDate = SprintsProject.Where(x => x.Id == sprintId).First().StartDate;
                model.SprintEndDate = SprintsProject.Where(x => x.Id == sprintId).First().EndDate;
                model.Sprints.Where(x => x.Value == sprintId.ToString()).First().Selected = true;
            }
            else if (SprintsProject.Count() > 0)
            {
                model.SprintId = SprintsProject.First().Id;
                model.SprintStartDate = SprintsProject.First().StartDate;
                model.SprintEndDate = SprintsProject.First().EndDate;
            }

            model.UserStories = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType")
                .Include("Assigned").Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == null).ToList();

            model.ToDo = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType").Include("Assigned")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == model.SprintId && x.UserStoryStatusType.Name == "ToDo").ToList();
            model.InProgress = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType").Include("Assigned")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == model.SprintId && x.UserStoryStatusType.Name == "InProgress").ToList();
            model.ToVerify = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType").Include("Assigned")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == model.SprintId && x.UserStoryStatusType.Name == "ToVerify").ToList();
            model.Done = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType").Include("Assigned")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == model.SprintId && x.UserStoryStatusType.Name == "Done").ToList();

            return View(model);
        }

        public virtual ActionResult EditUserStory(Guid userStoryId)
        {
            Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;

            UserStoryScrumBoardViewModel model = new UserStoryScrumBoardViewModel();
            model.UserStoryId = userStoryId;
            List<User> users = _context.User.Where(x => x.Projects.Select(y => y.ProjectId == projectId).Count() > 0).ToList();
            model.Users = users.Select(v => new SelectListItem() { Text = v.FirstName + " " + v.LastName, Value = v.UserId.ToString() }).ToList();

            model.Progress = _context.UserStory.Where(x=>x.UserStoryId == userStoryId).Select(x=>x.PercentageOfProgress).FirstOrDefault();

            return View("EditUserStory", model);
        }

        [HttpPost]
        public virtual ActionResult EditUserStory(UserStoryScrumBoardViewModel model)
        {
            UserStory userStory = _context.UserStory.Where(x => x.UserStoryId == model.UserStoryId).FirstOrDefault();
            userStory.AssignedId = model.UserId;
            userStory.PercentageOfProgress = model.Progress;

            _context.Entry(userStory).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public virtual ActionResult ChangeSprint(Guid sprintId)
        {
            ScrumBoardViewModel model = new ScrumBoardViewModel();
            if (((Project)HttpContext.Application.Get("currentProject")) != null)
            {
                model.Project = ((Project)HttpContext.Application.Get("currentProject"));
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
            }

            model.UserStories = _context.UserStory.Where(x => x.ProjectId == model.Project.ProjectId && x.SprintId == null).ToList();

            List<Sprint> SprintsProject = _context.Sprint.Where(x => x.ProjectId == model.Project.ProjectId).ToList();

            model.Sprints = new List<SelectListItem>();
            model.Sprints = SprintsProject.Select(v => new SelectListItem
            {
                Text = v.Title,
                Value = v.Id.ToString(),
                Selected = v.Id == sprintId ? true : false
            }).ToList();

            model.SprintId = sprintId;
            model.SprintStartDate = SprintsProject.Where(x => x.Id == sprintId).First().StartDate;
            model.SprintEndDate = SprintsProject.Where(x => x.Id == sprintId).First().EndDate;

            model.ToDo = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.UserStoryStatusType.Name == "ToDo" && x.SprintId == sprintId).ToList();
            model.InProgress = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.UserStoryStatusType.Name == "InProgress" && x.SprintId == sprintId).ToList();
            model.ToVerify = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.UserStoryStatusType.Name == "ToVerify" && x.SprintId == sprintId).ToList();
            model.Done = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType")
                .Where(x => x.ProjectId == model.Project.ProjectId && x.UserStoryStatusType.Name == "Done" && x.SprintId == sprintId).ToList();

            // Create the html
            string html = RenderRazorViewToString<ScrumBoardViewModel>("Index", model, this.ControllerContext);

            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult CreateUserStoryNote(Guid userStoryId)
        {
            UserStory userStory = _context.UserStory.Include("Project").Include("Sprint").Include("UserStoryStatusType").Include("Assigned")
                .Where(x => x.UserStoryId == userStoryId).FirstOrDefault();

            // Create the html
            string html = RenderRazorViewToString<UserStory>("_UserStoryScrumBoardPartial", userStory, this.ControllerContext);

            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateUserStory(Guid userStoryId, Guid sprintId, string userStoryStatus)
        {
            UserStory userStory = _context.UserStory.Where(x => x.UserStoryId == userStoryId).FirstOrDefault();
            userStory.UserStoryStatusTypeId = _context.UserStoryStatusType.Where(x => x.Name == userStoryStatus).Select(x => x.Id).First();
            userStory.SprintId = sprintId;

            _context.Entry(userStory).State = EntityState.Modified;
            _context.SaveChanges();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult UpdateUserStoryStatus(Guid userStoryId, string userStoryStatus)
        {
            UserStory userStory = _context.UserStory.Where(x => x.UserStoryId == userStoryId).FirstOrDefault();
            userStory.UserStoryStatusTypeId = _context.UserStoryStatusType.Where(x => x.Name == userStoryStatus).Select(x => x.Id).First();

            if (userStoryStatus == "Done")
            {
                userStory.PercentageOfProgress = 100;
            }

            _context.Entry(userStory).State = EntityState.Modified;
            _context.SaveChanges();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public static string RenderRazorViewToString<T>(string viewName, T model, ControllerContext controllerContext)
        {
            using (var sw = new StringWriter())
            {
                // Find the view
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                // Create the view data with the model
                var vdd = new ViewDataDictionary<T>(model);
                // Create the view context
                var viewContext = new ViewContext(controllerContext, viewResult.View, vdd, new TempDataDictionary(), sw);
                // Render the view in the string writer
                viewResult.View.Render(viewContext, sw);
                // Release the viex
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                // Replace the special characters before return the view
                return sw.GetStringBuilder().Replace("\r\n", "").ToString();
            }
        }
    }
}
