using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Models;
using quasitekWeb.Interface;
using AutoMapper;
using quasitekWeb.Dto;

namespace quasitekWeb.Controllers.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClassesApiController : Controller
    {
        private readonly IClassesRepository _classesRepository;
        private readonly IMapper _mapper;
  
        public ClassesApiController(IClassesRepository classesRepository, IMapper mapper)
        {
            _classesRepository = classesRepository;
            _mapper= mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Classes>))]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _classesRepository.GetAllClasses();
            var classesMapped = _mapper.Map<List<ClassesDto>>(classes);

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
             
            return Ok(classesMapped);
        }

        [HttpGet("{classesName}")]
        [ProducesResponseType(200, Type = typeof(Classes))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetClasses(string classesName)
        {
            var classes = await _classesRepository.GetClasses(classesName);
            var classesMapped = _mapper.Map<ClassesDto>(classes);

            if(classes == null){
                return NotFound();
            }

            return Ok(classesMapped);
        }   
    }
}
 