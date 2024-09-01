namespace quasitekWeb.Models;

public class Student{

    public long StudentId {get; set;}
    public string StudentNumber {get; set;}
    public string StudentName {get; set;}
    public long ClassesId {get; set;}
    public Classes Classes { get; set; } // Navigation property
}