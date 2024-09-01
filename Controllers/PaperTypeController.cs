using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using quasitekWeb.ViewModels;


namespace quasitekWeb.Controllers
{
    public class PaperTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PaperTypeController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult ViewPaper(long typeId)
        {
            // Fetching the PaperType and related RecordLog, Student, Classes, and Course
            var paperType = _db.PaperType
                                .Include(p => p.AcademicRecordLogs)
                                    .ThenInclude(r => r.Student)
                                        .ThenInclude(s => s.Classes)
                                .Include(p => p.TechnicalRecordLogs)
                                    .ThenInclude(r => r.Student)
                                        .ThenInclude(s => s.Classes)
                                .Include(p => p.AcademicRecordLogs)
                                    .ThenInclude(r => r.Course)
                                .Include(p => p.TechnicalRecordLogs)
                                    .ThenInclude(r => r.Course)
                                .FirstOrDefault(p => p.TypeId == typeId);

            if (paperType == null)
            {
                return NotFound(); // Handle the case where the paperType is not found
            }

            var firstRecordLog = paperType.AcademicRecordLogs.FirstOrDefault();
            
            if (firstRecordLog == null)
            {
                firstRecordLog = paperType.TechnicalRecordLogs.FirstOrDefault();
                if(firstRecordLog==null){
                    return NotFound("Record log not found.");
                }
            }

            // Mapping to the ViewModel
            var viewModel = new PaperTypeViewModel
            {
                TypeId = paperType.TypeId,
                TypeName = paperType.TypeName,
                Time = paperType.Time,
                CorrectNumber = paperType.CorrectNumber,
                WrongNumber = paperType.WrongNumber,
                CorrectAnswers = paperType.CorrectAnswers,
                WrongAnswers = paperType.WrongAnswers,
                StudentName = firstRecordLog.Student?.StudentName,
                ClassName = firstRecordLog.Student?.Classes?.ClassesName,
                CourseName = firstRecordLog.Course?.CourseName,
                Score=Math.Round((decimal)(paperType.CorrectNumber * 100) / (paperType.CorrectNumber + paperType.WrongNumber), 2) // Round to two decimal places
            };

            return View(viewModel); // Passing the ViewModel to the view
        }
    }
}