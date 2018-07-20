namespace Lexicon_LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Lexicon_LMS.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Lexicon-LMS.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //TODO: SEED DEBUGGER
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            var roleNames = new[] { "Teacher" };//REMINDER: add roles here!
            AddRoles(context, roleNames);

            var users = new[] { "teacher@shit.se", "student@shit.se" };//REMINDER: add users here!
            AddUsers(context, users);

            AddCourse(context);
        }

        private void AddRoles(ApplicationDbContext db, string[] roles)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (var roleName in roles)
            {
                if (db.Roles.Any(i => i.Name == roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("/n", result.Errors));
                }
            }
        }

        private void AddUsers(ApplicationDbContext db, string[] usersEmail)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var userEmail in usersEmail)
            {
                if (db.Users.Any(u => u.UserName == userEmail)) continue;
                var user = new ApplicationUser { UserName = userEmail, Email = userEmail, TimeOfRegistration = new DateTime(2000, 01, 01, 00, 00, 00), EmailConfirmed = (bool) true };
                var result = userManager.Create(user, "P@$$w0rd");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("/n", result.Errors));
                }
            }
            var teacherUser = userManager.FindByName("teacher@shit.se");
            userManager.AddToRole(teacherUser.Id, "Teacher");
        }

        private void AddCourse(ApplicationDbContext context)
        {
            context.Courses.AddOrUpdate(
                c => c.CourseName,
                new Course
                {
                    CourseName = ".NET Development",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2019, 7, 19),
                    Description = "A course in .NET Development",
                    CourseCode = "DN-18"
                });

            context.SaveChanges();
            Course seededCourse = context.Courses.Where(c => c.CourseCode == "DN-18").FirstOrDefault();

            context.Modules.AddOrUpdate(
                m => m.Description,
                new Module
                {
                    Description = "C# Basics",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2018, 7, 31),
                    Course = seededCourse,
                    CourseID = seededCourse.ID
                });

            context.SaveChanges();
            Module seededModule = context.Modules.Where(c => c.CourseID == seededCourse.ID).FirstOrDefault();
            seededCourse.CourseModules.Add(seededModule);

            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Hello World!",
                    Deadline = new DateTime(2018, 7, 20),
                    Module = seededModule,
                    ModuleID = seededModule.ID
                });
            context.SaveChanges();
            Activity seededActivity = context.Activities.Where(c => c.ModuleID == seededModule.ID).FirstOrDefault();
            seededModule.ModuleActivities.Add(seededActivity);
        }
    }
}
