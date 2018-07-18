using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Lexicon_LMS.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Filepath { get; set; }
    }
}