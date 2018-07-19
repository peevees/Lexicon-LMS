using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Lexicon_LMS.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ICollection<Module> CourseModules { get; set; }
        public ICollection<Document> CourseDocuments { get; set; }

        public ApplicationUser Teacher { get; set; }
        public ICollection<ApplicationUser> CourseParticipants { get; set; }
    }
}