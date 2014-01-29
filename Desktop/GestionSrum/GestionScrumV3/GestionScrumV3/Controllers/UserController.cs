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
    public class UserController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUserInTeam(Guid TeamId)
        {
            return View(new AddUserInTeamViewModel()
            {
                TeamId = TeamId,
                ProjectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId
            });
        }

        [HttpPost]
        public ActionResult AddUserInTeam(AddUserInTeamViewModel o)
        {
            if (ModelState.IsValid)
            {
                User usr = _context.User.Include("Function").Where(x => (x.LastName + " " + x.FirstName) == o.User).FirstOrDefault();
                if (!_context.Team.Include("Users").Where(x => x.TeamId == o.TeamId).FirstOrDefault().Users.Contains(usr))
                {
                    _context.Team.Include("Users").Where(x => x.TeamId == o.TeamId).FirstOrDefault().Users.Add(usr);

                    _context.ActionLog.Add(new ActionLog()
                    {
                        CreateDate = DateTime.Now,
                        LogType = _context.LogType.Where(x => x.Name == "Add User").FirstOrDefault(),
                        UserId = WebSecurity.CurrentUserId,
                        ProjectId = o.ProjectId,
                        ActionLogId = Guid.NewGuid()
                    });
                    
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Dashboard");
            }
            return View(o);
        }

        /// <summary>
        /// Search User name
        /// </summary>
        /// <param name="term">Text input by user</param>
        /// <returns>List of users</returns>
        public virtual ActionResult SearchUser(string term)
        {
            List<User> customers = _context.User.Include("Function").Where(x => x.FirstName.ToLower().Contains(term.ToLower()) 
                || x.LastName.ToLower().Contains(term.ToLower()) ).Take(10).ToList();

            IEnumerable<string> obj = customers.Select(x => x.LastName + " " + x.FirstName).Distinct();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int userId)
        {
            GestionScrumV3.Entity.User user = _context.User.Include("Function").Where(x => x.UserId == userId).FirstOrDefault();
            UserViewModel model = new UserViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Function = user.Function,
                LastName = user.LastName,
                Login = user.Login,
                PhoneNumber = user.PhoneNumber,
                TeamId = ((Project)HttpContext.Application.Get("currentProject")).TeamId,
                ProjectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveUserInTeam(UserViewModel o)
        {
            if (ModelState.IsValid)
            {
                User usr = _context.User.Include("Function")
                    .Where(x => (x.LastName + " " + x.FirstName) == (o.LastName + " " + o.FirstName)).FirstOrDefault();

                if (_context.Team.Include("Users").Where(x => x.TeamId == o.TeamId).FirstOrDefault().Users.Contains(usr))
                {
                    _context.Team.Include("Users").Where(x => x.TeamId == o.TeamId).FirstOrDefault().Users.Remove(usr);

                    _context.ActionLog.Add(new ActionLog()
                    {
                        CreateDate = DateTime.Now,
                        LogType = _context.LogType.Where(x => x.Name == "Remove User").FirstOrDefault(),
                        UserId = WebSecurity.CurrentUserId,
                        ProjectId = o.ProjectId,
                        ActionLogId = Guid.NewGuid()
                    });

                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Dashboard");
            }
            return View(o);
        }
    }
}
