using System;
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
        /*                           
            Why to not validate names, however they should probably be required but then a validation method is needed
        https://stackoverflow.com/questions/6999772/how-to-validate-names-in-asp-net-mvc-so-accents-are-allowed-%C3%A9-%C3%A1
            */

        //[Required(ErrorMessage = "The forename is required")]
        public string Forename { get; set; }

        //[Required(ErrorMessage = "The surname is required")]
        public string Surname { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get { return Forename + " " + Surname; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Registered since")]
        public DateTime TimeOfRegistration { get; set; }

        [Display(Name = "User course")]
        public virtual Course UserCourse { get; set; }

        public string UserCourseCode { get; set; }

        public string Street { get; set; }
        public string Postcode { get; set; }//regex example: ([0-9]){3} ([0-9]){2} (can be simplified?)
        public string City { get; set; }

        [DataType(DataType.PhoneNumber)]
        //[DisplayFormat(DataFormatString = "{#########}", ApplyFormatInEditMode = true)]//regex example: ([0-9]){3}-([0-9]){3} ([0-9]){2} ([0-9]){2}  (can be simplified?)
        [Display(Name = "Phone number")]
        public override string PhoneNumber { get; set; }


        //[Required]
        //[EmailAddress]
        //public override string Email { get; set; }


        public virtual IList<Notification> Notifications { get; set; }

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


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Course>()
        //        .HasRequired(a => a)
        //        .WithOptional(a => a)
        //        .WillCascadeOnDelete(true);
        //}

    }
}
