using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using quasitekWeb.ViewModels;

namespace quasitekWeb.Controllers;


public class CourseController : Controller
{
    private ApplicationDbContext _db;
    private static readonly Dictionary<string, string> CourseGroupDictionary = new Dictionary<string, string>
    {
        { "室內配線甲級", "001" },
        { "室內配線乙級", "002" },
        { "室內配線丙級", "003" }
    };

    private static readonly Dictionary<string, List<Tuple<string, string>>> Coursedictionary = new Dictionary<string, List<Tuple<string, string>>>
    {
        { "001", new List<Tuple<string, string>> 
            {
                Tuple.Create("001001", "屋內線路裝修甲級"),
                Tuple.Create("040001", "配電線路裝修甲級"),
                Tuple.Create("016001", "自來水管配管甲級")
            }
        },
        { "002", new List<Tuple<string, string>> 
            {
                Tuple.Create("001002", "屋內線路裝修乙級"),
                Tuple.Create("040002", "配電線路裝修乙級"),
                Tuple.Create("016002", "自來水管配管乙級")
            }
        },
        { "003", new List<Tuple<string, string>> 
            {
                Tuple.Create("001003", "屋內線路裝修丙級"),
                Tuple.Create("040003", "配電線路裝修丙級"),
                Tuple.Create("016003", "自來水管配管丙級")
            }
        }
    };
    public CourseController(ApplicationDbContext db){
        _db = db;
    }

    [HttpGet]
public IActionResult Index(int page = 1, int pageSize = 3)
{
    var sortedCourses = _db.Course
        .Include(c => c.Classes)
        .OrderBy(c => c.Classes.ClassesId)
        .ToList()
        .GroupBy(c => c.Classes.CourseGroupName)
        .Select(g => new CourseGroupViewModel
        {
            CourseGroupName = g.Key,
            Classes = g.Select(c => new ClassesViewModel
            {
                ClassesId = c.Classes.ClassesId,
                ClassesName = c.Classes.ClassesName,
            }).Distinct().ToList(),
            Courses = g.Select(c => new CourseViewModel
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
            }).Distinct().ToList()
        })
        .ToList();

    // Pagination logic
    var paginatedCourses = sortedCourses.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var totalItems = sortedCourses.Count;

    var viewModel = new PagedViewModel<CourseGroupViewModel>
    {
        Items = paginatedCourses,
        PageNumber = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    return View(viewModel);
}



}