using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<ApplicationUser> Recipients { get; set; }
        public ApplicationUser Sender { get; set; }
        public Document Attachment { get; set; }
        public bool Read { get; set; } = false;
    }

}