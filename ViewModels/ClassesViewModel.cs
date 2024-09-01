using Microsoft.AspNetCore.Mvc.Rendering;
using quasitekWeb.Models;
using System.Collections.Generic;
namespace quasitekWeb.ViewModels;

public class ClassesViewModel
{
    public long ClassesId { get; set; }
    public string ClassesName { get; set; }
    public string TeacherName { get; set; }
    public int StudentAmount { get; set; }
    public string CourseGroupName { get; set; }
}