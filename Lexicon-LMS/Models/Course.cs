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

        [Display(Name = "Course code")]
        public string CourseCode { get; set; }

        [Display(Name = "Course name")]
        public string CourseName { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }
        public virtual ICollection<Module> CourseModules { get; set; }
        public virtual ICollection<Document> CourseDocuments { get; set; }

        public virtual ApplicationUser Teacher { get; set; }
        public string TeacherID { get; set; }
        public virtual ICollection<ApplicationUser> CourseParticipants { get; set; }
    }
}