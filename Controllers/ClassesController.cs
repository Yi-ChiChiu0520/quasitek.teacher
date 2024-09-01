using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using quasitekWeb.ViewModels;
using OfficeOpenXml;

namespace quasitekWeb.Controllers;
public class ClassesController : Controller
{
    private static readonly Dictionary<string, string> CourseGroupDictionary = new Dictionary<string, string>
    {
        { "Indoor Wiring Level A", "001" },
        { "Indoor Wiring Level B", "002" },
        { "Indoor Wiring Level C", "003" }
    };

    private static readonly Dictionary<string, List<Tuple<string, string>>> Coursedictionary = new Dictionary<string, List<Tuple<string, string>>>
    {
        { "001", new List<Tuple<string, string>> 
            {
                Tuple.Create("001001", "Indoor Circuit Installation Level A"),
                Tuple.Create("040001", "Power Distribution Circuit Installation Level A"),
                Tuple.Create("016001", "Water Pipe Installation Level A")
            }
        },
        { "002", new List<Tuple<string, string>> 
            {
                Tuple.Create("001002", "Indoor Circuit Installation Level B"),
                Tuple.Create("040002", "Power Distribution Circuit Installation Level B"),
                Tuple.Create("016002", "Water Pipe Installation Level B")
            }
        },
        { "003", new List<Tuple<string, string>> 
            {
                Tuple.Create("001003", "Indoor Circuit Installation Level C"),
                Tuple.Create("040003", "Power Distribution Circuit Installation Level C"),
                Tuple.Create("016003", "Water Pipe Installation Level C")
            }
        }
    };

    private ApplicationDbContext _db;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public ClassesController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment){
        _db = db;
        _hostingEnvironment = hostingEnvironment;
    }
public IActionResult Index(string searchTerm, int page = 1, int pageSize = 8)
{
    // Start with the base query
    var classesQuery = _db.Classes
                        .Include(c => c.Course) // Include related courses
                        .OrderBy(c => c.ClassesName)  // Order by ClassesName
                        .AsQueryable();

    // Apply search filter if a search term is provided
    if (!string.IsNullOrEmpty(searchTerm))
    {
        classesQuery = classesQuery.Where(c => c.ClassesName==(searchTerm) || c.CourseGroupName==(searchTerm));

        // Check if no results were found
        if (!classesQuery.Any())
        {
            ViewBag.ErrorMessage = $"Class: {searchTerm} not found, please enter the correct class name or course subject.";
        }

        ViewBag.SearchTerm = searchTerm;
    }
    else
    {
        ViewBag.SearchTerm = "";
    }

    // Pagination logic
    var totalItems = classesQuery.Count();
    var paginatedClasses = classesQuery
                        .OrderBy(c => c.ClassesId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(c => new ClassesViewModel // Correct projection to view model
                        {
                            ClassesId = c.ClassesId,
                            ClassesName = c.ClassesName,
                            TeacherName = c.Teacher.TeacherName, // Assuming Teacher is a navigation property
                            StudentAmount = c.StudentAmount,
                            CourseGroupName = c.CourseGroupName,
                        })
                        .ToList();

    var viewModel = new PagedViewModel<ClassesViewModel>
    {
        Items = paginatedClasses,
        PageNumber = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    return View(viewModel);
}


    [HttpPost]
    public IActionResult SignUpClasses(string courseGroupName, string teacherId)
    {
        if (!CourseGroupDictionary.TryGetValue(courseGroupName, out string courseGroup))
        {
            // Handle the case where the course name doesn't exist in the dictionary
            return BadRequest("Invalid course name.");
        }

        string currentYear = DateTime.Now.Year.ToString().Substring(2);
        
        // Count the number of classes already created this year with the same course ID
        int classCount = _db.Classes
                            .Count(c => c.CourseGroupName == courseGroupName && c.ClassesId.ToString().StartsWith(currentYear));

        // Increment the count and format it as a two-digit number
        string newClassNumber = (classCount + 1).ToString("D2");

        string classesId = currentYear + courseGroup + newClassNumber;
        string classesName = "20"+currentYear + " " + courseGroupName + newClassNumber + " Class";

        var classes = new Classes
        {
            ClassesId = Int64.Parse(classesId),
            ClassesName = classesName,
            TeacherId = Int64.Parse(teacherId),
            StudentAmount = 0,
            CourseGroupName = courseGroupName
        };

        _db.Classes.Add(classes);
        _db.SaveChanges();

        // Check if courses for this class already exist
        bool coursesExist = _db.Course.Any(c => c.CourseGroup == courseGroup && c.ClassesId == classes.ClassesId);

        if (!coursesExist)
        {
            // Get the courses related to the courseGroup
            var courses = Coursedictionary[courseGroup];
            foreach (var course in courses)
            {
                // Create and add new courses to the Course table if they don't already exist
                var newCourse = new Course
                {
                    ClassesId = classes.ClassesId,
                    CourseGroup = courseGroup,
                    CourseCode = course.Item1,
                    CourseName = course.Item2,
                    CourseIntro = "(Course Details)",
                };
                _db.Course.Add(newCourse);
            }

            _db.SaveChanges(); // Save all new courses at once
        }

        return RedirectToAction("Index");
    }

    public IActionResult SignUp(){
        var courseGroupOptions = new List<SelectListItem>
            {
                new() { Value = "Indoor Wiring Level A", Text = "Indoor Wiring Level A" },
                new() { Value = "Indoor Wiring Level B", Text = "Indoor Wiring Level B" },
                new() { Value = "Indoor Wiring Level C", Text = "Indoor Wiring Level C" },
            };
        ViewBag.CourseGroupOptions = courseGroupOptions;
        
        var teachers = _db.Teacher
                            .OrderBy(c => c.TeacherName) // Sort teacher by name
                            .Select(c => new SelectListItem
                            {
                                Value = c.TeacherId.ToString(),
                                Text = c.TeacherName
                            }).ToList();

        // Pass the list of SelectListItem to the view using ViewBag
        ViewBag.Teachers = teachers;

        return View();
    }

    [HttpGet]
public IActionResult SeeStudent(long classesId, int page = 1, int pageSize = 7)
{
    // Fetch the total number of students in the class
    var totalItems = _db.Student.Count(s => s.ClassesId == classesId);

    // Fetch the students with pagination
    var students = _db.Student
        .Where(s => s.ClassesId == classesId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    // Fetch the selected class details
    var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);

    ViewBag.Class = selectedClass;
    ViewBag.ClassesId = classesId;

    // Create a paginated view model
    var viewModel = new PagedViewModel<Student>
    {
        Items = students,
        PageNumber = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    return View(viewModel);
}

    [HttpGet]
    public IActionResult SignUpInClasses(long classesId)
    {
        // Check if the class with the given ID exists
        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        if (selectedClass == null)
        {
            return NotFound("The class with the given ID does not exist.");
        }
        
        ViewBag.ClassesId = classesId; // Pass the classesId to the view
        ViewBag.ClassesName=selectedClass.ClassesName;
        ViewBag.SelectedClass = selectedClass;
        return View(); // Render the form
    }


    [HttpPost]
    public IActionResult SignUpInClasses(long classesId, string studentNumber, string studentName)
    {
        // Check if the class with the given ID exists
        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        if (selectedClass == null)
        {
            return NotFound("The class with the given ID does not exist.");
        }

        var student = new Student
        {
            ClassesId = classesId,
            StudentNumber = studentNumber,
            StudentName = studentName
        };

        // Check if student number already exists
        var studentNumberExists = _db.Student.Any(s => s.StudentNumber == student.StudentNumber);
        if (studentNumberExists)
        {
            TempData["ErrorMessage"] = "Student number already exists.";
            ViewBag.ClassesId = classesId; // Pass the classesId again to the view
            ViewBag.ClassesName = selectedClass.ClassesName;
            return View(student); // Return the same view with error message
        }

        // Add the student to the database
        _db.Student.Add(student);
        _db.SaveChanges();

        // Increment the student count in the specified class
        selectedClass.StudentAmount += 1;
        _db.SaveChanges();

        return RedirectToAction("SeeStudent", new { classesId = classesId });
    }
    [HttpGet]
    public IActionResult SeeCourse(long classesId)
    {
        var courses = _db.Course.Where(c => c.ClassesId == classesId).ToList();
        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        ViewBag.Class = selectedClass;
        ViewBag.Courses= courses;
        ViewBag.CourseNumber= courses.Count;
        return View(courses);
    }
    [HttpGet]
    public IActionResult SearchStudentInClasses(long classesId, string? studentNumber)
    {
        if (string.IsNullOrEmpty(studentNumber))
        {
            ViewBag.Students = _db.Student
                            .Where(s => s.ClassesId == classesId)
                            .Select(s => new SelectListItem
                            {
                                Value = s.StudentNumber,
                                Text = $"{s.StudentName} ({s.StudentNumber})" // Display both name and number in the dropdown
                            }).ToList();
            ViewBag.SelectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
            ViewBag.ClassesId = classesId;
            return View(); // Render the form
        }
        // Get the selected class
        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        var selectedStudent = _db.Student
                            .Where(s => s.ClassesId == classesId && s.StudentNumber == studentNumber)
                            .ToList();
        ViewBag.SelectedClass = selectedClass;
        ViewBag.ClassesId = classesId;
        
        return View(selectedStudent); // Pass the filtered students to the view
    }
    [HttpGet]
    public IActionResult DeleteStudentInClasses(long classesId){
        ViewBag.Students = _db.Student
                                .Where(s => s.ClassesId == classesId)
                                .Select(s => new SelectListItem
                                {
                                    Value = s.StudentNumber,
                                    Text = $"{s.StudentName} ({s.StudentNumber.ToUpper()})" // Display both name and number in the dropdown
                                }).ToList();

            ViewBag.SelectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
            ViewBag.ClassesId = classesId;
            return View(); // Render the form
    }
    [HttpPost]
    public IActionResult DeleteStudentInClasses(long classesId, string? studentNumber)
    {
        var student = _db.Student.FirstOrDefault(s => s.ClassesId == classesId && s.StudentNumber == studentNumber);
        if (student == null)
        {
            TempData["ErrorMessage"] = "Student does not exist";
            return RedirectToAction("DeleteStudentInClasses", new { classesId });
        }

        var studentHasRecords = _db.RecordLog.Any(r => r.StudentId == student.StudentId);
        if (studentHasRecords)
        {
            TempData["ErrorMessage"] = "Cannot delete student with records";
            return RedirectToAction("DeleteStudentInClasses", new { classesId });
        }
        
        _db.Student.Remove(student);
        _db.SaveChanges();

        var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
        if (selectedClass != null)
        {
            selectedClass.StudentAmount -= 1;
            _db.SaveChanges();
        }

        TempData["SuccessMessage"] = $"Successfully delete {student.StudentName}";
        return RedirectToAction("DeleteStudentInClasses", new { classesId });
    }
[HttpGet]
public IActionResult UpdateStudentInClasses(long classesId, int? studentId = null)
{
    // Get the selected class
    ViewBag.SelectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
    ViewBag.ClassesId = classesId;

    // Populate students for the selected class
    var students = _db.Student
                      .Where(s => s.ClassesId == classesId)
                      .Select(s => new SelectListItem
                      {
                          Value = s.StudentId.ToString(),
                          Text = $"{s.StudentName} ({s.StudentNumber.ToUpper()})"
                      }).ToList();
    ViewBag.Students = students;

    // Populate classes for the dropdown
    ViewBag.Classes = _db.Classes.Select(c => new SelectListItem
    {
        Value = c.ClassesId.ToString(),
        Text = c.ClassesName
    }).ToList();

    // If studentId is provided, fetch the student
    if (studentId != null && studentId != 0)
    {
        var student = _db.Student.FirstOrDefault(s => s.StudentId == studentId);
        return View(student);
    }

    return View();
}

[HttpPost]
public IActionResult UpdateStudentInClasses(long classesId, int studentId, string studentNumber, string studentName, long newClassesId)
{
    if (newClassesId == 0)
    {
        TempData["ErrorMessage"] = "Please select valid class.";
        return RedirectToAction("UpdateStudentInClasses", new { classesId, studentId });
    }
    if (ModelState.IsValid)
    {
        var student = _db.Student.FirstOrDefault(s => s.StudentId == studentId);
        // Ensure the newClassesId exists in the Classes table
        var newClasses = _db.Classes.FirstOrDefault(c => c.ClassesId == newClassesId);
        if (newClasses == null)
        {
            TempData["ErrorMessage"] = "Selected class does not exist";
            return RedirectToAction("UpdateStudentInClasses", new { classesId, studentId });
        }
        var studentHasRecords = _db.RecordLog.Any(r => r.StudentId == student.StudentId);
        if (studentHasRecords)
        {
            TempData["ErrorMessage"] = "Cannot update student with records";
            return RedirectToAction("UpdateStudentInClasses", new { classesId, studentId });
        }
        var studentNumberExists = _db.Student.Any(s => s.StudentNumber == studentNumber && s.StudentId != studentId);
        if (studentNumberExists)
        {
            TempData["ErrorMessage"] = $"Student number: {studentNumber} already exists";
            return RedirectToAction("UpdateStudentInClasses", new { classesId, studentId });
        }

        if (student != null)
        {
            // Update the student details
            var oldClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
            if (oldClass != null)
            {
                oldClass.StudentAmount -= 1;
            }

            student.StudentNumber = studentNumber;
            student.StudentName = studentName;
            student.ClassesId = newClassesId;

            var newClass = _db.Classes.FirstOrDefault(c => c.ClassesId == newClassesId);
            if (newClass != null)
            {
                newClass.StudentAmount += 1;
            }

            _db.SaveChanges();
            TempData["SuccessMessage"] = "Student updated successfully";
            return RedirectToAction("UpdateStudentInClasses", new { classesId, studentId });
        }
        else
        {
            TempData["ErrorMessage"] = "Student does not exist";
            return RedirectToAction("UpdateStudentInClasses", new { classesId });
        }
    }

    // Re-populate the dropdowns in case of validation failure
    ViewBag.Classes = _db.Classes.Select(c => new SelectListItem
    {
        Value = c.ClassesId.ToString(),
        Text = c.ClassesName
    }).ToList();
    
    ViewBag.SelectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
    ViewBag.ClassesId = classesId;

    return View();
}

    public IActionResult UploadExcelStudentInClass(long classesId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "File is empty";
            return RedirectToAction("SignUpInClasses", new { classesId = classesId });
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

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var studentNumber = worksheet.Cells[row, 1].Text;
                        var studentName = worksheet.Cells[row, 2].Text;

                        if (string.IsNullOrEmpty(studentNumber) || string.IsNullOrEmpty(studentName))
                        {
                            TempData["ErrorMessage"] = $"Row {row}: Student number and name are required";
                            return RedirectToAction("SignUpInClasses", new { classesId = classesId });
                        }
                        var studentExists = _db.Student.Any(s => s.StudentNumber == studentNumber);
                        if (studentExists)
                        {
                            TempData["ErrorMessage"] = $"Row {row}: Student number {studentNumber} already exists";
                            return RedirectToAction("SignUpInClasses", new { classesId = classesId });
                        }
                        var student = new Student
                        {
                            StudentNumber = studentNumber,
                            StudentName = studentName,
                            ClassesId = classesId
                        };

                        // Add the student to the database
                        _db.Student.Add(student);
                    }

                    var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId);
                    if (selectedClass != null)
                    {
                        selectedClass.StudentAmount += rowCount - 1; // Increment the student amount by the number of students added
                        _db.SaveChanges();
                    }

                    _db.SaveChanges();
                }
            }

            TempData["SuccessMessage"] = "Upload successful";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error: {ex.Message}";
        }

        return RedirectToAction("SignUpInClasses", new { classesId = classesId });
    }

    [HttpGet]
    public IActionResult DownloadExampleFileForClass()
    {
        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "files", "AddBulkStudentsExampleExcel.xlsx");
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AddBulkStudentsExampleExcel.xlsx");
    }
}