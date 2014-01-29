using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using GestionScrumV3.Entity;
using WebMatrix.WebData;

namespace GestionScrumV3.DAL
{
    public class GestionScrumInitializer : DropCreateDatabaseIfModelChanges<GestionScrumContext>
    {
        protected override void Seed(GestionScrumContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Users", "UserId", "Login", true);
            Roles.CreateRole("Admin");
            Roles.CreateRole("User");

            // Functions
            context.Function.Add(new Function() { 
                FunctionId = Guid.NewGuid(),
                Name = "Scrum Master",
                Description = "Scrum Master",
                Enabled = true
            });

            context.Function.Add(new Function()
            {
                FunctionId = Guid.NewGuid(),
                Name = "Product Owner",
                Description = "Product Owner",
                Enabled = true
            });

            context.Function.Add(new Function()
            {
                FunctionId = Guid.NewGuid(),
                Name = "Developer",
                Description = "Developer",
                Enabled = true
            });

            // Log Type
            context.LogType.Add(new LogType()
            {
                LogTypeId = Guid.NewGuid(),
                Name = "Remove User",
                Description = "Remove User to the Team"
            });

            context.LogType.Add(new LogType()
            {
                LogTypeId = Guid.NewGuid(),
                Name = "Add User",
                Description = "Add User to the Team"
            });

            context.LogType.Add(new LogType()
            {
                LogTypeId = Guid.NewGuid(),
                Name = "Create Project",
                Description = "Create the Project"
            });

            // Meeting Type
            context.MeetingType.Add(new MeetingType()
            {
                Id = Guid.NewGuid(),
                Name = "SprintPlanningTime",
                Description = "Sprint Planning Time"
            });

            context.MeetingType.Add(new MeetingType()
            {
                Id = Guid.NewGuid(),
                Name = "SprintReviewTime",
                Description = "Sprint Review Time"
            });

            context.MeetingType.Add(new MeetingType()
            {
                Id = Guid.NewGuid(),
                Name = "SprintRetrospectiveTime",
                Description = "Sprint Retrospective Time"
            });

            context.MeetingType.Add(new MeetingType()
            {
                Id = Guid.NewGuid(),
                Name = "DailyScrumTime",
                Description = "Daily Scrum Time"
            });

            // UserStoryStatusType
            context.UserStoryStatusType.Add(new UserStoryStatusType()
            {
                Id = Guid.NewGuid(),
                Name = "NotAssigned",
                Description = "Not assigned"
            });
            context.UserStoryStatusType.Add(new UserStoryStatusType()
            {
                Id = Guid.NewGuid(),
                Name = "ToDo",
                Description = "To Do"
            });
            context.UserStoryStatusType.Add(new UserStoryStatusType()
            {
                Id = Guid.NewGuid(),
                Name = "InProgress",
                Description = "In Progress"
            });
            context.UserStoryStatusType.Add(new UserStoryStatusType()
            {
                Id = Guid.NewGuid(),
                Name = "ToVerify",
                Description = "To Verify "
            });
            context.UserStoryStatusType.Add(new UserStoryStatusType()
            {
                Id = Guid.NewGuid(),
                Name = "Done",
                Description = "Done"
            });

            context.SaveChanges();
        }
    }
}