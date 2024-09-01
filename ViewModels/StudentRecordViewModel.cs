
using System.Numerics;

namespace quasitekWeb.ViewModels;
public class StudentRecordViewModel
{
    public long StudentId { get; set; }
    public string StudentName { get; set; }
    public SortedSet<int> PracticeQuestions { get; set; } = new SortedSet<int>();
    public SortedSet<int> TestQuestions { get; set; } = new SortedSet<int>();
    public List<(int score, int time)> TestScoresAndTimes { get; set; } = new List<(int score, int time)>();
}
