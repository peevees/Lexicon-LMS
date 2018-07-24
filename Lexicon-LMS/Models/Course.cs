using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Models
{
    public class Course
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Course code is required")]
        [Display(Name = "Course code")]
        [RegularExpression("^\\s*([A-Z]{2}-[0-9]{2})\\s*$",
            ErrorMessage ="The course code need to be in the format two capital letters, a dash and two numbers. For example:\"NG-18\"")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [Display(Name = "Course name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        [Display(Name = "Course modules")]
        public virtual ICollection<Module> CourseModules { get; set; }

        [Display(Name = "Course documents")]
        public virtual ICollection<Document> CourseDocuments { get; set; }

        public ApplicationUser Teacher { get; set; }

        [Display(Name = "Course participants")]
        public ICollection<ApplicationUser> CourseParticipants { get; set; }
    }
}
