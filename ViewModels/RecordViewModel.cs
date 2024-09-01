using System.Numerics;
using quasitekWeb.Models;
namespace quasitekWeb.ViewModels;
public class RecordViewModel
{
    public int RecordId { get; set; }
    public DateTime RecordDate { get; set; }
    public long CourseId {get;set;}
    public string CourseName { get; set; }
    public string Mode { get; set; }
    public int QuestionNum { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; }
    public long ClassesId { get; set; }
    public string ClassesName { get; set; }
    public int TestScore { get; set; }
    public int MaxTestScore {get; set;}
    public int TestTime { get; set; }
    public int MaxTestTime {get; set;}
    public Course Course { get; set; }
    public Student Student { get; set; }
    public Classes Classes { get; set; }
    public string FormattedTestScore => TestScore == -1 ? "無" : $"{TestScore}/{MaxTestScore}";
    public string FormattedTestTime => TestTime == -1 ? "無" : $"{TestTime}/{MaxTestTime}";
}
