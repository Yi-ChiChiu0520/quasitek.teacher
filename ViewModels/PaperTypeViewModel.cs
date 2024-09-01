using System.Numerics;
using quasitekWeb.Models;
namespace quasitekWeb.ViewModels;
public class PaperTypeViewModel
{
    public long TypeId { get; set; }
    public string TypeName { get; set; }
    public int Time { get; set; }
    public int CorrectNumber { get; set; }
    public int WrongNumber { get; set; }
    public string CorrectAnswers { get; set; }
    public string WrongAnswers { get; set; }
    public decimal Score { get; set; }

    // Additional properties for student, class, and course information
    public ICollection<RecordLog> AcedemicRecordLogs { get; set; }
    public ICollection<RecordLog> TechnicalRecordLogs { get; set; }
    public string StudentName { get; set; }
    public string ClassName { get; set; }
    public string CourseName { get; set; }

}
