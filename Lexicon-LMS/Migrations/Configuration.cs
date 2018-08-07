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
        private ApplicationDbContext db;

        //UNDONE: Add all properties that a user shall have by default
        private List<ApplicationUser> Teachers = new List<ApplicationUser>();
        private List<ApplicationUser> Students = new List<ApplicationUser>();
        private List<Course> courses = new List<Course>();
        private List<Module> modules = new List<Module>();
        private List<Activity> activities = new List<Activity>();

        #region structs
        //private struct Users
        //{
        //    public string Email { get; set; }
        //    public string Username => Email;
        //    public string Password { get; set; }
        //    public string Forename { get; set; }
        //    public string Surname { get; set; }
        //    public DateTime TimeofRegistration { get; set; }
        //    public string Street { get; set; }
        //    public string Postcode { get; set; }
        //    public string City { get; set; }
        //    public bool IsStudent { get; set; }
        //}

        ////UNDONE: Add all properties that a courses shall have by default
        //private struct Courses
        //{
        //    public string CourseName { get; set; }
        //    public DateTime StartDate { get; set; }
        //    public DateTime EndDate { get; set; }
        //    public string Description { get; set; }
        //    public string CourseCode { get; set; }
        //    public ApplicationUser Teacher { get; set; }
        //    public string TeacherId => Teacher.Id;
        //    public ApplicationUser CoursePaticipant { get; set; }
        //}

        //private struct Modules
        //{
        //    public string Description { get; set; }
        //    public DateTime StartDate { get; set; }
        //    public Course Course { get; set; }
        //    public int CourseId => Course.ID;
        //}

        //private struct Activities
        //{
        //    public string Name { get; set; }
        //    public DateTime Deadline { get; set; }
        //    public Module Module { get; set; }
        //    public int ModuleId => Module.ID;
        //}
        #endregion

        #endregion

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Lexicon-LMS.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            #region Explaination
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            #endregion

            //IDEA: instead of struct create array or list of application user, course, modules etc etc easier to work with perhaps?

            //HACK: SEED DEBUGGER
            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Launch();
            db = context;
            //if (context.Database.Exists())
            //{
            //    context.Database.Connection.Close();
            //    context.Database.Delete();
            //    throw new Exception("Database has no been purged please rerun with Update-Database -Force");
            //}

            //REMINDER: Add Teachers here!
            Teachers.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Email = "Teacher@shit.se",
                    UserName = "Teacher@shit.se",
                    Forename = "Mr.",
                    Surname = "Teacher",
                    TimeOfRegistration = new DateTime(1990, 01, 01),
                    Street = "SomeStreetName 0",
                    Postcode = "00000",
                    City = "SomeCityName"
                },
                new ApplicationUser
                {
                    Email = "John.Doe@shit.se",
                    UserName = "John.Doe@shit.se",
                    Forename = "John",
                    Surname = "Doe",
                    TimeOfRegistration = GetRandomDate(),
                    Street = "Primrose Lane 2",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Lillsand"
                },
                new ApplicationUser
                {
                    Email = "Jane.Doe@shit.se",
                    UserName = "Jane.Doe@shit.se",
                    Forename = "Jane",
                    Surname = "Doe",
                    TimeOfRegistration = GetRandomDate(),
                    Street = "Jackson Avenue 3",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Sickla"
                }

            });

            //REMINDER: Add Students here!
            Students.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Email = "student@shit.se",
                    UserName = "student@shit.se",
                    Forename = "Mr.",
                    Surname = "Student",
                    TimeOfRegistration = new DateTime(1990, 01, 01),
                    Street = "SomeStreetName 0",
                    Postcode = "00000",
                    City = "SomeCityName"
                },
                new ApplicationUser
                {
                    Email = "Johnny.Roe@shit.se",
                    UserName = "Johnny.Roe@shit.se",
                    Forename = "Johnny",
                    Surname = "Roe",
                    TimeOfRegistration = GetRandomDate(),
                    Street = "Howard Street 1",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Nocketorp"
                },
                new ApplicationUser
                {
                    Email = "Janie.Roe@shit.se",
                    UserName = "Janie.Roe@shit.se",
                    Forename = "Janie",
                    Surname = "Roe",
                    TimeOfRegistration = GetRandomDate(),
                    Street = "Roberts Road 1",
                    Postcode = GetRandomNumber().ToString(),
                    City = "Medvik"
                }
            });

            //REMINDER: Add Courses here!
            courses.AddRange(new List<Course>
            {
                new Course
                {
                    CourseCode = "DN-18",
                    CourseName = ".NET Development",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2019, 7, 19),
                    Description = "A course in .NET Development",
                },
                new Course
                {
                    CourseName = "Java Development",
                    StartDate = new DateTime(2018, 8, 19),
                    EndDate = new DateTime(2019, 8, 19),
                    Description = "Boil coffee",
                    CourseCode = "JD-18"
                },
                new Course
                {
                    CourseName = "Office 365",
                    StartDate = new DateTime(2019, 8, 19),
                    EndDate = new DateTime(2020, 8, 19),
                    Description = "Create Documents, Spreadsheets and Presentations",
                    CourseCode = "MO-19"
                }
            });

            //REMINDER: Add Modules here!
            modules.AddRange(new List<Module>
            {

                new Module
                {
                    ModuleTitle = "C# Basics",
                    Description = "We learn the basics of C#",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2018, 7, 31),
                },

                new Module
                {
                    ModuleTitle = "Java Measuring",
                    Description = "Measuring the right amount of Java",
                    StartDate = new DateTime(2018, 8, 19),
                    EndDate = new DateTime(2018, 8, 31)
                },

                new Module
                {
                    ModuleTitle = "Word of words",
                    Description = "Predicting words in Word",
                    StartDate = new DateTime(2019, 8, 19),
                    EndDate = new DateTime(2019, 8, 31),
                },

            });

            //REMINDER: Add Activities here!
            activities.AddRange(new List<Activity>
            {
                new Activity
                {
                    Name = "Hello World!",
                    Deadline = new DateTime(2018, 7, 20),
                },

                new Activity
                {
                    Name = "Java Cup!",
                    Deadline = new DateTime(2018, 8, 20),
                },

                new Activity
                {
                    Name = "Spreading the Sheets!",
                    Deadline = new DateTime(2019, 8, 20),
                },
            });


            #region adding data to structs
            //            var usersToAdd = new[]
            //            {
            //                #region Teachers
            //                new Users
            //                {
            //                    Email = "Teacher@shit.se",
            //                    Password = "P@$$w0rd",
            //                    Forename = "Mr. ",
            //                    Surname = "Teacher",
            //                    TimeofRegistration = new DateTime(1990, 01, 01),
            //                    Street = "SomeStreetName 0",
            //                    Postcode = "00000",
            //                    City = "SomeCityName",
            //                    IsStudent = false
            //                },
            //                new Users
            //                {
            //                    Email = "John.Doe@shit.se",
            //                    Password = "JohnDoe",
            //                    Forename = "John",
            //                    Surname = "Doe",
            //                    TimeofRegistration = GetRandomDate(),
            //                    Street = "Primrose Lane 2",
            //                    Postcode = GetRandomNumber().ToString(),
            //                    City = "Lillsand",
            //                    IsStudent = false
            //                },
            //                new Users
            //                {
            //                    Email = "Jane.Doe@shit.se",
            //                    Password = "JaneDoe",
            //                    Forename = "Jane",
            //                    Surname = "Doe",
            //                    TimeofRegistration = GetRandomDate(),
            //                    Street = "Jackson Avenue 3",
            //                    Postcode = GetRandomNumber().ToString(),
            //                    City = "Sickla",
            //                    IsStudent = false
            //                },
            //                #endregion

            //                #region Template
            //                /*Mock structure
            //                new Users
            //                {
            //                    Email = "",
            //                    Password = "",
            //                    Forename = "",
            //                    Surname = "",
            //                    TimeofRegistration = GetRandomDate(),
            //                    Street = "",
            //                    Postcode = GetRandomNumber().ToString(),
            //                    City = "",
            //                    IsStudent = false
            //                },*/
            //#endregion

            //                //REMINDER: Students below, Teachers above
            //                #region Students
            //                new Users
            //                {
            //                    Email = "student@shit.se",
            //                    Password = "P@$$w0rd",
            //                    Forename = "Mr. ",
            //                    Surname = "Student",
            //                    TimeofRegistration = new DateTime(),
            //                    Street = "SomeStreetName 0",
            //                    Postcode = "00000",
            //                    City = "SomeCityName",
            //                    IsStudent = true
            //                },
            //                new Users
            //                {
            //                    Email = "Johnny.Roe@shit.se",
            //                    Password = "JohnnyRoe",
            //                    Forename = "Johnny",
            //                    Surname = "Roe",
            //                    TimeofRegistration = GetRandomDate(),
            //                    Street = "Howard Street 1",
            //                    Postcode = GetRandomNumber().ToString(),
            //                    City = "Nocketorp",
            //                    IsStudent = true
            //                },
            //                new Users
            //                {
            //                    Email = "Janie.Roe@shit.se",
            //                    Password = "JanieRoe",
            //                    Forename = "Janie",
            //                    Surname = "Roe",
            //                    TimeofRegistration = GetRandomDate(),
            //                    Street = "Roberts Road 1",
            //                    Postcode = GetRandomNumber().ToString(),
            //                    City = "Medvik",
            //                    IsStudent = true
            //                }
            //#endregion
            //            };

            //            //REMINDER: Add courses here
            //            var coursesToAdd = new[]
            //            {
            //                 new Courses
            //                {
            //                    CourseName = ".NET Development",
            //                    StartDate = new DateTime(2018, 7, 19),
            //                    EndDate = new DateTime(2019, 7, 19),
            //                    Description = "A course in .NET Development",
            //                    CourseCode = "DN-18"
            //                },

            //                 new Courses
            //                {
            //                    CourseName = "Java Development",
            //                    StartDate = new DateTime(2018, 8, 19),
            //                    EndDate = new DateTime(2019, 8, 19),
            //                    Description = "Boil coffee",
            //                    CourseCode = "JD-18"
            //                },

            //                 new Courses
            //                {
            //                    CourseName = "Office 365",
            //                    StartDate = new DateTime(2019, 8, 19),
            //                    EndDate = new DateTime(2020, 8, 19),
            //                    Description = "Create Documents, Spreadsheets and Presentations",
            //                    CourseCode = "MO-19"
            //                }
            //            };
            #endregion

            var roleNames = new[] { "Teacher" };//REMINDER: add roles here!
            AddRoles(roleNames);

            //var users = new[] { "teacher@shit.se", "student@shit.se", "student2@shit.se", "teacher2@shit.se", "teacher3@shit.se", "student3@shit.se" };//REMINDER: add users here!
            AddUsers();

            AddCourse();

            AddModule();

            AddActivity();
        }



        #region Utilities
        private ApplicationUser GetRandomTeacher()
        {
            ApplicationUser Teacher = null;
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

        private ICollection<ApplicationUser> GetRandomStudent()
        {
            ApplicationUser student = null;
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

        private void AddRoles(string[] roles)
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

        private void AddUsers()
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var Teacher in Teachers)
            {
                /*
                #region check existing users
                if (db.Users.Any(u => u.UserName == Teacher.UserName))
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
                  */
                //var createdUser = new ApplicationUser { Forename = Teacher.Forename, Surname = Teacher.Surname, UserName = Teacher.Username, Email = Teacher.Email, TimeOfRegistration = Teacher.TimeofRegistration, City = Teacher.City, Street = Teacher.Street, Postcode = Teacher.Postcode };
                //var user = new ApplicationUser { Forename = "Mr", Surname = "JohnDoe", UserName = userEmail, Email = userEmail, TimeOfRegistration = new DateTime(2000, 01, 01, 00, 00, 00) };
                CreateUser(userManager, Teacher);
                userManager.AddToRole(Teacher.Id, "Teacher");
            }
            foreach (var Student in Students)
            {
                CreateUser(userManager, Student);
            }
        }
        private void CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var password = user.Forename + user.Surname;
            if (db.Users.Any(u => u.Email == user.Email))
            {

                //throw new Exception("User already exist, tried deleting your database?");
                return;
                /*
                var updateUser = db.Users.Where(u => u.Email == user.Email).First();

                //userManager.Delete(updateUser);


                updateUser.Email = user.Email;
                updateUser.UserName = user.UserName;
                updateUser.Forename = user.Forename;
                updateUser.Surname = user.Surname;
                updateUser.Postcode = user.Postcode;
                updateUser.Street = user.Street;
                updateUser.City = user.City;
                updateUser.PhoneNumber = user.PhoneNumber;
                updateUser.TimeOfRegistration = user.TimeOfRegistration;
                updateUser.Id = user.Id;

                userManager.RemovePassword(updateUser.Id);
                userManager.AddPassword(updateUser.Id, password);
                userManager.Update(updateUser);
                db.SaveChanges();

                SetConfirmEmailForSeedUsers(userManager, updateUser);
                Teachers.Remove(user);
                Teachers.Add(updateUser);
                  */
            }

            var result = userManager.Create(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("Failed to create user: " + "/n", result.Errors));
            }
            db.SaveChanges();
            SetConfirmEmailForSeedUsers(userManager, user);
        }

        //give Teacher user seed it's teacher role
        //var teacherUser = userManager.FindByName(usersEmail[0]);
        //var teacherUser2 = userManager.FindByName(usersEmail[3]);
        //var teacherUser3 = userManager.FindByName(usersEmail[4]);
        //userManager.AddToRole(teacherUser.Id, "Teacher");
        //userManager.AddToRole(teacherUser2.Id, "Teacher");
        //userManager.AddToRole(teacherUser3.Id, "Teacher");


        private void AddCourse()
        {

            foreach (var course in courses)
            {
                var teacher = GetRandomTeacher();
                var students = GetRandomStudent();
                course.Teacher = teacher;
                course.TeacherID = teacher.Id;
                course.CourseParticipants = students;
                db.Courses.AddOrUpdate(c => c.CourseName, course);

                var teacherToUpdate = Teachers.Where(s => s.UserName == teacher.UserName).FirstOrDefault();
                if (teacherToUpdate != null)
                {
                    teacherToUpdate.UserCourse = course;
                    teacherToUpdate.UserCourseCode = course.CourseCode;
                }

                foreach (var student in students)
                {
                    var studenttoChange = Students.Where(s => s.UserName == student.UserName).FirstOrDefault();
                    if (studenttoChange != null)
                    {
                        studenttoChange.UserCourse = course;
                        studenttoChange.UserCourseCode = course.CourseCode;
                    }
                }
                db.SaveChanges();
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
            Course seededCourse = db.Courses.Where(c => c.CourseCode == "DN-18").FirstOrDefault();
            Course seededCoursejava = db.Courses.Where(c => c.CourseCode == "JD-18").FirstOrDefault();
            Course seededCourseoffice = db.Courses.Where(c => c.CourseCode == "MO-19").FirstOrDefault();
            /*
            seededCourse.CourseParticipants.Add(CourseStudent);
            CourseStudent.UserCourse = seededCourse;
            CourseStudent.UserCourseCode = seededCourse.CourseCode;

            */
            db.Modules.AddOrUpdate(
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

            db.Modules.AddOrUpdate(
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

            db.Modules.AddOrUpdate(
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

            db.SaveChanges();
            Module seededModule = db.Modules.Where(c => c.CourseCode == seededCourse.CourseCode).FirstOrDefault();
            Module seededModulejava = db.Modules.Where(c => c.CourseCode == seededCoursejava.CourseCode).FirstOrDefault();
            Module seededModuleoffice = db.Modules.Where(c => c.CourseCode == seededCourseoffice.CourseCode).FirstOrDefault();

            seededCourse.CourseModules.Add(seededModule);
            seededCoursejava.CourseModules.Add(seededModulejava);
            seededCourseoffice.CourseModules.Add(seededModuleoffice);

            db.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Hello World!",
                    Deadline = new DateTime(2018, 7, 20),
                    Module = seededModule,
                    ModuleID = seededModule.ID
                });
            db.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Java Cup!",
                    Deadline = new DateTime(2018, 8, 20),
                    Module = seededModulejava,
                    ModuleID = seededModulejava.ID
                });
            db.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Spreading the Sheets!",
                    Deadline = new DateTime(2019, 8, 20),
                    Module = seededModuleoffice,
                    ModuleID = seededModuleoffice.ID
                });
            db.SaveChanges();
            Activity seededActivity = db.Activities.Where(c => c.ModuleID == seededModule.ID).FirstOrDefault();
            Activity seededActivityjava = db.Activities.Where(c => c.ModuleID == seededModulejava.ID).FirstOrDefault();
            Activity seededActivityoffice = db.Activities.Where(c => c.ModuleID == seededModuleoffice.ID).FirstOrDefault();
            seededModule.ModuleActivities.Add(seededActivity);
            seededModulejava.ModuleActivities.Add(seededActivityjava);
            seededModuleoffice.ModuleActivities.Add(seededActivityoffice);

        }

        private void AddModule()
        {
            throw new NotImplementedException();
        }
        private void AddActivity()
        {
            throw new NotImplementedException();
        }
    }
}
