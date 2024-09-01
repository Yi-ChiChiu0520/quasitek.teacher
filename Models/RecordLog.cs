using quasitekWeb.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace quasitekWeb.Models
{
    public class RecordLog{
        [Key]
        public long LogId { get; set; } // Primary Key
        public long? AcedemicTypeId { get; set; }
        public decimal? AcedemicTypeScore { get; set; }

        public long? TechTypeId { get; set; }
        public decimal? TechTypeScore { get; set; }
        public long CourseId { get; set; } // Foreign key to Course
        public string CourseCode { get; set; }
        public DateTime RecordDate { get; set; }
        public long StudentId { get; set; } // Foreign key to Student
        public string StudentNumber { get; set; }
        public int ModeId { get; set; }

        // Navigation properties
        public virtual PaperType AcedemicType { get; set; }
        public virtual PaperType TechType { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set;}
        public virtual Mode ModeNavigation { get; set; }
    }
}