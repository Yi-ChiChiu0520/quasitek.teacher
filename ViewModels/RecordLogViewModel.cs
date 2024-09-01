using System.Numerics;
using quasitekWeb.Models;
namespace quasitekWeb.ViewModels;
public class RecordLogViewModel
{
    public long LogId { get; set; }
    public long StudentId { get; set; }
    public string StudentNumber { get; set; }
    public string StudentName { get; set; }
    public long ClassesId { get; set; }
    public string ClassesName { get; set; }
    public long CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime RecordDate { get; set; }
    public int ModeId { get; set; }
    public string ModeName { get; set; }
    public long AcedemicTypeId { get; set; }
    public decimal? AcedemicTypeScore { get; set; }
    public long TechTypeId { get; set; }
    public decimal? TechTypeScore { get; set; }
    public string FormattedAcedemicTestScore => AcedemicTypeScore == null ? "N/A" : $"{AcedemicTypeScore}%";
    public string FormattedTechTestScore => TechTypeScore == null ? "N/A" : $"{TechTypeScore}%";

}
