﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lexicon_LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "The forename is required")]
        public string Forename { get; set; }

        [Required(ErrorMessage = "The surname is required")]
        public string Surname { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get { return Forename + " " + Surname; } }

        [Display(Name = "Registrated since")]
        public DateTime TimeOfRegistration { get; set; }

        public virtual Course UserCourse { get; set; }
        public string UserCourseCode { get; set; }

        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Lexicon_LMS.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<Lexicon_LMS.Models.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<Lexicon_LMS.Models.Activity> Activities { get; set; }

        public System.Data.Entity.DbSet<Lexicon_LMS.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<Lexicon_LMS.Models.Document> Documents { get; set; }

        //TODO: keep track of this line it is automatically created on scaffold and breaks the application
        //public System.Data.Entity.DbSet<Lexicon_LMS.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
