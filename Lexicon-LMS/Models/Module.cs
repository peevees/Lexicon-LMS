using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lexicon_LMS.Models
{
    public class Module
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Module title")]
        public string ModuleTitle { get; set; }

        [Display(Name = "Module description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Module activities")]
        public virtual ICollection<Activity> ModuleActivities { get; set; }

        [Display(Name = "Module documents")]
        public virtual ICollection<Document> Documents { get; set; }

        [Display(Name = "Course code")]
        public string CourseCode { get; set; }


        public virtual Course Course { get; set; }
    }
}
