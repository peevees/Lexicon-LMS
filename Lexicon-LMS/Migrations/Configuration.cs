namespace Lexicon_LMS.Migrations
{
    using System;
    using Lexicon_LMS.Models;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Lexicon_LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Lexicon_LMS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Courses.AddOrUpdate(
                c => c.CourseName,
                new Course
                {
                    CourseName = ".NET Development",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2019, 7, 19),
                    Description = "A course in .NET Development",
                    CourseCode = "DN-18"
                }
                );

            context.Modules.AddOrUpdate(
                m => m.Description,
                new Module
                {
                    Course = context.Courses.Where(c => c.CourseCode == "DN-18").FirstOrDefault(),
                    Description = "Basics of C#",
                    StartDate = new DateTime(2018, 7, 19),
                    EndDate = new DateTime(2018,8,6)
                }
                );

            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "Hello World!",
                    Deadline = new DateTime(2018,7,20)
                }
                );
        }
    }
}
