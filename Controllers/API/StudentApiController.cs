using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Interface;
using AutoMapper;
using quasitekWeb.Dto;

namespace quasitekWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
  
        public StudentApiController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper= mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Student>))]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetAllStudents();
            var studentsMapped = _mapper.Map<List<StudentDto>>(students);

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
             
            return Ok(studentsMapped);
        }

        [HttpGet("{studentNumber}")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudent(string studentNumber)
        {
            var student = await _studentRepository.GetStudent(studentNumber);
            var studentMapped = _mapper.Map<StudentDto>(student);

            if(student == null){
                return NotFound();
            }

            return Ok(studentMapped);
        }    
    }
}
 