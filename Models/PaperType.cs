using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace quasitekWeb.Models
{
    public class PaperType
    {
        [Key]
        public long TypeId { get; set; } // Primary Key
        public string TypeName { get; set; }
        public int Time { get; set; }
        public int CorrectNumber { get; set; }
        public int WrongNumber { get; set; }
        public string CorrectAnswers { get; set; }
        public string WrongAnswers { get; set; }
        public virtual ICollection<RecordLog> AcademicRecordLogs { get; set; } // For academic type
        public virtual ICollection<RecordLog> TechnicalRecordLogs { get; set; } // For technical type
    }
}