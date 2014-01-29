using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.DAL;
using GestionScrumV3.Entity;
using GestionScrumV3.Models;
using WebMatrix.WebData;

namespace GestionScrumV3.Controllers
{
    public class UserStoryController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();
        static List<int> listHours = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        //
        // GET: /UserStory/

        public ActionResult Index()
        {
            ProductBacklogViewModel model = new ProductBacklogViewModel();

            Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;
            Project project = _context.Project.Include("Team.Users").Include("UserStories").Where(x => x.ProjectId == projectId).FirstOrDefault();

            if (project != null)
            {
                // Get Users concerned by the project
                model.Users = project.Team.Users.ToList();
                model.TeamName = project.Team.Name;
                model.Project = project;
                model.UserStories = project.UserStories.ToList();
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
            }
            return View(model);
        }

        public ActionResult Create(Guid ProjectId)
        {
            UserStoryViewModel model = new UserStoryViewModel();
            model.ProjectId = ProjectId;

            model.Priorities = Enum.GetValues(typeof(Priority)).Cast<Priority>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();

            model.Complexities = Enum.GetValues(typeof(Fibo)).Cast<Fibo>().Select(v => new SelectListItem
            {
                Text = ((int)v).ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            model.Hours = listHours.Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserStoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserStory userStory = new UserStory()
                {
                    Title = model.Title,
                    Description = "En tant que " + model.AsARole + ", je veux " + model.IWantGoalOrDesire 
                        + " afin de " + model.SoThatBenefit,
                    AsARole = model.AsARole,
                    IWantGoalOrDesire = model.IWantGoalOrDesire,
                    SoThatBenefit = model.SoThatBenefit,
                    NumberOfHours = model.NumberOfHours,
                    Started = false,
                    Complexity = (int)model.Complexity,
                    Priority = model.Priority,
                    ProjectId = model.ProjectId,
                    UserStoryId = Guid.NewGuid(),
                    UserStoryStatusTypeId = _context.UserStoryStatusType.Where(x=>x.Name == "NotAssigned").First().Id
                };

                userStory.UserStoryValidations = new List<UserStoryValidation>();
                foreach (var item in model.Validations)
                {
                    userStory.UserStoryValidations.Add(new UserStoryValidation()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        UserStoryValidationId = Guid.NewGuid()
                    });
                }

                _context.UserStory.Add(userStory);
                _context.SaveChanges();

                return RedirectToAction("Index", "UserStory");
            }

            return View(model);
        }

        public ActionResult Detail(Guid userStoryId)
        {
            UserStory userStory = _context.UserStory.Include("UserStoryValidations").Include("Assigned").Where(x => x.UserStoryId == userStoryId).FirstOrDefault();
            if (userStory != null)
            {
                UserStoryViewModel model = new UserStoryViewModel()
                {
                    Title = userStory.Title,
                    Description = userStory.Description,
                    NumberOfHours = userStory.NumberOfHours,
                    Started = userStory.Started,
                    Complexity = userStory.Complexity,
                    Priority = userStory.Priority,
                    Validations = userStory.UserStoryValidations.Select(item =>
                                    new UserStoryValidationViewModel
                                    {
                                        Title = item.Title,
                                        Description = item.Description
                                    }
                                ).ToList()
                };

                return View(model);
            }
            return View();
        }

        public ActionResult Edit(Guid userStoryId)
        {
            UserStory userStory = _context.UserStory.Include("UserStoryValidations").Include("Assigned").Where(x => x.UserStoryId == userStoryId).FirstOrDefault();
            if (userStory != null)
            {
                UserStoryViewModel model = new UserStoryViewModel()
                {
                    Title = userStory.Title,
                    Description = userStory.Description,
                    AsARole = userStory.AsARole,
                    IWantGoalOrDesire = userStory.IWantGoalOrDesire,
                    SoThatBenefit = userStory.SoThatBenefit,
                    NumberOfHours = userStory.NumberOfHours,
                    Started = userStory.Started,
                    Complexity = userStory.Complexity,
                    Priority = userStory.Priority,
                    ProjectId = userStory.ProjectId,
                    UserStoryStatusTypeId = userStory.UserStoryStatusTypeId,
                    Validations = userStory.UserStoryValidations.Select(item =>
                                    new UserStoryValidationViewModel
                                    {
                                        Title = item.Title,
                                        Description = item.Description,
                                        UserStoryValidationId = item.UserStoryValidationId
                                    }
                                ).ToList()
                };

                model.Priorities = Enum.GetValues(typeof(Priority)).Cast<Priority>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                }).ToList();

                model.Priorities.Where(x => x.Text == model.Priority.ToString()).FirstOrDefault().Selected = true;

                model.Complexities = Enum.GetValues(typeof(Fibo)).Cast<Fibo>().Select(v => new SelectListItem
                {
                    Text = ((int)v).ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                model.Complexities.Where(x => x.Value == model.Complexity.ToString()).FirstOrDefault().Selected = true;

                model.Hours = listHours.Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                }).ToList();

                model.Hours.Where(x => x.Value == model.NumberOfHours.ToString()).FirstOrDefault().Selected = true;

                return View(model);
            }
            return RedirectToAction("Index", "UserStory");
        }

        [HttpPost]
        public ActionResult Edit(UserStoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserStory userStory = _context.UserStory.Include("UserStoryValidations").Include("Assigned").Include("Project")
                    .Where(x => x.UserStoryId == model.UserStoryId).FirstOrDefault();

                userStory.Title = model.Title;
                userStory.Description = "En tant que " + model.AsARole + ", je veux " + model.IWantGoalOrDesire 
                        + " afin de " + model.SoThatBenefit;
                userStory.AsARole = model.AsARole;
                userStory.IWantGoalOrDesire = model.IWantGoalOrDesire;
                userStory.SoThatBenefit = model.SoThatBenefit;
                userStory.NumberOfHours = model.NumberOfHours;
                userStory.Started = false;
                userStory.Complexity = (int)model.Complexity;
                userStory.Priority = model.Priority;
                userStory.ProjectId = model.ProjectId;
                userStory.UserStoryId = model.UserStoryId;

                _context.Entry(userStory).State = EntityState.Modified;
                _context.SaveChanges();

                #region Update, Add, Delete UserStoryValidation

                foreach (var item in model.Validations)
                {
                    if (item.UserStoryValidationId == new Guid())
                    {
                        UserStoryValidation userStoryValidation = new UserStoryValidation()
                        {
                            Title = item.Title,
                            Description = item.Description,
                            UserStoryValidationId = Guid.NewGuid(),
                            UserStoryId = model.UserStoryId
                        };

                        _context.UserStoryValidation.Add(userStoryValidation);
                    }
                    else
                    {
                        UserStoryValidation userStoryValidation = _context.UserStoryValidation
                            .Where(x => x.UserStoryValidationId == item.UserStoryValidationId).FirstOrDefault();

                        userStoryValidation.Title = item.Title;
                        userStoryValidation.Description = item.Description;

                        _context.Entry(userStoryValidation).State = EntityState.Modified;
                    }
                }

                foreach (var item in _context.UserStoryValidation.ToList())
                {
                    if(!model.Validations.Select(x=>x.UserStoryValidationId).Contains(item.UserStoryValidationId))
                    {
                        UserStoryValidation userStoryValidation = _context.UserStoryValidation
                            .Where(x => x.UserStoryValidationId == item.UserStoryValidationId).FirstOrDefault();
                        _context.Entry(userStoryValidation).State = EntityState.Deleted;
                    }
                }

                _context.SaveChanges();

                #endregion

                return RedirectToAction("Index", "UserStory");
            }

            return View(model);
        }
    }
}
