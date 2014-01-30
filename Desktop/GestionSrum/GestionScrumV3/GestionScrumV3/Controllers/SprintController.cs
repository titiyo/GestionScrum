using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionScrumV3.DAL;
using GestionScrumV3.Entity;
using GestionScrumV3.Models;
using GestionScrumV3.Utils;
using WebMatrix.WebData;

namespace GestionScrumV3.Controllers
{
    public class SprintController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();

        //
        // GET: /Sprint/

        public ActionResult Index()
        {
            SprintViewModel model = new SprintViewModel();
            if (((Project)HttpContext.Application.Get("currentProject")) != null)
            {
                model.Project = ((Project)HttpContext.Application.Get("currentProject"));
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                model.SprintDurationInDays = model.Project.SprintDurationInDays;
            }
            return View(model);
        }

        private DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }

        public virtual JsonResult GetSprints(double startDate, double endDate)
        {
            DateTime from = ConvertFromUnixTimestamp(startDate);
            DateTime to = ConvertFromUnixTimestamp(endDate);

            Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;
            List<Sprint> sprints = _context.Sprint.Include("Creator").Include("Project").Where(x => x.ProjectId == projectId).ToList();

            if (sprints != null && sprints.Count > 0)
            {
                var eventList = from e in sprints
                                select new
                                {
                                    id = e.Id,
                                    title = e.Title,
                                    start = e.StartDate.ToString("s", CultureInfo.CurrentCulture),
                                    end = e.EndDate.ToString("s", CultureInfo.CurrentCulture),
                                    projectId = e.ProjectId,
                                    creatorName = e.Creator.FirstName + " " + e.Creator.LastName,
                                    className = "Sprint_event",
                                    allDay = (((e.EndDate.Hour - e.StartDate.Hour) < 7
                                                && (e.EndDate.Hour - e.StartDate.Hour) >= 0)
                                                && (e.EndDate.Day - e.StartDate.Day) == 0)
                                                ? false : true
                                };
                // put all events in an array
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetMeeting(double startDate, double endDate)
        {
            DateTime from = ConvertFromUnixTimestamp(startDate);
            DateTime to = ConvertFromUnixTimestamp(endDate);

            Guid projectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId;
            List<Meeting> meeting = _context.Meeting.Include("MeetingType").Include("Project").Where(x => x.ProjectId == projectId).ToList();

            if (meeting != null && meeting.Count > 0)
            {
                var eventList = from e in meeting
                    select new
                    {
                        id = e.Id,
                        title = e.MeetingType.Description,
                        start = e.StartDate.ToString("s", CultureInfo.CurrentCulture),
                        end = e.EndDate.ToString("s", CultureInfo.CurrentCulture),
                        projectId = e.ProjectId,
                        className = e.MeetingType.Name + "_event",
                        allDay = (((e.EndDate.Hour - e.StartDate.Hour) < 7
                                    && (e.EndDate.Hour - e.StartDate.Hour) >= 0)
                                    && (e.EndDate.Day - e.StartDate.Day) == 0)
                                    ? false : true
                    };
                // put all events in an array
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetNotWorkingDaysBetweenTwoDate(double startDate, double endDate)
        {
            DateTime from = ConvertFromUnixTimestamp(startDate);
            DateTime to = ConvertFromUnixTimestamp(endDate);

            List<DateTime> notWorkingDays = WorkingDays.GetNationalHolidaysBetweenTwoDate(from, to);

            var dateList = from e in notWorkingDays
                           select e.ToString("yyyy-MM-dd");
            // put all events in an array
            var rows = dateList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult CreateSprint(SprintViewModel model)
        {
            model.EndDate = WorkingDays.AddWorkingDays(model.StartDate, model.SprintDurationInDays);

            if(_context.Sprint.Where(x => x.ProjectId == model.Project.ProjectId && model.EndDate >= x.StartDate && model.StartDate <= x.StartDate).Count() > 0
                || _context.Sprint.Where(x => x.ProjectId == model.Project.ProjectId && model.StartDate <= x.EndDate && model.EndDate >= x.EndDate).Count() > 0)
            {
                ModelState.AddModelError("Error", "A sprint already been created for these dates !");
            }

            if (ModelState.IsValid)
            {
                Sprint sprint = new Sprint()
                {
                    Title = model.Title,
                    CreatorId = model.CurrentUser.UserId,
                    ProjectId = model.Project.ProjectId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Id = Guid.NewGuid()
                };

                _context.Sprint.Add(sprint);
                _context.SaveChanges();

                List<MeetingType> meetingType = _context.MeetingType.ToList();

                // Create All Meeting
                // SprintPlanningTime
                //Meeting sprintPlanningTime = new Meeting()
                //{
                //    Id = Guid.NewGuid(),
                //    StartDate = model.StartDate.AddHours(14),
                //    EndDate = model.StartDate.AddHours(14).AddMinutes(model.SprintPlanningTime),
                //    MeetingTypeId = meetingType.Where(x => x.Name == "SprintPlanningTime").FirstOrDefault().Id,
                //    ProjectId = model.Project.ProjectId
                //};
                //_context.Meeting.Add(sprintPlanningTime);

                // SprintReviewTime
                Meeting sprintReviewTime = new Meeting()
                {
                    Id = Guid.NewGuid(),
                    StartDate = model.EndDate.AddHours(14),
                    EndDate = model.EndDate.AddHours(14).AddMinutes(model.SprintReviewTime),
                    MeetingTypeId = meetingType.Where(x => x.Name == "SprintReviewTime").FirstOrDefault().Id,
                    ProjectId = model.Project.ProjectId
                };
                _context.Meeting.Add(sprintReviewTime);

                // SprintRetrospectiveTime
                Meeting sprintRetrospectiveTime = new Meeting()
                {
                    Id = Guid.NewGuid(),
                    StartDate = model.EndDate.AddHours(17),
                    EndDate = model.EndDate.AddHours(17).AddMinutes(model.SprintRetrospectiveTime),
                    MeetingTypeId = meetingType.Where(x => x.Name == "SprintRetrospectiveTime").FirstOrDefault().Id,
                    ProjectId = model.Project.ProjectId
                };
                _context.Meeting.Add(sprintRetrospectiveTime);

                // DailyScrumTime
                DateTime date = model.StartDate;
                while (date < model.EndDate)
                {
                    if (WorkingDays.IsWorkingDay(date))
                    {
                        Meeting dailyScrumTime = new Meeting()
                        {
                            Id = Guid.NewGuid(),
                            StartDate = date.AddHours(9).AddMinutes(30),
                            EndDate = date.AddHours(9).AddMinutes(30 + model.DailyScrumTime),
                            MeetingTypeId = meetingType.Where(x => x.Name == "DailyScrumTime").FirstOrDefault().Id,
                            ProjectId = model.Project.ProjectId
                        };
                        _context.Meeting.Add(dailyScrumTime);
                    }
                    date = date.AddDays(1);
                }

                _context.ActionLog.Add(new ActionLog()
                {
                    CreateDate = DateTime.Now,
                    LogType = _context.LogType.Where(x => x.Name == "Create Sprint").FirstOrDefault(),
                    UserId = WebSecurity.CurrentUserId,
                    ProjectId = ((Project)HttpContext.Application.Get("currentProject")).ProjectId,
                    ActionLogId = Guid.NewGuid()
                });

                _context.SaveChanges();

                model.Title = string.Empty;
            }

            if (((Project)HttpContext.Application.Get("currentProject")) != null)
            {
                model.Project = ((Project)HttpContext.Application.Get("currentProject"));
                model.CurrentUser = _context.User.Include("Function").Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                model.SprintDurationInDays = model.Project.SprintDurationInDays;
            }

            return View("Index",model);
        }
    }
}
