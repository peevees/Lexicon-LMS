namespace Lexicon_LMS.Migrations
{
    using Lexicon_LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            //HACK: SEED DEBUGGER
            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Launch();

            var roleNames = new[] { "Teacher" };//REMINDER: add roles here!
            AddRoles(context, roleNames);

            var users = new[] { "teacher@shit.se", "student@shit.se", "student2@shit.se", "teacher2@shit.se", "teacher3@shit.se", "student3@shit.se" };//REMINDER: add users here!
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
                if (db.Users.Any(u => u.UserName == userEmail))
                {
                    if (db.Users.Any(u => u.EmailConfirmed == false))
                    {
                        List<ApplicationUser> usersUnconfirmed = new List<ApplicationUser>(db.Users.Where(u => u.EmailConfirmed == false));
                        foreach (var usertoConfirmed in usersUnconfirmed)
                        {
                            //get validation token to set confirmEmail
                            SetConfirmEmailForSeedUsers(userManager, usertoConfirmed);
                        }
                    }
                    continue;
                }

                var user = new ApplicationUser { Forename = "Mr", Surname = "JohnDoe", UserName = userEmail, Email = userEmail, TimeOfRegistration = new DateTime(2000, 01, 01, 00, 00, 00) };

                var result = userManager.Create(user, "P@$$w0rd");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("/n", result.Errors));
                }
                db.SaveChanges();
                SetConfirmEmailForSeedUsers(userManager, user);
            }

            //give Teacher user seed it's teacher role
            var teacherUser = userManager.FindByName(usersEmail[0]);
            var teacherUser2 = userManager.FindByName(usersEmail[3]);
            var teacherUser3 = userManager.FindByName(usersEmail[4]);
            userManager.AddToRole(teacherUser.Id, "Teacher");
            userManager.AddToRole(teacherUser2.Id, "Teacher");
            userManager.AddToRole(teacherUser3.Id, "Teacher");

        }

        private static void SetConfirmEmailForSeedUsers(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Lexicon-LMS");
            userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
            var token = userManager.GenerateEmailConfirmationToken(user.Id);

            var confirmResult = userManager.ConfirmEmail(user.Id, token);
            if (!confirmResult.Succeeded)
            {
                throw new Exception(string.Join("/n", confirmResult.Errors));
            }
        }

        private void AddCourse(ApplicationDbContext context)
        {
            var CourseTeacher = context.Users.Where(u => u.Email == "teacher@shit.se").FirstOrDefault();
            var CourseTeacher2 = context.Users.Where(u => u.Email == "teacher2@shit.se").FirstOrDefault();
            var CourseTeacher3 = context.Users.Where(u => u.Email == "teacher3@shit.se").FirstOrDefault();
            var CourseStudent = context.Users.Where(u => u.Email == "student@shit.se").FirstOrDefault();
            var CourseStudent2 = context.Users.Where(u => u.Email == "student2@shit.se").FirstOrDefault();
            var CourseStudent3 = context.Users.Where(u => u.Email == "student3@shit.se").FirstOrDefault();


            context.Courses.AddOrUpdate(
                c => c.CourseName,
                new Course
                {
                    CourseName = ".NET Development",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2019, 7, 19),
                    Description = "A course in .NET Development",
                    CourseCode = "DN-18",
                    Teacher = CourseTeacher,
                    TeacherID = CourseTeacher.Id
                });

            context.Courses.AddOrUpdate(
                c => c.CourseName,
                new Course
                {
                    CourseName = "Java Development",
                    StartDate = new DateTime(2018, 8, 19),
                    EndDate = new DateTime(2019, 8, 19),
                    Description = "Boil coffee",
                    CourseCode = "JD-18",
                    Teacher = CourseTeacher2,
                    TeacherID = CourseTeacher2.Id
                });

            context.Courses.AddOrUpdate(
                c => c.CourseName,
                new Course
                {
                    CourseName = "Office 365",
                    StartDate = new DateTime(2019, 8, 19),
                    EndDate = new DateTime(2020, 8, 19),
                    Description = "Create Documents, Spreadsheets and Presentations",
                    CourseCode = "MO-19"
                });

            context.SaveChanges();
            Course seededCourse = context.Courses.Where(c => c.CourseCode == "DN-18").FirstOrDefault();
            Course seededCoursejava = context.Courses.Where(c => c.CourseCode == "JD-18").FirstOrDefault();
            Course seededCourseoffice = context.Courses.Where(c => c.CourseCode == "MO-19").FirstOrDefault();

            seededCourse.CourseParticipants.Add(CourseStudent);
            seededCourse.CourseParticipants.Add(CourseStudent2);
            seededCoursejava.CourseParticipants.Add(CourseStudent3);

            CourseStudent.UserCourse = seededCourse;
            CourseStudent.UserCourseCode = seededCourse.CourseCode;

            CourseTeacher.UserCourse = seededCourse;
            CourseTeacher.UserCourseCode = seededCourse.CourseCode;

            context.Modules.AddOrUpdate(
                m => m.Description,
                new Module
                {
                    ModuleTitle = "C# Basics",
                    Description = "We learn the basics of C#",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2018, 7, 31),
                    Course = seededCourse,
                    CourseCode = seededCourse.CourseCode
                });

            context.Modules.AddOrUpdate(
                m => m.Description,
                new Module
                {
                    ModuleTitle = "Java Measuring",
                    Description = "Measuring the right amount of Java",
                    StartDate = new DateTime(2018, 8, 19),
                    EndDate = new DateTime(2018, 8, 31),
                    Course = seededCoursejava,
                    CourseCode = seededCoursejava.CourseCode
                });

            context.Modules.AddOrUpdate(
                m => m.Description,
                new Module
                {
                    ModuleTitle = "Word of words",
                    Description = "Predicting words in Word",
                    StartDate = new DateTime(2019, 8, 19),
                    EndDate = new DateTime(2019, 8, 31),
                    Course = seededCourseoffice,
                    CourseCode = seededCourseoffice.CourseCode
                });

            context.SaveChanges();
            Module seededModule = context.Modules.Where(c => c.CourseCode == seededCourse.CourseCode).FirstOrDefault();
            Module seededModulejava = context.Modules.Where(c => c.CourseCode == seededCoursejava.CourseCode).FirstOrDefault();
            Module seededModuleoffice = context.Modules.Where(c => c.CourseCode == seededCourseoffice.CourseCode).FirstOrDefault();

            seededCourse.CourseModules.Add(seededModule);
            seededCoursejava.CourseModules.Add(seededModulejava);
            seededCourseoffice.CourseModules.Add(seededModuleoffice);

            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Hello World!",
                    Deadline = new DateTime(2018, 7, 20),
                    Module = seededModule,
                    ModuleID = seededModule.ID
                });
            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Java Cup!",
                    Deadline = new DateTime(2018, 8, 20),
                    Module = seededModulejava,
                    ModuleID = seededModulejava.ID
                });
            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Spreading the Sheets!",
                    Deadline = new DateTime(2019, 8, 20),
                    Module = seededModuleoffice,
                    ModuleID = seededModuleoffice.ID
                });
            context.SaveChanges();
            Activity seededActivity = context.Activities.Where(c => c.ModuleID == seededModule.ID).FirstOrDefault();
            Activity seededActivityjava = context.Activities.Where(c => c.ModuleID == seededModulejava.ID).FirstOrDefault();
            Activity seededActivityoffice = context.Activities.Where(c => c.ModuleID == seededModuleoffice.ID).FirstOrDefault();
            seededModule.ModuleActivities.Add(seededActivity);
            seededModulejava.ModuleActivities.Add(seededActivityjava);
            seededModuleoffice.ModuleActivities.Add(seededActivityoffice);

        }
    }
}
