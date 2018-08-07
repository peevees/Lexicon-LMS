using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Upload date")]
        public DateTime UploadDate { get; set; }

        public string Filepath { get; set; }
        public int? CourseID { get; set; }
        public int? ModuleID { get; set; }
        public int? ActivityID { get; set; }
        public bool UserAssignment { get; set; } = false;

        public virtual ApplicationUser User { get; set; }
    }
}
