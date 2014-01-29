using GestionScrumV3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using GestionScrumV3.Entity;

namespace GestionScrumV3.DAL
{
    public class GestionScrumContext: DbContext
    {
        public GestionScrumContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Function> Function { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<LogType> LogType { get; set; }
        public DbSet<ActionLog> ActionLog { get; set; }
        public DbSet<UserStory> UserStory { get; set; }
        public DbSet<UserStoryStatusType> UserStoryStatusType { get; set; }
        public DbSet<UserStoryValidation> UserStoryValidation { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<MeetingType> MeetingType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Team>().HasMany(x => x.Users).WithMany(x => x.Teams).Map(
                    x => x.ToTable("UserTeam", "dbo")
                        .MapRightKey("UserId")
                        .MapLeftKey("TeamId"));

            modelBuilder.Entity<UserStory>()
                .HasMany(a => a.UserStoryValidations)
                .WithRequired(a => a.UserStory)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}