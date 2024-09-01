using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
namespace quasitekWeb.Controllers;

public class TeacherController : Controller
{
    private ApplicationDbContext _db;

    public TeacherController(ApplicationDbContext db){
        _db = db;
    }
    public IActionResult Index(){
        var teacher = _db.Teacher.ToList();
        return View(teacher); 
    }

    [HttpPost]
    public IActionResult SignUpTeacher(Teacher teacher){
        _db.Teacher.Add(teacher);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult SignUp(){

        return View();
    }
}