using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using quasitekWeb.ViewModels;

namespace quasitekWeb.Controllers
{
    public class RecordLogController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RecordLogController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
       public IActionResult Index(string classesSearchTerm, string studentSearchTerm, string sortOrder, int page = 1, int pageSize = 7)
{
    // Populate the Classes dropdown
    ViewBag.Classes = _db.Classes.Select(c => new SelectListItem
    {
        Value = c.ClassesName,
        Text = c.ClassesName
    }).ToList();

    // Start with the base query
    var recordsQuery = _db.RecordLog
                            .Include(r => r.Student)
                                .ThenInclude(s => s.Classes)
                            .Include(r => r.TechType)
                            .Include(r => r.AcedemicType)
                            .Include(r => r.Course)
                            .Include(r => r.ModeNavigation)
                            .Select(r => new RecordLogViewModel
                            {
                                LogId = r.LogId,
                                StudentId = r.StudentId,
                                StudentNumber = r.StudentNumber,
                                StudentName = r.Student.StudentName,
                                ClassesName = r.Student.Classes.ClassesName,
                                CourseId = r.CourseId,
                                CourseName = r.Course.CourseName,
                                RecordDate = r.RecordDate,
                                ModeName = r.ModeNavigation.ModeName,
                                AcedemicTypeId = r.AcedemicTypeId ?? 0,
                                AcedemicTypeScore = r.AcedemicTypeScore,
                                TechTypeId = r.TechTypeId ?? 0,
                                TechTypeScore = r.TechTypeScore
                            })
                            .AsQueryable();

    // Apply search filters
    if (!string.IsNullOrEmpty(studentSearchTerm) && string.IsNullOrEmpty(classesSearchTerm))
    {
        recordsQuery = recordsQuery.Where(r => r.StudentNumber==studentSearchTerm
                                            || r.StudentName==studentSearchTerm);
        if (!recordsQuery.Any())
        {
            ViewBag.ErrorMessage = $"Records for Student: {studentSearchTerm} not found，please re-enter the correct student number or name.";
        }
    }
    else if (!string.IsNullOrEmpty(classesSearchTerm) && string.IsNullOrEmpty(studentSearchTerm))
    {
        recordsQuery = recordsQuery.Where(r => r.ClassesName == classesSearchTerm);
        if (!recordsQuery.Any())
        {
            ViewBag.ErrorMessage = $"Class: {classesSearchTerm} does not have any records.";
        }
    }
    else if (!string.IsNullOrEmpty(studentSearchTerm) && !string.IsNullOrEmpty(classesSearchTerm))
    {
        recordsQuery = recordsQuery.Where(r => (r.StudentNumber==studentSearchTerm 
                                                || r.StudentName==studentSearchTerm)
                                                && r.ClassesName == classesSearchTerm);
                                                if (!recordsQuery.Any())
        {
            ViewBag.ErrorMessage = $"Student: {studentSearchTerm} does not have any records in Class: {classesSearchTerm}, please re-enter the correct student number or name.";
        }
    }

    // Sorting logic
    switch (sortOrder)
    {
        case "recordDate_asc":
            recordsQuery = recordsQuery.OrderBy(r => r.RecordDate);
            break;
        case "recordDate_desc":
            recordsQuery = recordsQuery.OrderByDescending(r => r.RecordDate);
            break;
        case "studentNumber_asc":
            recordsQuery = recordsQuery.OrderBy(r => r.StudentNumber);
            break;
        case "studentNumber_desc":
            recordsQuery = recordsQuery.OrderByDescending(r => r.StudentNumber);
            break;
        case "acedemicTestScore_asc":
            recordsQuery = recordsQuery.OrderBy(r => r.AcedemicTypeScore);
            break;
        case "acedemicTestScore_desc":
            recordsQuery = recordsQuery.OrderByDescending(r => r.AcedemicTypeScore);
            break;
        case "techTestScore_asc":
            recordsQuery = recordsQuery.OrderBy(r => r.TechTypeScore);
            break;
        case "techTestScore_desc":
            recordsQuery = recordsQuery.OrderByDescending(r => r.TechTypeScore);
            break;
        default:
            recordsQuery = recordsQuery.OrderBy(r => r.LogId);
            break;
    }

    // Pagination logic
    var totalItems = recordsQuery.Count();
    var paginatedRecords = recordsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    // Create PagedViewModel
    var viewModel = new PagedViewModel<RecordLogViewModel>
    {
        Items = paginatedRecords,
        PageNumber = page,
        PageSize = pageSize,
        TotalItems = totalItems
    };

    // Persist search terms and class selection
    ViewBag.StudentSearchTerm = studentSearchTerm;
    ViewBag.ClassesSearchTerm = classesSearchTerm;
    ViewBag.SortOrder = sortOrder;

    return View(viewModel);
}

        [HttpGet]
        public IActionResult SearchClassesRecord(long? classesId, string sortOrder, int page = 1, int pageSize = 7)
        {
            // Populate the class list for the dropdown
            var classes = _db.Classes
                            .Select(c => new SelectListItem
                            {
                                Value = c.ClassesId.ToString(),
                                Text = c.ClassesName
                            })
                            .ToList();

            ViewBag.Classes = classes;

            // Handle when no class is selected
            if (!classesId.HasValue)
            {
                // Just return the view with no records and the class dropdown populated
                return View(new SearchClassesRecordViewModel
                {
                    Records = new List<RecordLogViewModel>(), // No records to display
                    SearchPerformed = false, // Indicate that the search hasn't been performed yet
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = 0 // No records
                });
            }

            ViewBag.ClassesId = classesId;

            // Fetch records related to the selected class
            var records = _db.RecordLog
                            .Include(r => r.Student)
                                .ThenInclude(s => s.Classes)
                            .Include(r => r.Course)
                            .Include(r => r.ModeNavigation)
                            .Where(r => r.Student.ClassesId == classesId.Value)
                            .Select(r => new RecordLogViewModel
                            {
                                LogId = r.LogId,
                                StudentId = r.StudentId,
                                StudentNumber = r.StudentNumber,
                                StudentName = r.Student.StudentName,
                                ClassesName = r.Student.Classes.ClassesName,
                                CourseName = r.Course.CourseName,
                                RecordDate = r.RecordDate,
                                ModeId = r.ModeId,
                                ModeName = r.ModeNavigation.ModeName,
                                AcedemicTypeId = r.AcedemicTypeId ?? 0,
                                AcedemicTypeScore = r.AcedemicTypeScore,
                                TechTypeId = r.TechTypeId ?? 0,
                                TechTypeScore = r.TechTypeScore
                            })
                            .AsQueryable();

            // Sorting logic
            switch (sortOrder)
            {
                case "recordDate_asc":
                    records = records.OrderBy(r => r.RecordDate);
                    break;
                case "recordDate_desc":
                    records = records.OrderByDescending(r => r.RecordDate);
                    break;
                case "studentNumber_asc":
                    records = records.OrderBy(r => r.StudentNumber);
                    break;
                case "studentNumber_desc":
                    records = records.OrderByDescending(r => r.StudentNumber);
                    break;
                case "techTestScore_asc":
                    records = records.OrderBy(r => r.TechTypeScore);
                    break;
                case "techTestScore_desc":
                    records = records.OrderByDescending(r => r.TechTypeScore);
                    break;
                case "acedemicTestScore_asc":
                    records = records.OrderBy(r => r.AcedemicTypeScore);
                    break;
                case "acedemicTestScore_desc":
                    records = records.OrderByDescending(r => r.AcedemicTypeScore);
                    break;
                default:
                    records = records.OrderBy(r => r.LogId);
                    break;
            }

            var totalItems = records.Count();
            var paginatedRecords = records.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Fetch selected class details
            var selectedClass = _db.Classes.FirstOrDefault(c => c.ClassesId == classesId.Value);
            ViewBag.SelectedClass = selectedClass;
            ViewBag.SortOrder = sortOrder;

            var viewModel = new SearchClassesRecordViewModel
            {
                Records = paginatedRecords,
                SearchPerformed = true,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult SearchStudentRecord(string searchTerm, string sortOrder, int page = 1, int pageSize = 6)
        {
            // Query the RecordLog and include related entities
            var records = _db.RecordLog
                            .Include(r => r.Student)
                                .ThenInclude(s => s.Classes)
                            .Include(r => r.TechType)
                            .Include(r => r.AcedemicType)
                            .Include(r => r.Course)
                            .Include(r => r.ModeNavigation)
                            .Where(r => r.Student.StudentNumber == searchTerm)
                            .Select(r => new RecordLogViewModel
                            {
                                LogId = r.LogId,
                                StudentId = r.StudentId,
                                StudentNumber = r.StudentNumber,
                                StudentName = r.Student.StudentName,
                                ClassesName = r.Student.Classes.ClassesName,
                                CourseName = r.Course.CourseName,
                                RecordDate = r.RecordDate,
                                ModeName = r.ModeNavigation.ModeName,
                                AcedemicTypeId = r.AcedemicTypeId ?? 0,
                                AcedemicTypeScore = r.AcedemicTypeScore,
                                TechTypeId = r.TechTypeId ?? 0,
                                TechTypeScore = r.TechTypeScore
                            })
                            .AsQueryable();

            // Sorting logic
            switch (sortOrder)
            {
                case "recordDate_asc":
                    records = records.OrderBy(r => r.RecordDate);
                    break;
                case "recordDate_desc":
                    records = records.OrderByDescending(r => r.RecordDate);
                    break;
                case "techTestScore_asc":
                    records = records.OrderBy(r => r.TechTypeScore);
                    break;
                case "techTestScore_desc":
                    records = records.OrderByDescending(r => r.TechTypeScore);
                    break;
                case "acedemicTestScore_asc":
                    records = records.OrderBy(r => r.AcedemicTypeScore);
                    break;
                case "acedemicTestScore_desc":
                    records = records.OrderByDescending(r => r.AcedemicTypeScore);
                    break;
                default:
                    records = records.OrderBy(r => r.LogId);
                    break;
            }

            var totalItems = records.Count();
            var paginatedRecords = records.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var student = _db.Student.FirstOrDefault(s => s.StudentNumber == searchTerm);

            // Prepare the ViewModel for the view
            var viewModel = new SearchStudentRecordViewModel
            {
                Records = paginatedRecords,
                SearchPerformed = !string.IsNullOrEmpty(searchTerm),
                MessageIfNotFound = student == null ? "無查獲此學生，請再試一次" : null,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortOrder = sortOrder;

            return View(viewModel);
        }

    }
}
