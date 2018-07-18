using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Activity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<Document> Documents { get; set; }
    }

    public class Assignment : Activity
    {

    }
}