using System;
namespace quasitekWeb.Models
{
    public class LogWrapper
    {
        public List<Log> Log { get; set; }
    }
    public class Log
    {
        public string StudentNumber { get; set; } // This should match your JSON "StudentId"
        public DateTime LogDate { get; set; } // This should match your JSON "RecordDate"
        public string CourseCode { get; set; } // This should match your JSON "CourseNumber"
        public int ModeId { get; set; }
        public IEnumerable<TypeData> Type { get; set; } // This should match your JSON array "Type"
    }

    public class TypeData // Renaming the class to avoid conflict with the system `Type` class
    {
        public string Name { get; set; } // This should match your JSON "Name"
        public bool HasTest { get; set; }
        public int Time { get; set; } // This should match your JSON "Time"
        public IEnumerable<Detail> Detail { get; set; }
    }

    public class Detail
    {
        public List<string> Correct { get; set; }
        public List<string> Wrong { get; set; }
    }

    public class Answer
    {
        public int QuestionNumber { get; set; }
        public bool Correct { get; set; }
    }
}
