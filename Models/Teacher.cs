namespace quasitekWeb.Models;

public class Teacher{
    
    public long TeacherId {get; set;}
    public string TeacherName {get; set;}
    public string Email {get; set;}
    public string Pw {get;set;}
    public string Authorize {get;set;}
    public ICollection<Classes> Classes { get; set; } // If one teacher teaches multiple classes

 
}