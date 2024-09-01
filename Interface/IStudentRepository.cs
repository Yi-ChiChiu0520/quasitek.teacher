using quasitekWeb.Models;

namespace quasitekWeb.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student?> GetStudent(string studentNumber);
    } 
}