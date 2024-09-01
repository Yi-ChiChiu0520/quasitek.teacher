using Microsoft.AspNetCore.Mvc.Rendering;
using quasitekWeb.Models;
using System.Collections.Generic;
using quasitekWeb.ViewModels;

namespace quasitekWeb.ViewModels
{
    public class CourseGroupViewModel
    {
        public string CourseGroupName { get; set; }
        public List<ClassesViewModel> Classes { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}