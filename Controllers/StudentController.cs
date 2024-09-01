using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using quasitekWeb.ViewModels;
using OfficeOpenXml;

namespace quasitekWeb.Controllers;

// StudentController inherits from the Controller class that is provided by Microsoft.AspNetCore.Mvc
public class StudentController : Controller
{
    private ApplicationDbContext _db;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public StudentController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment){
        _db = db;
        _hostingEnvironment = hostingEnvironment;
    }
  
    public IActionResult Index(string searchTerm, int page = 1, int pageSize = 8)
{
    // Base query for students including the Classes relationship
    var studentsQuery = _db.Student.Include(s => s.Classes).AsQueryable();

    // Check if a search term is provided
    if (!string.IsNullOrEmpty(searchTerm))
    {
        // Apply filtering by student name or student number if search term is provided
        studentsQuery = studentsQuery.Where(s => s.StudentName==searchTerm || s.StudentNumber==searchTerm);
        if (studentsQuery.Count() == 0)
        {
            ViewBag.ErrorMessage = "Student not found, please enter the correct student name or student number.";
            return View();
        }
        // Save the search term in ViewBag to persist it in the view
        ViewBag.SearchTerm = searchTerm;
    }
    else
    {
        // If no search term is provided, just reset the search term
        ViewBag.SearchTerm = "";
    }

    // Calculate total items for pagination
    var totalItems = studentsQuery.Count();

    // Apply pagination
    var students = studentsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    // Create a view model for pagination
    var viewModel = new PagedViewModel<Student>
    {
        Items = students,
        PageNumber = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    // Return the view with the paginated student data
    return View(viewModel);
}


    [HttpGet]
    public IActionResult SignUp(long? classesId)
    {
        if (!classesId.HasValue || classesId == 0)
        {
            // Populate the dropdown with the available classes
            var classes = _db.Classes
                            .OrderBy(c => c.ClassesId)
                            .Select(c => new SelectListItem
                            {
                                Value = c.ClassesId.ToString(),
                                Text = c.ClassesName
                            })
                            .ToList();

            ViewBag.Classes = classes;
            
            // If no class is selected yet, return the view to select the class
            return View(new Student());  
        }

        // If a class ID is provided, check if it exists
        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        if (selectedClass == null)
        {
            TempData["ErrorMessage"] = "The class with the given ID does not exist.";
            return RedirectToAction("SignUp");
        }

        // If the class exists, redirect to the SignUpInClasses method in the Classes controller
        return RedirectToAction("SignUpInClasses", "Classes", new { classesId });
    }
    // [HttpGet]
    // public IActionResult SearchStudent(string searchTerm, int page = 1, int pageSize = 8)
    // {
    //     if (string.IsNullOrEmpty(searchTerm))
    //     {
    //         ViewBag.ErrorMessage = "請輸入搜尋條件";
    //         return View("Search");
    //     }

    //     var studentsQuery = _db.Student
    //         .Include(s => s.Classes) // Include the Classes entity
    //         .Where(s => s.StudentName.Contains(searchTerm) || s.StudentNumber.Contains(searchTerm))
    //         .AsQueryable();

    //     var totalItems = studentsQuery.Count();

    //     var students = studentsQuery
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToList();

    //     if (students.Count == 0)
    //     {
    //         ViewBag.ErrorMessage = "無查獲學生，請再試一次";
    //         return View("Search");
    //     }

    //     // Create the PagedViewModel
    //     var viewModel = new PagedViewModel<Student>
    //     {
    //         Items = students,
    //         PageNumber = page,
    //         PageSize = pageSize,
    //         TotalItems = totalItems
    //     };

    //     return View("Index", viewModel);
    // }
    // [HttpGet]
    // public IActionResult Search(){
    //     return View();
    // }
    
    [HttpPost]
    public IActionResult DeleteStudent(string searchTerm){
        var students = string.IsNullOrEmpty(searchTerm) 
                ? _db.Student.ToList() 
                : _db.Student
                .Include(s => s.Classes) // Include the Classes entity
                .Where(s => s.StudentName == searchTerm || s.StudentNumber==searchTerm)
                .ToList();
    
        if (students.Count != 0)
        {
            var studentHasRecord=_db.RecordLog.Any(r=>r.StudentNumber==students[0].StudentNumber);
            if(studentHasRecord){
                ViewBag.ErrorMessage = "Cannot delete student with records";
                return View("Delete");
            }

            foreach (var student in students){
                var studentClass = _db.Classes.FirstOrDefault(c => c.ClassesId == student.ClassesId);
                if (studentClass != null)
                {
                    studentClass.StudentAmount -= 1;
                }
            }
            _db.Student.RemoveRange(students);
            _db.SaveChanges();
            var studentName=students[0].StudentName;
            ViewBag.SuccessMessage = $"Successfully delete {studentName}";
            return View("Delete");
        }else{
            ViewBag.ErrorMessage = "Student not found, please enter the correct student name or student number.";
            return View("Delete");
        }
    }
    
    public IActionResult Delete(){
        return View();
    }

    [HttpGet]
    public IActionResult SelectStudentToUpdate(int studentId)
    {
        if (studentId == 0)
        {
            return RedirectToAction("Update");
        }

        return RedirectToAction("Update", new { studentId });
    }

    [HttpGet]
    public IActionResult Update(int studentId)
    {
        var students = _db.Student.ToList();
        ViewBag.Students = students;
        
        var classes = _db.Classes.ToList();
        ViewBag.Classes = classes;

        // If no student is selected, just show the selection form
        if (studentId == 0)
        {
            return View();
        }

        // Fetch the selected student's information
        var student = _db.Student.FirstOrDefault(s => s.StudentId == studentId);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    public IActionResult UpdateStudent(int studentId, string studentNumber, string studentName, long classesId)
    {
        if (ModelState.IsValid)
        {
            var student = _db.Student.FirstOrDefault(s => s.StudentId == studentId);

            var studentHasRecord=_db.RecordLog.Any(r=>r.StudentNumber==student.StudentNumber);
            if(studentHasRecord){
                TempData["ErrorMessage"] = "Cannot update student with records";
                ViewBag.Classes = _db.Classes.ToList();
                return View("Update", student);
            }
            
            var studentNumberExists = _db.Student.Any(s => s.StudentNumber == studentNumber && s.StudentId != studentId);
            if (studentNumberExists)
            {
                TempData["ErrorMessage"] = $"Student Number: {studentNumber} already exists";
                return View("Update", student);
            }
            if (student != null)
            {
                var oldClass = _db.Classes.FirstOrDefault(c => c.ClassesId == student.ClassesId);
                if (oldClass != null)
                {
                    oldClass.StudentAmount -= 1;
                }

                student.StudentNumber = studentNumber;
                student.StudentName = studentName;
                student.ClassesId = classesId;

                var newClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
                if (newClass != null)
                {
                    newClass.StudentAmount += 1;
                }

                _db.SaveChanges();
                TempData["SuccessMessage"] = "Student updated successfully";
                return View("Update", student);
            }
            else
            {
                ModelState.AddModelError("", "Student not found.");
            }
        }

        var classes = _db.Classes.ToList();
        ViewBag.Classes = classes;

        return View(_db.Student.FirstOrDefault(s => s.StudentId == studentId));
    }

    [HttpPost]
    public IActionResult UploadExcelStudent(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.ErrorMessage = "檔案錯誤";
            return View("SignUp"); // Return to the AddRecord view with an error message
        }
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var studentNumber = worksheet.Cells[row, 1].Text;
                        var studentName = worksheet.Cells[row, 2].Text;
                        var classesName = worksheet.Cells[row, 3].Text;
                        if (string.IsNullOrEmpty(studentNumber) || string.IsNullOrEmpty(studentName)|| string.IsNullOrEmpty(classesName))
                        {
                            ViewBag.ErrorMessage = $"列 {row}: 學號或姓名或班級名稱不能為空";
                            return View("SignUp");
                        }

                        var studentExists = _db.Student.Any(s => s.StudentNumber == studentNumber);
                        if (studentExists)
                        {
                            ViewBag.ErrorMessage = $"列 {row}: 學號 {studentNumber} 已存在";
                            return View("SignUp");
                        }

                        var classes= _db.Classes.FirstOrDefault(c => c.ClassesName == classesName);
                        if (classes == null)
                        {
                            ViewBag.ErrorMessage = $"列 {row}: 班級 {worksheet.Cells[row, 3].Text} 不存在";
                            return View("SignUp");
                        }

                        var student = new Student
                        {
                            StudentNumber = worksheet.Cells[row, 1].Text,
                            StudentName = worksheet.Cells[row, 2].Text,
                            ClassesId = classes.ClassesId
                        };

                        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == student.ClassesId);
                        if (selectedClass != null)
                        {
                            selectedClass.StudentAmount += 1; // Assuming StudentCount is a property in the Classes model
                            _db.SaveChanges();
                        }
                        // Additional properties can be set here

                        _db.Student.Add(student);
                    }

                    _db.SaveChanges();
                }
            }
            ViewBag.SuccessMessage = "上傳成功";
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
        }

        return View("SignUp");
    }
    [HttpGet]
    public IActionResult DownloadExampleFile()
    {
        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "files", "學生資料上傳範例檔.xlsx");
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "學生資料上傳範例檔.xlsx");
    }
}