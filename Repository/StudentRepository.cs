using Microsoft.EntityFrameworkCore;
using quasitekWeb.Data;
using quasitekWeb.Interface;
using quasitekWeb.Models;

namespace quasitekWeb.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private ApplicationDbContext _db;
        public StudentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _db.Student.ToListAsync();
        } 
        public async Task<Student?> GetStudent(string studentNumber)
        {
            var student = await _db.Student
                .Where(s => s.StudentNumber == studentNumber)
                .FirstOrDefaultAsync();

            return student;
        }
    }
}