namespace Lexicon_LMS.Migrations
{
    using Lexicon_LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        #region Variables
        private DateTime startDate = new DateTime(1990, 01, 01);
        private Random Rnd = new Random();
        //UNDONE: Add all properties that a user shall have by default
        private struct Users
        {
            public string Email { get; set; }
            public string Username => Email;
            public string Password { get; set; }
            public string Forename { get; set; }
            public string Surname { get; set; }
            public DateTime TimeofRegistration { get; set; }
            public string Street { get; set; }
            public string Postcode { get; set; }
            public string City { get; set; }
            public bool IsStudent { get; set; }
        }

        //UNDONE: Add all properties that a courses shall have by default
        private struct Courses
        {
            public string CourseName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }
            public string CourseCode { get; set; }
            public ApplicationUser Teacher { get; set; }
            public string TeacherId => Teacher.Id;
            public ApplicationUser CoursePaticipant { get; set; }
        }

        private struct Modules
        {
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public Course Course { get; set; }
            public int CourseId => Course.ID;
        }

        private struct Activities
        {
            public string Name { get; set; }
            public DateTime Deadline { get; set; }
            public Module Module { get; set; }
            public int ModuleId => Module.ID;
        }

        #endregion

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


            //REMINDER: Add Users here
            var usersToAdd = new[]
            {
                #region Teachers
                new Users
                {
                    Email = "Teacher@shit.se",
                    Password = "P@$$w0rd",
                    Forename = "Mr. ",
                    Surname = "Teacher",
                    TimeofRegistration = new DateTime(1990, 01, 01),
                    Street = "SomeStreetName 0",
                    Postcode = "00000",
                    City = "SomeCityName",
                    IsStudent = false
                },
                new Users
                {
                    Email = "John.Doe@shit.se",
                    Password = "JohnDoe",
                    Forename = "John",
                    Surname = "Doe",
                    TimeofRegistration = GetRandomDate(),
                    Street = "Primrose Lane 2",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Lillsand",
                    IsStudent = false
                },
                new Users
                {
                    Email = "Jane.Doe@shit.se",
                    Password = "JaneDoe",
                    Forename = "Jane",
                    Surname = "Doe",
                    TimeofRegistration = GetRandomDate(),
                    Street = "Jackson Avenue 3",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Sickla",
                    IsStudent = false
                },
                #endregion

                #region Template
                /*Mock structure
                new Users
                {
                    Email = "",
                    Password = "",
                    Forename = "",
                    Surname = "",
                    TimeofRegistration = GetRandomDate(),
                    Street = "",
                    Postcode = GetRandomNumber().ToString(),
                    City = "",
                    IsStudent = false
                },*/
#endregion

                //REMINDER: Students below, Teachers above
                #region Students
                new Users
                {
                    Email = "student@shit.se",
                    Password = "P@$$w0rd",
                    Forename = "Mr. ",
                    Surname = "Student",
                    TimeofRegistration = new DateTime(),
                    Street = "SomeStreetName 0",
                    Postcode = "00000",
                    City = "SomeCityName",
                    IsStudent = true
                },
                new Users
                {
                    Email = "Johnny.Roe@shit.se",
                    Password = "JohnnyRoe",
                    Forename = "Johnny",
                    Surname = "Roe",
                    TimeofRegistration = GetRandomDate(),
                    Street = "Howard Street 1",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Nocketorp",
                    IsStudent = true
                },
                new Users
                {
                    Email = "Janie.Roe@shit.se",
                    Password = "JanieRoe",
                    Forename = "Janie",
                    Surname = "Roe",
                    TimeofRegistration = GetRandomDate(),
                    Street = "Roberts Road 1",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Medvik",
                    IsStudent = true
                }
#endregion
            };

            //REMINDER: Add courses here
            var coursesToAdd = new[]
            {
                 new Courses
                {
                    CourseName = ".NET Development",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2019, 7, 19),
                    Description = "A course in .NET Development",
                    CourseCode = "DN-18"
                },

                 new Courses
                {
                    CourseName = "Java Development",
                    StartDate = new DateTime(2018, 8, 19),
                    EndDate = new DateTime(2019, 8, 19),
                    Description = "Boil coffee",
                    CourseCode = "JD-18"
                },

                 new Courses
                {
                    CourseName = "Office 365",
                    StartDate = new DateTime(2019, 8, 19),
                    EndDate = new DateTime(2020, 8, 19),
                    Description = "Create Documents, Spreadsheets and Presentations",
                    CourseCode = "MO-19"
                }
            };

            var roleNames = new[] { "Teacher" };//REMINDER: add roles here!
            var users = new[] { "teacher@shit.se", "student@shit.se", "student2@shit.se", "teacher2@shit.se", "teacher3@shit.se", "student3@shit.se" };//REMINDER: add users here!
            AddRoles(context, roleNames);
            AddUsers(context, usersToAdd);
            AddCourse(context, coursesToAdd, usersToAdd);
        }

        #region Utilities
        private ApplicationUser GetRandomTeacher(ApplicationDbContext db)
        {
            List<ApplicationUser> Teachers = new List<ApplicationUser>();
            var Users = db.Users.Include(u => u.Roles);
            var role = db.Roles.ToList();
            var usersinRole = role[0].Users;
            ApplicationUser Teacher = null;
            foreach (var teacher in Users)
            {
                foreach (var userin in usersinRole)
                {
                    if (teacher.Id == userin.UserId)
                    {
                        Teachers.Add(teacher);
                        break;
                    }
                }
            }
            for (int i = 0; i < Teachers.Count; i++)
            {
                Teacher = Teachers[Rnd.Next(0, (Teachers.Count))];
                if (Teacher.UserCourse == null)
                {
                    break;
                }
                Teacher = null;
            }

            if (Teacher == null)
            {
                return null;//no teachers left to add?
            }
            else
            {
                return Teacher;
            }

        }

        private ICollection<ApplicationUser> GetRandomStudent(ApplicationDbContext db, Users[] users)
        {
            List<ApplicationUser> Students = new List<ApplicationUser>();
            var Users = db.Users.Include(u => u.Roles);
            var role = db.Roles.ToList();
            var usersinRole = role[0].Users;
            ApplicationUser student = null;
            foreach (var studentInUsers in Users)
            {
                foreach (var user in users)
                {
                    if (user.IsStudent)
                    {
                        if (studentInUsers.Email == user.Email)
                        {

                            Students.Add(studentInUsers);
                            break;

                        }
                    }

                }
            }

            List<ApplicationUser> StudentsToAdd = new List<ApplicationUser>();
            for (int i = 0; i < 3; i++)
            {
                student = Students[Rnd.Next(0, (Students.Count))];
                if (student.UserCourse == null)
                {
                    StudentsToAdd.Add(student);
                }
                student = null;
            }
            if (Students == null)
            {
                return null;//no more students to add
            }
            else
            {
                return StudentsToAdd;
            }
        }

        private int GetRandomNumber()
        {
            return Rnd.Next(00001, 99999);
        }
        private DateTime GetRandomDate()
        {
            int range = (DateTime.Today - startDate).Days;
            return startDate.AddDays(Rnd.Next(range));
        }

        /*REMINDER: Idea for postcode formatting?
            postCode = postCode.Replace(" ", "");//remove current spaces
            if (postCode.Length > 3)
                postCode = postCode.Insert(postCode.Length - 3, " ");//but set max length in validation
        */

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

        #endregion

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

        private void AddUsers(ApplicationDbContext db, Users[] usersToCreate)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var user in usersToCreate)
            {
                #region check existing users
                if (db.Users.Any(u => u.UserName == user.Username))
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
                #endregion

                var createdUser = new ApplicationUser { Forename = user.Forename, Surname = user.Surname, UserName = user.Username, Email = user.Email, TimeOfRegistration = user.TimeofRegistration, City = user.City, Street = user.Street, Postcode = user.Postcode };

                var result = userManager.Create(createdUser, user.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("/n", result.Errors));
                }

                db.SaveChanges();
                SetConfirmEmailForSeedUsers(userManager, createdUser);

                if (!user.IsStudent)
                {
                    userManager.AddToRole(createdUser.Id, "Teacher");
                }
            }

            //give Teacher user seed it's teacher role
            var teacherUser = userManager.FindByName(usersEmail[0]);
            var teacherUser2 = userManager.FindByName(usersEmail[3]);
            var teacherUser3 = userManager.FindByName(usersEmail[4]);
            userManager.AddToRole(teacherUser.Id, "Teacher");
            userManager.AddToRole(teacherUser2.Id, "Teacher");
            userManager.AddToRole(teacherUser3.Id, "Teacher");

        }



        private void AddCourse(ApplicationDbContext context, Courses[] coursesToCreate, Users[] users)
        {

            foreach (var course in coursesToCreate)
            {
                var teacher = GetRandomTeacher(context);
                var students = GetRandomStudent(context, users);
                context.Courses.AddOrUpdate(c => c.CourseName, new Course { CourseCode = course.CourseCode, CourseName = course.CourseName, StartDate = course.StartDate, EndDate = course.EndDate, Teacher = teacher, TeacherID = teacher.Id, Description = course.Description, CourseParticipants = students });
                context.SaveChanges();
                teacher = null;
                students = null;
            }


            /*
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
                      */
            Course seededCourse = context.Courses.Where(c => c.CourseCode == "DN-18").FirstOrDefault();
            Course seededCoursejava = context.Courses.Where(c => c.CourseCode == "JD-18").FirstOrDefault();
            Course seededCourseoffice = context.Courses.Where(c => c.CourseCode == "MO-19").FirstOrDefault();
            /*
            seededCourse.CourseParticipants.Add(CourseStudent);
            CourseStudent.UserCourse = seededCourse;
            CourseStudent.UserCourseCode = seededCourse.CourseCode;

            */
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
