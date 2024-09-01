using quasitekWeb.Interface;
using quasitekWeb.Data;
using quasitekWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace quasitekWeb.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _db;

        public LogRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string> UploadLogsFromJson(LogWrapper logWrapper)
        {
            try
            {
                // Check if logs are received
                if (logWrapper == null || logWrapper.Log == null || logWrapper.Log.Count == 0)
                {
                    return "Invalid JSON data.";
                }

                // Loop through each log and save it to the database
                foreach (var log in logWrapper.Log)
                {
                    // Find the student by StudentNumber
                    var studentId = await _db.Student
                        .Where(s => s.StudentNumber == log.StudentNumber)
                        .Select(s => s.StudentId)
                        .FirstOrDefaultAsync();

                    if (studentId == 0)
                    {
                        // Student not found, handle accordingly
                        return $"Student with student number: {log.StudentNumber} not found.";
                    }
                    
                    long? academicTypeId = null;
                    long? techTypeId = null;
                    decimal? academicScore = null;
                    decimal? techScore = null;

                    foreach (var type in log.Type)
                    {
                        if (type.Name != "Academic" && type.Name != "Technical")
                        {
                            // Invalid type name, handle accordingly
                            return $"{type.Name} is not a valid exam subject name.";
                        }

                        // Check if test was conducted
                        if (!type.HasTest)
                        {
                            continue;
                        }

                        var newType = new PaperType
                        {
                            TypeName = type.Name,
                            Time = type.Time,
                            CorrectAnswers = "",
                            WrongAnswers = "",
                            CorrectNumber = 0,
                            WrongNumber = 0
                        };

                        // Process correct and wrong answers
                        foreach (var detail in type.Detail)
                        {
                            foreach (var correctAnswer in detail.Correct)
                            {
                                newType.CorrectAnswers += correctAnswer + ",";
                                newType.CorrectNumber++;
                            }

                            foreach (var wrongAnswer in detail.Wrong)
                            {
                                newType.WrongAnswers += wrongAnswer + ",";
                                newType.WrongNumber++;
                            }
                        }

                        // Trim trailing commas from CorrectAnswers and WrongAnswers
                        if (!string.IsNullOrEmpty(newType.CorrectAnswers))
                            newType.CorrectAnswers = newType.CorrectAnswers.TrimEnd(',');

                        if (!string.IsNullOrEmpty(newType.WrongAnswers))
                            newType.WrongAnswers = newType.WrongAnswers.TrimEnd(',');

                        // Add the type to the PaperType table and save it
                        _db.PaperType.Add(newType);
                        await _db.SaveChangesAsync(); // Save to get the TypeId

                        // Calculate the score as a percentage
                        decimal score = (decimal)newType.CorrectNumber / (newType.CorrectNumber + newType.WrongNumber) * 100;

                        // Set the academic or tech type based on the name
                        if (type.Name == "Academic")
                        {
                            academicTypeId = newType.TypeId;
                            academicScore = score;
                        }
                        else if (type.Name == "Technical")
                        {
                            techTypeId = newType.TypeId;
                            techScore = score;
                        }
                    }
                    var courseId = await _db.Course
                        .Where(s => s.CourseCode == log.CourseCode)
                        .Select(s => s.CourseId)
                        .FirstOrDefaultAsync();

                    if (courseId == 0)
                    {
                        // Student not found, handle accordingly
                        return $"Course with course code: {log.CourseCode} not found.";
                    }
                    var studentClassesId = await _db.Student
                        .Where(s => s.StudentId == studentId)
                        .Select(s => s.ClassesId)
                        .FirstOrDefaultAsync();

                    var courseExists = await _db.Course
                        .Where(c => c.ClassesId == studentClassesId && c.CourseCode == log.CourseCode)
                        .AnyAsync();

                    if (!courseExists)
                    {
                        return $"Student is not taking course with course code: {log.CourseCode}";
                    }
                    
                    var modeExists = await _db.Mode
                        .Where(m => m.ModeId == log.ModeId)
                        .AnyAsync();
                    if (!modeExists)
                    {
                        return $"Mode with mode id {log.ModeId} not found.";
                    }
                    // Add the record log with the academic and tech type IDs and scores
                    var newLog = new RecordLog
                    {
                        StudentId = studentId,
                        StudentNumber = log.StudentNumber,
                        RecordDate = log.LogDate,
                        CourseId = courseId,
                        CourseCode = log.CourseCode,
                        ModeId = log.ModeId,
                        AcedemicTypeId = academicTypeId,
                        AcedemicTypeScore = academicScore,
                        TechTypeId = techTypeId,
                        TechTypeScore = techScore
                    };

                    _db.RecordLog.Add(newLog);
                    await _db.SaveChangesAsync();
                }

                return "Records uploaded successfully.";
            }
            catch (JsonException ex)
            {
                return $"Error deserializing JSON: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Internal server error: {ex.Message}";
            }
        }
    }
}
