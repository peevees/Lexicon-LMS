using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lexicon_LMS.Models
{
    public class Notification
    {
        public int ID { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Body { get; set; }
        public virtual List<ApplicationUser> Recipients { get; set; }
        [Required]
        public string RecipientID { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public Document Attachment { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateSent { get; set; }

        public bool Read { get; set; } = false;
    }

}
