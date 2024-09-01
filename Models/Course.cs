namespace quasitekWeb.Models;

public class Course{
    public long ClassesId {get; set;}
    public string CourseGroup {get; set;}
    public string CourseCode {get; set;}
    public long CourseId {get; set;}
    public string CourseName {get; set;}
    public string CourseIntro {get;set;}
    public virtual Classes Classes { get; set; } // Navigation property
}