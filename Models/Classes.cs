namespace quasitekWeb.Models
{
    public class Classes
    {
        public long ClassesId { get; set; }
        public string ClassesName { get; set; }
        public long TeacherId { get; set; }
        public int StudentAmount { get; set; }
        public string CourseGroupName { get; set; }
        public Teacher Teacher { get; set; } // Navigation property to Teacher
        public ICollection<Student> Students { get; set; } 
        public ICollection<Course> Course { get; set; }
    }
}
