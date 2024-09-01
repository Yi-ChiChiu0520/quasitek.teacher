using AutoMapper;
using quasitekWeb.Dto;
using quasitekWeb.Models;
using quasitekWeb.ViewModels;

namespace quasitekWeb.Helper.Mappers
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();
            CreateMap<Classes, ClassesDto>();
            CreateMap<ClassesDto, Classes>();
        }
    }
}  