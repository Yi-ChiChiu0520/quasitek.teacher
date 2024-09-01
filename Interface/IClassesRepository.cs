using quasitekWeb.Models;
using quasitekWeb.ViewModels;

namespace quasitekWeb.Interface
{
    public interface IClassesRepository
    {
        Task<IEnumerable<Classes>> GetAllClasses();
        Task<Classes?> GetClasses(string classesName);
    } 
}