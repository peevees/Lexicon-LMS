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
            AutomaticMigrationsEnabled = false;
            ContextKey = "Lexicon-LMS.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var roleNames = new[] { "Teacher" };//REMINDER: add roles here!
            AddRoles(context, roleNames);

            var users = new[] { "teacher@shit.se", "student@shit.se" };//REMINDER: add users here!
            AddUsers(context, users);
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
                var user = new ApplicationUser { UserName = userEmail, Email = userEmail, TimeOfRegistration = new DateTime(2000, 01, 01, 00, 00, 00) };
                var result = userManager.Create(user, "P@$$w0rd");
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("/n", result.Errors));
                }
            }
            var teacherUser = userManager.FindByName("teacher@shit.se");
            userManager.AddToRole(teacherUser.Id, "Teacher");
        }
    }
}
