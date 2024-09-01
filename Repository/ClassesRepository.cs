using quasitekWeb.Data;
using quasitekWeb.Interface;
using quasitekWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace quasitekWeb.Repository
{
    public class ClassesRepository : IClassesRepository
    {
        private ApplicationDbContext _db;

        public ClassesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Classes>> GetAllClasses()
        {
            return await _db.Classes.ToListAsync();
        } 
        public async Task<Classes?> GetClasses(string classesName){
            var classes = await _db.Classes
                .Where(cl => cl.ClassesName==classesName)
                .FirstOrDefaultAsync();

            return classes;
        }
    }
}