using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Module
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ICollection<Activity> ModuleActivities { get; set; }
        public ICollection<Document> ModuleDocuments { get; set; }

        public virtual Course Course { get; set; }
    }
}