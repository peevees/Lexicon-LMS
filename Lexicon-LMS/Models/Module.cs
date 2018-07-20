using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexicon_LMS.Models
{
    public class Module
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Activity> ModuleActivities { get; set; }
        public virtual ICollection<Document> ModuleDocuments { get; set; }

        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
    }
}